using System;
using SharpDX.Direct2D1;
using CrossUI.Drawing;

namespace CrossUI.SharpDX.Geometry
{
	sealed class GeometryTarget : IGeometryTarget
	{
		readonly Factory _factory;
		readonly GeometrySink _sink;

		const GeometrySimplificationOption SimplificationOption = GeometrySimplificationOption.CubicsAndLines;

		double _startX;
		double _startY;
		FigureTargetRecorder _figure_;

		public GeometryTarget(Factory factory, GeometrySink sink)
		{
			_factory = factory;
			_sink = sink;
		}

		public void Line(Point p1, Point p2)
		{
			endOpenFigure();

			_sink.BeginFigure(Import.Point(p1), FigureBegin.Hollow);
			_sink.AddLine(Import.Point(p2));
			_sink.EndFigure(FigureEnd.Open);
		}

		public void Rectangle(Rectangle rectangle)
		{
			endOpenFigure();

			using (var geometry = new RectangleGeometry(_factory, Import.Rectangle(rectangle)))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void RoundedRectangle(Rectangle rectangle, Size cornerRadius)
		{
			endOpenFigure();

			using (var geometry = new RoundedRectangleGeometry(_factory,
				new RoundedRectangle
				{
					RadiusX = cornerRadius.Width.import(),
					RadiusY = cornerRadius.Height.import(),
					Rect = Import.Rectangle(rectangle)
				}))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void Polygon(Point[] points)
		{
			endOpenFigure();

			if (points.Length < 2)
				return;

			if (points.Length == 2)
			{
				Line(points[0], points[1]);
				return;
			}

			_sink.BeginFigure(Import.Point(points[0]), FigureBegin.Filled);

			for (int i = 1; i != points.Length; ++i)
				_sink.AddLine(Import.Point(points[i]));

			_sink.EndFigure(FigureEnd.Closed);
		}

		public void Ellipse(Rectangle rectangle)
		{
			endOpenFigure();

			var rx = rectangle.Width/2;
			var ry = rectangle.Height/2;

			using (var geometry = new EllipseGeometry(_factory,
				new Ellipse
				{
					Point = Import.Point(rectangle.X + rx, rectangle.Y + ry),
					RadiusX = rx.import(),
					RadiusY = ry.import()
				}
				))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void Arc(Rectangle rectangle, double start, double stop)
		{
			endOpenFigure();

			var r = Import.Rectangle(rectangle);
			_sink.BeginFigure(ArcGeometry.pointOn(r, start), FigureBegin.Hollow);
			ArcGeometry.add(r, start, stop, _sink);
			_sink.EndFigure(FigureEnd.Open);
		}

		public void Bezier(CubicBezier bezier)
		{
			endOpenFigure();

			_sink.BeginFigure(Import.Point(bezier.Start), FigureBegin.Hollow);

			_sink.AddBezier(new BezierSegment
			{
				Point1 = Import.Point(bezier.Span1),
				Point2 = Import.Point(bezier.Span2),
				Point3 = Import.Point(bezier.End)
			});

			_sink.EndFigure(FigureEnd.Open);
		}

		public void MoveTo(double x, double y)
		{
			endOpenFigure();

			_startX = x;
			_startY = y;
			_figure_ = new FigureTargetRecorder();
		}

		public void LineTo(double x, double y)
		{
			if (_figure_ == null)
				throw new Exception("LineTo() requires an open figure");

			_figure_.LineTo(x, y);
		}

		public void ArcTo(double x,
			double y,
			double width,
			double height,
			double start,
			double stop,
			ArcDirection direction)
		{
			if (_figure_ == null)
				throw new Exception("ArcTo() requires an open figure");

			_figure_.ArcTo(x, y, width, height, start, stop, direction);
		}

		public void BezierTo(double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			if (_figure_ == null)
				throw new Exception("BezierTo() requires an open figure");

			_figure_.BezierTo(s1x, s1y, s2x, s2y, ex, ey);
		}

		public void Close()
		{
			endOpenFigure(FigureEnd.Closed);
		}

		public void endOpenFigure(FigureEnd end = FigureEnd.Open)
		{
			if (_figure_ == null)
				return;

			var fill = end == FigureEnd.Open ? FigureBegin.Hollow : FigureBegin.Filled;
			_sink.BeginFigure(Import.Point(_startX, _startY), fill);
			var sinkTarget = new GeometrySinkFigureTarget(_sink);
			_figure_.Replay(sinkTarget);
			_sink.EndFigure(end);
			_figure_ = null;
		}
	}
}
