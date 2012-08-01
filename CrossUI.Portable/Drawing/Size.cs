namespace CrossUI.Drawing
{
	public struct Size
	{
		public readonly Vector Vector;

		public Size(double width, double height)
		{
			Vector = new Vector(width, height);
		}

		public double Width
		{
			get { return Vector.X; }
		}

		public double Height
		{
			get { return Vector.Y; }
		}

		#region Equality members

		public bool Equals(Size other)
		{
			return Vector.Equals(other.Vector);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is Size && Equals((Size)obj);
		}

		public override int GetHashCode()
		{
			return Vector.GetHashCode();
		}

		public static bool operator ==(Size left, Size right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Size left, Size right)
		{
			return !left.Equals(right);
		}

		#endregion
	}

	public static class SizeExtensions
	{
		public static Size ToSize(this Vector vector)
		{
			return new Size(vector.X, vector.Y);
		}
	}
}
