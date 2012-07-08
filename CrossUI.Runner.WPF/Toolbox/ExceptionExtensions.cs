using System;

namespace Toolbox
{
	public static class ExceptionExtensions
	{
		public static Exception innermost(this Exception e)
		{
			while (e.InnerException != null)
				e = e.InnerException;

			return e;
		}
	}
}
