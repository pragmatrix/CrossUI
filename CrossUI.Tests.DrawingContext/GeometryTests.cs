using System;

namespace CrossUI.Tests.DrawingContext
{

	[BitmapDrawingTest(Width = 80, Height = 40)]
	class GeometryTests
	{
		const int Left = 1;
		const int Top = 1;
		const int Width = 78;
		const int Height = 38;

		public void MultipleGeometryFigures(IGeometryTarget geometry)
		{
			geometry.Line(10, 10, 70, 30);
			geometry.Line(10, 15, 70, 35);
		}

		public void Line(IGeometryTarget geometry)
		{
			geometry.Line(10, 10, 70, 30);
		}

		public void Rect(IGeometryTarget geometry)
		{
			geometry.Rect(Left, Top, Width, Height);
		}

		public void RoundedRect(IGeometryTarget geometry)
		{
			geometry.RoundedRect(Left, Top, Width, Height, 8);
		}

		public void Ellipse(IGeometryTarget target)
		{
			target.Ellipse(Left, Top, Width, Height);
		}

		public void Arc(IGeometryTarget target)
		{
			const double pi = Math.PI;
			target.Arc(Left, Top, Width, Height, 0, pi / 2);
			target.Arc(Left, Top, Width, Height, pi, pi * 1.5);
		}

		public void Bezier(IGeometryTarget target)
		{
			target.Bezier(Left, Top, Width, Top, Left, Height, Width, Height);
		}

		public void Polygon(IGeometryTarget target)
		{
			target.Polygon(new double[] { Left, Top, Width, Height/2, Height/2, Height });
		}
	}
}
