using System;

namespace CrossUI
{
	public interface IBitmapDrawingContext : IDisposable
	{
		IDisposable BeginDraw(out IDrawingTarget drawingTarget);
		byte[] ExtractRawBitmap();
	}
}
