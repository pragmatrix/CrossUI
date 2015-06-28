﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using CrossUI.Drawing;
using CrossUI.Toolbox;

namespace CrossUI
{
	public interface IDrawingTarget :
		IDrawingState,
		IDrawingTransform,
		IGeometryFigures,
		IDrawingElements,
		ITextMeasurements,
		IDrawingTargetBitmap,
		IReportingTarget,
		IDisposable
	{
		IDrawingBackend Backend { get; }
	}

	public interface IDrawingState
	{
		void Fill(Color? color = null);
		void NoFill();

		void Stroke(Color? color = null, double? weight = null, StrokeAlignment? alignment = null);
		void NoStroke();

		void PixelAlign();
		void NoPixelAlign();

		void Font(string name = null, FontWeight? weight = null, FontStyle? style = null);

		void Text(
			double? size = null,
			Color? color = null,
			TextAlignment? alignment = null,
			ParagraphAlignment? paragraphAlignment = null,
			WordWrapping? wordWrapping = null);

		void SaveState();
		void RestoreState();
	}

	public static class DrawingStateExtensions
	{
		public static IDisposable PushPixelAlign(this IDrawingState state)
		{
			var pushed = state.PushState();
			state.PixelAlign();
			return pushed;
		}
		
		public static IDisposable PushState(this IDrawingState state)
		{
			state.SaveState();
			return new DisposeAction(state.RestoreState);
		}
	}

	/// Stroke alignment applies to rectangular shapes only.

	public enum StrokeAlignment
	{
		Inside, Center, Outside
	}

	public enum TextAlignment
	{
		Leading, Center, Trailing, Justified
	}

	public enum ParagraphAlignment
	{
		Near, Center, Far
	}

	public enum WordWrapping
	{
		Wrap, NoWrap
	}

	public enum FontWeight
	{
		Normal, Bold
	}

	public enum FontStyle
	{
		Normal, Italic
	}

	public interface IDrawingElements
	{
		void Text(string text, Rectangle rectangle);
		void Geometry(IGeometry geometry);
	}

	public static class DrawingElementsExtensions
	{
		public static void Text(this IDrawingElements _, string text, double x, double y, double width, double height)
		{
			_.Text(text, new Rectangle(x, y, width, height));
		}
	}

	public interface ITextMeasurements
	{
		TextSize MeasureText(string text, double maxWidth = double.PositiveInfinity, double maxHeight = double.PositiveInfinity);
	}

	public struct TextSize
	{
		public readonly Size Size;

		public TextSize(double width, double height)
		{
			Size = new Size(width, height);
		}

		public double Width { get { return Size.Width; } }
		public double Height { get { return Size.Height; } }

		public override string ToString()
		{
			return Width + ", " + Height;
		}
	}

	public interface IDrawingTargetBitmap
	{
		int Width { get; }
		int Height { get; }
	}

	public interface IReportingTarget
	{
		void Report(string text);
		IEnumerable<string> Reports { get; }
	}

	public static class ReportingTargetExtensions
	{
		[Conditional("DEBUG")]
		public static void Debug(this IReportingTarget target, string str)
		{
			target.Report(str);
		}
	}
}
