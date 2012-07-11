namespace CrossUI
{
	public enum StrokeAlign
	{
		Inside,
		Center,
		Outside
	}

	public enum TextAlign
	{
		Leading,
		Center,
		Trailing
	}

	public interface IDrawingContext : IClosedFigureContext
	{
		int Width { get; }
		int Height { get; }

		void Fill(Color? color = null);
		void NoFill();

		void Stroke(Color? color = null, double? weight = null, StrokeAlign? align = null);
		void NoStroke();

		void Text(string font = null, double? size = null, TextAlign? align = null);
		void Text(string text, double x, double y, double width, double height);
	}
}
