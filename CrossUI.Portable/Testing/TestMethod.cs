using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrossUI.Toolbox;

namespace CrossUI.Testing
{
	struct TestMethod
	{
		public readonly MethodInfo Info;
		public readonly BitmapDrawingTestAttribute Attribute;
		public readonly bool Ignorable;

		public TestMethod(MethodInfo info, BitmapDrawingTestAttribute attribute, bool ignorable)
		{
			Info = info;
			Attribute = attribute;
			Ignorable = ignorable;
		}

		public bool canTest(out string whyNot)
		{
			if (Info.IsStatic || Info.IsGenericMethod)
			{
				whyNot = "{0}: is not allowed to be static or generic".format(Info);
				return false;
			}

			var parameters = Info.GetParameters();
			if (parameters.Length < 1)
			{
				whyNot = "{0}: expect at least one parameter".format(Info);
				return false;
			}

			if (!parameters.All(isValidParameter))
			{
				whyNot = "{0}: invalid parameter type";
				return false;
			}

			whyNot = string.Empty;
			return true;
		}

		public bool isValidParameter(ParameterInfo info)
		{
			return info.ParameterType == typeof (IDrawingTarget);
		}

		public void invoke(object instance, IDrawingTarget drawingTarget)
		{
			invoke(instance, new object[] { drawingTarget });
		}

		void invoke(object instance, IEnumerable<object> unmappedArguments)
		{
			var methodParameters = Info.GetParameters();
			var parameterCount = methodParameters.Length;
			var mappedArguments = new object[parameterCount];

			for (int i = 0; i != parameterCount; ++i)
			{
				var parameter = methodParameters[i];
				object mapped = tryMapArgument(unmappedArguments, parameter);
				if (mapped == null)
					throw new Exception("failed to map to instance of type {0}".format(parameter.ParameterType));
				mappedArguments[i] = mapped;
			}

			Info.Invoke(instance, mappedArguments);
		}

		static object tryMapArgument(IEnumerable<object> args, ParameterInfo parameter)
		{
			var parameterType = parameter.ParameterType;
			return args.FirstOrDefault(parameterType.IsInstanceOfType);
		}
	}
}