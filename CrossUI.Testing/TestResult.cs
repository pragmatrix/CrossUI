using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResult
	{
		public readonly TestSource Source;
		public readonly Exception Error_;
		public readonly TestResultBitmap Bitmap_;
	
		public TestResult(TestSource source, Exception error)
		{
			Source = source;
			Error_ = error;
		}

		public TestResult(TestSource source, TestResultBitmap bitmap)
		{
			Source = source;
			Bitmap_ = bitmap;
		}
	}
}
