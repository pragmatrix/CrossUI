namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 160, Height = 40)]
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
			ctx.Text(wordWrapping:WordWrapping.NoWrap);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void Centered(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(alignment: TextAlignment.Center);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void RightAligned(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(alignment:TextAlignment.Trailing);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void Colored(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(color: new Color(1, 0, 0));
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 60)]
		public void ParagraphCentered(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(paragraphAlignment:ParagraphAlignment.Center);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 60)]
		public void ParagraphBottom(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(paragraphAlignment: ParagraphAlignment.Far);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 20)]
		public void Bold(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(weight: FontWeight.Bold);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 20)]
		public void Italic(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(style: FontStyle.Italic);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 20)]
		public void BoldItalic(IDrawingContext ctx)
		{
			setup(ctx);
			ctx.Text(weight:FontWeight.Bold, style: FontStyle.Italic);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		void setup(IDrawingContext ctx)
		{
			ctx.Text(font: "Tahoma", size: 10);
		}
	}
}
