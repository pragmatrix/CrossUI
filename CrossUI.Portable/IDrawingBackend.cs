using System;
using CrossUI.Geometry;

namespace CrossUI
{
	public interface IDrawingBackend : IDisposable
	{
		IBitmapDrawingTarget CreateBitmapDrawingTarget(int width, int height);
		IGeometry CreateGeometry(IRecorder<IGeometryTarget> records);
	}

	public static class DrawingBackendExtensions
	{
		public static IGeometry CreateGeometry(this IDrawingBackend backend, Action<IGeometryTarget> geometryBuilder)
		{
			var recorder = new GeometryTargetRecorder();
			geometryBuilder(recorder);
			return backend.CreateGeometry(recorder);
		}
	}
}
