using CrossUI.Drawing;
using CrossUI.SharpDX.Geometry;
using SharpDX;
using SharpDX.Direct2D1;
using DW = SharpDX.DirectWrite;
using D2 = SharpDX.Direct2D1;
using Matrix = CrossUI.Drawing.Matrix;
using Rectangle = CrossUI.Drawing.Rectangle;
using Point = CrossUI.Drawing.Point;

namespace CrossUI.SharpDX
{
	static class Import
	{
		public static Vector2 Point(Point p)
		{
			return Point(p.X, p.Y);
		}

		public static Vector2 Point(double x, double y)
		{
			return new Vector2(x.import(), y.import());
		}

		public static RectangleF Rectangle(Rectangle r)
		{
			return new RectangleF(r.X.import(), r.Y.import(), r.Width.import(), r.Height.import());
		}

		public static RectangleF import(this Bounds bounds)
		{
			var left = bounds.Left.import();
			var top = bounds.Top.import();
			var right = bounds.Right.import();
			var bottom = bounds.Bottom.import();
			var width = right - left;
			var height = bottom - top;

			return new RectangleF(left, top, width, height);
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

		public static SweepDirection import(this ArcDirection arcDirection)
		{
			switch (arcDirection)
			{
				case ArcDirection.Clockwise: 
					return SweepDirection.Clockwise;

				case ArcDirection.CounterClockwise:
					return SweepDirection.CounterClockwise;
			}

			return SweepDirection.Clockwise;
		}

		public static D2.CombineMode import(this CombineMode mode)
		{
			switch (mode)
			{
				case CombineMode.Union:
					return D2.CombineMode.Union;
				case CombineMode.Intersect:
					return D2.CombineMode.Intersect;
				case CombineMode.XOR:
					return D2.CombineMode.Xor;
				case CombineMode.Exclude:
					return D2.CombineMode.Exclude;
			}

			return D2.CombineMode.Union;
		}

		public static GeometryRelation export(this D2.GeometryRelation relation)
		{
			switch (relation)
			{
				case D2.GeometryRelation.Disjoint:
					return GeometryRelation.Disjoint;
				case D2.GeometryRelation.IsContained:
					return GeometryRelation.IsContained;
				case D2.GeometryRelation.Contains:
					return GeometryRelation.Contains;
				case D2.GeometryRelation.Overlap:
					return GeometryRelation.Overlap;
			}

			return GeometryRelation.Disjoint;
		}

		public static Matrix3x2 import(this Matrix m)
		{
			return new Matrix3x2
			{
				M11 = m.M11.import(),
				M12 = m.M12.import(),
				M21 = m.M21.import(),
				M22 = m.M22.import(),
				M31 = m.M31.import(),
				M32 = m.M32.import()
			};
		}
	}
}