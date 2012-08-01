using System;
using System.IO;
using System.Reflection;

namespace CrossUI.Testing
{
	public sealed class TestRunner : MarshalByRefObject
	{
		const string TestRunnerAssembly = "CrossUI.TestRunner.Portable.dll";
		const string TestRunnerType = "CrossUI.Testing.TestRunnerPortable";

		public TestResultAssembly run(string testAssemblyPath)
		{
			// http://blogs.msdn.com/b/suzcook/archive/2003/05/29/choosing-a-binding-context.aspx#57147
			// LoadFrom differs from Load in that dependent assemblies can be resolved outside from the 
			// BasePath.

			try
			{
				var testDir = Path.GetDirectoryName(testAssemblyPath);
				var testAssembly = Assembly.LoadFrom(testAssemblyPath);
				var crossUIAssemblyPath = Path.Combine(testDir, TestRunnerAssembly);
				var crossUIAssembly = Assembly.LoadFrom(crossUIAssemblyPath);
				var testRunner = (ITestRunner)crossUIAssembly.CreateInstance(TestRunnerType);
				if (testRunner == null)
					throw new Exception(string.Format("Failed to instantiate type {0} in assembly {1}", TestRunnerType, TestRunnerAssembly));
				var resultFactory = new TestResultFactory();
				return (TestResultAssembly) (testRunner.run(resultFactory, testAssemblyPath, testAssembly));
			}
			catch (Exception e)
			{
				return new TestResultAssembly(testAssemblyPath, e);
			}
		}
	}
}
