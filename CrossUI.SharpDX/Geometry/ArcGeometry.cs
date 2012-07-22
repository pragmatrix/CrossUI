using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	static class ArcGeometry
	{
		public static DrawingPointF pointOn(RectangleF r, double angle)
		{
			var rx = r.Width/2;
			var ry = r.Height/2;
			var cx = r.X + rx;
			var cy = r.Y + ry;

			var dx = Math.Cos(angle)*rx;
			var dy = Math.Sin(angle)*ry;

			return new DrawingPointF((cx + dx).import(), (cy + dy).import());
		}

		public static void add(RectangleF r, double start, double stop, GeometrySink sink, SweepDirection direction = SweepDirection.Clockwise)
		{
			if (direction == SweepDirection.Clockwise)
				addClockwise(r, start, stop, sink);
			else
				addCounterClockwise(r, start, stop, sink);
		}

		static void addClockwise(RectangleF r, double start, double stop, GeometrySink sink)
		{
			var rx = r.Width/2;
			var ry = r.Height/2;
			var angle = start;

			// the quality of Direct2D arcs are lousy, so we render them in 16 segments per circle

			const int MaxSegments = 16;
			const double SegmentAngle = Math.PI*2/MaxSegments;

			for (var segment = 0; angle < stop && segment != MaxSegments; ++segment)
			{
				var angleLeft = stop - angle;
				var angleNow = Math.Min(SegmentAngle, angleLeft);
				var nextAngle = angle + angleNow;
				var nextPoint = pointOn(r, nextAngle);

				sink.AddArc(new ArcSegment
				{
					ArcSize = ArcSize.Small,
					Size = new DrawingSizeF(rx, ry),
					Point = nextPoint,
					RotationAngle = angleNow.import(),
					SweepDirection = SweepDirection.Clockwise
				});

				angle = nextAngle;
			}
		}

		public static void addCounterClockwise(RectangleF r, double start, double stop, GeometrySink sink)
		{
			var rx = r.Width / 2;
			var ry = r.Height / 2;
			var angle = start;

			// the quality of Direct2D arcs are lousy, so we render them in 16 segments per circle

			const int MaxSegments = 16;
			const double SegmentAngle = Math.PI * 2 / MaxSegments;

			for (var segment = 0; angle > stop && segment != MaxSegments; ++segment)
			{
				var angleLeft = angle - stop;
				var angleNow = Math.Min(SegmentAngle, angleLeft);
				var nextAngle = angle - angleNow;
				var nextPoint = pointOn(r, nextAngle);

				sink.AddArc(new ArcSegment
				{
					ArcSize = ArcSize.Small,
					Size = new DrawingSizeF(rx, ry),
					Point = nextPoint,
					RotationAngle = angleNow.import(),
					SweepDirection = SweepDirection.CounterClockwise
				});

				angle = nextAngle;
			}
		}
	}
}
