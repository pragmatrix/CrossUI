using System;
using Toolbox;

namespace CrossUI.Runner.WPF
{
	/*
		This class
			- debounces change notifications from the actual test-runs 
			  by introducing a delay.
			- makes the actual test run sequentially.
	*/

	sealed class TestScheduler : IDisposable
	{
		readonly object _oneAtATime = new object();
		readonly Action _asyncTestRunner;

		static readonly Milliseconds DebounceDelay = 50.milliseconds();

		IDisposable _notifier_;

		public TestScheduler(Action asyncTestRunner)
		{
			_asyncTestRunner = asyncTestRunner;
		}

		public void Dispose()
		{
			cancelNotifier();
		}

		public void schedule()
		{
			cancelNotifier();
			_notifier_ = Signal.action(run).onceAfter(DebounceDelay);
		}

		void cancelNotifier()
		{
			if (_notifier_ == null)
				return;
			_notifier_.Dispose();
			_notifier_ = null;
		}

		public void run()
		{
			lock (_oneAtATime)
				_asyncTestRunner();
		}
	}
}
