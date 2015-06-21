using CrossUI.SharpDX.Drawing;
using CrossUI.SharpDX.Geometry;
using SharpDX.Direct2D1;
#if NETFX_CORE
using SharpDX;
using DriverType = SharpDX.Direct3D.DriverType;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;
using DeviceCreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;
#else
using SharpDX.Direct3D10;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;
using Device = SharpDX.Direct3D10.Device1;
#endif

namespace CrossUI.SharpDX
{
	public sealed class DrawingBackend : IDrawingBackend
	{
		internal readonly Factory Factory;
		internal readonly Device1 Device;

		public DrawingBackend()
		{
			Factory = new Factory(FactoryType.SingleThreaded, DebugLevel.None);
#if NETFX_CORE
			using (var device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport))
			{
				Device = device.QueryInterface<Device1>();
			}
#else
			Device = new Device1(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);
#endif
		}

		public void Dispose()
		{
			Device.Dispose();
			Factory.Dispose();
		}

		public IDrawingSurface CreateBitmapDrawingSurface(int width, int height)
		{
			return new DrawingSurface(this, width, height);
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
