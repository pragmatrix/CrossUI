namespace CrossUI.Drawing
{
	public struct CubicBezier
	{
		public readonly Point Start;
		public readonly Point Span1;
		public readonly Point Span2;
		public readonly Point End;

		public CubicBezier(Point start, Point span1, Point span2, Point end)
		{
			Start = start;
			Span1 = span1;
			Span2 = span2;
			End = end;
		}

		#region Equality members

		public bool Equals(CubicBezier other)
		{
			return Start.Equals(other.Start) && Span1.Equals(other.Span1) && Span2.Equals(other.Span2) && End.Equals(other.End);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is CubicBezier && Equals((CubicBezier)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Start.GetHashCode();
				hashCode = (hashCode * 397) ^ Span1.GetHashCode();
				hashCode = (hashCode * 397) ^ Span2.GetHashCode();
				hashCode = (hashCode * 397) ^ End.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(CubicBezier left, CubicBezier right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(CubicBezier left, CubicBezier right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}
