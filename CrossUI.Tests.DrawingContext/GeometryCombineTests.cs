namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width=80, Height=40)]
	class GeometryCombineTests
	{
		public void Union(IDrawingTarget target, IDrawingBackend backend)
		{
			test(CombineMode.Union, target, backend);
		}

		public void Intersect(IDrawingTarget target, IDrawingBackend backend)
		{
			test(CombineMode.Intersect, target, backend);
		}

		public void XOR(IDrawingTarget target, IDrawingBackend backend)
		{
			test(CombineMode.XOR, target, backend);
		}

		public void Exclude(IDrawingTarget target, IDrawingBackend backend)
		{
			test(CombineMode.Exclude, target, backend);
		}

		void test(CombineMode mode, IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Geometry(t => t.Ellipse(15, 5, 30, 30));

			var c2 = backend.Geometry(t => t.Ellipse(35, 5, 30, 30));

			var r = c1.Combine(mode, c2);

			target.Fill(color: new Color(0.7, 0.7, 1.0));
			target.Geometry(r);
		}
	}
}
