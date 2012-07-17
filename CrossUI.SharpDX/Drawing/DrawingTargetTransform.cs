using System;
using CrossUI.Toolbox;
using SharpDX;

namespace CrossUI.SharpDX.Drawing
{
	partial class RenderTargetDrawingContext
	{
		public IDisposable SaveTransform()
		{
			var transform = _target.Transform;
			return new DisposeAction(() =>
				{
					_target.Transform = transform;
				});
		}

		public void Scale(double sx, double sy, double? centerX = null, double? centerY = null)
		{
			var scale = new Matrix3x2()
			{
				M11 = sx.import(),
				M22 = sy.import()
			};

			if (centerX == null && centerY == null)
			{
				transform(scale);
				return;
			}

			double cx = centerX ?? 0;
			double cy = centerY ?? 0;

			transformCentered(scale, cx, cy);
		}

		public void Rotate(double radians, double? centerX = null, double? centerY = null)
		{
			// note: y-is inverted, so we invert the sin-result.

			var cos = Math.Cos(radians).import();
			var sin = -Math.Sin(radians).import();

			var rot = new Matrix3x2()
			{
				M11 = cos,
				M12 = -sin,
				M21 = sin,
				M22 = cos
			};

			if (centerX == null && centerY == null)
			{
				transform(rot);
				return;
			}

			double cx = centerX ?? 0;
			double cy = centerY ?? 0;

			transformCentered(rot, cx, cy);
		}

		public void Translate(double dx, double dy)
		{
			var m = _target.Transform;
			m.M31 += dx.import();
			m.M32 += dy.import();
			_target.Transform = m;
		}

		void transform(Matrix3x2 r)
		{
			var l = _target.Transform;
			_target.Transform = multiply(l, r);
		}

		void transformCentered(Matrix3x2 mtx, double cx, double cy)
		{
			var m = Matrix3x2.Identity;
			m = translate(m, -cx, -cy);
			m = multiply(m, mtx);
			m = translate(m, cx, cy);
			transform(m);
		}

		static Matrix3x2 multiply(Matrix3x2 l, Matrix3x2 r)
		{
			return new Matrix3x2()
			{
				// l.M13 = 0
				// l.M23 = 0
				// l.M33 = 1
				
				// l: row1, r:column 1
				M11 = l.M11 * r.M11 + l.M12 * r.M21 /* + (l.M13) * r.M31*/,
				// l: row1, r:column 2
				M12 = l.M11 * r.M12 + l.M12 * r.M22 /* + (l.M13) * r.M32 */,
				// l: row2, r:column 1
				M21 = l.M21 * r.M11 + l.M22 * r.M21 /* + (l.M23) * r.M31 */,
				// l: row2, r:column 2
				M22 = l.M21 * r.M12 + l.M22 * r.M22 /* + (l.M23) * r.M32 */,
				// l: row3, r: column 1
				M31 = l.M31 * r.M11 + l.M32 * r.M21 + /* l.M33 * */ r.M31,
				// l: row3, r: column 2
				M32 = l.M31 * r.M12 + l.M32 * r.M22 + /* l.M33 * */ r.M32
			};
		}

		static Matrix3x2 translate(Matrix3x2 l, double dx, double dy)
		{
			Matrix3x2 m = l;
			m.M31 += dx.import();
			m.M32 += dy.import();
			return m;
		}
	}
}
