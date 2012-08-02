using System;

namespace CrossUI.Drawing
{
	public struct Point
	{
		public readonly Vector Vector;

		public Point(double left, double top)
			: this(new Vector(left, top))
		{
		}

		public Point(Vector v)
		{
			Vector = v;
		}

		public double Left
		{
			get { return Vector.X; }
		}

		public double Top
		{
			get { return Vector.Y; }
		}

		public double X
		{
			get { return Left; }
		}

		public double Y
		{
			get { return Top; }
		}

		public static Vector operator - (Point l, Point r)
		{
			return new Vector(l.X - r.X, l.Y - r.Y);
		}

		#region Equality members

		public bool Equals(Point other)
		{
			return Vector.Equals(other.Vector);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is Point && Equals((Point) obj);
		}

		public override int GetHashCode()
		{
			return Vector.GetHashCode();
		}

		public static bool operator ==(Point left, Point right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right)
		{
			return !left.Equals(right);
		}

		#endregion
	}

	public static class PointExtensions
	{
		public static Point ToPoint(this Vector v)
		{
			return new Point(v);
		}

		public static double[] ToPairs(this Point[] points)
		{
			var pairs = new double[points.Length * 2];
			for (int i = 0; i != points.Length; ++i)
			{
				var p = points[i];
				pairs[i * 2] = p.X;
				pairs[i * 2 + 1] = p.Y;
			}

			return pairs;
		}

		public static Point[] ToPoints(this double[] pairs)
		{
			var points = new Point[pairs.Length/2];
			var pi = 0;
			for (int i = 0; i != pairs.Length; i += 2)
			{
				points[pi++] = new Point(pairs[i], pairs[i + 1]);
			}

			return points;
		}
	}
}
