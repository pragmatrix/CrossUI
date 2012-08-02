using System;

namespace CrossUI.Drawing
{
	public sealed class PixelAligningDrawingTarget : IGeometryFigures, IDrawingElements
	{
		readonly IDrawingTarget _target;
		readonly DrawingState _state;
		readonly DrawingTransform _transform;

		bool _hasTransformationThatCanPixelAligned;
		Vector _scaleCache;
		Vector _translationCache;

		public static IDrawingTarget Create(IDrawingTarget target, Action disposer, DrawingState state, DrawingTransform transform)
		{
			var pixelAligner = new PixelAligningDrawingTarget(target, state, transform);

			return new DrawingTargetSplitter
				(
					target.Backend, 
					state, 
					transform,
					pixelAligner, 
					pixelAligner, 
					target, 
					target, 
					target, () =>
						{
							pixelAligner.Dispose();
							disposer();
						}
					);
		}

		PixelAligningDrawingTarget(IDrawingTarget target, DrawingState state, DrawingTransform transform)
		{
			_target = target;
			_state = state;
			_transform = transform;

			_transform.Changed += transformChanged;

			transformChanged();
		}

		public void Dispose()
		{
			_transform.Changed -= transformChanged;
		}

		void transformChanged()
		{
			var m = _transform.Current;
			_hasTransformationThatCanPixelAligned = canPixelAlign(m);

			_scaleCache = new Vector(m.M11, m.M22);
			_translationCache = new Vector(m.M31, m.M32);
		}

		static bool canPixelAlign(Matrix m)
		{
			// scaled and / or translated!
			return m.M12 == 0.0 && m.M21 == 0.0;
		}

		bool ShouldPixelAlignNow
		{
			get { return _hasTransformationThatCanPixelAligned && _state.PixelAligned; }
		}

		#region Pixel Aligning Forwarders

		public void Line(double x1, double y1, double x2, double y2)
		{
			var p1 = pixelAlign(x1, y1);
			var p2 = pixelAlign(x2, y2);

			_target.Line(p1, p2);
		}

		public void Rectangle(double x, double y, double width, double height)
		{
			var r = pixelAlign(x, y, width, height);

			_target.Rectangle(r);
		}

		public void RoundedRectangle(double x, double y, double width, double height, double cornerRadius)
		{
			var r = pixelAlign(x, y, width, height);

			_target.RoundedRectangle(r, cornerRadius);
		}

		public void Polygon(params double[] coordinatePairs)
		{
			var aligned = coordinatePairs.Transform(pixelAlign);
			_target.Polygon(aligned);
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			var r = pixelAlign(x, y, width, height);
			_target.Ellipse(r);
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			var r = pixelAlign(x, y, width, height);
			_target.Arc(r, start, stop);
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			var start = pixelAlign(x, y);
			var span1 = pixelAlign(s1x, s1y);
			var span2 = pixelAlign(s2x, s2y);
			var end = pixelAlign(ex, ey);

			_target.Bezier(start, span1, span2, end);
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			// text is _not_ aligned right, because it would change width and so could affect word-breaks.
			_target.Text(text, x, y, width, width);
		}

		public void Geometry(IGeometry geometry)
		{
			// also not aligned... needs to be aligned when the geometry is created.
			_target.Geometry(geometry);
		}

		#endregion

		Point pixelAlign(double x, double y)
		{
			return pixelAlign(new Point(x, y));
		}

		Rectangle pixelAlign(double x, double y, double width, double height)
		{
			return pixelAlign(new Rectangle(x, y, width, height));
		}

		/*
			A single coordinate (of a line, spline, etc.) is aligned to 
			the center! of a pixel.
		*/

		Point pixelAlign(Point p)
		{
			if (!ShouldPixelAlignNow)
				return p;

			var pixels = toPixel(p.Vector);

			var shifted = pixels + HalfAPixel;
			var floored = shifted.Floor();
			var final = floored + HalfAPixel;

			var transformedBack = toLogical(final);
			return transformedBack.ToPoint();
		}

		static readonly Vector HalfAPixel = new Vector(0.5, 0.5);

		/*
			A rectangle is aligned so that it fills _all_ pixels that it touches. 
			This results in a rectangle that has always the same size or is larger.
		*/

		Rectangle pixelAlign(Rectangle r)
		{
			if (!ShouldPixelAlignNow)
				return r;

			var lt = r.Location.Vector;
			var rb = r.RightBottom.Vector;

			var ltp = toPixel(lt).Floor();
			var rbp = toPixel(rb).Ceiling();

			var ltr = toLogical(ltp);
			var rbr = toLogical(rbp);

			return new Rectangle(ltr.ToPoint(), (rbr - ltr).ToSize());
		}

		Vector toPixel(Vector logical)
		{
			return logical*_scaleCache + _translationCache;
		}

		Vector toLogical(Vector pixel)
		{
			return (pixel - _translationCache) / _scaleCache;
		}
	}
}
