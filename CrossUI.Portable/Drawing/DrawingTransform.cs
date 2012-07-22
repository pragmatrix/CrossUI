using System;
using System.Collections.Generic;
using CrossUI.Toolbox;

namespace CrossUI.Drawing
{
	public sealed class DrawingTransform : IDrawingTransform
	{
		Matrix _current = Matrix.Identity;
		readonly Stack<Matrix> _stack = new Stack<Matrix>();

		public Matrix Current
		{
			get { return _current; }
		}

		public event Action Changed;

		public Vector Transform(Vector vector)
		{
			return _current.Transform(vector);
		}

		public void SaveTransform()
		{
			_stack.Push(_current);
		}

		public void RestoreTransform()
		{
			change(_stack.Pop());
		}

		public void Scale(double sx, double sy, double? centerX = null, double? centerY = null)
		{
			Matrix m = centerX != null || centerY != null
				? Matrix.Scaling(sx, sy, centerX ?? 0, centerY ?? 0)
				: Matrix.Scaling(sx, sy);

			change(m * _current);
		}

		public void Rotate(double radians, double? centerX = null, double? centerY = null)
		{
			Matrix m = centerX != null || centerY != null
				? Matrix.Rotation(radians, centerX ?? 0, centerY ?? 0)
				: Matrix.Rotation(radians);

			change(m * _current);
		}

		public void Translate(double dx, double dy)
		{
			Matrix m = Matrix.Translation(dx, dy);
			change(m * _current);
		}

		void change(Matrix m)
		{
			_current = m;
			Changed.raise();
		}
	}
}
