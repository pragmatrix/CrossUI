using System;
using SharpDX.DirectWrite;

namespace CrossUI.SharpDX.Drawing
{
	partial class RenderTargetDrawingContext
	{
		string _font = "Arial";
		double _fontSize = 10;

		TextAlign _textAlign;
		Factory _writeFactory_;

		public void Text(string font, double? size, TextAlign? align)
		{
			if (font != null)
				_font = font;

			if (size != null)
				_fontSize = size.Value;

			if (align != null)
				_textAlign = align.Value;
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			using (var format = new TextFormat(requireWriteFactory(), _font, _fontSize.import()))
			{
				using (var layout = new TextLayout(requireWriteFactory(), text, format, width.import(), height.import()))
				{
					layout.TextAlignment = _textAlign.import();
					_target.DrawTextLayout(importPoint(x, y), layout, _strokeBrush);
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
