using System;

namespace CrossUI.Testing
{
	sealed class TestException : Exception
	{
		public TestException(string description)
			: base(description)
		{
		}
	}
}
