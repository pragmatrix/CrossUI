using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultMethod : ITestResultMethod
	{
		public readonly string Name;
		public readonly Exception Error_;
		public readonly TestResultBitmap Bitmap_;
		public readonly TestResultReport Report_;
	
		public TestResultMethod(string name, Exception error)
		{
			Name = name;
			Error_ = error;
		}

		public TestResultMethod(string name, TestResultBitmap bitmap, TestResultReport report)
		{
			Name = name;
			Bitmap_ = bitmap;
			Report_ = report;
		}
	}
}
