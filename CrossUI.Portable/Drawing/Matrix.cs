using System;

namespace CrossUI.Drawing
{
	struct Matrix
	{
		public readonly double M11;
		public readonly double M12;
		public readonly double M21;
		public readonly double M22;
		public readonly double M31;
		public readonly double M32;

		public Matrix(
			double m11, double m12,
			double m21, double m22,
			double m31, double m32)
		{
			M11 = m11;
			M12 = m12;
			M21 = m21;
			M22 = m22;
			M31 = m31;
			M32 = m32;
		}

		public Vector Transform(Vector v)
		{
			return new Vector(
				v.X * M11 + v.Y * M21 + M31,
				v.Y * M12 + v.Y * M22 + M32);
		}

		public Matrix Translated(double dx, double dy)
		{
			return new Matrix(M11, M12, M21, M22, M31 + dx, M32 + dy);
		}

		public static Matrix operator *(Matrix l, Matrix r)
		{
			return new Matrix(
				// l.M13 = 0
				// l.M23 = 0
				// l.M33 = 1

				// l: row1, r:column 1
				l.M11 * r.M11 + l.M12 * r.M21 /* + (l.M13) * r.M31*/,
				// l: row1, r:column 2
				l.M11 * r.M12 + l.M12 * r.M22 /* + (l.M13) * r.M32 */,
				// l: row2, r:column 1
				l.M21 * r.M11 + l.M22 * r.M21 /* + (l.M23) * r.M31 */,
				// l: row2, r:column 2
				l.M21 * r.M12 + l.M22 * r.M22 /* + (l.M23) * r.M32 */,
				// l: row3, r: column 1
				l.M31 * r.M11 + l.M32 * r.M21 + /* l.M33 * */ r.M31,
				// l: row3, r: column 2
				l.M31 * r.M12 + l.M32 * r.M22 + /* l.M33 * */ r.M32
			);
		}

		public static Matrix Scaling(double sx, double sy)
		{
			return new Matrix(
				sx, 0,
				0, sy,
				0, 0
				);
		}

		public static Matrix Scaling(double sx, double sy, double cx, double cy)
		{
			return centered(cx, cy, Scaling(sx, sy));
		}

		public static Matrix Rotation(double radians)
		{
			var cos = Math.Cos(radians);
			var sin = -Math.Sin(radians);

			return new Matrix(
				cos, -sin,
				sin, cos,
				0, 0);
		}

		public static Matrix Rotation(double radians, double cx, double cy)
		{
			return centered(cx, cy, Rotation(radians));
		}

		static Matrix centered(double cx, double cy, Matrix mtx)
		{
			return Translation(-cx, -cy) * mtx * Translation(cx, cy);
		}

		public static Matrix Translation(double dx, double dy)
		{
			return new Matrix(
				1, 0,
				0, 1,
				dx, dy);
		}

		public static readonly Matrix Identity = new Matrix(
			1, 0,
			0, 1,
			0, 0);
	}
}
