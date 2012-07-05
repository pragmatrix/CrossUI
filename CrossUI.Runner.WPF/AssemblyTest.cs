using System;
using System.IO;
using System.Threading;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		public readonly UI.AssemblyTestControl Control;
		public readonly FileWatcher _watcher;
		readonly object _testSequencer = new object();

		public AssemblyTest(AssemblyTestConfiguration config, UI.AssemblyTestControl control)
		{
			_config = config;
			Control = control;

			var path = _config.AssemblyPath;

			control.Title.Content = Path.GetFileName(path);

			_watcher = new FileWatcher(path);
			_watcher.Changed += refresh;

			refresh();
		}

		public void Dispose()
		{
			_watcher.Dispose();
		}

		public AssemblyTestConfiguration Config
		{
			get
			{
				return _config;
			}
		}

		void refresh()
		{
			ThreadPool.QueueUserWorkItem(s =>
				{
					lock (_testSequencer)
					{
						runTest();
					}
				});
		}

		void runTest()
		{
			var testRunner = new DomainTestRunner(_config.AssemblyPath);
			testRunner.run();
		}
	}
}
