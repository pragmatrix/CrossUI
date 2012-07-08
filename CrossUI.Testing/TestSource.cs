using System;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestSource
	{
		public readonly string Namespace;
		public readonly string Type;
		public readonly string Method;

		public TestSource(string namespaceName, string typeName, string methodName)
		{
			Namespace = namespaceName;
			Type = typeName;
			Method = methodName;
		}

		public override string ToString()
		{
			return Namespace + "." + Type + "." + Method;
		}
	}
}
