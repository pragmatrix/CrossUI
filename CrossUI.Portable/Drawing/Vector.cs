using System;

namespace CrossUI.Drawing
{
	public struct Vector
	{
		public readonly double X;
		public readonly double Y;

		public Vector(double x, double y)
		{
			X = x;
			Y = y;
		}

		#region Equality members

		public bool Equals(Vector other)
		{
			return X.Equals(other.X) && Y.Equals(other.Y);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is Vector && Equals((Vector)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X.GetHashCode() * 397) ^ Y.GetHashCode();
			}
		}

		public static bool operator ==(Vector left, Vector right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector left, Vector right)
		{
			return !left.Equals(right);
		}

		#endregion

		#region Addition / Subtraction

		public static Vector operator + (Vector left, Vector right)
		{
			return new Vector(left.X + right.X, left.Y + right.Y);
		}

		public static Vector operator -(Vector left, Vector right)
		{
			return new Vector(left.X - right.X, left.Y - right.Y);
		}

		public static Vector operator -(Vector left)
		{
			return new Vector(-left.X, -left.Y);
		}

		#endregion
	}

	public static class VectorExtensions
	{
		public static double Length(this Vector v)
		{
			return Math.Sqrt(v.SquaredLength());
		}

		public static double SquaredLength(this Vector v)
		{
			return v.X * v.X + v.Y * v.Y;
		}
	}

}
