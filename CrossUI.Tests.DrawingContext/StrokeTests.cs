using System;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public sealed class StrokeTests
	{
		public void RoundedRect(IDrawingContext context)
		{
			context.RoundedRect(0, 0, 80, 40, 8);
		}

		public void ThickRoundedRect(IDrawingContext context)
		{
			context.Stroke(weight: 3);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		public void ColoredRoundedRect(IDrawingContext context)
		{
			context.Stroke(new Color(1, 0, 0));
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		public void OutsideAligned(IDrawingContext context)
		{
			context.Stroke(alignment: StrokeAlignment.Outside);
			context.RoundedRect(0, 0, context.Width, context.Height, 8);
		}

		public void Line(IDrawingContext context)
		{
			context.Line(1, 1, context.Width-1, context.Height-1);
		}

		public void ThickLine(IDrawingContext context)
		{
			context.Stroke(weight: 5);
			context.Line(10, 10, context.Width - 10, context.Height - 10);
		}

		public void RegularRect(IDrawingContext context)
		{
			context.Rect(0, 0, context.Width, context.Height);
		}

		public void Ellipse(IDrawingContext context)
		{
			context.Ellipse(0, 0, context.Width, context.Height);
		}

		public void Arc(IDrawingContext context)
		{
			const double pi = Math.PI;
			context.Arc(0, 0, context.Width, context.Height, 0, pi/2);
			context.Arc(0, 0, context.Width, context.Height, pi, pi *1.5);
		}

		public void Bezier(IDrawingContext context)
		{
			context.Bezier(0, 0, context.Width, 0, 0, context.Height, context.Width, context.Height);
		}

		public void Polygon(IDrawingContext context)
		{
			context.Polygon(new double[] { 0, 0, 80, 20, 20, 40 });
		}
	}
}
