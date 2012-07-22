using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace CrossUI.SharpDX.Geometry
{
	static class Path
	{
		public static PathGeometry Figure(Factory factory, bool filled, DrawingPointF begin, Action<GeometrySink> figureBuilder)
		{
			return Geometry(factory,
				sink =>
					{
						sink.BeginFigure(begin, filled ? FigureBegin.Filled : FigureBegin.Hollow);
						figureBuilder(sink);
						sink.EndFigure(filled ? FigureEnd.Closed : FigureEnd.Open);
					});
		}

		public static PathGeometry Geometry(Factory factory, Action<GeometrySink> figureBuilder)
		{
			var pg = new PathGeometry(factory);

			using (var sink = pg.Open())
			{
				figureBuilder(sink);
				sink.Close();
			}

			return pg;
		}

	}
}
