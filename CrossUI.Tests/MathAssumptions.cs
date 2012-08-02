using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CrossUI.Tests
{
	[TestFixture]
	public class MathAssumptions
	{
		[Test]
		public void testFloor()
		{
			Assert.That(Math.Floor(-2.1), Is.EqualTo(-3.0));
		}
	}
}
