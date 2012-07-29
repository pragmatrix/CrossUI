using System;
using CrossUI.Drawing;
using CrossUI.Toolbox;

namespace CrossUI
{
	public interface IDrawingTransform
	{
		void SaveTransform();
		void RestoreTransform();

		void Scale(double sx, double sy, double? centerX = null, double? centerY = null);

		// positive: clockwise, in radians
		void Rotate(double radians, double? centerX = null, double? centerY = null);
	
		void Translate(double dx, double dy);
	}

	public static class DrawingTransformExtensions
	{
		public static IDisposable PushTransform(this IDrawingTransform space)
		{
			space.SaveTransform();
			return new DisposeAction(space.RestoreTransform);
		}

		public static void Scale(this IDrawingTransform _, double sx, double sy, Point? center = null)
		{
			_.Scale(sx, sy, center != null ? center.Value.X : (double?)null, center != null ? center.Value.Y : (double?)null);
		}

		public static void Rotate(this IDrawingTransform _, double radians, Point? center = null)
		{
			_.Rotate(radians, center != null ? center.Value.X : (double?)null, center != null ? center.Value.Y : (double?)null);
		}

		public static void Translate(this IDrawingTransform _, Vector delta)
		{
			_.Translate(delta.X, delta.Y);
		}
	}
}
