namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width=80, Height=40)]
	class GeometryTransformTests
	{
		public void BaseShape(IDrawingTarget target, IDrawingBackend backend)
		{
			var geometry = makeGeometry(backend);
			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(geometry);
		}

		public void Outlined(IDrawingTarget target, IDrawingBackend backend)
		{
			var geometry = makeGeometry(backend);
			var widened = geometry.Outline();

			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(widened);
		}

		public void Widened(IDrawingTarget target, IDrawingBackend backend)
		{
			var geometry = makeGeometry(backend);
			var widened = geometry.Widen(6);
	
			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(widened);
		}

		public void Inflated(IDrawingTarget target, IDrawingBackend backend)
		{
			var geometry = makeGeometry(backend);
			var inflated = geometry.Inflate(3);

			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(inflated);
		}

		static IGeometry makeGeometry(IDrawingBackend backend)
		{
			var geometry = backend.Geometry(gt =>
				{
					gt.Line(5, 5, 75, 35);
					gt.Ellipse(25, 5, 30, 30);
				});

			return geometry;
		}
	}
}
