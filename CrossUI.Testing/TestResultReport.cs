using System;
using System.Collections.Generic;

namespace CrossUI.Testing
{
	[Serializable]
	public sealed class TestResultReport : ITestResultReport
	{
		public readonly IEnumerable<string> Report;

		public TestResultReport(IEnumerable<string> report)
		{
			Report = report;
		}
	}
}
