using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrossUI.Drawing;
using CrossUI.Toolbox;

namespace CrossUI.Testing
{
	public sealed class TestRunner : MarshalByRefObject
	{
		public TestResult[] run(Assembly assembly)
		{
			var results = new List<TestResult>();

			foreach (var type in assembly.GetTypes())
			{
				var testMethods = type.GetMethods()
					.Where(mi => mi.GetCustomAttributes(typeof(BitmapDrawingTestAttribute), false).Length == 1)
					.ToArray();

				var typeTests = runType(type, testMethods);
				results.AddRange(typeTests);
			}

			return results.ToArray();
		}

		TestResult[] runType(Type type, MethodInfo[] methods)
		{
			return runInstanceTests(type, methods);
		}

		TestResult[] runInstanceTests(Type type, MethodInfo[] methods)
		{
			var constructor = type.GetConstructor(new Type[0]);
			if (constructor == null)
				throw new TestException("No constructor found for {0}".format(type));

			var instance = constructor.Invoke(null);

			try
			{
				return runMethodTests(instance, methods);
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

		TestResult[] runMethodTests(object instance, MethodInfo[] methods)
		{
			var results = new List<TestResult>();
			foreach (var method in methods)
			{
				var source = new TestSource(instance.GetType(), method);
				try
				{

					var bitmap = runMethodTest(instance, method);
					results.Add(new TestResult(source, bitmap));
				}
				catch (Exception e)
				{
					results.Add(new TestResult(source, e));
				}
			}

			return results.ToArray();
		}

		TestResultBitmap runMethodTest(object instance, MethodInfo method)
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

			using (var context = new BitmapDrawingContext(attribute.Width, attribute.Height))
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
