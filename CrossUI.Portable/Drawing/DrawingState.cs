using System.Diagnostics;

namespace CrossUI.Drawing
{
	public sealed class DrawingState : IDrawingState
	{
		public bool FillEnabled { get; private set; }
		public Color FillColor { get; private set; }

		public bool StrokeEnabled { get; private set; }
		public Color StrokeColor { get; private set; }
		public double StrokeWeight { get; private set; }
		public StrokeAlignment StrokeAlignment { get; private set; }

		public string FontName { get; private set; }
		public FontWeight FontWeight { get; private set; }
		public FontStyle FontStyle { get; private set; }

		public double TextSize { get; private set; }
		public Color TextColor { get; private set; }
		public TextAlignment TextAlignment { get; private set; }
		public ParagraphAlignment ParagraphAlignment { get; private set; }
		public WordWrapping WordWrapping { get; private set; }

		public bool PixelAligned { get; private set; }

		public DrawingState()
		{
			// defaults

			FillColor = Colors.Black;

			StrokeEnabled = true;
			StrokeColor = Colors.Black;
			StrokeWeight = 1;

			FontName = "Arial";
			TextSize = 10;

			TextColor = Colors.Black;
			PixelAligned = false;
		}

		public void Fill(Color? color)
		{
			if (color != null)
				FillColor = color.Value;

			FillEnabled = true;
		}

		public void NoFill()
		{
			FillEnabled = false;
		}

		public void Stroke(Color? color, double? weight, StrokeAlignment? alignment)
		{
			if (color != null)
				StrokeColor = color.Value;

			if (weight != null)
				StrokeWeight = weight.Value;

			if (alignment != null)
				StrokeAlignment = alignment.Value;

			StrokeEnabled = true;
		}

		public void NoStroke()
		{
			StrokeEnabled = false;
		}

		public void PixelAlign()
		{
			PixelAligned = true;
		}

		public void NoPixelAlign()
		{
			PixelAligned = false;
		}

		public void Font(string name, FontWeight? weight, FontStyle? style)
		{
			if (name != null)
				FontName = name;

			if (weight != null)
				FontWeight = weight.Value;

			if (style != null)
				FontStyle = style.Value;
		}

		public void Text(
			double? size, 
			Color? color, 
			TextAlignment? alignment, 
			ParagraphAlignment? paragraphAlignment,
			WordWrapping? wordWrapping)
		{
			if (size != null)
				TextSize = size.Value;

			if (color != null)
				TextColor = color.Value;

			if (alignment != null)
				TextAlignment = alignment.Value;

			if (paragraphAlignment != null)
				ParagraphAlignment = paragraphAlignment.Value;

			if (wordWrapping != null)
				WordWrapping = wordWrapping.Value;
		}
	}

	public static class DrawingStateExtensions
	{
		public static Bounds StrokeAlignedBounds(this DrawingState state, double x, double y, double width, double height)
		{
			var strokeShift = StrokeAlignShift(state);
			return new Bounds(x + strokeShift, y + strokeShift, x + width - strokeShift, y + height - strokeShift);
		}

		// Fill bounds of a stroke aligned rect that gets drawed stroke + filled

		public static Bounds StrokeFillBounds(this DrawingState state, double x, double y, double width, double height)
		{
			if (!state.StrokeEnabled)
				return new Bounds(x, y, x + width, y + height);

			var fs = StrokeFillShift(state);

			return new Bounds(x + fs, y + fs, x + width - fs, y + height - fs);
		}

		public static double StrokeAlignShift(this DrawingState state)
		{
			var width = state.StrokeWeight;

			switch (state.StrokeAlignment)
			{
				case StrokeAlignment.Center:
					return 0;
				case StrokeAlignment.Inside:
					return width / 2;
				case StrokeAlignment.Outside:
					return -width / 2;
			}

			Debug.Assert(false);
			return 0;
		}

		public static double StrokeFillShift(this DrawingState state)
		{
			var width = state.StrokeWeight;

			switch (state.StrokeAlignment)
			{
				case StrokeAlignment.Center:
					return width / 2;
				case StrokeAlignment.Inside:
					return width;
				case StrokeAlignment.Outside:
					return 0;
			}

			Debug.Assert(false);
			return 0;
		}
	}
}
