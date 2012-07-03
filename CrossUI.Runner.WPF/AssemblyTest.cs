using System;
using System.IO;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		readonly UI.AssemblyTestControl _control;

		public AssemblyTest(AssemblyTestConfiguration config, UI.AssemblyTestControl control)
		{
			_config = config;
			_control = control;

			control.Title.Content = Path.GetFileName(_config.AssemblyPath);
		}

		public void Dispose()
		{
		}
	}
}
