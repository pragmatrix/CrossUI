using CrossUI.Drawing;
using System;
using System.Collections.Generic;
using CrossUI.SharpDX.Geometry;
using SharpDX;
using SharpDX.Direct2D1;
using Rectangle = CrossUI.Drawing.Rectangle;
using Point = CrossUI.Drawing.Point;

namespace CrossUI.SharpDX.Drawing
{
	sealed partial class DrawingTarget : IDrawingTargetBitmap,
		IGeometryFigures,
		IDrawingElements,
		ITextMeasurements,
		IReportingTarget,
		IDisposable
	{
		readonly DrawingState _state;
		readonly DrawingTransform _transform;
		readonly RenderTarget _target;
		readonly List<string> _reports = new List<string>();
		readonly BrushCache _strokeBrush;
		readonly BrushCache _fillBrush;
		readonly BrushCache _textBrush;

		public DrawingTarget(DrawingState state, DrawingTransform transform, RenderTarget target, int width, int height)
		{
			_state = state;
			_transform = transform;

			_target = target;
			_target.AntialiasMode = AntialiasMode.PerPrimitive;

			Width = width;
			Height = height;

			_strokeBrush = new BrushCache(createBrush, () => _state.StrokeColor);
			_fillBrush = new BrushCache(createBrush, () => state.FillColor);
			_textBrush = new BrushCache(createBrush, () => state.TextColor);

			_transform.Changed += transformChanged;
		}

		public void Dispose()
		{
			_transform.Changed -= transformChanged;

			_textBrush.Dispose();
			_fillBrush.Dispose();
			_strokeBrush.Dispose();
		}

		SolidColorBrush createBrush(Color color)
		{
			return new SolidColorBrush(_target,
				new Color4(color.Red.import(), color.Green.import(), color.Blue.import(), color.Alpha.import()));
		}

		void transformChanged()
		{
			var c = _transform.Current;

			var m = new Matrix3x2
			{
				M11 = c.M11.import(),
				M12 = c.M12.import(),
				M21 = c.M21.import(),
				M22 = c.M22.import(),
				M31 = c.M31.import(),
				M32 = c.M32.import(),
			};

			_target.Transform = m;
		}

		public int Width { get; private set; }
		public int Height { get; private set; }

		public void Report(string text)
		{
			_reports.Add(text);
		}

		public IEnumerable<string> Reports
		{
			get { return _reports; }
		}

		bool Filling
		{
			get { return _state.FillEnabled; }
		}

		bool Stroking
		{
			get { return _state.StrokeEnabled; }
		}

		float StrokeWeight
		{
			get { return _state.StrokeWeight.import(); }
		}

		public void Geometry(IGeometry geometry)
		{
			var native = geometry.import();

			if (Filling)
			{
				_target.FillGeometry(native, _fillBrush.Brush);
			}

			if (Stroking)
			{
				_target.DrawGeometry(native, _strokeBrush.Brush, _state.StrokeWeight.import());
			}
		}

		public void Line(Point p1, Point p2)
		{
			if (Stroking)
				_target.DrawLine(Import.Point(p1), Import.Point(p2), _strokeBrush.Brush, StrokeWeight);
		}

		public void Rectangle(Rectangle rectangle)
		{
			if (Filling)
			{
				var r = fillRect(rectangle);
				_target.FillRectangle(r, _fillBrush.Brush);
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(rectangle);
				_target.DrawRectangle(r, _strokeBrush.Brush, StrokeWeight);
			}
		}

		public void RoundedRectangle(Rectangle rectangle, double cornerRadius)
		{
			if (Filling)
			{
				// adjust corner radius if we do stroke afterwards.

				var filledCornerRadius = Stroking
					? Math.Max(0, cornerRadius - _state.StrokeWeight/2)
					: cornerRadius;

				var roundedRect = new RoundedRectangle
				{
					Rect = fillRect(rectangle),
					RadiusX = filledCornerRadius.import(),
					RadiusY = filledCornerRadius.import()
				};

				_target.FillRoundedRectangle(roundedRect, _fillBrush.Brush);
			}

			if (Stroking)
			{
				var roundedRect = new RoundedRectangle
				{
					Rect = strokeAlignedRect(rectangle),
					RadiusX = cornerRadius.import(),
					RadiusY = cornerRadius.import()
				};

				_target.DrawRoundedRectangle(roundedRect, _strokeBrush.Brush, StrokeWeight);
			}
		}

		public void Polygon(Point[] points)
		{
			if (points.Length < 2)
				return;

			if (points.Length == 2)
			{
				Line(points[0], points[1]);
				return;
			}

			var startPoint = Import.Point(points[0]);

			if (Filling)
			{
				fillPath(startPoint,
					sink =>
						{
							for (int i = 1; i != points.Length; ++i)
								sink.AddLine(Import.Point(points[i]));
						});
			}

			if (Stroking)
			{
				drawClosedPath(startPoint,
					sink =>
						{
							for (int i = 1; i != points.Length; ++i)
								sink.AddLine(Import.Point(points[i]));
						});
			}
		}

		public void Ellipse(Rectangle rectangle)
		{
			if (Filling)
			{
				var r = fillRect(rectangle);
				var ellipse = new Ellipse(Import.Point(r.Left + r.Width/2, r.Top + r.Height/2), r.Width/2, r.Height/2);
				_target.FillEllipse(ellipse, _fillBrush.Brush);
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(rectangle);
				var ellipse = new Ellipse(Import.Point(r.Left + r.Width / 2, r.Top + r.Height / 2), r.Width / 2, r.Height / 2);
				_target.DrawEllipse(ellipse, _strokeBrush.Brush, StrokeWeight);
			}
		}

		public void Arc(Rectangle rectangle, double start, double stop)
		{
			if (Stroking)
			{
				var r = strokeAlignedRect(rectangle);
				var currentPoint = ArcGeometry.pointOn(r, start);
				drawOpenPath(currentPoint, sink => ArcGeometry.add(r, start, stop, sink));
			}
		}

		public void Bezier(CubicBezier bezier)
		{
			if (Stroking)
			{
				drawOpenPath(Import.Point(bezier.Start),
					sink =>
						{
							var bezierSegment = new BezierSegment()
							{
								Point1 = Import.Point(bezier.Span1),
								Point2 = Import.Point(bezier.Span2),
								Point3 = Import.Point(bezier.End)
							};

							sink.AddBezier(bezierSegment);
						});
			}
		}

		void drawOpenPath(Vector2 begin, Action<GeometrySink> figureBuilder)
		{
			using (var geometry = createPath(false, begin, figureBuilder))
			{
				_target.DrawGeometry(geometry, _strokeBrush.Brush, StrokeWeight);
			}
		}

		void drawClosedPath(Vector2 begin, Action<GeometrySink> figureBuilder)
		{
			using (var geometry = createPath(true, begin, figureBuilder))
			{
				_target.DrawGeometry(geometry, _strokeBrush.Brush, StrokeWeight);
			}
		}

		void fillPath(Vector2 begin, Action<GeometrySink> figureBuilder)
		{
			using (var geometry = createPath(true, begin, figureBuilder))
			{
				_target.FillGeometry(geometry, _fillBrush.Brush);
			}
		}

		PathGeometry createPath(bool filled, Vector2 begin, Action<GeometrySink> figureBuilder)
		{
			return Path.Figure(_target.Factory, filled, begin, figureBuilder);
		}

		RectangleF fillRect(Rectangle rectangle)
		{
			if (!Stroking)
			{
				return new RectangleF(rectangle.X.import(), rectangle.Y.import(), rectangle.Width.import(), rectangle.Height.import());
			}

			return _state.StrokeFillBounds(rectangle).import();
		}

		RectangleF strokeAlignedRect(Rectangle rectangle)
		{
			return _state.StrokeAlignedBounds(rectangle).import();
		}
	}
}
