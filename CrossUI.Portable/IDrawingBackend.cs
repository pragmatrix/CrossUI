namespace CrossUI
{
	public interface IDrawingBackend
	{
		IBitmapDrawingContext CreateBitmapDrawingContext(int width, int height);
	}
}
