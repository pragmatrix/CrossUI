using System;
using System.Collections.Generic;
using System.Linq;
using CrossUI.Drawing;

namespace CrossUI.Geometry
{
	sealed class GeometryTargetRecorder : IGeometryTargetRecorder
	{
		readonly List<Action<IGeometryTarget>> _records = new List<Action<IGeometryTarget>>();

		public void Replay(IGeometryTarget target)
		{
			foreach (var record in _records)
				record(target);
		}

		public void Line(Point p1, Point p2)
		{
			record(t => t.Line(p1, p2));
		}

		public void Rectangle(Rectangle rectangle)
		{
			record(t => t.Rectangle(rectangle));
		}

		public void RoundedRectangle(Rectangle rectangle, Size cornerRadius)
		{
			record(t => t.RoundedRectangle(rectangle, cornerRadius));
		}

		public void Polygon(Point[] points)
		{
			var copy = points.ToArray();
			record(t => t.Polygon(copy));
		}

		public void Ellipse(Rectangle rectangle)
		{
			record(t => t.Ellipse(rectangle));
		}

		public void Arc(Rectangle rectangle, double start, double stop)
		{
			record(t => t.Arc(rectangle, start, stop));
		}

		public void Bezier(CubicBezier bezier)
		{
			record(t => t.Bezier(bezier));
		}

		public void MoveTo(double x, double y)
		{
			record(t => t.MoveTo(x, y));
		}

		public void Close()
		{
			record(t => t.Close());
		}

		public void LineTo(double x, double y)
		{
			record(t => t.LineTo(x, y));
		}

		public void ArcTo(double x, double y, double width, double height, double start, double stop, ArcDirection direction = ArcDirection.Clockwise)
		{
			record(t => t.ArcTo(x, y, width, height, start, stop, direction));
		}

		public void BezierTo(double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			record(t => t.BezierTo(s1x, s1y, s2x, s2y, ex, ey));
		}

		void record(Action<IGeometryTarget> record)
		{
			_records.Add(record);
		}
	}
}
