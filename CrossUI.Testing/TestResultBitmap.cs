using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultBitmap : ITestResultBitmap
	{
		public readonly int Width;
		public readonly int Height;
		public readonly byte[] Data;

		public TestResultBitmap(int width, int height, byte[] data)
		{
			Width = width;
			Height = height;
			Data = data;
		}
	}
}
