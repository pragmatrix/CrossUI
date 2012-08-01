using System;

namespace CrossUI
{
	public interface IDrawingSurface : IDisposable
	{
		IDrawingTarget BeginDraw();
		byte[] ExtractRawBitmap();
	}
}
