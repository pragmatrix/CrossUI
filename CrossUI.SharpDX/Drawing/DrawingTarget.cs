using CrossUI.Drawing;
using System;
using System.Collections.Generic;
using CrossUI.SharpDX.Geometry;
using SharpDX;
using SharpDX.Direct2D1;

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

		public void Line(double x1, double y1, double x2, double y2)
		{
			if (Stroking)
				_target.DrawLine(Import.Point(x1, y1), Import.Point(x2, y2), _strokeBrush.Brush, StrokeWeight);
		}

		public void Rect(double x, double y, double width, double height)
		{
			if (Filling)
			{
				var r = fillRect(x, y, width, height);
				_target.FillRectangle(r, _fillBrush.Brush);
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(x, y, width, height);
				_target.DrawRectangle(r, _strokeBrush.Brush, StrokeWeight);
			}
		}

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			if (Filling)
			{
				// adjust corner radius if we do stroke afterwards.

				var filledCornerRadius = Stroking
					? Math.Max(0, cornerRadius - _state.StrokeWeight/2)
					: cornerRadius;

				var roundedRect = new RoundedRect
				{
					Rect = fillRect(x, y, width, height),
					RadiusX = filledCornerRadius.import(),
					RadiusY = filledCornerRadius.import()
				};

				_target.FillRoundedRectangle(roundedRect, _fillBrush.Brush);
			}

			if (Stroking)
			{
				var roundedRect = new RoundedRect
				{
					Rect = strokeAlignedRect(x, y, width, height),
					RadiusX = cornerRadius.import(),
					RadiusY = cornerRadius.import()
				};

				_target.DrawRoundedRectangle(roundedRect, _strokeBrush.Brush, StrokeWeight);
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

			var startPoint = Import.Point(pairs[0], pairs[1]);

			if (Filling)
			{
				fillPath(startPoint,
					sink =>
						{
							for (int i = 2; i != pairs.Length; i += 2)
								sink.AddLine(Import.Point(pairs[i], pairs[i + 1]));
						});
			}

			if (Stroking)
			{
				drawClosedPath(startPoint,
					sink =>
						{
							for (int i = 2; i != pairs.Length; i += 2)
								sink.AddLine(Import.Point(pairs[i], pairs[i + 1]));
						});
			}
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			if (Filling)
			{
				var r = fillRect(x, y, width, height);
				var ellipse = new Ellipse(Import.Point(r.Left + r.Width/2, r.Top + r.Height/2), r.Width/2, r.Height/2);
				_target.FillEllipse(ellipse, _fillBrush.Brush);
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(x, y, width, height);
				var ellipse = new Ellipse(Import.Point(r.Left + r.Width / 2, r.Top + r.Height / 2), r.Width / 2, r.Height / 2);
				_target.DrawEllipse(ellipse, _strokeBrush.Brush, StrokeWeight);
			}
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			if (Filling)
			{
				var r = fillRect(x, y, width, height);

				var centerPoint = new DrawingPointF(r.X + r.Width/2, r.Y + r.Height/2);

				fillPath(centerPoint,
					sink =>
						{
							var startPoint = ArcGeometry.pointOn(r, start);
							sink.AddLine(startPoint);
							ArcGeometry.add(r, start, stop, sink);
						});
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(x, y, width, height);
				var currentPoint = ArcGeometry.pointOn(r, start);
				drawOpenPath(currentPoint, sink => ArcGeometry.add(r, start, stop, sink));
			}
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			if (Filling)
			{
				fillPath(Import.Point(x, y),
					sink =>
						{
							var bezierSegment = new BezierSegment()
							{
								Point1 = Import.Point(s1x, s1y),
								Point2 = Import.Point(s2x, s2y),
								Point3 = Import.Point(ex, ey)
							};

							sink.AddBezier(bezierSegment);
						});
			}

			if (Stroking)
			{
				drawOpenPath(Import.Point(x, y),
					sink =>
						{
							var bezierSegment = new BezierSegment()
							{
								Point1 = Import.Point(s1x, s1y),
								Point2 = Import.Point(s2x, s2y),
								Point3 = Import.Point(ex, ey)
							};

							sink.AddBezier(bezierSegment);
						});
			}
		}

		void drawOpenPath(DrawingPointF begin, Action<GeometrySink> figureBuilder)
		{
			using (var geometry = createPath(false, begin, figureBuilder))
			{
				_target.DrawGeometry(geometry, _strokeBrush.Brush, StrokeWeight);
			}
		}

		void drawClosedPath(DrawingPointF begin, Action<GeometrySink> figureBuilder)
		{
			using (var geometry = createPath(true, begin, figureBuilder))
			{
				_target.DrawGeometry(geometry, _strokeBrush.Brush, StrokeWeight);
			}
		}

		void fillPath(DrawingPointF begin, Action<GeometrySink> figureBuilder)
		{
			using (var geometry = createPath(true, begin, figureBuilder))
			{
				_target.FillGeometry(geometry, _fillBrush.Brush);
			}
		}

		public PathGeometry createPath(bool filled, DrawingPointF begin, Action<GeometrySink> figureBuilder)
		{
			var pg = new PathGeometry(_target.Factory);

			using (var sink = pg.Open())
			{
				sink.BeginFigure(begin, filled ? FigureBegin.Filled : FigureBegin.Hollow);
				figureBuilder(sink);
				sink.EndFigure(filled ? FigureEnd.Closed : FigureEnd.Open);
				sink.Close();
			}

			return pg;
		}

		RectangleF fillRect(double x, double y, double width, double height)
		{
			if (!Stroking)
			{
				return new RectangleF(x.import(), y.import(), (x + width).import(), (y + height).import());
			}

			return _state.StrokeFillBounds(x, y, width, height).import();
		}


		RectangleF strokeAlignedRect(double x, double y, double width, double height)
		{
			return _state.StrokeAlignedBounds(x, y, width, height).import();
		}
	}
}
