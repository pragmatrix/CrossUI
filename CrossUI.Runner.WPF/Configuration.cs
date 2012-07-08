using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ServiceStack.Text;

namespace CrossUI.Runner.WPF
{
	[Obfuscation]
	sealed class Configuration
	{
		public Configuration()
		{
			AssemblyTests = new List<AssemblyTestConfiguration>();
		}

		public List<AssemblyTestConfiguration> AssemblyTests { get; set; }

		public void store()
		{
			var contents = JsonSerializer.SerializeToString(this);
			File.WriteAllText(ConfigurationPath, contents);
		}

		public static Configuration load()
		{
			if (!File.Exists(ConfigurationPath))
				return new Configuration();

			var contents = File.ReadAllText(ConfigurationPath);
			return JsonSerializer.DeserializeFromString<Configuration>(contents);
		}

		static readonly string ConfigurationPath = makeConfigurationPath();
		
		// 2: accidentally serialized jsv instead of json
		//    roaming configuration does not make sense when we store paths.
		const int Version = 2;

		static string makeConfigurationPath()
		{
			var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var app = Path.Combine(root, "CrossUI.Runner");
			Directory.CreateDirectory(app);
			return Path.Combine(app, "Configuration." + Version + ".json");
		}
	}
}
