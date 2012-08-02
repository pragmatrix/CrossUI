namespace CrossUI.Drawing
{
	public struct Rectangle
	{
		public readonly Point Location;
		public readonly Size Size;

		public double X { get { return Location.X; } }
		public double Y { get { return Location.Y; } }
		public double Width { get { return Size.Width; } }
		public double Height { get { return Size.Height; } }

		public Rectangle(double left, double top, double width, double height)
			: this(new Point(left, top), new Size(width, height) )
		{
		}

		public Rectangle(Point location, Size size)
		{
			Location = location;
			Size = size;
		}

		public double Right { get { return Location.X + Size.Width; } }
		public double Bottom { get { return Location.Y + Size.Height; } }

		public Point RightBottom
		{
			get { return (Location.Vector + Size.Vector).ToPoint(); }
		}

		#region Equality members

		public bool Equals(Rectangle other)
		{
			return Location.Equals(other.Location) && Size.Equals(other.Size);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is Rectangle && Equals((Rectangle)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Location.GetHashCode() * 397) ^ Size.GetHashCode();
			}
		}

		public static bool operator ==(Rectangle left, Rectangle right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Rectangle left, Rectangle right)
		{
			return !left.Equals(right);
		}

		#endregion

		public Bounds ToBounds()
		{
			return new Bounds(X, Y, X + Width, Y + Height);
		}
	}
}
