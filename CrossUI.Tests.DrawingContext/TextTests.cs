namespace CrossUI.Tests.DrawingContext
{
	[BitmapDrawingTest(Width = 160, Height = 40)]
	class TextTests
	{
		const string Text =
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam lectus. Sed sit amet ipsum mauris.";

		public void Default(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Width = 160, Height = 60)]
		public void DefaultLarger(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(size:12);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void NoWordWrap(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(wordWrapping:WordWrapping.NoWrap);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void Centered(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(alignment: TextAlignment.Center);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void RightAligned(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(alignment:TextAlignment.Trailing);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		public void Colored(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(color: new Color(1, 0, 0));
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 60)]
		public void ParagraphCentered(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(paragraphAlignment:ParagraphAlignment.Center);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 60)]
		public void ParagraphBottom(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Text(paragraphAlignment: ParagraphAlignment.Far);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 20)]
		public void Bold(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Font(weight: FontWeight.Bold);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 20)]
		public void Italic(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Font(style: FontStyle.Italic);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		[BitmapDrawingTest(Height = 20)]
		public void BoldItalic(IDrawingTarget ctx)
		{
			setup(ctx);
			ctx.Font(weight:FontWeight.Bold, style: FontStyle.Italic);
			ctx.Text(Text, 0, 0, ctx.Width, ctx.Height);
		}

		void setup(IDrawingTarget ctx)
		{
			ctx.Font(name: "Tahoma");
			ctx.Text(size: 10);
		}
	}
}
