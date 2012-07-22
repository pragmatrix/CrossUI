using CrossUI.SharpDX.Drawing;
using CrossUI.SharpDX.Geometry;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX
{
	public sealed class DrawingBackend : IDrawingBackend
	{
		readonly Factory _factory;

		public DrawingBackend()
		{
			_factory = new Factory(FactoryType.SingleThreaded, DebugLevel.None);
		}

		public void Dispose()
		{
			_factory.Dispose();
		}

		public IBitmapDrawingTarget CreateBitmapDrawingTarget(int width, int height)
		{
			return new BitmapDrawingTarget(_factory, width, height);
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
					target.end();
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
