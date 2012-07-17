using System;

namespace CrossUI
{
	public interface ICoordinateSpace
	{
		IDisposable SaveTransform();

		void Scale(double sx, double sy, double? centerX = null, double? centerY = null);

		// positive: clockwise, in radians
		void Rotate(double radians, double? centerX = null, double? centerY = null);
	
		void Translate(double dx, double dy);
	}
}
