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

		public void Widened(IDrawingTarget target, IDrawingBackend backend)
		{
			var geometry = makeGeometry(backend);
			var widened = geometry.Widen(6);
	
			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(widened);
		}

		public void WidenedAndOutlined(IDrawingTarget target, IDrawingBackend backend)
		{
			var geometry = makeGeometry(backend);
			var widened = geometry.Widen(6);
			var outlined = widened.Outline();

			target.Fill(color: new Color(0.7, 0.7, 1));
			target.Geometry(outlined);
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
			var line = backend.Line(5, 5, 75, 35);
			var ellipse = backend.Ellipse(25, 5, 30, 30);
			var geometry = line.Union(ellipse);
			return geometry;
		}
	}
}
