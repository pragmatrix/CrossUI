using System.Reflection;

namespace CrossUI.Testing
{
	public interface ITestRunner
	{
		ITestResultAssembly run(ITestResultFactory resultFactory, string testAssemblyPath, Assembly testAssembly);
	}
}
