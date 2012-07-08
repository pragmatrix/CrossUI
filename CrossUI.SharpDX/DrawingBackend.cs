using CrossUI.SharpDX.Drawing;

namespace CrossUI.SharpDX
{
	public sealed class DrawingBackend : IDrawingBackend
	{
		public IBitmapDrawingContext createBitmapDrawingContext(int width, int height)
		{
			return new BitmapDrawingContext(width, height);
		}
	}
}
