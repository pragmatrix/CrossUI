using SharpDX;
using DW = SharpDX.DirectWrite;
using CrossUI.Drawing;

namespace CrossUI.SharpDX.Drawing
{
	static class Import
	{
		public static DrawingPointF Point(double x, double y)
		{
			return new DrawingPointF(x.import(), y.import());
		}

		public static RectangleF import(this Bounds bounds)
		{
			return new RectangleF(bounds.Left.import(), bounds.Top.import(), bounds.Right.import(), bounds.Bottom.import());
		}

		public static float import(this double d)
		{
			return (float) d;
		}

		public static Color4 import(this Color color)
		{
			return new Color4(color.Red.import(), color.Green.import(), color.Blue.import(), color.Alpha.import());
		}

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
				case WordWrapping.Wrap: return DW.WordWrapping.Wrap;
				case WordWrapping.NoWrap: return DW.WordWrapping.NoWrap;
			}

			return default(DW.WordWrapping);
		}

		public static DW.FontWeight import(this FontWeight weight)
		{
			switch (weight)
			{
				case FontWeight.Normal: return DW.FontWeight.Normal;
				case FontWeight.Bold: return DW.FontWeight.Bold;
			}

			return DW.FontWeight.Normal;
		}

		public static DW.FontStyle import(this FontStyle style)
		{
			switch (style)
			{
				case FontStyle.Normal: return DW.FontStyle.Normal;
				case FontStyle.Italic: return DW.FontStyle.Italic;
			}

			return DW.FontStyle.Normal;
		}
	}
}