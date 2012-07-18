using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 80, Height = 40)]
	class FillAndStrokeRoundedRectTests
	{
		void setup(IDrawingTarget target)
		{
			var fillColor = new Color(0, 0.5, 1);
			var strokeColor = new Color(0, 0, 0, 0.25);

			target.Fill(fillColor);
			target.Stroke(strokeColor, weight: 2);
		}

		public void RoundedRect(IDrawingTarget target)
		{
			setup(target);
			target.RoundedRect(0, 0, 80, 40, 8);
		}

		public void RoundedRect8_4(IDrawingTarget target)
		{
			setup(target);
			target.Stroke(weight: 4);
			target.RoundedRect(0, 0, 80, 40, 8);
		}

		public void RoundedRect16_2(IDrawingTarget target)
		{
			setup(target);
			target.RoundedRect(0, 0, 80, 40, 16);
		}
	}
}
