namespace CrossUI
{
	public interface IGeometryFigures
	{
		void Line(double x1, double y1, double x2, double y2);

		void Rect(double x, double y, double width, double height);
		void RoundedRect(double x, double y, double width, double height, double cornerRadius);
		void Polygon(double[] coordinatePairs);

		void Ellipse(double x, double y, double width, double height);
		void Arc(double x, double y, double width, double height, double start, double stop);

		// cubic
		void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey);
	}
}
