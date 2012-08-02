namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public class PixelAlignmentTests
	{
		public void CrispScaledRectangle(IDrawingTarget target)
		{
			target.PixelAlign();
			target.Scale(0.75, 0.75, 40, 20);
			target.Rectangle(10, 5, 60, 30);
		}

		public void CrispLine(IDrawingTarget target)
		{
			target.PixelAlign();
			target.Line(10, 20, 70, 20);
		}

		public void CrispTranslatedLine(IDrawingTarget target)
		{
			target.PixelAlign();
			target.Translate(10, 20);
			target.Line(0, 0, 60, 0);
		}
	}
}
