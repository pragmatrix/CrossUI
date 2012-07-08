using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrossUI.Toolbox;

namespace CrossUI.Testing
{
	public sealed class TestRunner : MarshalByRefObject
	{
		public TestResultAssembly run(string testAssemblyPath)
		{
			// http://blogs.msdn.com/b/suzcook/archive/2003/05/29/choosing-a-binding-context.aspx#57147
			// LoadFrom differs from Load in that dependent assemblies can be resolved outside from the 
			// BasePath.

			try
			{
				var assembly = Assembly.LoadFrom(testAssemblyPath);
				var drawingBackendType = tryLocateDrawingBackend(assembly);
				if (drawingBackendType == null)
					throw new Exception("Missing [DrawingBackend] attribute. Please add [assembly:DrawingBackend] to your test assembly.");

				var drawingBackend = (IDrawingBackend)Activator.CreateInstance(drawingBackendType);
				var classes = run(drawingBackend, assembly);
				return new TestResultAssembly(testAssemblyPath, classes);
			}
			catch (Exception e)
			{
				return new TestResultAssembly(testAssemblyPath, e);
			}
		}

		static Type tryLocateDrawingBackend(Assembly assembly)
		{
			var attribute = assembly.GetCustomAttributes(typeof (DrawingBackendAttribute), false);
			return attribute.Length == 0 ? null : ((DrawingBackendAttribute)attribute[0]).Type;
		}

		public TestResultClass[] run(IDrawingBackend drawingBackend, Assembly assembly)
		{
			var results = new List<TestResultClass>();

			foreach (var type in assembly.GetTypes())
			{
				var testMethods = type.GetMethods()
					.Where(mi => mi.GetCustomAttributes(typeof(BitmapDrawingTestAttribute), false).Length == 1)
					.ToArray();

				if (testMethods.Length == 0)
					continue;

				try
				{
					var methods = runClassTest(drawingBackend, type, testMethods);
					results.Add(new TestResultClass(type.Namespace, type.Name, methods));
				}
				catch (Exception e)
				{
					results.Add(new TestResultClass(type.Namespace, type.Name, e));
				}
			}

			return results.ToArray();
		}

		TestResultMethod[] runClassTest(IDrawingBackend drawingBackend, Type type, MethodInfo[] methods)
		{
			var constructor = type.GetConstructor(new Type[0]);
			if (constructor == null)
				throw new Exception("No constructor found for {0}".format(type));

			var instance = constructor.Invoke(null);

			try
			{
				return runMethodTests(drawingBackend, instance, methods);
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

		TestResultMethod[] runMethodTests(IDrawingBackend drawingBackend, object instance, MethodInfo[] methods)
		{
			var results = new List<TestResultMethod>();
			foreach (var method in methods)
			{
				var type = instance.GetType();
				var source = new TestSource(type.Namespace, type.Name, method.Name);
				try
				{

					var bitmap = runMethodTest(drawingBackend, instance, method);
					results.Add(new TestResultMethod(method.Name, bitmap));
				}
				catch (Exception e)
				{
					results.Add(new TestResultMethod(method.Name, e));
				}
			}

			return results.ToArray();
		}

		TestResultBitmap runMethodTest(IDrawingBackend drawingBackend, object instance, MethodInfo method)
		{
			if (method.IsGenericMethod)
				throw new Exception("{0}: is not allowed to be generic".format(method));

			var parameters = method.GetParameters();
			if (parameters.Length != 1)
				throw new Exception("{0}: expect one parameter".format(method));

			var firstParameter = parameters[0];
			if (firstParameter.ParameterType != typeof(IDrawingContext))
				throw new Exception("{0}: expect IDrawingContext as first and only parameter");

			var attribute = (BitmapDrawingTestAttribute)method.GetCustomAttributes(typeof (BitmapDrawingTestAttribute), false)[0];

			using (var context = drawingBackend.createBitmapDrawingContext(attribute.Width, attribute.Height))
			{
				IDrawingContext drawingContext;
				using (context.beginDraw(out drawingContext))
				{
					method.Invoke(instance, new object[] { drawingContext });
				}
	
				return new TestResultBitmap(attribute.Width, attribute.Height, context.extractRawBitmap());
			}
		}
	}
}
