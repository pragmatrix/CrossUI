using CrossUI;

[assembly: DrawingBackend(typeof(CrossUI.SharpDX.DrawingBackend))]

namespace CrossUI.Tests
{

	public sealed class RoundedRectangleTest
	{
		[BitmapDrawingTest(Width=80, Height=40)]
		public void roundedRect(IDrawingContext context)
		{
			context.roundedRect(0, 0, 80, 40, 8);
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
