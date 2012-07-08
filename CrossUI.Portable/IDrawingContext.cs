namespace CrossUI
{
	public interface IDrawingContext
	{
		int Width { get; }
		int Height { get; }

		void Font(Font font);

		void Fill(Color color);
		void NoFill();

		void Stroke(Color? color = null, double? weight = null, StrokeAlign? align = null);
		void NoStroke();

		void Line(double x1, double y1, double x2, double y2);

		void Rect(double x, double y, double width, double height);
		void Ellipse(double x, double y, double width, double height);
		void Arc(double x, double y, double width, double height, double start, double stop);

		void RoundedRect(double x, double y, double width, double height, double cornerRadius);

		void Text(string text, double x, double y, double width, double height);
	}

	public static class DrawingContextExtensions
	{
		public static void Stroke(this IDrawingContext c, double r, double g, double b, double a = 1.0)
		{
			c.Stroke(new Color(r, g, b));
		}
	}
}
