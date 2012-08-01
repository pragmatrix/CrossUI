using System;

namespace CrossUI
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public sealed class BitmapDrawingTestAttribute : Attribute
	{
		public const int DefaultWidth = 64;
		public const int DefaultHeight = 64;

		public BitmapDrawingTestAttribute()
		{
		}

		BitmapDrawingTestAttribute(int? width, int? height)
		{
			_width_ = width;
			_height_ = height;
		}

		int? _width_;
		int? _height_;

		public int Width
		{
			get { return _width_ ?? DefaultWidth; }
			set { _width_ = value; }
		}

		public int Height
		{
			get { return _height_ ?? DefaultHeight; }
			set { _height_ = value; }
		}

		public BitmapDrawingTestAttribute refine(BitmapDrawingTestAttribute nested)
		{
			return new BitmapDrawingTestAttribute(
				nested._width_ ?? Width,
				nested._height_ ?? Height
				);
		}
	}
}
