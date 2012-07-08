using System;

namespace CrossUI
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public sealed class BitmapDrawingTestAttribute : Attribute
	{
		const int DefaultWidth = 256;
		const int DefaultHeight = 256;

		public BitmapDrawingTestAttribute()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;
		}

		public int Width { get; set; }
		public int Height { get; set; }
	}
}
