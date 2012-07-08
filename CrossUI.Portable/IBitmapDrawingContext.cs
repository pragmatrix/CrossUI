using System;

namespace CrossUI
{
	public interface IBitmapDrawingContext : IDisposable
	{
		IDisposable beginDraw(out IDrawingContext drawingContext);
		byte[] extractRawBitmap();
	}
}
