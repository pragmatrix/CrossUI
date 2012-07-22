﻿using System.Collections.Generic;
using System.Diagnostics;

namespace CrossUI
{
	public interface IDrawingTarget : 
		IDrawingState,
		IDrawingTransform,
		IGeometryFigures,
		IDrawingElements, 
		ITextMeasurements,
		IDrawingTargetBitmap,
		IReportingTarget
	{
	}

	public interface IDrawingState
	{
		void Fill(Color? color = null);
		void NoFill();

		void Stroke(Color? color = null, double? weight = null, StrokeAlignment? alignment = null);
		void NoStroke();

		void Font(string name = null, FontWeight? weight = null, FontStyle? style = null);

		void Text(
			double? size = null,
			Color? color = null,
			TextAlignment? alignment = null,
			ParagraphAlignment? paragraphAlignment = null,
			WordWrapping? wordWrapping = null);
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
		void Text(string text, double x, double y, double width, double height);
		void Geometry(IGeometry geometry);
	}

	public interface ITextMeasurements
	{
		TextSize MeasureText(string text, double maxWidth = double.PositiveInfinity, double maxHeight = double.PositiveInfinity);
	}

	public struct TextSize
	{
		public TextSize(double width, double height)
		{
			Width = width;
			Height = height;
		}

		public readonly double Width;
		public readonly double Height;

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