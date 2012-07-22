using System;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	sealed class GeometryTarget : IGeometryTarget
	{
		readonly Factory _factory;
		readonly GeometrySink _sink;

		const FigureBegin FBegin = FigureBegin.Hollow;
		const GeometrySimplificationOption SimplificationOption = GeometrySimplificationOption.CubicsAndLines;
		bool _figure;

		public GeometryTarget(Factory factory, GeometrySink sink)
		{
			_factory = factory;
			_sink = sink;
		}

		public void Line(double x1, double y1, double x2, double y2)
		{
			End();

			_sink.BeginFigure(Import.Point(x1, y1), FBegin);
			_sink.AddLine(Import.Point(x2, y2));
			_sink.EndFigure(FigureEnd.Open);
		}


		public void Rectangle(double x, double y, double width, double height)
		{
			End();

			using (var geometry = new RectangleGeometry(_factory, Import.Rectangle(x, y, width, height)))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void RoundedRectangle(double x, double y, double width, double height, double cornerRadius)
		{
			End();

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
			End();

			if ((pairs.Length & 1) == 1)
				throw new Exception("Number of polygon pairs need to be even.");

			if (pairs.Length < 4)
				return;

			if (pairs.Length == 4)
			{
				Line(pairs[0], pairs[1], pairs[2], pairs[3]);
				return;
			}

			_sink.BeginFigure(Import.Point(pairs[0], pairs[1]), FBegin);

			for (int i = 2; i != pairs.Length; i += 2)
				_sink.AddLine(Import.Point(pairs[i], pairs[i + 1]));

			_sink.EndFigure(FigureEnd.Closed);
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			End();

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
			End();

			var r = Import.Rectangle(x, y, width, height);
			_sink.BeginFigure(ArcGeometry.pointOn(r, start), FBegin);
			ArcGeometry.add(Import.Rectangle(x, y, width, height), start, stop, _sink);
			_sink.EndFigure(FigureEnd.Open);
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			End();
			_sink.BeginFigure(Import.Point(x, y), FBegin);

			_sink.AddBezier(new BezierSegment
			{
				Point1 = Import.Point(s1x, s1y),
				Point2 = Import.Point(s2x, s2y),
				Point3 = Import.Point(ex, ey)
			});

			_sink.EndFigure(FigureEnd.Open);
		}

		public void MoveTo(double x, double y)
		{
			End();
			_sink.BeginFigure(Import.Point(x, y), FBegin);
			_figure = true;
		}

		public void Close()
		{
			End(FigureEnd.Closed);
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

		public void End(FigureEnd end = FigureEnd.Open)
		{
			if (!_figure)
				return;
			_sink.EndFigure(end);
			_figure = false;
		}
	}
}
