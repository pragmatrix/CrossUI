using System;

namespace CrossUI.Testing
{
	static class Measure
	{
		public static TimeSpan RunningTime(Action action)
		{
			var dt = DateTime.UtcNow;
			action();
			return DateTime.UtcNow - dt;
		}

		public static T RunningTime<T>(out TimeSpan time, Func<T> function)
		{
			T r = default(T);
			time = RunningTime(() =>
				{
					r = function();
				});

			return r;
		}
	}
}
