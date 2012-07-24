using System;
using CrossUI.Drawing;

namespace CrossUI
{
	public interface IGeometry : IDisposable
	{
		Bounds Bounds { get; }
		double Area { get; }
		double Length { get; }
		
		IGeometry Combine(CombineMode mode, IGeometry other);

		IGeometry Widen(double strokeWeight);
		IGeometry Outline();
		IGeometry Transform(Matrix matrix);
		
		GeometryRelation Compare(IGeometry geometry);
		bool Contains(double x, double y);
	}

	public enum CombineMode
	{
		Union,
		Intersect,
		XOR,
		Exclude
	}

	public enum GeometryRelation
	{
		Disjoint,
		IsContained,
		Contains,
		Overlap
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

		public static IGeometry Inflate(this IGeometry source, double inflate)
		{
			var widened = source.Widen(inflate * 2);
			var combined = widened.Combine(CombineMode.Union, source);
			return combined;
		}
	}
}
