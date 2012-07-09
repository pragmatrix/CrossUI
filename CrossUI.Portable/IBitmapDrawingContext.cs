using System;

namespace CrossUI
{
	public interface IBitmapDrawingContext : IDisposable
	{
		IDisposable BeginDraw(out IDrawingContext drawingContext);
		byte[] ExtractRawBitmap();
	}
}
