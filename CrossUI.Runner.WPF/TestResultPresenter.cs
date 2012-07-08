using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Toolbox;
using CrossUI.Testing;

namespace CrossUI.Runner.WPF
{
	static class TestResultPresenter
	{
		public static void present(UI.AssemblyTestControl control, TestResultAssembly result)
		{
			if (result.Error_ != null)
			{
				showError(control.AssemblyErrorLabel, result.Error_);
				control.ClassResults.Children.Clear();
				return;
			}

			hideError(control.AssemblyErrorLabel);

			var controls = result.Classes_.Select(createClassResultControl);

			var children = control.ClassResults.Children;
			children.Clear();
			foreach (var c in controls)
				children.Add(c);
		}

		static Control createClassResultControl(TestResultClass result)
		{
			Debug.Assert(result.Error_ != null || result.Methods_ != null);

			var control = new UI.TestResultClass
			{
				ClassName = { Content = result.ClassName }
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
