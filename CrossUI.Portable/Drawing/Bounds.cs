using System.Globalization;
using CrossUI.Toolbox;

namespace CrossUI.Drawing
{
	public struct Bounds
	{
		public readonly Point LeftTop;
		public readonly Point RightBottom;

		public double Left { get { return LeftTop.X; } }
		public double Top { get { return LeftTop.Y; } }
		public double Right { get { return RightBottom.X; } }
		public double Bottom { get { return RightBottom.Y; } }

		public Bounds(double left, double top, double right, double bottom)
			: this(new Point(left, top), new Point(right, bottom))
		{
		}

		public Bounds(Point leftTop, Point rightBottom)
		{
			LeftTop = leftTop;
			RightBottom = rightBottom;
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

		public Rectangle ToRectangle()
		{
			return new Rectangle(Left, Top, Right - Left, Bottom - Top);
		}
	}
}