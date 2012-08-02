using System;
using CrossUI.Drawing;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width=80, Height=40)]
	class GeometryQueryTests
	{
		const double Diameter = 20;
		const double Radius = Diameter/2;

		public void Bounds(IDrawingTarget target, IDrawingBackend backend)
		{
			var circle = visualize(target, backend);

			report(target, new Bounds(30, 10, 50, 30).ToString(), circle.Bounds.ToString());
		}

		public void Area(IDrawingTarget target, IDrawingBackend backend)
		{
			var circle = visualize(target, backend);
			report(target, Math.PI * Radius * Radius, circle.Area);
		}

		public void Length(IDrawingTarget target, IDrawingBackend backend)
		{
			var circle = visualize(target, backend);
			report(target, Math.PI * Diameter, circle.Length);
		}

		static IGeometry visualize(IDrawingTarget target, IDrawingBackend backend)
		{
			var circle = backend.Ellipse(30, 10, Diameter, Diameter);

			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(circle);
			return circle;
		}

		static void report(IDrawingTarget target, double expected, double actual)
		{
			report(target, expected.ToString(), actual.ToString());
			var error = actual - expected;
			target.Report(error+ " error");
		}

		static void report(IDrawingTarget target, string expected, string actual)
		{
			target.Report(expected + " expected");
			target.Report(actual + " actual");
		}
	}
}
