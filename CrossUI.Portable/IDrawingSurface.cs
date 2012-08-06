using System;

namespace CrossUI
{
	public interface IDrawingSurface : IDisposable
	{
		IDrawingTarget BeginDraw(Color? clearColor = null);

		/// Premultiplied BGRA
		byte[] ExtractRawBitmap();
	}
}
