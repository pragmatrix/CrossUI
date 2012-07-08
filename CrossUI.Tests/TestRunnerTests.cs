using System;
using CrossUI;

[assembly: DrawingBackend(typeof(CrossUI.SharpDX.DrawingBackend))]

namespace CrossUI.Tests
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public sealed class RoundedRectangleTest
	{
		public void RoundedRect(IDrawingContext context)
		{
			context.RoundedRect(0, 0, 80, 40, 8);
		}

		public void ThickRect(IDrawingContext context)
		{
			context.Stroke(weight: 3);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		public void ColoredRect(IDrawingContext context)
		{
			context.Stroke(new Color(1, 0, 0));
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		[BitmapDrawingTest(Width = 120)]
		public void LargerRect(IDrawingContext context)
		{
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		public void ShouldBeIgnored()
		{
		}
	}

	public sealed class MethodErrorTest
	{
		[BitmapDrawingTest(Width=80,Height=40)]
		public void Error(IDrawingContext context)
		{
			throw new Exception("Method Error");
		}

		[BitmapDrawingTest]
		public void InvalidParameters()
		{
		}
	}

	public sealed class PrivateConstructorTest
	{
		private PrivateConstructorTest()
		{
		}

		[BitmapDrawingTest]
		public void ShouldBeIgnored()
		{
		}
	}
}
