using System;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public class FillAndStrokeTests
	{
		void setup(IDrawingContext context)
		{
			context.Fill(new Color(0, 1, 0));
			context.Stroke(new Color(1, 0, 0), weight: 2);
		}

		public void RegularRect(IDrawingContext context)
		{
			setup(context);
			context.Rect(0, 0, context.Width, context.Height);
		}

		public void RoundedRect(IDrawingContext context)
		{
			setup(context);
			context.RoundedRect(0, 0, 80, 40, 8);
		}

		public void Ellipse(IDrawingContext context)
		{
			setup(context);
			context.Ellipse(0, 0, context.Width, context.Height);
		}

		public void Arc(IDrawingContext context)
		{
			setup(context);
			const double pi = Math.PI;
			context.Arc(0, 0, context.Width, context.Height, 0, pi / 2);
			context.Arc(0, 0, context.Width, context.Height, pi, pi * 1.5);
		}

		public void Bezier(IDrawingContext context)
		{
			setup(context);
			context.Bezier(0, 0, context.Width, 0, 0, context.Height, context.Width, context.Height);
		}
	}
}
