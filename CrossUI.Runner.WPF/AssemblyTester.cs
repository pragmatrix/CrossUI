using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTester
	{
		readonly MainWindow _window;
		readonly ListBox _testListBox;
		readonly List<AssemblyTest> _tests;

		public AssemblyTester(MainWindow window)
		{
			_window = window;
			_testListBox = _window.Tests;

			var config = Configuration.load();

			config.AssemblyTests.Select(c =>
				{
					var control = new AssemblyTestControl();
					_testListBox.Items.Add(control);
					return new AssemblyTest(c, control);
				}).ToList();

			var newControl = new AssemblyTestNewControl();
			_testListBox.Items.Add(newControl);
		}
	}
}
