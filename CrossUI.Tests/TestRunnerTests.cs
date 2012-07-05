using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;

namespace CrossUI.Tests
{
	[TestFixture]
	public class TestRunnerTests
	{
		[Test]
		public void runTestsDirectly()
		{
			var testRunner = new Testing.TestRunner();
			var results = testRunner.run(Assembly.GetExecutingAssembly());
			Assert.That(results.Length == 1);
		}

#if false

		// this isn't testable for now, because the testrunner shadow copies the assemblies.
		[Test]
		public void runTestInDifferentAppDomain()
		{
			var assemblyPath = Assembly.GetExecutingAssembly().Location;
			var testRunner = new DomainTestRunner(assemblyPath);
			testRunner.run();
		}

#endif
	}

	public sealed class RoundedRectangleTest
	{
		[BitmapDrawingTest(Width=80, Height=40)]
		public void test(IDrawingContext context)
		{
			Debug.Assert(context.Width == 80);
			Debug.Assert(context.Height == 40);

			context.roundedRect(0, 0, context.Width, context.Height, 8);
		}
	}
}
