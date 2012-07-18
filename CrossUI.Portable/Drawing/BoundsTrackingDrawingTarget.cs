using System;
using System.Collections.Generic;

namespace CrossUI.Drawing
{
	sealed class BoundsTrackingDrawingTarget : 
		IDrawingFigures, 
		IDrawingText, 

		IReportingTarget, 
		ITextMeasurements, 
		IDrawingTargetBitmap
	{
		readonly BoundsTracker _tracker;
	
		public Bounds? Bounds 
		{
			get { return _tracker.Bounds; }
		}

		public static IDrawingTarget Create(BoundsTracker boundsTracker)
		{
			var target = new BoundsTrackingDrawingTarget(boundsTracker);
			return new DrawingTargetSplitter(
				boundsTracker.State, 
				boundsTracker.Transform, 
				target, 
				target, 
				target,
				target, 
				target);
		}

		BoundsTrackingDrawingTarget(BoundsTracker tracker)
		{
			_tracker = tracker;
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
