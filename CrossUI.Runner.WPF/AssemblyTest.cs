using System;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		readonly AssemblyTestControl _control;

		public AssemblyTest(AssemblyTestConfiguration config, AssemblyTestControl control)
		{
			_config = config;
			_control = control;
		}

		public void Dispose()
		{
		}
	}
}
