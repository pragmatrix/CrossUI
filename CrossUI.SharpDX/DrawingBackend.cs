using System;
using CrossUI.Drawing;
using CrossUI.SharpDX.Drawing;
using CrossUI.SharpDX.Geometry;
using SharpDX;
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
		readonly bool _owningFactoryAndDevice;

		static Factory CreateFactory()
		{
			return new Factory(FactoryType.SingleThreaded, DebugLevel.None);
		}

		static Device1 CreateDevice()
		{
#if NETFX_CORE
			using (var device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport))
			{
				return device.QueryInterface<Device1>();
			}
#else
			return new Device1(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);
#endif
		}

		public DrawingBackend()
			: this(CreateFactory(), CreateDevice(), true)
		{
		}

		DrawingBackend(Factory factory, Device1 device, bool ownsIt)
		{
			Factory = factory;
			Device = device;
			_owningFactoryAndDevice = ownsIt;
		}

		public void Dispose()
		{
			if (!_owningFactoryAndDevice)
				return;
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

		public static IDrawingTarget CreateDrawingTarget(RenderTarget renderTarget)
		{
			var width = renderTarget.Size.Width;
			var height = renderTarget.Size.Height;

			var state = new DrawingState();
			var transform = new DrawingTransform();
			var drawingTarget = new DrawingTarget(state, transform, renderTarget, (int)Math.Floor(width), (int)Math.Floor(height));

			var target = new DrawingTargetSplitter(
				null /* no backend */,
				state,
				transform,
				drawingTarget,
				drawingTarget,
				drawingTarget,
				drawingTarget,
				drawingTarget,
				() =>
				{
					drawingTarget.Dispose();
				});

			var pixelAligner = PixelAligningDrawingTarget.Create(target, target.Dispose, state, transform);
			return pixelAligner;
		}

		public static IDrawingBackend FromFactoryAndDevice(Factory factory, Device1 device)
		{
			return new DrawingBackend(factory, device, false);
		}
	}
}
