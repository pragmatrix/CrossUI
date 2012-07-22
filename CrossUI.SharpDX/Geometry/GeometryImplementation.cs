using System;
using CrossUI.Toolbox;
using SharpDX;
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

		public double Area
		{
			get
			{
				return Geometry.ComputeArea();
			}
		}

		public double Length
		{
			get
			{
				return Geometry.ComputeLength();
			}
		}

		public IGeometry Combine(CombineMode mode, IGeometry other)
		{
			var otherImplementation = other.import();

			var combined = Path.Geometry(
				Geometry.Factory, 
				sink => checkResult(Geometry.Combine(otherImplementation, mode.import(), sink)));
		
			return new GeometryImplementation(combined);
		}

		public IGeometry Widen(double strokeWeight)
		{
			var widened = Path.Geometry(
				Geometry.Factory,
				sink => checkResult(Geometry.Widen(strokeWeight.import(), sink)));

			return new GeometryImplementation(widened).Outline();
		}

		public IGeometry Outline()
		{
			var widened = Path.Geometry(
				Geometry.Factory,
				sink => checkResult(Geometry.Outline(sink)));

			return new GeometryImplementation(widened);
		}

		public GeometryRelation Compare(IGeometry geometry)
		{
			return Geometry.Compare(geometry.import()).export();
		}

		public bool Contains(double x, double y)
		{
			return Geometry.FillContainsPoint(Import.Point(x, y));
		}

		static void checkResult(Result rc)
		{
			if (rc != 0)
				throw new Exception("Direct2D error {0}".format(rc));
		}
	}
}

