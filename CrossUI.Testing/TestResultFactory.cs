using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossUI.Testing
{
	sealed class TestResultFactory : ITestResultFactory
	{
		public ITestResultAssembly Assembly(string path, ITestResultClass[] classes)
		{
			return new TestResultAssembly(path, classes.Cast<TestResultClass>().ToArray());
		}

		public ITestResultAssembly Assembly(string path, Exception e)
		{
			return new TestResultAssembly(path, e);
		}

		public ITestResultClass Class(string ns, string name, ITestResultMethod[] methods)
		{
			return new TestResultClass(ns, name, methods.Cast<TestResultMethod>().ToArray());
		}

		public ITestResultClass Class(string ns, string name, Exception e)
		{
			return new TestResultClass(ns, name, e);
		}

		public ITestResultMethod Method(string name, ITestResultBitmap bitmap, ITestResultReport report)
		{
			return new TestResultMethod(name, (TestResultBitmap)bitmap, (TestResultReport)report);
		}

		public ITestResultMethod Method(string name, Exception e)
		{
			return new TestResultMethod(name, e);
		}

		public ITestResultBitmap Bitmap(int width, int height, byte[] data)
		{
			return new TestResultBitmap(width, height, data);
		}

		public ITestResultReport Report(IEnumerable<string> report)
		{
			return new TestResultReport(report);
		}
	}
}
