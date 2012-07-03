using System;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Markup;
using CrossUI.Runner.WPF.Properties;

namespace CrossUI.Runner.WPF
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] arguments)
		{
			var app = new App();

			var resource = Resources.Simple_Styles_Default;
			using (var stringReader = new StringReader(resource))
			using (var reader = XmlReader.Create(stringReader))
			{
				app.Resources = (ResourceDictionary)XamlReader.Load(reader);
			}

			var window = new MainWindow();
			using (new AssemblyTester(window))
			{
				app.Run(window);
			}
		}
	}
}
