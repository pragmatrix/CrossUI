using System;

namespace CrossUI
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class DrawingBackendAttribute : Attribute
	{
		public readonly Type Type;

		public DrawingBackendAttribute(Type type)
		{
			Type = type;
		}
	}
}
