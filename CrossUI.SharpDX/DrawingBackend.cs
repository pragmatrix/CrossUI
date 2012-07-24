using CrossUI.SharpDX.Drawing;
using CrossUI.SharpDX.Geometry;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;

namespace CrossUI.SharpDX
{
	public sealed class DrawingBackend : IDrawingBackend
	{
		readonly Factory _factory;
		readonly Device1 _device;

		public DrawingBackend()
		{
			_factory = new Factory(FactoryType.SingleThreaded, DebugLevel.None);
			_device = new Device1(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);
		}

		public void Dispose()
		{
			_device.Dispose();
			_factory.Dispose();
		}

		public IBitmapDrawingTarget CreateBitmapDrawingTarget(int width, int height)
		{
			return new BitmapDrawingTarget(_factory, _device, width, height);
		}

		public IGeometry Geometry(IRecorder<IGeometryTarget> records)
		{
			var path = new PathGeometry(_factory);
			try
			{
				using (var sink = path.Open())
				{
					var target = new GeometryTarget(_factory, sink);
					records.Replay(target);
					target.endOpenFigure();
					sink.Close();
				}

				return new GeometryImplementation(path);
			}
			catch
			{
				path.Dispose();
				throw;
			}
		}
	}
}
