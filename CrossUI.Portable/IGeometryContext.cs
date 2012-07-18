namespace CrossUI
{
	public enum ArcDirection
	{
		Clockwise,
		CounterClockwise
	}

	interface IGeometryContext : IDrawingFigures
	{
		// ends the current figure and starts a new one.
		void MoveTo(double x, double y);

		// connects the current point with the starting point of the current figure and ends the figure
		void Close();

		void LineTo(double x, double y);

		// may connect a line to the starting point of the arc
		void ArcTo(double x, double y, double width, double height, double start, double stop, ArcDirection direction = ArcDirection.Clockwise);

		void BezierTo(double s1x, double s1y, double s2x, double s2y, double tx, double ty);
	}
}
