using CrossUI.Drawing;

namespace CrossUI
{
	public interface IGeometryFigures
	{
		void Line(double x1, double y1, double x2, double y2);

		void Rectangle(double x, double y, double width, double height);
		void RoundedRectangle(double x, double y, double width, double height, double cornerRadius);
		void Polygon(params double[] coordinatePairs);

		void Ellipse(double x, double y, double width, double height);
		void Arc(double x, double y, double width, double height, double start, double stop);

		// cubic
		void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey);
	}

	public static class GeometryFiguresExtensions
	{
		public static void Line(this IGeometryFigures _, Point from, Point to)
		{
			_.Line(from.X, from.Y, to.X, to.Y);
		}

		public static void Rectangle(this IGeometryFigures _, Rectangle rectangle)
		{
			_.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		public static void RoundedRectangle(this IGeometryFigures _, Rectangle rectangle, double cordnerRadius)
		{
			_.RoundedRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, cordnerRadius);
		}

		public static void Polygon(this IGeometryFigures _, params Point[] points)
		{
			_.Polygon(points.ToPairs());
		}

		public static void Ellipse(this IGeometryFigures _, Rectangle rectangle)
		{
			_.Ellipse(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		public static void Arc(this IGeometryFigures _, Rectangle rectangle, double start, double stop)
		{
			_.Arc(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, start, stop);
		}

		public static void Bezier(this IGeometryFigures _, Point start, Point span1, Point span2, Point end)
		{
			_.Bezier(new CubicBezier(start, span1, span2, end));
		}

		public static void Bezier(this IGeometryFigures _, CubicBezier bezier)
		{
			_.Bezier(
				bezier.Start.X, bezier.Start.Y, 
				bezier.Span1.X, bezier.Span1.Y, 
				bezier.Span2.X, bezier.Span2.Y, 
				bezier.End.X, bezier.End.Y);
		}
	}
}
