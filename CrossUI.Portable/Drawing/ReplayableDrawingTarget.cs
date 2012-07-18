using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossUI.Drawing
{
	sealed class ReplayableDrawingTarget : IDrawingTarget
	{
		readonly int? _width;
		readonly int? _height;

		readonly ITextMeasurements _textMeasurements;

		readonly List<Action<IDrawingTarget>> _actions;
		readonly List<string>  _reports = new List<string>();

		public ReplayableDrawingTarget(int? width, int? height, ITextMeasurements textMeasurements)
		{
			_width = width;
			_height = height;
			_textMeasurements = textMeasurements;
		}

		public void replay(IDrawingTarget target)
		{
			foreach (var action in _actions)
				action(target);
		}

		public void Line(double x1, double y1, double x2, double y2)
		{
			add(t => t.Line(x1, y1, x2, y2));
		}

		public void Rect(double x, double y, double width, double height)
		{
			add(t => t.Rect(x, y, width, height));
		}

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			add(t => t.RoundedRect(x, y, width, height, cornerRadius));
		}

		// client should be forced to pass an immutable array here, but how?

		public void Polygon(double[] coordinatePairs)
		{
			var copy = coordinatePairs.ToArray();
			add(t => t.Polygon(copy));
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			add(t => t.Ellipse(x, y, width, height));
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			add(t => t.Arc(x, y, width, height, start, stop));
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			add(t => t.Bezier(x, y, s1x, s1y, s2x, s2y, ex, ey));
		}

		public void SaveTransform()
		{
			add(t => t.SaveTransform());
		}

		public void RestoreTransform()
		{
			add(t => t.RestoreTransform());
		}

		public void Scale(double sx, double sy, double? centerX, double? centerY)
		{
			add(t => t.Scale(sx, sy, centerX, centerY));
		}

		public void Rotate(double radians, double? centerX, double? centerY)
		{
			add(t => t.Rotate(radians, centerX, centerY));
		}

		public void Translate(double dx, double dy)
		{
			add(t => t.Translate(dx, dy));
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
			add(t => t.Fill(color));
		}

		public void NoFill()
		{
			add(t => t.NoFill());
		}

		public void Stroke(Color? color, double? weight, StrokeAlignment? alignment)
		{
			add(t => t.Stroke(color, weight, alignment));
		}

		public void NoStroke()
		{
			add(t => t.NoStroke());
		}

		public void Font(string name, FontWeight? weight, FontStyle? style)
		{
			add(t => t.Font(name, weight, style));
		}

		public void Text(
			double? size, 
			Color? color, 
			TextAlignment? alignment, 
			ParagraphAlignment? paragraphAlignment, 
			WordWrapping? wordWrapping)
		{
			add(t => t.Text(size, color, alignment, paragraphAlignment, wordWrapping));
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			add(t => Text(text, x, y, width, height));
		}

		public TextSize MeasureText(string text, double maxWidth, double maxHeight)
		{
			return _textMeasurements.MeasureText(text, maxWidth, maxHeight);
		}

		public void Report(string text)
		{
			_reports.Add(text);
			add(t => t.Report(text));
		}

		public IEnumerable<string> Reports
		{
			get { return _reports; }
		}

		void add(Action<IDrawingTarget> action)
		{
			_actions.Add(action);
		}
	}
}
