using System;
using CrossUI.Geometry;

namespace CrossUI
{
	public interface IDrawingBackend : IDisposable
	{
		IBitmapDrawingTarget CreateBitmapDrawingTarget(int width, int height);

		IGeometry Geometry(IRecorder<IGeometryTarget> records);
	}

	public static class DrawingBackendExtensions
	{
		public static IGeometry Geometry(this IDrawingBackend backend, Action<IGeometryTarget> geometryBuilder)
		{
			var recorder = new GeometryTargetRecorder();
			geometryBuilder(recorder);
			return backend.Geometry(recorder);
		}

		#region Simple Geometry

		public static IGeometry Line(this IDrawingBackend backend, double x1, double y1, double x2, double y2)
		{
			return backend.Geometry(gt => gt.Line(x1, y1, x2, y2));
		}

		public static IGeometry Polygon(this IDrawingBackend backend, params double[] coordinatePairs)
		{
			return backend.Geometry(gt => gt.Polygon(coordinatePairs));
		}

		public static IGeometry Rectangle(this IDrawingBackend backend, double x, double y, double width, double height)
		{
			return backend.Geometry(gt => gt.Rectangle(x, y, width, height));
		}

		public static IGeometry RoundedRectangle(this IDrawingBackend backend, double x, double y, double width, double height, double cornerRadius)
		{
			return backend.Geometry(gt => gt.RoundedRectangle(x, y, width, height, cornerRadius));
		}

		public static IGeometry Ellipse(this IDrawingBackend backend, double x, double y, double width, double height)
		{
			return backend.Geometry(gt => gt.Ellipse(x, y, width, height));
		}

		#endregion
	}
}
