using System;
using System.Reflection;

namespace CrossUI.Testing
{
	public sealed class TestSource
	{
		public readonly Type Type;
		public readonly MethodInfo Method;

		public TestSource(Type type, MethodInfo method)
		{
			Type = type;
			Method = method;
		}
	}
}
