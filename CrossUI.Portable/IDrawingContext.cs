namespace CrossUI
{
	public enum StrokeAlignment
	{
		Inside, Center, Outside
	}

	public enum TextAlignment
	{
		Leading, Center, Trailing, Justified
	}

	public enum ParagraphAlignment
	{
		Near, Center, Far
	}

	public enum WordWrapping
	{
		Wrap, NoWrap
	}

	public interface IDrawingContext : IClosedFigureContext
	{
		int Width { get; }
		int Height { get; }

		void Fill(Color? color = null);
		void NoFill();

		void Stroke(Color? color = null, double? weight = null, StrokeAlignment? alignment = null);
		void NoStroke();

		void Text(
			string font = null, 
			double? size = null, 
			Color? color = null, 
			TextAlignment? alignment = null, 
			ParagraphAlignment? paragraphAlignment = null,
			WordWrapping? wordWrapping = null);

		void Text(string text, double x, double y, double width, double height);
	}
}
