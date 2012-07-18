namespace CrossUI.Drawing
{
	struct Rectangle
	{
		public readonly double X;
		public readonly double Y;
		public readonly double Width;
		public readonly double Height;

		public Rectangle(double x, double y, double width, double height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
	}
}
