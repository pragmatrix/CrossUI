using System;
using System.Collections.Generic;

namespace CrossUI.Drawing
{
	public sealed class DrawingTargetSplitter : IDrawingTarget
	{
		readonly IDrawingBackend _backend;
		readonly IDrawingState _state;
		readonly IDrawingTransform _transform;
		readonly IGeometryFigures _figures;
		readonly IDrawingElements _elements;
		readonly ITextMeasurements _measurements;
		readonly IDrawingTargetBitmap _bitmap;
		readonly IReportingTarget _reporting;
		readonly Action _disposer;

		public DrawingTargetSplitter(
			IDrawingBackend backend,
			IDrawingState state,
			IDrawingTransform transform,
			IGeometryFigures figures,
			IDrawingElements elements,
			ITextMeasurements measurements,
			IDrawingTargetBitmap bitmap,
			IReportingTarget reporting,
			Action disposer)
		{
			_backend = backend;
			_bitmap = bitmap;
			_state = state;
			_transform = transform;
			_figures = figures;
			_elements = elements;
			_measurements = measurements;
			_reporting = reporting;
			_disposer = disposer;
		}

		public void Dispose()
		{
			_disposer();
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

		public void PixelAlign()
		{
			_state.PixelAlign();
		}

		public void NoPixelAlign()
		{
			_state.NoPixelAlign();
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

		public void Line(Point p1, Point p2)
		{
			_figures.Line(p1, p2);
		}

		public void Rectangle(Rectangle rectangle)
		{
			_figures.Rectangle(rectangle);
		}

		public void RoundedRectangle(Rectangle rectangle, double cornerRadius)
		{
			_figures.RoundedRectangle(rectangle, cornerRadius);
		}

		public void Polygon(Point[] points)
		{
			_figures.Polygon(points);
		}

		public void Ellipse(Rectangle rectangle)
		{
			_figures.Ellipse(rectangle);
		}

		public void Arc(Rectangle rectangle, double start, double stop)
		{
			_figures.Arc(rectangle, start, stop);
		}

		public void Bezier(CubicBezier bezier)
		{
			_figures.Bezier(bezier);
		}

		public void Text(string text, Rectangle rectangle)
		{
			_elements.Text(text, rectangle);
		}

		public void Geometry(IGeometry geometry)
		{
			_elements.Geometry(geometry);
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

		public IDrawingBackend Backend
		{
			get { return _backend; }
		}
	}
}
