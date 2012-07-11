using System;

namespace CrossUI.Testing
{
	public interface ITestResultFactory
	{
		ITestResultAssembly Assembly(string path, ITestResultClass[] classes);
		ITestResultAssembly Assembly(string path, Exception e);
		ITestResultClass Class(string ns, string name, ITestResultMethod[] methods);
		ITestResultClass Class(string ns, string name, Exception e);
		ITestResultMethod Method(string name, ITestResultBitmap bitmap);
		ITestResultMethod Method(string name, Exception e);
		ITestResultBitmap Bitmap(int width, int height, byte[] data);
	}

	public interface ITestResultAssembly
	{
	}

	public interface ITestResultClass
	{
	}

	public interface ITestResultMethod
	{
	}

	public interface ITestResultBitmap
	{
	}
}
