using System;
using CrossUI.Drawing;

namespace CrossUI.Geometry
{
	public enum BezierEnd
	{
		Start,
		End
	}

	public static class Intersect
	{
		/*
			Intersect a geometry with a bezier. 
			Assumes that the given bezier end is inside the geometry.
		*/

		public static Point? TryIntersectWithBezier(this IGeometry geometry, CubicBezier bezier, BezierEnd insideEnd)
		{
			var t = bezierEnd(bezier, insideEnd, v => geometry.Contains(v.X, v.Y));
			if (t != null)
				return bezier.AtT(t.Value);
			return null;
		}

		const int DefaultIntersectionIterations = 64;
		const double DefaultIntersectionTolerance = 0.25;

		public static double? bezierEnd(CubicBezier bezier, BezierEnd end, Func<Point, bool> isPointInside)
		{
			double start = end == BezierEnd.Start ? 0.0 : 1.0;
			double fin = end == BezierEnd.Start ? 1 : 0;

			// bisect always returns a valid value.

			return linearBisect(start, fin, DefaultIntersectionIterations,
				(a, b) =>
				{
					var p1 = bezier.AtT(a);
					var p2 = bezier.AtT(b);
					return (p2 - p1).Length() > DefaultIntersectionTolerance;
				},
				(h) =>
				{
					var p = bezier.AtT(h);
					return isPointInside(p);
				}
			)
			;
		}

		static double linearBisect(
			double inside,
			double outside,
			int maxIterations,
			Func<double, double, bool> isSignificantChange,
			Func<double, bool> isInside)
		{
			// we start where we assume an inside hit.

			double current = inside;
			double nextStep = (outside - inside) / 2;

			double? previous = null;

			for (int i = 0; i != maxIterations; ++i)
			{
				// break if we haven't moved a significant amount
				if (previous != null && !isSignificantChange(previous.Value, current))
					break;
				previous = current;

				if (isInside(current))
					current += nextStep;
				else
					current -= nextStep;

				nextStep /= 2.0;
			}

			return current;
		}
	}
}
