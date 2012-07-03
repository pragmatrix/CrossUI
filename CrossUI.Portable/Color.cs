namespace CrossUI
{
	public sealed class Color
	{
		public Color(double red, double green, double blue, double alpha)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		public readonly double Red;
		public readonly double Green;
		public readonly double Blue;
		public readonly double Alpha;
	}
}
