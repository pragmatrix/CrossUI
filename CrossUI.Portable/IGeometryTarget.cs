using CrossUI.Drawing;

namespace CrossUI
{
	public enum ArcDirection
	{
		Clockwise,
		CounterClockwise
	}

	/*
		A geometry consists of a number of figures. Each figure is either opened or closed.
	
		Figures drawn using the methods in IGeometryFigures are all closed even Arcs and Beziers.
		which are left open at their ending position.
	*/

	public interface IGeometryTarget : IGeometryFigures, IFigureTarget
	{
		// ends the current figure (if there is one leaves it open) and
		// starts a new one at x,y.
		void MoveTo(double x, double y);

		// closes the current figure by connecting a line with the starting point.
		void Close();

	}

	public static class GeometryTargetExtensions
	{
		public static void MoveTo(this IGeometryTarget _, Point p)
		{
			_.MoveTo(p.X, p.Y);
		}
	}


	public interface IFigureTarget
	{
		// Relative movement, requires a current, open figure
		void LineTo(double x, double y);

		// note: connects a line to the starting point of the arc
		void ArcTo(double x, double y, double width, double height, double start, double stop, ArcDirection direction = ArcDirection.Clockwise);

		// starting point is the current point.
		void BezierTo(double s1x, double s1y, double s2x, double s2y, double ex, double ey);
	}

	public static class FigureTargetExtensions
	{
		public static void LineTo(this IFigureTarget _, Point p)
		{
			_.LineTo(p.X, p.Y);
		}

		public static void ArcTo(this IFigureTarget _, Rectangle r, double start, double stop, ArcDirection direction = ArcDirection.Clockwise)
		{
			_.ArcTo(r.X, r.Y, r.Width, r.Height, start, stop, direction);
		}

		public static void BezierTo(this IFigureTarget _, Point span1, Point span2, Point end)
		{
			_.BezierTo(span1.X, span1.Y, span2.X, span2.Y, end.X, end.Y);
		}
	}

	public interface IGeometryTargetRecorder : IGeometryTarget, IRecorder<IGeometryTarget>
	{
	}
}
