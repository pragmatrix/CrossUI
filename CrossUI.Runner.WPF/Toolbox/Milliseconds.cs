namespace Toolbox
{
	public struct Milliseconds
	{
		public Milliseconds(long value)
		{
			Value = value;
		}

		public readonly long Value;

		#region R# generated

		public bool Equals(Milliseconds other)
		{
			return other.Value == Value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (obj.GetType() != typeof (Milliseconds))
				return false;
			return Equals((Milliseconds) obj);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool operator ==(Milliseconds left, Milliseconds right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Milliseconds left, Milliseconds right)
		{
			return left.Equals(right);
		}

		public static bool operator <(Milliseconds left, Milliseconds right)
		{
			return left.Value < right.Value;
		}

		public static bool operator >(Milliseconds left, Milliseconds right)
		{
			return left.Value > right.Value;
		}

		public static bool operator >=(Milliseconds left, Milliseconds right)
		{
			return left.Value >= right.Value;
		}

		public static bool operator <=(Milliseconds left, Milliseconds right)
		{
			return left.Value <= right.Value;
		}

		#endregion
	}

	public static class MillisecondsExtensions
	{
		public static Milliseconds milliseconds(this int value)
		{
			return new Milliseconds(value);
		}

		public static Milliseconds milliseconds(this uint value)
		{
			return new Milliseconds(value);
		}
	}
}
