using CrossUI.Drawing;

namespace CrossUI
{
	public interface IGeometryFigures
	{
		void Line(Point p1, Point p2);

		void Rectangle(Rectangle rectangle);
		void RoundedRectangle(Rectangle rectangle, double cornerRadius);
		void Polygon(params Point[] points);

		void Ellipse(Rectangle rectangle);
		void Arc(Rectangle rectangle, double start, double stop);

		void Bezier(CubicBezier bezier);
	}

	public static class GeometryFiguresExtensions
	{
		public static void Line(this IGeometryFigures _, double x1, double y1, double x2, double y2)
		{
			_.Line(new Point(x1, y1), new Point(x2, y2));
		}

		public static void Rectangle(this IGeometryFigures _, double x, double y, double width, double height)
		{
			_.Rectangle(new Rectangle(x, y, width, height));
		}

		public static void RoundedRectangle(this IGeometryFigures _, double x, double y, double width, double height, double cornerRadius)
		{
			_.RoundedRectangle(new Rectangle(x, y, width, height), cornerRadius);
		}

		public static void Polygon(this IGeometryFigures _, params double[] pairs)
		{
			_.Polygon(pairs.ToPoints());
		}

		public static void Ellipse(this IGeometryFigures _, double x, double y, double width, double height)
		{
			_.Ellipse(new Rectangle(x, y, width, height));
		}

		public static void Arc(this IGeometryFigures _, double x, double y, double width, double height, double start, double stop)
		{
			_.Arc(new Rectangle(x, y, width, height), start, stop);
		}

		public static void Bezier(this IGeometryFigures _, double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			_.Bezier(new Point(x, y), new Point(s1x, s1y), new Point(s2x, s2y), new Point(ex, ey));
		}

		public static void Bezier(this IGeometryFigures _, Point start, Point span1, Point span2, Point end)
		{
			_.Bezier(new CubicBezier(start, span1, span2, end));
		}
	}
}
