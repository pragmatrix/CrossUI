using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace CrossUI.Runner.WPF
{
	sealed class DomainTestRunner
	{
		readonly string _assembly;

		public DomainTestRunner(string assembly)
		{
			_assembly = assembly;
		}

		public void run()
		{
			var evidence = AppDomain.CurrentDomain.Evidence;
			var dir = Path.GetDirectoryName(_assembly);
			var fn = Path.GetFileName(_assembly);

			var friendlyName = "CrossUI.DomainTestRunner.AppDomain." + fn;
			var appDomain = AppDomain.CreateDomain(
				friendlyName, evidence, dir, ".", shadowCopyFiles: true);
			try
			{
				var assembly = appDomain.Load(new AssemblyName(_assembly));
				throw new NotImplementedException("");
				//appDomain.CreateInstanceAndUnwrap();
				//runAssembly(appDomain, assembly);
			}
			finally
			{
				AppDomain.Unload(appDomain);
			}
		}
	}
}
