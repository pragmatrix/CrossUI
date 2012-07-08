using System;
using CrossUI;

[assembly: DrawingBackend(typeof(CrossUI.SharpDX.DrawingBackend))]

namespace CrossUI.Tests
{
	public sealed class RoundedRectangleTest
	{
		[BitmapDrawingTest(Width=80, Height=40)]
		public void RoundedRect(IDrawingContext context)
		{
			context.RoundedRect(0, 0, 80, 40, 8);
		}

		[BitmapDrawingTest(Width = 80, Height = 40)]
		public void ThickRect(IDrawingContext context)
		{
			context.StrokeWeight(3);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		[BitmapDrawingTest(Width = 80, Height = 40)]
		public void ColoredRect(IDrawingContext context)
		{
			context.Stroke(1, 0, 0);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
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

	public sealed class PrivateConstructor
	{
		private PrivateConstructor()
		{
		}

		[BitmapDrawingTest]
		public void NeverTested()
		{
		}
	}
}
