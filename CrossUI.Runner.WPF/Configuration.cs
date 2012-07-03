using System;
using System.Collections.Generic;
using System.IO;
using ServiceStack.Text;

namespace CrossUI.Runner.WPF
{
	sealed class Configuration
	{
		public Configuration()
		{
			AssemblyTests = new List<AssemblyTestConfiguration>();
		}

		public List<AssemblyTestConfiguration> AssemblyTests;

		public void store()
		{
			var contents = TypeSerializer.SerializeToString(this);
			File.WriteAllText(ConfigurationPath, contents);
		}

		public static Configuration load()
		{
			if (!File.Exists(ConfigurationPath))
				return new Configuration();

			var contents = File.ReadAllText(ConfigurationPath);
			return TypeSerializer.DeserializeFromString<Configuration>(contents);
		}

		static readonly string ConfigurationPath = makeConfigurationPath();
		const int Version = 1;

		static string makeConfigurationPath()
		{
			var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			return Path.Combine(root, "Configuration." + Version + ".json");
		}
	}
}
