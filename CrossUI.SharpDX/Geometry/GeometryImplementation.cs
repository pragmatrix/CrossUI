using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	sealed class GeometryImplementation : IGeometry
	{
		public readonly PathGeometry Geometry;

		public GeometryImplementation(PathGeometry geometry)
		{
			Geometry = geometry;
		}

		public void Dispose()
		{
			Geometry.Dispose();
		}

		public Bounds Bounds
		{
			get
			{
				return Geometry.GetBounds().export();
			} 
		}
	}
}
