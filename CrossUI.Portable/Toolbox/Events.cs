using System;

namespace CrossUI.Toolbox
{
	// http://stackoverflow.com/questions/786383/c-sharp-events-and-thread-safety

	public static class EventExtensions
	{
		public static void raise(this Action action)
		{
			if (action != null)
				action();
		}

		public static void raise<T1>(this Action<T1> action, T1 value1)
		{
			if (action != null)
				action(value1);
		}

		public static void raise<T1, T2>(this Action<T1, T2> action, T1 value1, T2 value2)
		{
			if (action != null)
				action(value1, value2);
		}
	}
}
