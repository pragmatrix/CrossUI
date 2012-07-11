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

				var drawingBackend = (IDrawingBackend)Activator.CreateInstance(drawingBackendType);
				var classes = run(resultFactory, drawingBackend, testAssembly);
				return resultFactory.Assembly(testAssemblyPath, classes);
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

		struct CandidateMethod
		{
			public readonly MethodInfo Info;
			public readonly BitmapDrawingTestAttribute Attribute;
			public readonly bool Ignorable;

			public CandidateMethod(MethodInfo info, BitmapDrawingTestAttribute attribute, bool ignorable)
			{
				Info = info;
				Attribute = attribute;
				Ignorable = ignorable;
			}
		}

		static IEnumerable<CandidateMethod> getTestableMethodsForType(Type type)
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
						yield return new CandidateMethod(method, typeAttribute_, ignorable: true);
						break;

					case 1:
						var attribute = (BitmapDrawingTestAttribute) attributes[0];
						if (typeAttribute_ != null)
							attribute = typeAttribute_.refine(attribute);

						yield return new CandidateMethod(method, attribute, ignorable: false);
						break;
				}
			}
		}

		static ITestResultMethod[] runClassTest(ITestResultFactory resultFactory, IDrawingBackend drawingBackend, Type type, IEnumerable<CandidateMethod> methods)
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

		static ITestResultMethod[] runMethodTests(ITestResultFactory resultFactory, IDrawingBackend drawingBackend, object instance, IEnumerable<CandidateMethod> methods)
		{
			var results = new List<ITestResultMethod>();
			foreach (var method in methods)
			{
				var info = method.Info;

				try
				{
					string whyNot;
					if (!canTestMethod(info, out whyNot))
					{
						if (!method.Ignorable)
							throw new Exception(whyNot);
						continue;
					}

					var bitmap = runMethodTest(resultFactory, drawingBackend, instance, method);
					results.Add(resultFactory.Method(info.Name, bitmap));
				}
				catch (Exception e)
				{
					results.Add(resultFactory.Method(info.Name, e));
				}
			}

			return results.ToArray();
		}

		static ITestResultBitmap runMethodTest(ITestResultFactory resultFactory, IDrawingBackend drawingBackend, object instance, CandidateMethod candidateMethod)
		{
			var info = candidateMethod.Info;
			var attribute = candidateMethod.Attribute;

			var width = attribute.Width;
			var height = attribute.Height;

			using (var context = drawingBackend.CreateBitmapDrawingContext(width, height))
			{
				IDrawingContext drawingContext;
				using (context.BeginDraw(out drawingContext))
				{
					info.Invoke(instance, new object[] { drawingContext });
				}
	
				return resultFactory.Bitmap(width, height, context.ExtractRawBitmap());
			}
		}

		static bool canTestMethod(MethodInfo method, out string whyNot)
		{
			if (method.IsStatic || method.IsGenericMethod)
			{
				whyNot = "{0}: is not allowed to be static or generic".format(method);
				return false;
			}

			var parameters = method.GetParameters();
			if (parameters.Length != 1)
			{
				whyNot = "{0}: expect one parameter".format(method);
				return false;
			}

			var firstParameter = parameters[0];
			if (firstParameter.ParameterType != typeof(IDrawingContext))
			{
				whyNot = "{0}: expect IDrawingContext as first and only parameter";
				return false;
			}

			whyNot = string.Empty;
			return true;
		}
	}
}
