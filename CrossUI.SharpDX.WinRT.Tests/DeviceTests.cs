using NUnit.Framework;

namespace CrossUI.SharpDX.WinRT.Tests
{
	/*
		Just some tests to see that basic setup and teardown of the backend works as expected.
	*/
	
	[TestFixture]
    public class DeviceTests
    {
        [Test]
        public void TestDeviceCreation()
        {
			using (var backend = new DrawingBackend())
			{
			}
        }

		[Test]
		public void TestCreateBitmapDrawingTarget()
		{
			using (var backend = new DrawingBackend())
			{
				using (var target = backend.CreateBitmapDrawingTarget(10, 10))
				{
				}
			}
		}

		[Test]
		public void TestCreateBeginDraw()
		{
			using (var backend = new DrawingBackend())
			{
				using (var target = backend.CreateBitmapDrawingTarget(10, 10))
				{
					IDrawingTarget dt;
					using (var drawingSession = target.BeginDraw(out dt))
					{
					}
				}
			}
		}

		[Test]
		public void TestDrawingAndQueryBitmap()
		{
			using (var backend = new DrawingBackend())
			{
				using (var target = backend.CreateBitmapDrawingTarget(10, 10))
				{
					IDrawingTarget dt;
					using (var drawingSession = target.BeginDraw(out dt))
					{
						dt.Line(0, 0, 10, 10);
					}

					var rawBitmap = target.ExtractRawBitmap();
				}
			}
		}
	}
}
