using System.Collections.Generic;

namespace CrossUI.Drawing
{
	sealed class DrawingTransform : IDrawingTransform
	{
		Matrix _current = Matrix.Identity;
		readonly Stack<Matrix> _stack = new Stack<Matrix>();

		public Vector transform(Vector vector)
		{
			return _current.Transform(vector);
		}

		public void SaveTransform()
		{
			_stack.Push(_current);
		}

		public void RestoreTransform()
		{
			_current = _stack.Pop();
		}

		public void Scale(double sx, double sy, double? centerX = null, double? centerY = null)
		{
			Matrix m = centerX != null || centerY != null
				? Matrix.Scaling(sx, sy, centerX ?? 0, centerY ?? 0)
				: Matrix.Scaling(sx, sy);

			_current *= m;
		}

		public void Rotate(double radians, double? centerX = null, double? centerY = null)
		{
			Matrix m = centerX != null || centerY != null
				? Matrix.Rotation(radians, centerX ?? 0, centerY ?? 0)
				: Matrix.Rotation(radians);

			_current *= m;
		}

		public void Translate(double dx, double dy)
		{
			_current = _current.Translated(dx, dy);
		}
	}
}
