using System;
using CrossUI.Toolbox;
using SharpDX;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	sealed class GeometryImplementation : IGeometry
	{
		public readonly PathGeometry Geometry;

		public GeometryImplementation(PathGeometry geometry)
		{
			Geometry = geometry;
		}

		public void Dispose()
		{
			Geometry.Dispose();
		}

		public Bounds Bounds
		{
			get
			{
				return Geometry.GetBounds().export();
			} 
		}

		public IGeometry Combine(CombineMode mode, IGeometry other)
		{
			var otherImplementation = other.import();

			var combined = Path.Geometry(
				Geometry.Factory, 
				sink => checkResult(Geometry.Combine(otherImplementation, mode.import(), sink)));
		
			return new GeometryImplementation(combined);
		}

		static void checkResult(Result rc)
		{
			if (rc != 0)
				throw new Exception("Direct2D error {0}".format(rc));
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

