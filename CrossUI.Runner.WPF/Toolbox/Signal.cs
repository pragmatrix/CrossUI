using System;
using System.Timers;
using System.Windows.Threading;

namespace Toolbox
{
	public sealed class Signal : IDisposable
	{
		readonly Dispatcher _dispatcher;
		readonly Action _action;
		readonly Timer _timer;
		bool _disposed;

		Signal(long timeoutMS, Action action, bool autoReset)
		{
			_dispatcher = Dispatcher.CurrentDispatcher;
			_action = action;

			_timer = new Timer {AutoReset = false, Interval = timeoutMS};
			_timer.Elapsed += elapsed;
			_timer.AutoReset = autoReset;
			_timer.Start();
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			_disposed = true;
			_timer.Stop();
			_timer.Elapsed -= elapsed;
			_timer.Dispose();
		}

		void elapsed(object sender, ElapsedEventArgs e)
		{
			Action dispatchedElapsed = () =>
				{
					if (_disposed)
						return;

					_action();
				};

			_dispatcher.BeginInvoke(DispatcherPriority.Normal, dispatchedElapsed);
		}

		#region DSL

		public static SignalAction action(Action action)
		{
			return new SignalAction(action);
		}

		public sealed class SignalAction
		{
			readonly Action _action;

			public SignalAction(Action action)
			{
				_action = action;
			}

			public IDisposable onceAfter(Milliseconds value)
			{
				return new Signal(value.Value, _action, false);
			}

			public IDisposable every(Milliseconds value)
			{
				return new Signal(value.Value, _action, true);
			}
		}

		#endregion
	}
}
