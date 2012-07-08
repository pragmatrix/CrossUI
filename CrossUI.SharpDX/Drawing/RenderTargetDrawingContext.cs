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

		Brush _strokeBrush;
		float _strokeWeight;
		StrokeAlign _strokeAlign;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public void Font(Font font)
		{
			throw new NotImplementedException();
		}

		public void Fill(Color color)
		{
			throw new NotImplementedException();
		}

		public void NoFill()
		{
			throw new NotImplementedException();
		}

		public void Stroke(Color? color = null, double? weight = null, StrokeAlign? align = null)
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

		public void Line(double x1, double y1, double x2, double y2)
		{
			_target.DrawLine(importPoint(x1, y1), importPoint(x2, y2), _strokeBrush, _strokeWeight);
		}

		public void Rect(double x, double y, double width, double height)
		{
			var r = strokeAlignedRect(x, y, width, height);
			_target.DrawRectangle(r, _strokeBrush, _strokeWeight);
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			var r = strokeAlignedRect(x, y, width, height);
			var ellipse = new Ellipse(importPoint(r.Left + r.Width/2, r.Top + r.Height/2), r.Width/2, r.Height/2);
			_target.DrawEllipse(ellipse, _strokeBrush, _strokeWeight);
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			var r = strokeAlignedRect(x, y, width, height);

			using (var pg = new PathGeometry(_target.Factory))
			{
				using (var sink = pg.Open())
				{
					var rx = r.Width/2;
					var ry = r.Height/2;

					var currentPoint = pointOnArc(r, start);

					sink.BeginFigure(currentPoint, FigureBegin.Hollow);

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

					sink.EndFigure(FigureEnd.Open);
					sink.Close();
				}

				_target.DrawGeometry(pg, _strokeBrush, _strokeWeight);
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

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			var roundedRect = new RoundedRect
			{
				Rect = strokeAlignedRect(x, y, width, height),
				RadiusX = import(cornerRadius),
				RadiusY = import(cornerRadius)
			};

			_target.DrawRoundedRectangle(roundedRect, _strokeBrush, _strokeWeight);
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
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
