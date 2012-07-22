using System;
using CrossUI.Toolbox;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width=80, Height=40)]
	class GeometryRelationTests
	{
		public void Disjoint(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Ellipse(5, 5, 30, 30);
			var c2 = backend.Ellipse(45, 5, 30, 30);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Disjoint);
		}

		public void Overlap(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Ellipse(15, 5, 30, 30);
			var c2 = backend.Ellipse(35, 5, 30, 30);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Overlap);
		}

		public void IsContained(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Ellipse(30, 10, 20, 20);
			var c2 = backend.Ellipse(25, 5, 30, 30);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.IsContained);
		}

		public void Contains(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Ellipse(25, 5, 30, 30);
			var c2 = backend.Ellipse(30, 10, 20, 20);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Contains);
		}

		public void StrokeFillDisjoint(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Line(25, 10, 25, 30);
			var c2 = backend.Ellipse(30, 10, 20, 20);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Disjoint);
		}

		public void StrokeFillOverlap(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Line(30, 10, 50, 30);
			var c2 = backend.Ellipse(30, 10, 20, 20);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Overlap);
		}


		public void StrokeFillIsContained(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Line(35, 15, 45, 25);
			var c2 = backend.Ellipse(30, 10, 20, 20);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.IsContained);
		}

		public void StrokeStrokeDisjoint(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Line(35, 15, 45, 25);
			var c2 = backend.Line(30, 10, 20, 20);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Disjoint);
		}

		public void StrokeStrokeOverlaps(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Line(35, 15, 45, 25);
			var c2 = backend.Line(50, 10, 35, 20);
			draw(target, c1, c2);
			expectRelation(c1, c2, GeometryRelation.Overlap);
		}

		public void ContainsPoint(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Ellipse(30, 10, 20, 20);
			var c2 = backend.Rectangle(40, 20, 1, 1);

			target.Fill(color: new Color(0.7, 0.7, 1, 0.7));
			target.Geometry(c1);
			target.Geometry(c2);

			if (!c1.Contains(40, 20))
				throw new Exception("circle is expected to contain point at 40, 20");
		}

		public void NotContainsPoint(IDrawingTarget target, IDrawingBackend backend)
		{
			var c1 = backend.Ellipse(30, 10, 20, 20);
			var c2 = backend.Rectangle(30, 10, 1, 1);

			target.Fill(color: new Color(0.7, 0.7, 1, 0.7));
			target.Geometry(c1);
			target.Geometry(c2);

			if (c1.Contains(30, 10))
				throw new Exception("circle is not expected to contain point at 30, 10");
		}

		static void draw(IDrawingTarget target, IGeometry c1, IGeometry c2)
		{
			target.Fill(color: new Color(0.7, 0.7, 1, 0.7));
			target.Geometry(c1);
			target.Geometry(c2);
		}

		static void expectRelation(IGeometry a, IGeometry b, GeometryRelation expected)
		{
			var actual = a.Compare(b);
			if (actual != expected)
				throw new Exception("Expeted relation {0}, but is {1}".format(expected, actual));
		}
	}
}
