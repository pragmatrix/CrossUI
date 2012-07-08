using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Drawing
{
	sealed class RenderTargetDrawingContext : IDrawingContext, IDisposable
	{
		readonly RenderTarget _target;

		public RenderTargetDrawingContext(RenderTarget target, int width, int height)
		{
			_target = target;
			_target.AntialiasMode = AntialiasMode.PerPrimitive;

			Width = width;
			Height = height;

			_strokeBrush = new SolidColorBrush(_target, new Color4(0, 0, 0, 1));
			
			_strokeWeight = 1;
		}

		public void Dispose()
		{
			_strokeBrush.Dispose();
		}

		Brush _fillBrush_;

		Brush _strokeBrush;
		float _strokeWeight;
		StrokeAlign _strokeAlign;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public void Font(Font font)
		{
			throw new NotImplementedException();
		}

		public void Fill(Color? color)
		{
			if (color != null)
				_fillBrush_ = new SolidColorBrush(_target, color.Value.import());
		}

		public void NoFill()
		{
			_fillBrush_ = null;
		}

		public void Stroke(Color? color, double? weight, StrokeAlign? align)
		{
			if (color != null)
				_strokeBrush = new SolidColorBrush(_target, color.Value.import());

			if (weight != null)
				_strokeWeight = weight.Value.import();

			if (align != null)
				_strokeAlign = align.Value;
		}

		public void NoStroke()
		{
			_strokeWeight = 0;
		}

		bool Filling
		{
			get { return _fillBrush_ != null; }
		}

		bool Stroking
		{
			get { return _strokeWeight != 0f; }
		}

		public void Line(double x1, double y1, double x2, double y2)
		{
			if (Stroking)
				_target.DrawLine(importPoint(x1, y1), importPoint(x2, y2), _strokeBrush, _strokeWeight);
		}

		public void Rect(double x, double y, double width, double height)
		{
			if (Stroking)
			{
				var r = strokeAlignedRect(x, y, width, height);
				_target.DrawRectangle(r, _strokeBrush, _strokeWeight);
			}

			if (Filling)
			{
				var r = fillRect(x, y, width, height);
				_target.FillRectangle(r, _fillBrush_);
			}

		}

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			if (Filling)
			{
				var roundedRect = new RoundedRect
				{
					Rect = fillRect(x, y, width, height),
					RadiusX = import(cornerRadius),
					RadiusY = import(cornerRadius)
				};

				_target.FillRoundedRectangle(roundedRect, _fillBrush_);
			}

			if (Stroking)
			{
				var roundedRect = new RoundedRect
				{
					Rect = strokeAlignedRect(x, y, width, height),
					RadiusX = import(cornerRadius),
					RadiusY = import(cornerRadius)
				};

				_target.DrawRoundedRectangle(roundedRect, _strokeBrush, _strokeWeight);
			}
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			if (Filling)
			{
				var r = fillRect(x, y, width, height);
				var ellipse = new Ellipse(importPoint(r.Left + r.Width / 2, r.Top + r.Height / 2), r.Width / 2, r.Height / 2);
				_target.FillEllipse(ellipse, _fillBrush_);
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(x, y, width, height);
				var ellipse = new Ellipse(importPoint(r.Left + r.Width / 2, r.Top + r.Height / 2), r.Width / 2, r.Height / 2);
				_target.DrawEllipse(ellipse, _strokeBrush, _strokeWeight);
			}
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			if (Filling)
			{
				var r = fillRect(x, y, width, height);

				using (var pg = new PathGeometry(_target.Factory))
				{
					using (var sink = pg.Open())
					{
						var centerPoint = new DrawingPointF(r.X + r.Width/2, r.Y + r.Height/2);
						var startPoint = pointOnArc(r, start);

						sink.BeginFigure(centerPoint, FigureBegin.Filled);
						sink.AddLine(startPoint);
						addArc(r, start, stop, sink);

						sink.EndFigure(FigureEnd.Closed);
						sink.Close();
					}

					_target.FillGeometry(pg, _fillBrush_);
				}
			}

			if (Stroking)
			{
				var r = strokeAlignedRect(x, y, width, height);

				using (var pg = new PathGeometry(_target.Factory))
				{
					using (var sink = pg.Open())
					{
						var currentPoint = pointOnArc(r, start);

						sink.BeginFigure(currentPoint, FigureBegin.Hollow);
						addArc(r, start, stop, sink);

						sink.EndFigure(FigureEnd.Open);
						sink.Close();
					}

					_target.DrawGeometry(pg, _strokeBrush, _strokeWeight);
				}
			}
		}

		void addArc(RectangleF r, double start, double stop, GeometrySink sink)
		{
			var rx = r.Width / 2;
			var ry = r.Height / 2;
			var angle = start;

			// the quality of Direct2D arcs are lousy, so we render them in 16 segments per circle

			const int MaxSegments = 16;
			const double SegmentAngle = Math.PI*2/MaxSegments;

			for (var segment = 0; angle < stop && segment != MaxSegments; ++segment)
			{
				var angleLeft = stop - angle;
				var angleNow = Math.Min(SegmentAngle, angleLeft);
				var nextAngle = angle + angleNow;
				var nextPoint = pointOnArc(r, nextAngle);

				sink.AddArc(new ArcSegment
				{
					ArcSize = ArcSize.Small,
					Size = new DrawingSizeF(rx, ry),
					Point = nextPoint,
					RotationAngle = (stop - start).import(),
					SweepDirection = SweepDirection.Clockwise
				});

				angle = nextAngle;
			}
		}

		DrawingPointF pointOnArc(RectangleF r, double angle)
		{
			var rx = r.Width/2;
			var ry = r.Height/2;
			var cx = r.X + rx;
			var cy = r.Y + ry;

			var dx = Math.Cos(angle)*rx;
			var dy = Math.Sin(angle)*ry;

			return new DrawingPointF((cx + dx).import(), (cy + dy).import());
		}


		public void Text(string text, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		RectangleF fillRect(double x, double y, double width, double height)
		{
			if (!Stroking)
			{
				return new RectangleF(
					import(x),
					import(y),
					import(x + width),
					import(y + height));
			}

			var fs = strokeFillShift();

			return new RectangleF(
				import(x+fs),
				import(y+fs),
				import(x + width - fs),
				import(y + height - fs));
		}


		RectangleF strokeAlignedRect(double x, double y, double width, double height)
		{
			var strokeShift = strokeAlignShift();

			var rect = new RectangleF(
				import(x + strokeShift),
				import(y + strokeShift),
				import(x + width - strokeShift),
				import(y + height - strokeShift));

			return rect;
		}

		double strokeAlignShift()
		{
			var width = _strokeWeight;

			switch (_strokeAlign)
			{
				case StrokeAlign.Center:
					return 0;
				case StrokeAlign.Inside:
					return width/2;
				case StrokeAlign.Outside:
					return -width/2;
			}

			Debug.Assert(false);
			return 0;
		}

		double strokeFillShift()
		{
			var width = _strokeWeight;

			switch (_strokeAlign)
			{
				case StrokeAlign.Center:
					return width/2;
				case StrokeAlign.Inside:
					return width;
				case StrokeAlign.Outside:
					return 0;
			}

			Debug.Assert(false);
			return 0;
		}

		static float import(double d)
		{
			return d.import();
		}

		static DrawingPointF importPoint(double x, double y)
		{
			return new DrawingPointF(x.import(), y.import());
		}
	}

	static class Conversions
	{
		public static float import(this double d)
		{
			return (float) d;
		}

		public static Color4 import(this Color color)
		{
			return new Color4(color.Red.import(), color.Green.import(), color.Blue.import(), color.Alpha.import());
		}
	}
}
