namespace CrossUI.Tests
{
	namespace MyDrawings
	{
		public class RoundedRectangleTest
		{
			[BitmapDrawingTest(Width = 80, Height = 40)]
			public void RoundedRect(IDrawingTarget target)
			{
				target.RoundedRectangle(0, 0, 80, 40, 8);
			}
		}
	}
}
