using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrossUI.Toolbox;

namespace CrossUI.Testing
{
	public sealed class TestRunnerPortable : ITestRunner
	{
		public ITestResultAssembly run(ITestResultFactory resultFactory, string testAssemblyPath, Assembly testAssembly)
		{
			// http://blogs.msdn.com/b/suzcook/archive/2003/05/29/choosing-a-binding-context.aspx#57147
			// LoadFrom differs from Load in that dependent assemblies can be resolved outside from the 
			// BasePath.

			try
			{
				var drawingBackendType = tryLocateDrawingBackend(testAssembly);
				if (drawingBackendType == null)
					throw new Exception("Missing [DrawingBackend] attribute. Please add [assembly:DrawingBackend] to your test assembly.");

				using (var drawingBackend = (IDrawingBackend)Activator.CreateInstance(drawingBackendType))
				{
					var classes = run(resultFactory, drawingBackend, testAssembly);
					return resultFactory.Assembly(testAssemblyPath, classes);
				}
			}
			catch (Exception e)
			{
				return resultFactory.Assembly(testAssemblyPath, e);
			}
		}

		static Type tryLocateDrawingBackend(Assembly assembly)
		{
			var attribute = assembly.GetCustomAttributes(typeof (DrawingBackendAttribute), false);
			return attribute.Length == 0 ? null : ((DrawingBackendAttribute)attribute[0]).Type;
		}

		public ITestResultClass[] run(ITestResultFactory resultFactory, IDrawingBackend drawingBackend, Assembly assembly)
		{
			var results = new List<ITestResultClass>();

			foreach (var type in assembly.GetTypes())
			{
				try
				{
					var testable = getTestableMethodsForType(type).ToArray();

					if (testable.Length == 0)
						continue;

					var methods = runClassTest(resultFactory, drawingBackend, type, testable);
					results.Add(resultFactory.Class(type.Namespace, type.Name, methods));
				}
				catch (Exception e)
				{
					results.Add(resultFactory.Class(type.Namespace, type.Name, e));
				}
			}

			return results.ToArray();
		}

		static IEnumerable<TestMethod> getTestableMethodsForType(Type type)
		{
			var typeAttributes = type.GetCustomAttributes(typeof (BitmapDrawingTestAttribute), inherit: false);
			var typeAttribute_ = typeAttributes.Length == 1 ? (BitmapDrawingTestAttribute) typeAttributes[0] : null;

			foreach (var method in type.GetMethods())
			{
				var attributes = method.GetCustomAttributes(typeof (BitmapDrawingTestAttribute), inherit: false);

				switch (attributes.Length)
				{
					case 0:
						if (typeAttribute_ == null)
							break;
						yield return new TestMethod(method, typeAttribute_, ignorable: true);
						break;

					case 1:
						var attribute = (BitmapDrawingTestAttribute) attributes[0];
						if (typeAttribute_ != null)
							attribute = typeAttribute_.refine(attribute);

						yield return new TestMethod(method, attribute, ignorable: false);
						break;
				}
			}
		}

		static ITestResultMethod[] runClassTest(ITestResultFactory resultFactory, IDrawingBackend drawingBackend, Type type, IEnumerable<TestMethod> methods)
		{
			var constructor = type.GetConstructor(new Type[0]);
			if (constructor == null)
				throw new Exception("No constructor found for {0}".format(type));

			var instance = constructor.Invoke(null);

			try
			{
				return runMethodTests(resultFactory, drawingBackend, instance, methods);
			}
			finally
			{
				var disposable = instance as IDisposable;
				if (disposable != null)
				{
					try
					{
						disposable.Dispose();
					}
					catch
					{
						// and where to put this result, should we tamper with the method results or even
						// invalidate all?
					}
				}
			}
		}

		static ITestResultMethod[] runMethodTests(
			ITestResultFactory resultFactory, 
			IDrawingBackend drawingBackend, 
			object instance, 
			IEnumerable<TestMethod> methods)
		{
			var results = new List<ITestResultMethod>();
			foreach (var method in methods)
			{
				var info = method.Info;

				try
				{
					string whyNot;
					if (!method.canTest(out whyNot))
					{
						if (!method.Ignorable)
							throw new Exception(whyNot);
						continue;
					}

					var methodResult = runMethodTest(resultFactory, drawingBackend, instance, method);
					results.Add(methodResult);
				}
				catch (Exception e)
				{
					results.Add(resultFactory.Method(info.Name, e));
				}
			}

			return results.ToArray();
		}

		static ITestResultMethod runMethodTest(
			ITestResultFactory resultFactory, 
			IDrawingBackend drawingBackend, 
			object instance, 
			TestMethod testMethod)
		{
			var firstParameterType = testMethod.FirstParamterType;
			if (firstParameterType.IsAssignableFrom(typeof(IDrawingTarget)))
			{
				return runDrawingTargetTest(resultFactory, drawingBackend, instance, testMethod);
			}

			if (firstParameterType.IsAssignableFrom(typeof(IGeometryTarget)))
			{
				return runGeometryTargetTest(resultFactory, drawingBackend, instance, testMethod);
			}

			throw new Exception("Unable to decide what test to run based on first parameter type {0}\nShould be either IDrawingTarget or IGeometryTarget".format(firstParameterType));
		}

		static ITestResultMethod runDrawingTargetTest(
			ITestResultFactory resultFactory, 
			IDrawingBackend drawingBackend, 
			object instance, 
			TestMethod testMethod)
		{
			return runMethodTest(resultFactory, drawingBackend, testMethod, drawingTarget => testMethod.invoke(instance, drawingTarget, drawingBackend));
		}

		static ITestResultMethod runGeometryTargetTest(
				ITestResultFactory resultFactory,
				IDrawingBackend drawingBackend,
				object instance,
				TestMethod testMethod)
		{
			using (var geometry = drawingBackend.Geometry(target => testMethod.invoke(instance, target, drawingBackend)))
			{
				return runMethodTest(resultFactory, drawingBackend, testMethod, dt => dt.Geometry(geometry));
			}
		}

		static ITestResultMethod runMethodTest(
			ITestResultFactory resultFactory,
			IDrawingBackend drawingBackend,
			TestMethod testMethod,
			Action<IDrawingTarget> action)
		{
			var attribute = testMethod.Attribute;

			var width = attribute.Width;
			var height = attribute.Height;

			using (var target = drawingBackend.CreateBitmapDrawingTarget(width, height))
			{
				IDrawingTarget drawingTarget;
				using (target.BeginDraw(out drawingTarget))
				{
					action(drawingTarget);
				}

				var bitmap = resultFactory.Bitmap(width, height, target.ExtractRawBitmap());
				var testReport = resultFactory.Report(drawingTarget.Reports);

				return resultFactory.Method(testMethod.Info.Name, bitmap, testReport);
			}
		}

	}
}
