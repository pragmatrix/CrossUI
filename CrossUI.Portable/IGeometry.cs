using System;

namespace CrossUI
{
	public enum CombineMode
	{
		Union,
		Intersect,
		XOR,
		Exclude
	}

	public interface IGeometry : IDisposable
	{
		Bounds Bounds { get; }
		IGeometry Combine(CombineMode mode, IGeometry other);
	}

	public static class GeometryExtensions
	{
		public static IGeometry Union(this IGeometry _, IGeometry other)
		{
			return _.Combine(CombineMode.Union, other);
		}

		public static IGeometry Intersect(this IGeometry _, IGeometry other)
		{
			return _.Combine(CombineMode.Intersect, other);
		}

		public static IGeometry XOR(this IGeometry _, IGeometry other)
		{
			return _.Combine(CombineMode.XOR, other);
		}

		public static IGeometry Exclude(this IGeometry _, IGeometry other)
		{
			return _.Combine(CombineMode.Exclude, other);
		}
	}
}
