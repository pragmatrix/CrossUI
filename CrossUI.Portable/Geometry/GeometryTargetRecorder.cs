using System;
using System.Collections.Generic;
using System.Linq;

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

		public void Line(double x1, double y1, double x2, double y2)
		{
			record(t => t.Line(x1, y1, x2, y2));
		}

		public void Rectangle(double x, double y, double width, double height)
		{
			record(t => t.Rectangle(x, y, width, height));
		}

		public void RoundedRectangle(double x, double y, double width, double height, double cornerRadius)
		{
			record(t => t.RoundedRectangle(x, y, width, height, cornerRadius));
		}

		public void Polygon(double[] coordinatePairs)
		{
			var copy = coordinatePairs.ToArray();
			record(t => t.Polygon(copy));
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			record(t => t.Ellipse(x, y, width, height));
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			record(t => t.Arc(x, y, width, height, start, stop));
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			record(t => t.Bezier(x, y, s1x, s1y, s2x, s2y, ex, ey));
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
