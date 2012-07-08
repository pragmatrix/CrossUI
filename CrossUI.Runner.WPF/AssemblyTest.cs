using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;
using CrossUI.Runner.Config;
using CrossUI.Runner.WPF.UI;
using CrossUI.Testing;
using Toolbox;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		public readonly AssemblyTestControl Control;
		readonly LenientFileWatcher _watcher;
		readonly TestResultPresenter _presenter;
		readonly TestScheduler _scheduler;
		readonly Dispatcher _uiDispatcher = Dispatcher.CurrentDispatcher;

		bool _disposed;

		public event Action ConfigChanged;

		public AssemblyTest(AssemblyTestConfiguration config, AssemblyTestControl control)
		{
			_config = config;
			Control = control;

			var path = _config.AssemblyPath;

			control.Title.Content = Path.GetFileName(path);

			_presenter = new TestResultPresenter(config, control);
			_presenter.ClassCollapsed += classCollapsed;
			_presenter.ClassExpanded += classExpanded;

			_scheduler = new TestScheduler(asyncRunTest);

			_watcher = new LenientFileWatcher(
				Path.GetDirectoryName(path), 
				"*.dll");
			_watcher.Changed += _scheduler.schedule;

			_scheduler.schedule();
		}

		void classExpanded(string ns, string className)
		{
			Debug.WriteLine(">>! class expanded");
			_config.classExpanded(ns, className);
			ConfigChanged.raise();
			_scheduler.schedule();
		}

		void classCollapsed(string ns, string className)
		{
			Debug.WriteLine(">>! class collapsed");
			_config.classCollapsed(ns, className);
			ConfigChanged.raise();
		}

		public void Dispose()
		{
			_watcher.Dispose();
			_scheduler.Dispose();
			_disposed = true;
		}

		public AssemblyTestConfiguration Config
		{
			get
			{
				return _config;
			}
		}

		void asyncRunTest()
		{
			var results = runTest();
			_uiDispatcher.BeginInvoke((Action)(() =>
				{
					if (!_disposed)
						_presenter.present(results);
				}
				));
		}

		TestResultAssembly runTest()
		{
			Debug.WriteLine(">>! running test");
			var testRunner = new DomainTestRunner(_config.AssemblyPath);
			return testRunner.run();
		}
	}
}
