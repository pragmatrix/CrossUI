using CrossUI;

[assembly: DrawingBackend(typeof(CrossUI.SharpDX.DrawingBackend))]

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public sealed class DrawingContextTest
	{
		public void RoundedRect(IDrawingContext context)
		{
			context.RoundedRect(0, 0, 80, 40, 8);
		}

		public void ThickRect(IDrawingContext context)
		{
			context.StrokeWeight(3);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		public void ColoredRect(IDrawingContext context)
		{
			context.Stroke(1, 0, 0);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}
	}
}
