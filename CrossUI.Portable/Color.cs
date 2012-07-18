using CrossUI.Toolbox;

namespace CrossUI
{
	public struct Color
	{
		public Color(double red, double green, double blue, double alpha = 1)
		{
			Red = red;
			Green = green;
			Blue = blue;
			Alpha = alpha;
		}

		public readonly double Red;
		public readonly double Green;
		public readonly double Blue;
		public readonly double Alpha;

		public static Color FromBytes(byte r, byte g, byte b, byte a)
		{
			return new Color(r/255d, g/255d, b/255d, a/255d);
		}

		public byte[] ToBytes()
		{
			return new[]
			{
				toByte(Red), toByte(Green), toByte(Blue), toByte(Alpha)
			};
		}

		public override string ToString()
		{
			return "{0},{1},{2},{3}".format(
				toByte(Red),
				toByte(Green),
				toByte(Blue),
				toByte(Alpha));
		}

		static byte toByte(double v)
		{
			var m = (int)(v * 255d);
			if (m < 0)
				return 0;
			if (m > 255)
				return 255;
			return (byte)m;
		}

		#region Equality members

		public bool Equals(Color other)
		{
			return Red.Equals(other.Red) && Green.Equals(other.Green) && Blue.Equals(other.Blue) && Alpha.Equals(other.Alpha);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj is Color && Equals((Color)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Red.GetHashCode();
				hashCode = (hashCode * 397) ^ Green.GetHashCode();
				hashCode = (hashCode * 397) ^ Blue.GetHashCode();
				hashCode = (hashCode * 397) ^ Alpha.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(Color left, Color right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Color left, Color right)
		{
			return !left.Equals(right);
		}

		#endregion
	}

	public static class ColorExtensions
	{
		// 0: current color, 1: other color.

		public static Color Mixed(this Color _, Color other, double f)
		{
			return new Color(
				interpolate(_.Red, other.Red, f),
				interpolate(_.Green, other.Green, f),
				interpolate(_.Blue, other.Blue, f),
				interpolate(_.Alpha, other.Alpha, f))
			;
		}

		// Mixes the current color with a black that has the same alpha.

		public static Color Darkened(this Color _, double f)
		{
			var black = new Color(0, 0, 0, _.Alpha);
			return _.Mixed(black, f);
		}

		// Mixes the current color with a white that has the same alpha.

		public static Color Lightened(this Color _, double f)
		{
			var white = new Color(1, 1, 1, _.Alpha);
			return _.Mixed(white, f);
		}

		static double interpolate(double a, double b, double f)
		{
			return clamp(a * (1.0 - f) + b * f, 0, 1);
		}

		static double clamp(double b, double low, double high)
		{
			return b < low ? low : (b > high ? high : b);
		}
	}

	public static class Colors
	{
		public static readonly Color Black = new Color(0, 0, 0);
		public static readonly Color White = new Color(1, 1, 1);
	}
}
