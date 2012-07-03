using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace CrossUI.Drawing
{
	sealed class RenderTargetDrawingContext : IDrawingContext, IDisposable
	{
		readonly RenderTarget _target;

		public RenderTargetDrawingContext(RenderTarget target, int width, int height)
		{
			_target = target;

			Width = width;
			Height = height;

			_strokeBrush = new SolidColorBrush(_target, new Color4(0, 0, 0, 1));
			_strokeWidth = 1;
		}

		public void Dispose()
		{
			_strokeBrush.Dispose();
		}

		Brush _strokeBrush;
		float _strokeWidth;

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
			throw new NotImplementedException();
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
			var rect = new RoundedRect
			{
				Rect = new RectangleF(f(x), f(y), f(x+width), f(y + height)),
				RadiusX = f(cornerRadius),
				RadiusY = f(cornerRadius)
			};

			_target.DrawRoundedRectangle(rect, _strokeBrush, _strokeWidth);
		}

		public void text(string text, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		static float f(double d)
		{
			return (float) d;
		}
	}
}
