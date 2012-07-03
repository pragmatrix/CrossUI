using System;

namespace CrossUI.Portable
{
	public interface IShapeContext : IDisposable
	{
		void vertex(double x, double y);
	}
}
