using System;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 60, Height = 60)]
	class CoordinateSpaceTests
	{
		public void Translation(IDrawingContext context)
		{
			drawOriginal(context);
			context.Translate(5, 5);
			drawTransformed(context);
		}

		public void Rotation(IDrawingContext context)
		{
			drawOriginal(context);
			context.Rotate(Math.PI/8, 30, 30);
			drawTransformed(context);
		}

		public void Scale(IDrawingContext context)
		{
			drawOriginal(context);
			context.Scale(0.75, 0.75, 30, 30);
			drawTransformed(context);
		}

		void drawOriginal(IDrawingContext context)
		{
			context.Rect(10, 15, 40, 30);
		}

		void drawTransformed(IDrawingContext context)
		{
			context.Stroke(color: new Color(1, 0.5, 0.5));
			context.Rect(10, 15, 40, 30);
		}
	}
}
