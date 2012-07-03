using System;
using System.Diagnostics;

namespace Toolbox
{
	public struct DisposeAction : IDisposable
	{
		readonly Action _action_;

		public DisposeAction(Action action)
		{
			Debug.Assert(action != null);
			_action_ = action;
		}

		public void Dispose()
		{
			if (_action_ != null)
				_action_();
		}

		// that should be faster than an empty DisposeAction 
		// (because boxing to IDisposable would always create a new instance).

		public static readonly IDisposable None = new DummyDisposable();

		sealed class DummyDisposable : IDisposable
		{
			public void Dispose()
			{
			}
		}
	}
}
