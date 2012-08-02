using System;
using System.Collections.Generic;

namespace CrossUI.Drawing
{
	sealed class BoundsTrackingDrawingTarget : 
		IGeometryFigures, 
		IDrawingElements, 

		IReportingTarget, 
		ITextMeasurements, 
		IDrawingTargetBitmap
	{
		readonly BoundsTracker _tracker;
	
		public Bounds? Bounds 
		{
			get { return _tracker.Bounds; }
		}

		public static IDrawingTarget Create(IDrawingBackend backend, BoundsTracker boundsTracker)
		{
			var target = new BoundsTrackingDrawingTarget(boundsTracker);
			return new DrawingTargetSplitter(
				backend,
				boundsTracker.State, 
				boundsTracker.Transform, 
				target, 
				target, 
				target,
				target, 
				target,
				() => { });
		}

		BoundsTrackingDrawingTarget(BoundsTracker tracker)
		{
			_tracker = tracker;
		}

		public void Line(Point p1, Point p2)
		{
			_tracker.trackPoints(p1, p2);
		}

		public void Rectangle(Rectangle rectangle)
		{
			_tracker.trackAlignedRect(rectangle);
		}

		public void RoundedRectangle(Rectangle rectangle, double cornerRadius)
		{
			_tracker.trackAlignedRect(rectangle);
		}

		public void Polygon(Point[] points)
		{
			_tracker.trackPoints(points);
		}

		public void Ellipse(Rectangle rectangle)
		{
			_tracker.trackAlignedRect(rectangle);
		}

		public void Arc(Rectangle rectangle, double start, double stop)
		{
			// todo: optimize by using a geometry?
			_tracker.trackAlignedRect(rectangle);
		}

		public void Bezier(CubicBezier bezier)
		{
			_tracker.trackPoints(bezier.Start, bezier.Span1, bezier.Span2, bezier.End);
		}

		public void Text(string text, Rectangle rectangle)
		{
			_tracker.trackRect(rectangle);
		}

		public void Geometry(IGeometry geometry)
		{
			throw new NotImplementedException();
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
