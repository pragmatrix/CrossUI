namespace CrossUI
{
	public interface IDrawingBackend
	{
		IBitmapDrawingContext createBitmapDrawingContext(int width, int height);
	}
}
