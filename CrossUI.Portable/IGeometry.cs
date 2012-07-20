using System;

namespace CrossUI
{
	public interface IGeometry : IDisposable
	{
		Bounds Bounds { get; }
	}
}
