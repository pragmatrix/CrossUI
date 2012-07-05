using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestSource
	{
		public readonly string Type;
		public readonly string Method;

		public TestSource(string typeFullName, string methodName)
		{
			Type = typeFullName;
			Method = methodName;
		}
	}
}
