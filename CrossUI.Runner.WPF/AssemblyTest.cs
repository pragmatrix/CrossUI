using System;

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
		}

		public void Dispose()
		{
		}
	}
}
