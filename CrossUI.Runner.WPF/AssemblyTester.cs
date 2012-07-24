using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CrossUI.Runner.Config;
using CrossUI.Runner.WPF.Config;
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

			if (config.WindowRect_ != null)
			{
				var wr = config.WindowRect_;
				_window.Left = wr.Left;
				_window.Top = wr.Top;
				_window.Width = wr.Width;
				_window.Height = wr.Height;
				_window.WindowStartupLocation = WindowStartupLocation.Manual;
			}

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
			storeConfig();

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

			var config = AssemblyTestConfiguration.create(dialog.FileName, Enumerable.Empty<CollapsedClass>());
			addTest(config);

			storeConfig();
		}

		void addTest(AssemblyTestConfiguration config)
		{
			var control = new AssemblyTestControl();
			var test = new AssemblyTest(config, control);
			test.ConfigChanged += storeConfig;

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
			Debug.WriteLine(">>! storing config");
			var config = createConfig();
			config.store();
		}

		Configuration createConfig()
		{
			var config = new Configuration();
			var rect = new WindowRect
				{
					Left = (int) _window.Left,
					Top = (int) _window.Top,
					Width = (int) _window.ActualWidth,
					Height = (int) _window.ActualHeight
				};

			config.WindowRect_ = rect;
			
			foreach (var test in _tests)
			{
				config.AssemblyTests.Add(test.Config);
			}
			return config;
		}
	}
}