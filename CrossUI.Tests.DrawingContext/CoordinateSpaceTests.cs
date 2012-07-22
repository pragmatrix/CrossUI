using System;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 60, Height = 60)]
	class CoordinateSpaceTests
	{
		public void Translation(IDrawingTarget target)
		{
			drawOriginal(target);
			target.Translate(5, 5);
			drawTransformed(target);
		}

		public void Rotation(IDrawingTarget target)
		{
			drawOriginal(target);
			target.Rotate(Math.PI/8, 30, 30);
			drawTransformed(target);
		}

		public void Scale(IDrawingTarget target)
		{
			drawOriginal(target);
			target.Scale(0.75, 0.75, 30, 30);
			drawTransformed(target);
		}

		void drawOriginal(IDrawingTarget target)
		{
			target.Rectangle(10, 15, 40, 30);
		}

		void drawTransformed(IDrawingTarget target)
		{
			target.Stroke(color: new Color(1, 0.5, 0.5));
			target.Rectangle(10, 15, 40, 30);
		}
	}
}
