using System;
using System.Diagnostics;
using Toolbox;

namespace CrossUI.Runner.WPF
{
	/*
		A FileWatcher that handles startup and processing errors and restarts automatically.

		Nice, the underlying pattern is a generic toolbox candidate. (Watchdog, LenientActivator?)
	*/

	sealed class LenientFileWatcher : IDisposable
	{
		readonly string _directory;
		readonly string _filter;
		readonly IDisposable _watchdog;

		FileWatcher _watcher_;
		Exception _exception_;

		public event Action Changed;
		public event Action ActivationChanged;

		public Exception Error_
		{
			get { return _exception_; }
		}

		public LenientFileWatcher(string directory, string filter)
		{
			_directory = directory;
			_filter = filter;

			tryBegin();

			_watchdog = Signal.action(watchdog).every(2000.milliseconds());
		}


		public void Dispose()
		{
			_watchdog.Dispose();

			end();
		}

		void watchdog()
		{
			// check if the watcher is running
			if (_exception_ == null)
				return;

			Debug.Assert(_watcher_ == null);
			tryBegin();

			ActivationChanged.raise();
		}

		void tryBegin()
		{
			end();

			Debug.Assert(_watcher_ == null && _exception_ == null);

			try
			{
				_watcher_ = new FileWatcher(_directory, _filter);
				_watcher_.Changed += () => Changed.raise();
				_watcher_.Error += error;
			}
			catch (Exception e)
			{
				_exception_ = e;
			}
		}

		void end()
		{
			if (_watcher_ == null)
			{
				_exception_ = null;
				return;
			}

			Debug.Assert(_exception_ == null);
			_watcher_.Dispose();
			_watcher_ = null;
		}


		void error(Exception e)
		{
			if (_watcher_ == null)
				return;

			end();
			_exception_ = e;

			ActivationChanged.raise();
		}

	}
}
