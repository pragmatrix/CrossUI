using System;
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
	}
}
