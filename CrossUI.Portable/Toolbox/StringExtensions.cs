using System;

namespace CrossUI.Toolbox
{
	public static class StringExtensions
	{
		public static string format(this string format, params object[] objects)
		{
			// very important: if there is no formatting intended, use the format literally.
			// Otherwise output that contains {} may break formatting.

			if (objects.Length == 0)
				return format;

			try
			{
				return string.Format(format, objects);
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public static string quote(this string s, char l, char r)
		{
			return l + s + r;
		}

		public static string quote(this string s, char c)
		{
			return s.quote(c, c);
		}

		public static string quote(this string s)
		{
			return s.quote('\"');
		}

		public static string quoteNice(this string s)
		{
			return s.quote('\u201c', '\u201d');
		}

		public static string lexicalConcat(this string l, string r)
		{
			if (l == "" || r == "")
				return l + r;

			return l + ", " + r;
		}
	}
}