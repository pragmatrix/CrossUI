namespace CrossUI
{
	public interface IDrawingContext
	{
		int Width { get; }
		int Height { get; }

		void font(Font font);
	
		void fill(Color color);
		void noFill();

		void stroke(Color color);
		void noStroke();

		void point(double x, double y);
		void line(double x1, double y1, double x2, double y2);
		void rect(double x, double y, double width, double height);
		void ellipse(double x, double y, double width, double height);
		void arc(double x, double y, double width, double height, double start, double stop);

		void triangle(double x1, double y1, double x2, double y2, double x3, double y3);
		void quad(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4);
		void roundedRect(double x, double y, double width, double height, double cornerRadius);

		void text(string text, double x, double y, double width, double height);
	}
}
