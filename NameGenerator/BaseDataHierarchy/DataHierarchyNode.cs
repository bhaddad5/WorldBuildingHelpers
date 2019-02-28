using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDataHierarchy
{
	class DataHierarchyNode
	{
		public string Name;
		public DataHierarchyNode Parent;
		public List<DataHierarchyNode> Children = new List<DataHierarchyNode>();

		public DataHierarchyNode(string name, DataHierarchyNode parent)
		{
			Parent = parent;
			Name = name;
		}

		public virtual string ResolvePath()
		{
			return Path.Combine(Parent.ResolvePath(), Name);
		}
	}
}
