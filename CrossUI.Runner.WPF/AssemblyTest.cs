using System;
using System.IO;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		public readonly UI.AssemblyTestControl Control;
		public readonly FileWatcher _watcher;

		public AssemblyTest(AssemblyTestConfiguration config, UI.AssemblyTestControl control)
		{
			_config = config;
			Control = control;

			var path = _config.AssemblyPath;

			control.Title.Content = Path.GetFileName(path);

			_watcher = new FileWatcher(path);
			_watcher.Changed += refresh;
		}

		public void Dispose()
		{
			_watcher.Dispose();
		}

		void refresh()
		{
			var x = 10;
		}

		public AssemblyTestConfiguration Config
		{
			get
			{
				return _config;	
			}
		}
	}
}
