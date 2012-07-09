using CrossUI.SharpDX.Drawing;

namespace CrossUI.SharpDX
{
	public sealed class DrawingBackend : IDrawingBackend
	{
		public IBitmapDrawingContext CreateBitmapDrawingContext(int width, int height)
		{
			return new BitmapDrawingContext(width, height);
		}
	}
}
