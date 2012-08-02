using System;

namespace CrossUI
{
	public interface IDrawingSurface : IDisposable
	{
		IDrawingTarget BeginDraw();

		/// Premultiplied BGRA
		byte[] ExtractRawBitmap();
	}
}
