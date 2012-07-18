using System;

namespace CrossUI.Drawing
{
	sealed class BoundsTracker
	{
		public readonly DrawingState State;
		public readonly DrawingTransform Transform;

		public BoundsTracker(DrawingState state, DrawingTransform transform)
		{
			State = state;
			Transform = transform;
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
			if (!State.StrokeEnabled)
			{
				trackRect(x, y, width, height);
				return;
			}

			var b = State.StrokeAlignedBounds(x, y, width, height);
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
			var v = Transform.transform(new Vector(x, y));

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
