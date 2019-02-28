using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDataHierarchy
{
	class DataTableNode : DataHierarchyNode
	{
		public List<DataEntry> Data;

		public DataTableNode(string name, DataHierarchyNode parent, List<DataEntry> data) : base(name, parent)
		{
			Data = data;
		}

		public override string ResolvePath()
		{
			return base.ResolvePath() + ".txt";
		}
	}
}
