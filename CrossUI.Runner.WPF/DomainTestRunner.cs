using System;
using System.IO;
using CrossUI.Testing;

namespace CrossUI.Runner.WPF
{
	[Serializable]
	public sealed class DomainTestRunner
	{
		readonly string _assemblyPath;

		public DomainTestRunner(string assemblyPath)
		{
			_assemblyPath = assemblyPath;
		}

		public TestResult[] run()
		{
			var evidence = AppDomain.CurrentDomain.Evidence;
			var fn = Path.GetFileName(_assemblyPath);

			var friendlyName = "CrossUI.DomainTestRunner.AppDomain." + fn;

			var appDomain = AppDomain.CreateDomain(
				friendlyName, evidence);

			try
			{
				var testRunnerType = typeof (TestRunner);

				var testRunner = (TestRunner)appDomain.CreateInstanceAndUnwrap(
					testRunnerType.Assembly.FullName, 
					testRunnerType.FullName);

				return testRunner.run(_assemblyPath);
			}
			finally
			{
				AppDomain.Unload(appDomain);
			}
		}
	}
}
