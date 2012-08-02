using System;
using System.Diagnostics;

namespace CrossUI.Drawing
{
	public sealed class DrawingState : IDrawingState
	{
		DrawingState _previous_;

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

		public static void Copy(DrawingState @from, DrawingState to)
		{
			to.FillEnabled = @from.FillEnabled;
			to.FillColor = @from.FillColor;

			to.StrokeEnabled = @from.StrokeEnabled;
			to.StrokeWeight = @from.StrokeWeight;
			to.StrokeAlignment = @from.StrokeAlignment;

			to.FontName = @from.FontName;
			to.FontWeight = @from.FontWeight;
			to.FontStyle = @from.FontStyle;
	
			to.TextSize = @from.TextSize;
			to.TextColor = @from.TextColor;
			to.TextAlignment = @from.TextAlignment;
			
			to.ParagraphAlignment = @from.ParagraphAlignment;
			to.WordWrapping = @from.WordWrapping;
			
			to.PixelAligned = @from.PixelAligned;
		}

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

		public void SaveState()
		{
			var clone = Clone();
			_previous_ = clone;
		}

		public void RestoreState()
		{
			var p = _previous_;
			if (p == null)
				throw new Exception("No Previous State found");

			Copy(p, this);
			_previous_ = p._previous_;
		}

		public DrawingState Clone()
		{
			return (DrawingState)MemberwiseClone();
		}
	}

	public static class DrawingStateExtensions
	{
		public static Bounds StrokeAlignedBounds(this DrawingState state, Rectangle rectangle)
		{
			var strokeShift = StrokeAlignShift(state);
			return new Bounds(rectangle.X + strokeShift, rectangle.Y + strokeShift, rectangle.Right - strokeShift, rectangle.Bottom - strokeShift);
		}

		// Fill bounds of a stroke aligned rect that gets drawed stroke + filled

		public static Bounds StrokeFillBounds(this DrawingState state, Rectangle r)
		{
			if (!state.StrokeEnabled)
				return new Bounds(r.X, r.Y, r.Right, r.Bottom);

			var fs = StrokeFillShift(state);

			return new Bounds(r.X + fs, r.Y + fs, r.Right - fs, r.Bottom - fs);
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
