using System;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	sealed class GeometryTarget : IGeometryTarget
	{
		readonly Factory _factory;
		readonly GeometrySink _sink;

		const FigureBegin FBegin = FigureBegin.Hollow;
		const GeometrySimplificationOption SimplificationOption	= GeometrySimplificationOption.CubicsAndLines;

		public GeometryTarget(Factory factory, GeometrySink sink)
		{
			_factory = factory;
			_sink = sink;
		}

		public void Line(double x1, double y1, double x2, double y2)
		{
			_sink.BeginFigure(Import.Point(x1, y1), FBegin);
			_sink.AddLine(Import.Point(x2, y2));
			_sink.EndFigure(FigureEnd.Open);
		}


		public void Rect(double x, double y, double width, double height)
		{
			using (var geometry = new RectangleGeometry(_factory, Import.Rectangle(x, y, width, height)))
			{
				geometry.Simplify(SimplificationOption, _sink);
			}
		}

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
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
			var rx = width/2;
			var ry = height/2;
			using (var geometry = new EllipseGeometry(_factory, new Ellipse
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
			var r = Import.Rectangle(x, y, width, height);
			_sink.BeginFigure(ArcGeometry.pointOn(r, start), FBegin);
			ArcGeometry.add(Import.Rectangle(x, y, width, height), start, stop, _sink);
			_sink.EndFigure(FigureEnd.Open);
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
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
			throw new NotImplementedException();
		}

		public void Close()
		{
			throw new NotImplementedException();
		}

		public void LineTo(double x, double y)
		{
			throw new NotImplementedException();
		}

		public void ArcTo(double centerX, double centerY, double width, double height, double start, double stop, ArcDirection direction = ArcDirection.Clockwise)
		{
			throw new NotImplementedException();
		}

		public void BezierTo(double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			throw new NotImplementedException();
		}
	}

#if false

namespace BrainSharper.Graphics
{
	public sealed class NativeGeometry : IGeometry
	{
		public readonly Geometry Geometry;

		public NativeGeometry(Geometry geometry)
		{
			Geometry = geometry;
		}

		public IGeometry inflated(double inflate)
		{
			return inflated(this, inflate);
		}

		public IGeometry union(IGeometry other)
		{
			return union(this, other);
		}

		public bool intersectsWith(IGeometry other)
		{
			return intersectsWith(this, other);
		}

		public double? tryIntersectWithBezier(CubicBezier bezier, BezierEnd end)
		{
			return tryIntersectWithBezier(this, bezier, end);
		}

		const float DefaultTolerance = 1.0f;

		static readonly Factory Factory = new Factory(FactoryType.MultiThreaded);

		static PathGeometry makePath(Action<GeometrySink> init)
		{
			var path = new PathGeometry(Factory);
			var sink = path.Open();
			init(sink);
			sink.Close();
			return path;
		}


		static void checkResult(Result rc, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
		{
			if (rc != 0)
				throw new Exception("result code {0} at line {1} in {2}".format(rc, line, member));
		}

		public IGeometry inflated(IGeometry geometry, double inflate)
		{
			var native = geometry.export();

			var widened = makePath(sink => checkResult(native.Widen((float) (inflate*2), DefaultTolerance, sink)));
			var outlined = makePath(sink => checkResult(native.Outline(DefaultTolerance, sink)));
			var combined = makePath(sink => checkResult(widened.Combine(outlined, CombineMode.Union, DefaultTolerance, sink)));
			var combinedOutline = makePath(sink => checkResult(combined.Outline(DefaultTolerance, sink)));

			return combinedOutline.import();
		}

		public IGeometry union(IGeometry geometry, IGeometry other)
		{
			var native1 = geometry.export();
			var native2 = other.export();

			var union = makePath(sink => checkResult(native1.Combine(native2, CombineMode.Union, DefaultTolerance, sink)));
			return union.import();
		}

		public bool intersectsWith(IGeometry geometry, IGeometry other)
		{
			var native1 = geometry.export();
			var native2 = other.export();

			return native1.Compare(native2, DefaultTolerance) != GeometryRelation.Disjoint;
		}

		public double? tryIntersectWithBezier(IGeometry geometry, CubicBezier bezier, BezierEnd end)
		{
			var native = geometry.export();

			Func<G.Point, bool> isInside = p =>
				{
					var dp = new DrawingPointF((float) p.X, (float) p.Y);
					return native.FillContainsPoint(dp, DefaultTolerance);
				};

			return Intersect.bezierEnd(bezier, end, isInside);
		}

		public static IGeometry createRoundedRectangle(G.Rect rect, double radius)
		{
			var r = new RectangleF((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);

			var roundedRect = new RoundedRect() {RadiusX = (float)radius, RadiusY = (float)radius, Rect = r};
			return new RoundedRectangleGeometry(Factory, roundedRect).import();
		}

		public static IGeometry createEllipse(G.Rect rect)
		{
			var center = rect.centerOf();
			var ellipse = new Ellipse(new DrawingPointF((float) center.X, (float) center.Y), (float) rect.Width/2,
			                          (float) rect.Height/2);

			return new EllipseGeometry(Factory, ellipse).import();
		}

		public static IGeometry createPolygon(IEnumerable<G.Point> points)
		{
			var path = makePath(sink =>
				{
					using (var enumerator = points.GetEnumerator())
					{
						if (!enumerator.MoveNext())
							return;

						var start = enumerator.Current;
						sink.BeginFigure(drawingPointOf(start), FigureBegin.Filled);

						while (enumerator.MoveNext())
						{
							var next = enumerator.Current;
							sink.AddLine(drawingPointOf(next));
						}

						sink.EndFigure(FigureEnd.Closed);
					}
				});

			return path.import();
		}

		static DrawingPointF drawingPointOf(G.Point p)
		{
			return new DrawingPointF((float)p.X, (float)p.Y);
		}

	}
#endif
}
