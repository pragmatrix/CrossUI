using System;
using System.IO;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		public readonly UI.AssemblyTestControl Control;

		public AssemblyTest(AssemblyTestConfiguration config, UI.AssemblyTestControl control)
		{
			_config = config;
			Control = control;

			control.Title.Content = Path.GetFileName(_config.AssemblyPath);
		}

		public void Dispose()
		{
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
