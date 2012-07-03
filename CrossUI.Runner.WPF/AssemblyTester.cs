using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTester
	{
		readonly MainWindow _window;
		readonly ListBox _testListBox;
		readonly List<AssemblyTest> _tests = new List<AssemblyTest>();

		public AssemblyTester(MainWindow window)
		{
			_window = window;
			_testListBox = _window.Tests;

			var config = Configuration.load();

			var newControl = new AssemblyTestNewControl();
			newControl.AddTestButton.Click += addTest;
			_testListBox.Items.Add(newControl);

			foreach (var c in config.AssemblyTests)
			{
				addTest(c);
			}
		}

		void addTest(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				Filter = ".NET Assemblies|*.exe;*.dll|All Files|*.*", 
			};

			bool? res = dialog.ShowDialog();
			if (res == null || !res.Value)
				return;

			var config = AssemblyTestConfiguration.create(dialog.FileName);
			addTest(config);
		}

		void addTest(AssemblyTestConfiguration config)
		{
			var control = new AssemblyTestControl();
			var test = new AssemblyTest(config, control);

			var insertPos = _testListBox.Items.Count - 1;

			_testListBox.Items.Insert(insertPos, control);
			_tests.Add(test);
		}
	}
}