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

		public void Line(Point p1, Point p2)
		{
			record(t => t.Line(p1, p2));
		}

		public void Rectangle(Rectangle rectangle)
		{
			record(t => t.Rectangle(rectangle));
		}

		public void RoundedRectangle(Rectangle rectangle, double cornerRadius)
		{
			record(t => t.RoundedRectangle(rectangle, cornerRadius));
		}

		// client should be forced to pass an immutable array here, but how?

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

		public void PixelAlign()
		{
			record(t => t.PixelAlign());
		}

		public void NoPixelAlign()
		{
			record(t => t.NoPixelAlign());
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

		public void Text(string text, Rectangle rectangle)
		{
			record(t => Text(text, rectangle));
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

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
