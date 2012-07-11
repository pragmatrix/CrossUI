using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultAssembly : ITestResultAssembly
	{
		public readonly string Name;
		public readonly Exception Error_;
		public readonly TestResultClass[] Classes_;

		public TestResultAssembly(string name, Exception error)
		{
			Name = name;
			Error_ = error;
		}

		public TestResultAssembly(string name, TestResultClass[] classes)
		{
			Name = name;
			Classes_ = classes;
		}
	}
}
