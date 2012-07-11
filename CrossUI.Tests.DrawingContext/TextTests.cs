namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 120, Height = 40)]
	class TextTests
	{
		const string Text =
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam lectus. Sed sit amet ipsum mauris.";

		public void Default(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void NoWordWrap(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void RightAligned(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(align:TextAlign.Trailing);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void Centered(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(align: TextAlign.Center);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void Colored(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(color: new Color(1, 0, 0));
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 80)]
		public void ParagraphCentered(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(paragraphAlign:ParagraphAlign.Center);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 80)]
		public void ParagraphBottom(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(paragraphAlign: ParagraphAlign.Far);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		void setup(IDrawingContext ctx)
		{
			ctx.Text(font: "Tahoma", size: 10);
		}
	}
}
