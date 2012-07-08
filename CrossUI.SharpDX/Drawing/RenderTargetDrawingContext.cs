using System;
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
		double _strokeWeight;

		public int Width { get; private set; }
		public int Height { get; private set; }

		public void font(Font font)
		{
			throw new NotImplementedException();
		}

		public void fill(Color color)
		{
			throw new NotImplementedException();
		}

		public void noFill()
		{
			throw new NotImplementedException();
		}

		public void stroke(Color color)
		{
			_strokeBrush = new SolidColorBrush(_target, color.import());
		}

		public void strokeWeight(double weight)
		{
			_strokeWeight = weight;
		}

		public void noStroke()
		{
			throw new NotImplementedException();
		}

		public void point(double x, double y)
		{
			throw new NotImplementedException();
		}

		public void line(double x1, double y1, double x2, double y2)
		{
			throw new NotImplementedException();
		}

		public void rect(double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public void ellipse(double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public void arc(double x, double y, double width, double height, double start, double stop)
		{
			throw new NotImplementedException();
		}

		public void triangle(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			throw new NotImplementedException();
		}

		public void quad(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
		{
			throw new NotImplementedException();
		}

		public void roundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			var hsw = _strokeWeight/2;

			var rect = new RectangleF(
				import(x+hsw),
				import(y+hsw), 
				import(x + width - hsw), 
				import(y + height - hsw));

			var roundedRect = new RoundedRect
			{
				Rect = rect,
				RadiusX = import(cornerRadius),
				RadiusY = import(cornerRadius)
			};

			_target.DrawRoundedRectangle(roundedRect, _strokeBrush, _strokeWeight.import());
		}

		public void text(string text, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		static float import(double d)
		{
			return d.import();
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
