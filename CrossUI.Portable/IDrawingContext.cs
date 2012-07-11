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
		Trailing,
		Center,
		Justified
	}

	public enum ParagraphAlign
	{
		Near,
		Far,
		Center
	}

	public interface IDrawingContext : IClosedFigureContext
	{
		int Width { get; }
		int Height { get; }

		void Fill(Color? color = null);
		void NoFill();

		void Stroke(Color? color = null, double? weight = null, StrokeAlign? align = null);
		void NoStroke();

		void Text(
			string font = null, 
			double? size = null, 
			Color? color = null, 
			TextAlign? align = null, 
			ParagraphAlign? paragraphAlign = null);

		void Text(string text, double x, double y, double width, double height);
	}
}
