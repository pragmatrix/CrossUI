using System;
using SharpDX.Direct2D1;

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

		public void Line(double x1, double y1, double x2, double y2)
		{
			end();

			_sink.BeginFigure(Import.Point(x1, y1), FigureBegin.Hollow);
			_sink.AddLine(Import.Point(x2, y2));
			_sink.EndFigure(FigureEnd.Open);
		}

		public void Rectangle(double x, double y, double width, double height)
		{
			end();

			using (var geometry = new RectangleGeometry(_factory, Import.Rectangle(x, y, width, height)))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void RoundedRectangle(double x, double y, double width, double height, double cornerRadius)
		{
			end();

			using (var geometry = new RoundedRectangleGeometry(_factory,
				new RoundedRect
				{
					RadiusX = cornerRadius.import(),
					RadiusY = cornerRadius.import(),
					Rect = Import.Rectangle(x, y, width, height)
				}))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void Polygon(double[] pairs)
		{
			end();

			if ((pairs.Length & 1) == 1)
				throw new Exception("Number of polygon pairs need to be even.");

			if (pairs.Length < 4)
				return;

			if (pairs.Length == 4)
			{
				Line(pairs[0], pairs[1], pairs[2], pairs[3]);
				return;
			}

			_sink.BeginFigure(Import.Point(pairs[0], pairs[1]), FigureBegin.Filled);

			for (int i = 2; i != pairs.Length; i += 2)
				_sink.AddLine(Import.Point(pairs[i], pairs[i + 1]));

			_sink.EndFigure(FigureEnd.Closed);
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			end();

			var rx = width/2;
			var ry = height/2;

			using (var geometry = new EllipseGeometry(_factory,
				new Ellipse
				{
					Point = Import.Point(x + rx, y + ry),
					RadiusX = rx.import(),
					RadiusY = ry.import()
				}
				))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			end();

			var r = Import.Rectangle(x, y, width, height);
			_sink.BeginFigure(ArcGeometry.pointOn(r, start), FigureBegin.Filled);
			ArcGeometry.add(Import.Rectangle(x, y, width, height), start, stop, _sink);
			_sink.EndFigure(FigureEnd.Closed);
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			end();
			_sink.BeginFigure(Import.Point(x, y), FigureBegin.Filled);

			_sink.AddBezier(new BezierSegment
			{
				Point1 = Import.Point(s1x, s1y),
				Point2 = Import.Point(s2x, s2y),
				Point3 = Import.Point(ex, ey)
			});

			_sink.EndFigure(FigureEnd.Closed);
		}

		public void MoveTo(double x, double y)
		{
			end();
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
			end(FigureEnd.Closed);
		}

		public void end(FigureEnd end = FigureEnd.Open)
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
