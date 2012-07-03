using System;

namespace CrossUI.Portable
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class DrawingTestAttribute : Attribute
	{
		const double DefaultWidth = 256;
		const double DefaultHeight = 256;

		public DrawingTestAttribute()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;
		}

		public double Width { get; set; }
		public double Height { get; set; }
	}
}
