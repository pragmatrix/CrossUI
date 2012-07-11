using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;

namespace CrossUI.SharpDX.Drawing
{
	partial class RenderTargetDrawingContext
	{
		string _font = "Arial";
		double _fontSize = 10;

		Brush _textBrush;
		TextAlign _textAlign;
		Factory _writeFactory_;

		public void Text(string font, double? size, TextAlign? align, Color? color)
		{
			if (font != null)
				_font = font;

			if (size != null)
				_fontSize = size.Value;

			if (align != null)
				_textAlign = align.Value;

			if (color != null)
			{
				_textBrush.Dispose();
				_textBrush = new SolidColorBrush(_target, color.Value.import());
			}

		}

		public void Text(string text, double x, double y, double width, double height)
		{
			using (var format = new TextFormat(requireWriteFactory(), _font, _fontSize.import()))
			{
				using (var layout = new TextLayout(requireWriteFactory(), text, format, width.import(), height.import()))
				{
					layout.TextAlignment = _textAlign.import();
					_target.DrawTextLayout(importPoint(x, y), layout, _textBrush);
				}
			}
		}

		Factory requireWriteFactory()
		{
			return _writeFactory_ ?? (_writeFactory_ = new Factory());
		}
	}

	static partial class Converters
	{
		public static TextAlignment import(this TextAlign align)
		{
			switch (align)
			{
				case TextAlign.Leading: 
					return TextAlignment.Leading;
				case TextAlign.Center:
					return TextAlignment.Center;
				case TextAlign.Trailing:
					return TextAlignment.Trailing;
			}

			return TextAlignment.Leading;
		}
	}
}
