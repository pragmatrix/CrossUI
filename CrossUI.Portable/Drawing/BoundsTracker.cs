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

		public void trackAlignedRect(Rectangle rectangle)
		{
			if (!State.StrokeEnabled)
			{
				trackRect(rectangle);
				return;
			}

			var b = State.StrokeAlignedBounds(rectangle);
			trackPoints(b.LeftTop, b.RightTop, b.RightBottom, b.LeftBottom);
		}

		public void trackRect(Rectangle rectangle)
		{
			var x = rectangle.X;
			var y = rectangle.Y;
			var right = rectangle.Right;
			var bottom = rectangle.Bottom;
			trackPoints(new Point(x, y), new Point(right, y), new Point(right, bottom), new Point(x, bottom));
		}
			
		public void trackPoints(params Point[] points)
		{
			foreach (var p in points)
				trackPoint(p.X, p.Y);
		}

		void trackPoint(double x, double y)
		{
			var v = Transform.Transform(new Vector(x, y));

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
