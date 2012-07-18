using System;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public sealed class StrokeTests
	{
		public void RoundedRect(IDrawingTarget target)
		{
			target.RoundedRect(0, 0, 80, 40, 8);
		}

		public void ThickRoundedRect(IDrawingTarget target)
		{
			target.Stroke(weight: 3);
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
		}

		public void ColoredRoundedRect(IDrawingTarget target)
		{
			target.Stroke(new Color(1, 0, 0));
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
		}

		public void OutsideAligned(IDrawingTarget target)
		{
			target.Stroke(alignment: StrokeAlignment.Outside);
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
		}

		public void Line(IDrawingTarget target)
		{
			target.Line(1, 1, target.Width-1, target.Height-1);
		}

		public void ThickLine(IDrawingTarget target)
		{
			target.Stroke(weight: 5);
			target.Line(10, 10, target.Width - 10, target.Height - 10);
		}

		public void RegularRect(IDrawingTarget target)
		{
			target.Rect(0, 0, target.Width, target.Height);
		}

		public void Ellipse(IDrawingTarget target)
		{
			target.Ellipse(0, 0, target.Width, target.Height);
		}

		public void Arc(IDrawingTarget target)
		{
			const double pi = Math.PI;
			target.Arc(0, 0, target.Width, target.Height, 0, pi/2);
			target.Arc(0, 0, target.Width, target.Height, pi, pi *1.5);
		}

		public void Bezier(IDrawingTarget target)
		{
			target.Bezier(0, 0, target.Width, 0, 0, target.Height, target.Width, target.Height);
		}

		public void Polygon(IDrawingTarget target)
		{
			target.Polygon(new double[] { 0, 0, 80, 20, 20, 40 });
		}
	}
}
