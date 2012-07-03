using System;
using System.Collections.Generic;
using System.Windows.Controls;
using CrossUI.Runner.WPF.UI;
using Microsoft.Win32;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTester : IDisposable
	{
		readonly MainWindow _window;
		readonly StackPanel _testPanel;
		readonly List<AssemblyTest> _tests = new List<AssemblyTest>();

		public AssemblyTester(MainWindow window)
		{
			_window = window;
			_testPanel = _window.Tests;

			var config = Configuration.load();

			var newControl = new AssemblyTestNewControl();
			newControl.AddTestButton.Click += (s, e) => addTestUser();
			_testPanel.Children.Add(newControl);

			foreach (var c in config.AssemblyTests)
			{
				addTest(c);
			}
		}

		public void Dispose()
		{
			while (_tests.Count != 0)
				removeTest(_tests[_tests.Count - 1]);
		}

		void addTestUser()
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

			storeConfig();
		}

		void addTest(AssemblyTestConfiguration config)
		{
			var control = new AssemblyTestControl();
			var test = new AssemblyTest(config, control);

			_tests.Add(test);
			var insertPos = _testPanel.Children.Count - 1;
			_testPanel.Children.Insert(insertPos, test.Control);

			control.RemoveButton.Click += (sender, e) => removeTestUser(test);
		}

		void removeTestUser(AssemblyTest test)
		{
			removeTest(test);
			storeConfig();
		}

		void removeTest(AssemblyTest test)
		{
			_testPanel.Children.Remove(test.Control);
			_tests.Remove(test);

			test.Dispose();
		}

		void storeConfig()
		{
			var config = createConfig();
			config.store();
		}

		Configuration createConfig()
		{
			var config = new Configuration();
			foreach (var test in _tests)
			{
				config.AssemblyTests.Add(test.Config);
			}
			return config;
		}
	}
}