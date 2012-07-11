using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultClass : ITestResultClass
	{
		public readonly string Namespace;
		public readonly string ClassName;
		public readonly Exception Error_;
		public readonly TestResultMethod[] Methods_;

		public TestResultClass(string ns, string className, TestResultMethod[] methods)
		{
			Namespace = ns;
			ClassName = className;
			Methods_ = methods;
		}

		public TestResultClass(string ns, string className, Exception error)
		{
			Namespace = ns;
			ClassName = className;
			Error_ = error;
		}
	}
}
