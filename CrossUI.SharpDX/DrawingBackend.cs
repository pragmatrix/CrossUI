using CrossUI.SharpDX.Drawing;
using CrossUI.SharpDX.Geometry;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;

namespace CrossUI.SharpDX
{
	public sealed class DrawingBackend : IDrawingBackend
	{
		internal readonly Factory Factory;
		internal readonly Device1 Device;

		public DrawingBackend()
		{
			Factory = new Factory(FactoryType.SingleThreaded, DebugLevel.None);
			Device = new Device1(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);
		}

		public void Dispose()
		{
			Device.Dispose();
			Factory.Dispose();
		}

		public IBitmapDrawingTarget CreateBitmapDrawingTarget(int width, int height)
		{
			return new BitmapDrawingTarget(this, width, height);
		}

		public IGeometry Geometry(IRecorder<IGeometryTarget> records)
		{
			var path = new PathGeometry(Factory);
			try
			{
				using (var sink = path.Open())
				{
					var target = new GeometryTarget(Factory, sink);
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
