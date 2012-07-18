using System;
using System.Collections.Generic;

namespace CrossUI.Drawing
{
	public sealed class DrawingTargetSplitter : IDrawingTarget
	{
		readonly IDrawingState _state;
		readonly IDrawingTransform _transform;
		readonly IDrawingFigures _figures;
		readonly IDrawingText _text;
		readonly ITextMeasurements _measurements;
		readonly IDrawingTargetBitmap _bitmap;
		readonly IReportingTarget _reporting;

		public DrawingTargetSplitter(
			IDrawingState state,
			IDrawingTransform transform,
			IDrawingFigures figures,
			IDrawingText text,
			ITextMeasurements measurements,
			IDrawingTargetBitmap bitmap,
			IReportingTarget reporting)
		{
			_bitmap = bitmap;
			_state = state;
			_transform = transform;
			_figures = figures;
			_text = text;
			_measurements = measurements;
			_reporting = reporting;
		}

		public int Width { get { return _bitmap.Width; } }
		public int Height { get { return _bitmap.Height; } }
		public void Fill(Color? color = null)
		{
			_state.Fill(color);
		}

		public void NoFill()
		{
			_state.NoFill();
		}

		public void Stroke(Color? color = null, double? weight = null, StrokeAlignment? alignment = null)
		{
			_state.Stroke(color, weight, alignment);
		}

		public void NoStroke()
		{
			_state.NoStroke();
		}

		public void Font(string name = null, FontWeight? weight = null, FontStyle? style = null)
		{
			_state.Font(name, weight, style);
		}

		public void Text(double? size = null, Color? color = null, TextAlignment? alignment = null, ParagraphAlignment? paragraphAlignment = null, WordWrapping? wordWrapping = null)
		{
			_state.Text(size, color, alignment, paragraphAlignment, wordWrapping);
		}

		public void SaveTransform()
		{
			_transform.SaveTransform();
		}

		public void RestoreTransform()
		{
			_transform.RestoreTransform();
		}

		public void Scale(double sx, double sy, double? centerX = null, double? centerY = null)
		{
			_transform.Scale(sx, sy, centerX, centerY);
		}

		public void Rotate(double radians, double? centerX = null, double? centerY = null)
		{
			_transform.Rotate(radians, centerX, centerY);
		}

		public void Translate(double dx, double dy)
		{
			_transform.Translate(dx, dy);
		}

		public void Line(double x1, double y1, double x2, double y2)
		{
			_figures.Line(x1, y1, x2, y2);
		}

		public void Rect(double x, double y, double width, double height)
		{
			_figures.Rect(x, y, width, height);
		}

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			_figures.RoundedRect(x, y, width, height, cornerRadius);
		}

		public void Polygon(double[] coordinatePairs)
		{
			_figures.Polygon(coordinatePairs);
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			_figures.Ellipse(x, y, width, height);
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			_figures.Arc(x, y, width, height, start, stop);
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			_figures.Bezier(x, y, s1x, s1y, s2x, s2y, ex, ey);
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			_text.Text(text, x, y, width, height);
		}

		public TextSize MeasureText(string text, double maxWidth = Double.PositiveInfinity, double maxHeight = Double.PositiveInfinity)
		{
			return _measurements.MeasureText(text, maxWidth, maxHeight);
		}

		public void Report(string text)
		{
			_reporting.Report(text);
		}

		public IEnumerable<string> Reports { get { return _reporting.Reports; } }
	}
}
