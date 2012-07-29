using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossUI.Drawing
{
	sealed class DrawingTargetRecorder : IDrawingTarget
	{
		readonly int? _width;
		readonly int? _height;

		readonly ITextMeasurements _textMeasurements;

		readonly List<Action<IDrawingTarget>> _records = new List<Action<IDrawingTarget>>();
		readonly List<string> _reports = new List<string>();

		public DrawingTargetRecorder(int? width, int? height, ITextMeasurements textMeasurements)
		{
			_width = width;
			_height = height;
			_textMeasurements = textMeasurements;
		}

		public void replay(IDrawingTarget target)
		{
			foreach (var action in _records)
				action(target);
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

		// client should be forced to pass an immutable array here, but how?

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

		public void SaveTransform()
		{
			record(t => t.SaveTransform());
		}

		public void RestoreTransform()
		{
			record(t => t.RestoreTransform());
		}

		public void Scale(double sx, double sy, double? centerX, double? centerY)
		{
			record(t => t.Scale(sx, sy, centerX, centerY));
		}

		public void Rotate(double radians, double? centerX, double? centerY)
		{
			record(t => t.Rotate(radians, centerX, centerY));
		}

		public void Translate(double dx, double dy)
		{
			record(t => t.Translate(dx, dy));
		}

		public int Width
		{
			get { return _width ?? 0; }
		}

		public int Height
		{
			get { return _height ?? 0; }
		}

		public void Fill(Color? color)
		{
			record(t => t.Fill(color));
		}

		public void NoFill()
		{
			record(t => t.NoFill());
		}

		public void Stroke(Color? color, double? weight, StrokeAlignment? alignment)
		{
			record(t => t.Stroke(color, weight, alignment));
		}

		public void NoStroke()
		{
			record(t => t.NoStroke());
		}

		public void Font(string name, FontWeight? weight, FontStyle? style)
		{
			record(t => t.Font(name, weight, style));
		}

		public void Text(
			double? size, 
			Color? color, 
			TextAlignment? alignment, 
			ParagraphAlignment? paragraphAlignment, 
			WordWrapping? wordWrapping)
		{
			record(t => t.Text(size, color, alignment, paragraphAlignment, wordWrapping));
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			record(t => Text(text, x, y, width, height));
		}

		public void Geometry(IGeometry geometry)
		{
			// IDisposable indicates that geometries are not immutable... we may need to change that
			record(t => Geometry(geometry));
		}

		public TextSize MeasureText(string text, double maxWidth, double maxHeight)
		{
			return _textMeasurements.MeasureText(text, maxWidth, maxHeight);
		}

		public void Report(string text)
		{
			_reports.Add(text);
			record(t => t.Report(text));
		}

		public IEnumerable<string> Reports
		{
			get { return _reports; }
		}

		void record(Action<IDrawingTarget> action)
		{
			_records.Add(action);
		}

		public IDrawingBackend Backend
		{
			get { throw new NotImplementedException(); }
		}
	}
}
