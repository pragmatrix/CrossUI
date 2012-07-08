using System.Reflection;

namespace CrossUI.Runner.Config
{
	[Obfuscation]
	sealed class AssemblyTestConfiguration
	{
		public string AssemblyPath { get; set; }

		public static AssemblyTestConfiguration create(string assemblyPath)
		{
			return new AssemblyTestConfiguration {AssemblyPath = assemblyPath};
		}
	}
}
