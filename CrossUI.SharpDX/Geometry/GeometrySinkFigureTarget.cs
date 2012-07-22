using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	sealed class GeometrySinkFigureTarget : IFigureTarget
	{
		readonly GeometrySink _sink;

		public GeometrySinkFigureTarget(GeometrySink sink)
		{
			_sink = sink;
		}
		public void LineTo(double x, double y)
		{
			_sink.AddLine(Import.Point(x, y));
		}

		public void ArcTo(double x,
			double y,
			double width,
			double height,
			double start,
			double stop,
			ArcDirection direction = ArcDirection.Clockwise)
		{
			var r = Import.Rectangle(x, y, width, height);
			var startPoint = ArcGeometry.pointOn(r, start);
			_sink.AddLine(startPoint);
			ArcGeometry.add(r, start, stop, _sink, direction.import());
		}

		public void BezierTo(double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			var bezier = new BezierSegment
			{
				Point1 = Import.Point(s1x, s1y),
				Point2 = Import.Point(s2x, s2y),
				Point3 = Import.Point(ex, ey)
			};

			_sink.AddBezier(bezier);
		}
	}
}
