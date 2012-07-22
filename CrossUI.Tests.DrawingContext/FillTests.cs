using System;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public class FillTests
	{
		void setup(IDrawingTarget target)
		{
			target.Fill(new Color(0, 0, 0));
			target.NoStroke();
		}

		public void RegularRect(IDrawingTarget target)
		{
			setup(target);
			target.Rectangle(0, 0, target.Width, target.Height);
		}

		public void RoundedRect(IDrawingTarget target)
		{
			setup(target);
			target.RoundedRectangle(0, 0, 80, 40, 8);
		}

		public void Ellipse(IDrawingTarget target)
		{
			setup(target);
			target.Ellipse(0, 0, target.Width, target.Height);
		}

		public void Arc(IDrawingTarget target)
		{
			setup(target);
			const double pi = Math.PI;
			target.Arc(0, 0, target.Width, target.Height, 0, pi / 2);
			target.Arc(0, 0, target.Width, target.Height, pi, pi * 1.5);
		}

		public void Bezier(IDrawingTarget target)
		{
			setup(target);
			target.Bezier(0, 0, target.Width, 0, 0, target.Height, target.Width, target.Height);
		}

		public void Polygon(IDrawingTarget target)
		{
			setup(target);
			target.Polygon(new double[] { 0, 0, 80, 20, 20, 40 });
		}
	}
}
