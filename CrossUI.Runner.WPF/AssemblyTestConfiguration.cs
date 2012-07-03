using System.Reflection;

namespace CrossUI.Runner.WPF
{
	[Obfuscation]
	sealed class AssemblyTestConfiguration
	{
		public string AssemblyPath;

		public static AssemblyTestConfiguration create(string assemblyPath)
		{
			return new AssemblyTestConfiguration {AssemblyPath = assemblyPath};
		}
	}
}
