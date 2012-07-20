using CrossUI.SharpDX.Geometry;
using SharpDX;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX
{
	static class Import
	{
		public static DrawingPointF Point(double x, double y)
		{
			return new DrawingPointF(x.import(), y.import());
		}

		public static RectangleF Rectangle(double x, double y, double width, double height)
		{
			return new RectangleF(x.import(), y.import(), (x+width).import(), (y + height).import());
		}

		public static RectangleF import(this Bounds bounds)
		{
			return new RectangleF(bounds.Left.import(), bounds.Top.import(), bounds.Right.import(), bounds.Bottom.import());
		}

		public static Bounds export(this RectangleF rect)
		{
			return new Bounds(rect.Left, rect.Top, rect.Right, rect.Bottom);
		}

		public static PathGeometry import(this IGeometry geometry)
		{
			return 
				((GeometryImplementation) geometry).Geometry;
		}

		public static float import(this double d)
		{
			return (float) d;
		}

		public static Color4 import(this Color color)
		{
			return new Color4(color.Red.import(), color.Green.import(), color.Blue.import(), color.Alpha.import());
		}

		public static global::SharpDX.DirectWrite.TextAlignment import(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Leading: return global::SharpDX.DirectWrite.TextAlignment.Leading;
				case TextAlignment.Center: return global::SharpDX.DirectWrite.TextAlignment.Center;
				case TextAlignment.Trailing: return global::SharpDX.DirectWrite.TextAlignment.Trailing;
			}

			return default(global::SharpDX.DirectWrite.TextAlignment);
		}

		public static global::SharpDX.DirectWrite.ParagraphAlignment import(this ParagraphAlignment alignment)
		{
			switch (alignment)
			{
				case ParagraphAlignment.Near: return global::SharpDX.DirectWrite.ParagraphAlignment.Near;
				case ParagraphAlignment.Far: return global::SharpDX.DirectWrite.ParagraphAlignment.Far;
				case ParagraphAlignment.Center: return global::SharpDX.DirectWrite.ParagraphAlignment.Center;
			}

			return default(global::SharpDX.DirectWrite.ParagraphAlignment);
		}

		public static global::SharpDX.DirectWrite.WordWrapping import(this WordWrapping wrapping)
		{
			switch (wrapping)
			{
				case WordWrapping.Wrap: return global::SharpDX.DirectWrite.WordWrapping.Wrap;
				case WordWrapping.NoWrap: return global::SharpDX.DirectWrite.WordWrapping.NoWrap;
			}

			return default(global::SharpDX.DirectWrite.WordWrapping);
		}

		public static global::SharpDX.DirectWrite.FontWeight import(this FontWeight weight)
		{
			switch (weight)
			{
				case FontWeight.Normal: return global::SharpDX.DirectWrite.FontWeight.Normal;
				case FontWeight.Bold: return global::SharpDX.DirectWrite.FontWeight.Bold;
			}

			return global::SharpDX.DirectWrite.FontWeight.Normal;
		}

		public static global::SharpDX.DirectWrite.FontStyle import(this FontStyle style)
		{
			switch (style)
			{
				case FontStyle.Normal: return global::SharpDX.DirectWrite.FontStyle.Normal;
				case FontStyle.Italic: return global::SharpDX.DirectWrite.FontStyle.Italic;
			}

			return global::SharpDX.DirectWrite.FontStyle.Normal;
		}
	}
}