using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CrossUI.Runner.Config;
using CrossUI.Runner.WPF.UI;
using Toolbox;
using CrossUI.Testing;
using TestResultClass = CrossUI.Testing.TestResultClass;

namespace CrossUI.Runner.WPF
{
	sealed class TestResultPresenter
	{
		readonly AssemblyTestConfiguration _config;
		readonly AssemblyTestControl _control;

		public event Action<string, string> ClassCollapsed;
		public event Action<string, string> ClassExpanded;

		public TestResultPresenter(AssemblyTestConfiguration config, UI.AssemblyTestControl control)
		{
			_config = config;
			_control = control;
		}

		void connectClassExpander(TestResultClass result, UI.TestResultClass ui)
		{
			var expander = ui.Expander;

			expander.Expanded += (s, args) => classExpanded(result, ui);
			expander.Collapsed += (s, args) => classCollapsed(result, ui);
		}

		void classCollapsed(TestResultClass result, UI.TestResultClass ui)
		{
			ui.ErrorLabel.Visibility = Visibility.Collapsed;
			ui.Methods.Visibility = Visibility.Collapsed;

			ClassCollapsed.raise(result.Namespace, result.ClassName);
		}

		void classExpanded(TestResultClass result, UI.TestResultClass ui)
		{
			ui.ErrorLabel.Visibility = Visibility.Visible;
			ui.Methods.Visibility = Visibility.Visible;

			ClassExpanded.raise(result.Namespace, result.ClassName);
		}

		public void present(TestResultAssembly result)
		{
			if (result.Error_ != null)
			{
				showError(_control.AssemblyErrorLabel, result.Error_);
				_control.ClassResults.Children.Clear();
				return;
			}

			hideError(_control.AssemblyErrorLabel);

			var controls = result.Classes_.Select(createClassResultControl);

			var children = _control.ClassResults.Children;
			children.Clear();
			foreach (var c in controls)
				children.Add(c);
		}

		Control createClassResultControl(TestResultClass result)
		{
			Debug.Assert(result.Error_ != null || result.Methods_ != null);

			var control = new UI.TestResultClass
			{
				ClassName = { Content = result.ClassName }
			};

			hideError(control.ErrorLabel);

			if (_config.isClassCollapsed(result.Namespace, result.ClassName))
			{
				control.Expander.IsExpanded = false;
				connectClassExpander(result, control);
				return control;
			}
			
			control.Expander.IsExpanded = true;
			connectClassExpander(result, control);

			if (result.Error_ != null)
			{
				showError(control.ErrorLabel, result.Error_);
				return control;
			}

			foreach (var methodControl in result.Methods_.Select(createMethodResultControl))
			{
				control.Methods.Children.Add(methodControl);
			}

			return control;
		}

		static Control createMethodResultControl(TestResultMethod result)
		{
			Debug.Assert(result.Bitmap_ != null || result.Error_ != null);

			var control = new UI.DrawingTestResultMethod { MethodName = { Content = result.Name } };

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
				bitmap.Width * 4);

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
