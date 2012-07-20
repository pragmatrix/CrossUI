using System.Globalization;
using CrossUI.Toolbox;

namespace CrossUI
{
	public struct Bounds
	{
		public readonly double Left;
		public readonly double Top;
		public readonly double Right;
		public readonly double Bottom;

		public Bounds(double left, double top, double right, double bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public override string ToString()
		{
			return "{0},{1},{2},{3}".format(
				tos(Left), 
				tos(Top), 
				tos(Right), 
				tos(Bottom));
		}

		static string tos(double d)
		{
			return d.ToString(CultureInfo.InvariantCulture);
		}
		
		#region Equality members

		public bool Equals(Bounds other)
		{
			return Left.Equals(other.Left) && Top.Equals(other.Top) && Right.Equals(other.Right) && Bottom.Equals(other.Bottom);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is Bounds && Equals((Bounds) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Left.GetHashCode();
				hashCode = (hashCode*397) ^ Top.GetHashCode();
				hashCode = (hashCode*397) ^ Right.GetHashCode();
				hashCode = (hashCode*397) ^ Bottom.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Bounds left, Bounds right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Bounds left, Bounds right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}