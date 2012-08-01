using System;
using System.Collections.Generic;

namespace CrossUI.SharpDX.Geometry
{
	sealed class FigureTargetRecorder : IFigureTarget, IRecorder<IFigureTarget>
	{
		readonly List<Action<IFigureTarget>> _records = new List<Action<IFigureTarget>>();

		public void Replay(IFigureTarget target)
		{
			foreach (var record in _records)
				record(target);
		}

		public void LineTo(double x, double y)
		{
			_records.Add(ft => ft.LineTo(x, y));
		}

		public void ArcTo(double x, double y, double width, double height, double start, double stop, ArcDirection direction = ArcDirection.Clockwise)
		{
			_records.Add(ft => ft.ArcTo(x, y, width, height, start, stop, direction));
		}

		public void BezierTo(double s1x, double s1y, double s2x, double s2y, double ex, double ey)
		{
			_records.Add(ft => ft.BezierTo(s1x, s1y, s2x, s2y, ex, ey));
		}
	}
}
