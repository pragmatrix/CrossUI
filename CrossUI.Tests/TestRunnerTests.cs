using System;
using CrossUI;
using CrossUI.Testing;

[assembly: DrawingBackend(typeof(CrossUI.SharpDX.DrawingBackend))]

namespace CrossUI.Tests
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	public sealed class RoundedRectangleTest
	{
		public void RoundedRect(IDrawingTarget target)
		{
			target.RoundedRect(0, 0, 80, 40, 8);
		}

		public void ThickRect(IDrawingTarget target)
		{
			target.Stroke(weight: 3);
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
		}

		public void ColoredRect(IDrawingTarget target)
		{
			target.Stroke(new Color(1, 0, 0));
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
		}

		[BitmapDrawingTest(Width = 120)]
		public void LargerRect(IDrawingTarget target)
		{
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
		}

		public void ShouldBeIgnored()
		{
		}

		public void Report(IDrawingTarget target)
		{
			target.RoundedRect(0, 0, target.Width, target.Height, 8);
			target.Report("Report A");
			target.Report("Report B");
		}
	}

	public sealed class MethodErrorTest
	{
		[BitmapDrawingTest(Width=80,Height=40)]
		public void Error(IDrawingTarget target)
		{
			throw new Exception("Method Error");
		}

		[BitmapDrawingTest]
		public void InvalidParameters()
		{
		}
	}

	public sealed class PrivateConstructorTest
	{
		private PrivateConstructorTest()
		{
		}

		[BitmapDrawingTest]
		public void ShouldBeIgnored()
		{
		}
	}
}
