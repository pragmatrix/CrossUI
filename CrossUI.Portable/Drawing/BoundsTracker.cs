using System;

namespace CrossUI.Drawing
{
	sealed class BoundsTracker
	{
		readonly DrawingState _state;
		readonly DrawingTransform _transform;

		public BoundsTracker(DrawingState state, DrawingTransform transform)
		{
			_state = state;
			_transform = transform;
		}

		public Bounds? Bounds
		{
			get
			{
				return !_set ? (Bounds?) null : new Bounds(_left, _top, _right, _bottom);
			}
		}

		bool _set;
		double _left;
		double _top;
		double _right;
		double _bottom;

		public void trackAlignedRect(double x, double y, double width, double height)
		{
			if (!_state.StrokeEnabled)
			{
				trackRect(x, y, width, height);
				return;
			}

			var b = _state.StrokeAlignedBounds(x, y, width, height);
			trackPoints(b.Left, b.Top, b.Right, b.Top, b.Right, b.Bottom, b.Left, b.Bottom);
		}

		public void trackRect(double x, double y, double width, double height)
		{
			var right = x + width;
			var bottom = y + height;
			trackPoints(x, y, right, y, right, bottom, x, bottom);
		}
			
		public void trackPoints(params double[] points)
		{
			for (int i = 0; i != points.Length; i += 2)
			{
				var x = points[i];
				var y = points[i + 1];
				trackPoint(x, y);
			}
		}

		void trackPoint(double x, double y)
		{
			var v = _transform.transform(new Vector(x, y));

			if (!_set)
			{
				_left = v.X;
				_top = v.Y;
				_right = v.X;
				_bottom = v.Y;

				_set = true;
				return;
			}

			_left = Math.Min(_left, v.X);
			_right = Math.Max(_right, v.X);
			_top = Math.Min(_top, v.Y);
			_bottom = Math.Max(_bottom, v.Y);
		}
	}
}
