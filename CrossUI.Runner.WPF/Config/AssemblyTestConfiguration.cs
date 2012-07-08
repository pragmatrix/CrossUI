using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrossUI.Runner.Config
{
	[Obfuscation]
	sealed class AssemblyTestConfiguration
	{
		public string AssemblyPath { get; set; }
		public List<CollapsedClass> CollapsedClasses { get; set; }

		public static AssemblyTestConfiguration create(string assemblyPath, IEnumerable<CollapsedClass> collapsedClasses)
		{
			return new AssemblyTestConfiguration
			{
				AssemblyPath = assemblyPath,
				CollapsedClasses = collapsedClasses.ToList()
			};
		}

		public void sanitize()
		{
			if (CollapsedClasses == null)
				CollapsedClasses = new List<CollapsedClass>();
		}

		public bool isClassCollapsed(string ns, string className)
		{
			return CollapsedClasses.Any(cc => cc.Namespace == ns && cc.ClassName == className);
		}

		public void classExpanded(string ns, string className)
		{
			removeFromCollapsedClasses(ns, className);
		}

		public void classCollapsed(string ns, string className)
		{
			removeFromCollapsedClasses(ns, className);
			CollapsedClasses.Add(new CollapsedClass {Namespace = ns, ClassName = className});
		}

		void removeFromCollapsedClasses(string ns, string className)
		{
			CollapsedClasses.RemoveAll(cc => cc.Namespace == ns && cc.ClassName == className);
		}
	}
}
