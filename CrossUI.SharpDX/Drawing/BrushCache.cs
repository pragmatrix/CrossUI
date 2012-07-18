using System;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Drawing
{
	sealed class BrushCache : IDisposable
	{
		readonly Func<Color, SolidColorBrush> _brushGen;
		readonly Func<Color> _currentColor;

		Color? _color_;
		SolidColorBrush _brush_;

		public BrushCache(Func<Color, SolidColorBrush> brushGen, Func<Color> currentColor)
		{
			_currentColor = currentColor;
			_brushGen = brushGen;
		}

		public void Dispose()
		{
			disposeBrush();
		}

		public SolidColorBrush Brush
		{
			get
			{
				var current = _currentColor();

				if (_color_ == null || (_color_.Value != current))
				{
					disposeBrush();
					_brush_ = _brushGen(current);
					_color_ = current;
				}

				return _brush_;
			}
		}

		void disposeBrush()
		{
			if (_brush_ == null)
				return;
			_brush_.Dispose();
			_brush_ = null;
		}
	}
}
