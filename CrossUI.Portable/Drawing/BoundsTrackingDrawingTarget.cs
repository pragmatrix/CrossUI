using System;
using System.Collections.Generic;

namespace CrossUI.Drawing
{
	sealed class BoundsTrackingDrawingTarget : IDrawingTarget
	{
		readonly DrawingState _state = new DrawingState();
		readonly DrawingTransform _transform = new DrawingTransform();
		readonly BoundsTracker _tracker;
	
		public Bounds? Bounds 
		{
			get { return _tracker.Bounds; }
		}

		public BoundsTrackingDrawingTarget()
		{
			_tracker = new BoundsTracker(_state, _transform);
		}

		public void Line(double x1, double y1, double x2, double y2)
		{
			_tracker.trackPoints(x1, y1, x2, y2);
		}

		public void Rect(double x, double y, double width, double height)
		{
			_tracker.trackAlignedRect(x, y, width, height);
		}

		public void RoundedRect(double x, double y, double width, double height, double cornerRadius)
		{
			_tracker.trackAlignedRect(x, y, width, height);
		}

		public void Polygon(double[] coordinatePairs)
		{
			_tracker.trackPoints(coordinatePairs);
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			_tracker.trackAlignedRect(x, y, width, height);
		}

		public void Arc(double x, double y, double width, double height, double start, double stop)
		{
			// todo: optimize using a geometry?
			_tracker.trackAlignedRect(x, y, width, height);
		}

		public void Bezier(double x, double y, double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			_tracker.trackPoints(x, y, s1x, s1y, s2x, s2y, ex, ey);
		}

		public void Text(string text, double x, double y, double width, double height)
		{
			_tracker.trackRect(x, y, width, height);
		}

		#region State Forwarders

		public void Fill(Color? color)
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

		#endregion

		#region Transform Forwarders

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

		#endregion

		public int Width
		{
			get { throw new NotImplementedException(); }
		}

		public int Height
		{
			get { throw new NotImplementedException(); }
		}

		public TextSize MeasureText(string text, double maxWidth = Double.PositiveInfinity, double maxHeight = Double.PositiveInfinity)
		{
			throw new NotImplementedException();
		}

		public void Report(string text)
		{
		}

		public IEnumerable<string> Reports
		{
			get { throw new NotImplementedException(); }
		}
	}
}
