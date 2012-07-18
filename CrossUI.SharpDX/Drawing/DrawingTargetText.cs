using DW = SharpDX.DirectWrite;
using Factory = SharpDX.DirectWrite.Factory;

namespace CrossUI.SharpDX.Drawing
{
	partial class DrawingTarget
	{
		Factory _writeFactory_;

		public void Text(string text, double x, double y, double width, double height)
		{
			using (var format = createTextFormat())
			using (var layout = createTextLayout(text, format, width, height))
			{
				_target.DrawTextLayout(Import.Point(x, y), layout, _textBrush.Brush);
			}
		}

		public TextSize MeasureText(string text, double layoutWidth, double layoutHeight)
		{
			using (var format = createTextFormat())
			using (var layout = createTextLayout(text, format, layoutWidth, layoutHeight))
			{
				return new TextSize(layout.Metrics.Width, layout.Metrics.Height);
			}
		}

		DW.TextFormat createTextFormat()
		{
			return new DW.TextFormat(
				requireWriteFactory(),
				_state.FontName,
				null,
				_state.FontWeight.import(),
				_state.FontStyle.import(),
				DW.FontStretch.Normal,
				_state.TextSize.import());
		}

		DW.TextLayout createTextLayout(string text, DW.TextFormat format, double width, double height)
		{
			var layout = new DW.TextLayout(requireWriteFactory(), text, format, width.import(), height.import());
			layout.TextAlignment = _state.TextAlignment.import();
			layout.ParagraphAlignment = _state.ParagraphAlignment.import();
			layout.WordWrapping = _state.WordWrapping.import();

			return layout;
		}


		Factory requireWriteFactory()
		{
			return _writeFactory_ ?? (_writeFactory_ = new Factory());
		}
	}
}
