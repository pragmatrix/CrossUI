namespace CrossUI
{
	public struct Color
	{
		public Color(double red, double green, double blue, double alpha = 1)
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
