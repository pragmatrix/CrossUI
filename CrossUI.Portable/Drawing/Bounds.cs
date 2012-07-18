namespace CrossUI.Drawing
{
	struct Bounds
	{
		public readonly double Left;
		public readonly double Top;
		public readonly double Right;
		public readonly double Bottom;

		public Bounds(double left, double top, double right, double bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
	}
}