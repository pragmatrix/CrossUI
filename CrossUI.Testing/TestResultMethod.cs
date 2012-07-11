using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultMethod : ITestResultMethod
	{
		public readonly string Name;
		public readonly Exception Error_;
		public readonly TestResultBitmap Bitmap_;
	
		public TestResultMethod(string name, Exception error)
		{
			Name = name;
			Error_ = error;
		}

		public TestResultMethod(string name, TestResultBitmap bitmap)
		{
			Name = name;
			Bitmap_ = bitmap;
		}
	}
}
