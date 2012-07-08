using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CrossUI.Runner.WPF.UI;
using CrossUI.Testing;
using Toolbox;
using TestResultClass = CrossUI.Testing.TestResultClass;

namespace CrossUI.Runner.WPF
{
	sealed class AssemblyTest : IDisposable
	{
		readonly AssemblyTestConfiguration _config;
		public readonly AssemblyTestControl Control;
		readonly LenientFileWatcher _watcher;
		readonly TestScheduler _scheduler;
		readonly Dispatcher _uiDispatcher = Dispatcher.CurrentDispatcher;

		bool _disposed;

		public AssemblyTest(AssemblyTestConfiguration config, AssemblyTestControl control)
		{
			_config = config;
			Control = control;

			var path = _config.AssemblyPath;

			control.Title.Content = Path.GetFileName(path);

			_scheduler = new TestScheduler(asyncRunTest);

			_watcher = new LenientFileWatcher(
				Path.GetDirectoryName(path), 
				"*.dll");
			_watcher.Changed += _scheduler.schedule;

			_scheduler.schedule();
		}

		public void Dispose()
		{
			_watcher.Dispose();
			_scheduler.Dispose();
			_disposed = true;
		}

		public AssemblyTestConfiguration Config
		{
			get
			{
				return _config;
			}
		}

		void asyncRunTest()
		{
			var results = runTest();
			_uiDispatcher.BeginInvoke((Action)(() =>
				{
					if (!_disposed)
						presentTestResults(results);
				}
				));
		}

		TestResultAssembly runTest()
		{
			Debug.WriteLine(">>! running test");
			var testRunner = new DomainTestRunner(_config.AssemblyPath);
			return testRunner.run();
		}

		void presentTestResults(TestResultAssembly result)
		{
			if (result.Error_ != null)
			{
				showError(Control.AssemblyErrorLabel, result.Error_);
				Control.ClassResults.Children.Clear();
				return;
			}

			hideError(Control.AssemblyErrorLabel);

			var controls = result.Classes_.Select(createClassResultControl);
		
			var children = Control.ClassResults.Children;
			children.Clear();
			foreach (var c in controls)
				children.Add(c);
		}

		static Control createClassResultControl(TestResultClass result)
		{
			Debug.Assert(result.Error_ != null || result.Methods_ != null);

			var control = new UI.TestResultClass
			{
				ClassName = {Content = result.ClassName}
			};

			if (result.Error_ != null)
			{
				showError(control.ErrorLabel, result.Error_);
				return control;
			}

			hideError(control.ErrorLabel);

			foreach (var methodControl in result.Methods_.Select(createMethodResultControl))
			{
				control.Methods.Children.Add(methodControl);
			}

			return control;
		}

		static Control createMethodResultControl(TestResultMethod result)
		{
			Debug.Assert(result.Bitmap_ != null || result.Error_ != null);

			var control = new DrawingTestResultMethod {MethodName= {Content = result.Name}};

			if (result.Error_ != null)
			{
				var label = new Label();
				showError(label, result.Error_);
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

		static void showError(Label label, Exception e)
		{
			var tb = new TextBlock { Text = e.innermost().Message, TextWrapping = TextWrapping.Wrap };
			label.Content = tb;
			label.Foreground = RedBrush;
			label.Visibility = Visibility.Visible;
		}

		static void hideError(Label label)
		{
			label.Content = null;
			label.Visibility = Visibility.Hidden;
		}

		static readonly SolidColorBrush RedBrush = new SolidColorBrush(Colors.Red);
	}
}
