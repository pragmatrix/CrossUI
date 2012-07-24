using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultAssembly : ITestResultAssembly
	{
		public readonly string Name;
		public readonly Exception Error_;
		public readonly TestResultClass[] Classes_;
		public readonly TimeSpan RunningTime;

		public TestResultAssembly(string name, Exception error)
		{
			Name = name;
			Error_ = error;
		}

		public TestResultAssembly(string name, TestResultClass[] classes, TimeSpan runningTime)
		{
			Name = name;
			Classes_ = classes;
			RunningTime = runningTime;
		}
	}
}
