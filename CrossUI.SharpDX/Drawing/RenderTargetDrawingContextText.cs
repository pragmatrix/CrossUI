﻿using SharpDX.Direct2D1;
using DW = SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;

namespace CrossUI.SharpDX.Drawing
{
	partial class RenderTargetDrawingContext
	{

		Factory _writeFactory_;

		string _font = "Arial";
		double _fontSize = 10;
		Brush _textBrush;
		DW.TextAlignment _textAlign;
		DW.ParagraphAlignment _paragraphAlign;
		DW.WordWrapping _wrapping;

		public void Text(
			string font, 
			double? size, 
			Color? color, 
			TextAlignment? alignment, 
			ParagraphAlignment? paragraphAlignment,
			WordWrapping? wordWrapping
			)
		{
			if (font != null)
				_font = font;

			if (size != null)
				_fontSize = size.Value;

			if (alignment != null)
				_textAlign = alignment.Value.import();

			if (color != null)
			{
				_textBrush.Dispose();
				_textBrush = new SolidColorBrush(_target, color.Value.import());
			}

			if (paragraphAlignment != null)
				_paragraphAlign = paragraphAlignment.Value.import();

			if (wordWrapping != null)
			{
				_wrapping = wordWrapping.Value.import();
			}
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			using (var format = new DW.TextFormat(requireWriteFactory(), _font, _fontSize.import()))
			{
				using (var layout = new DW.TextLayout(requireWriteFactory(), text, format, width.import(), height.import()))
				{
					layout.TextAlignment = _textAlign;
					layout.ParagraphAlignment = _paragraphAlign;
					layout.WordWrapping = _wrapping;

					_target.DrawTextLayout(importPoint(x, y), layout, _textBrush);

				}
			}
		}

		Factory requireWriteFactory()
		{
			return _writeFactory_ ?? (_writeFactory_ = new Factory());
		}
	}

	static class TextConverters
	{
		public static DW.TextAlignment import(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Leading: return DW.TextAlignment.Leading;
				case TextAlignment.Center: return DW.TextAlignment.Center;
				case TextAlignment.Trailing: return DW.TextAlignment.Trailing;
			}

			return default(DW.TextAlignment);
		}

		public static DW.ParagraphAlignment import(this ParagraphAlignment alignment)
		{
			switch (alignment)
			{
				case ParagraphAlignment.Near: return DW.ParagraphAlignment.Near;
				case ParagraphAlignment.Far: return DW.ParagraphAlignment.Far;
				case ParagraphAlignment.Center: return DW.ParagraphAlignment.Center;
			}

			return default(DW.ParagraphAlignment);
		}

		public static DW.WordWrapping import(this WordWrapping wrapping)
		{
			switch (wrapping)
			{
				case WordWrapping.Wrap:
					return DW.WordWrapping.Wrap;
				case WordWrapping.NoWrap:
					return DW.WordWrapping.NoWrap;
			}

			return default(DW.WordWrapping);
		}
	}
}
