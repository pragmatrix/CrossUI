using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CrossUI.Runner.WPF.UI;
using CrossUI.Testing;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		public readonly UI.AssemblyTestControl Control;
		public readonly FileWatcher _watcher;
		readonly object _oneTestAtATime = new object();
		readonly Dispatcher _uiDispatcher = Dispatcher.CurrentDispatcher;

		public AssemblyTest(AssemblyTestConfiguration config, UI.AssemblyTestControl control)
		{
			_config = config;
			Control = control;

			var path = _config.AssemblyPath;

			control.Title.Content = Path.GetFileName(path);

			_watcher = new FileWatcher(path);
			_watcher.Changed += queueTestRun;

			queueTestRun();
		}

		public void Dispose()
		{
			_watcher.Dispose();
		}

		public AssemblyTestConfiguration Config
		{
			get
			{
				return _config;
			}
		}

		void queueTestRun()
		{
			ThreadPool.QueueUserWorkItem(s =>
				{
					lock (_oneTestAtATime)
					{
						var results = runTest();
						_uiDispatcher.BeginInvoke((Action)(() => presentTestResults(results)), DispatcherPriority.Normal);
					}
				});
		}

		IEnumerable<TestResult> runTest()
		{
			var testRunner = new DomainTestRunner(_config.AssemblyPath);
			return testRunner.run();
		}

		void presentTestResults(IEnumerable<TestResult> results)
		{
			var controls = results.Select(createResultControl);
			var children = Control.ResultsPanel.Children;
			children.Clear();
			foreach (var c in controls)
				children.Add(c);
		}

		Control createResultControl(TestResult result)
		{
			Debug.Assert(result.Bitmap_ != null || result.Error_ != null);

			var control = new DrawingTestResult
			{
				Title =
				{
					Content = result.Source.Type + "." + result.Source.Method
				}
			};

			if (result.Error_ != null)
			{
				var label = new Label() { Content=result.Error_.Message};
				control.Result.Content = label;
				return control;
			}

			var bitmap = result.Bitmap_;
			Debug.Assert(bitmap != null);

			var bs = BitmapSource.Create(
				bitmap.Width,
				bitmap.Height,
				96,
				96,
				PixelFormats.Pbgra32,
				null,
				bitmap.Data,
				bitmap.Width*4);

			var image = new Image
			{
				Source = bs
			};

			control.Result.Content = image;

			return control;
		}
	}
}
