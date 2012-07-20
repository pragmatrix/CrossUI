using System;

namespace CrossUI
{
	public interface IBitmapDrawingTarget : IDisposable
	{
		IDisposable BeginDraw(out IDrawingTarget drawingTarget);
		byte[] ExtractRawBitmap();
	}
}
