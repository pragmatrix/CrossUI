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
	}

	public sealed class RoundedRectangleTest
	{
		[BitmapDrawingTest(Width=80, Height=40)]
		public void lightRect(IDrawingContext context)
		{
			context.roundedRect(0, 0, context.Width, context.Height, 8);
		}

		[BitmapDrawingTest(Width = 80, Height = 40)]
		public void thickRect(IDrawingContext context)
		{
			context.strokeWeight(3);
			context.roundedRect(0, 0, context.Width, context.Height, 8);
		}

		[BitmapDrawingTest(Width = 80, Height = 40)]
		public void coloredRect(IDrawingContext context)
		{
			context.stroke(1, 0, 0);
			context.roundedRect(0, 0, context.Width, context.Height, 8);
		}
	}
}
