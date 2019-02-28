using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseDataHierarchy
{
	public class DataHierarchyManager
	{
		private DataHierarchyNode root = null;
		public DataHierarchyManager(string path)
		{
			if (!Directory.Exists(path))
				return;
			root = new DataHierarchyNode("", null);
		}

		/*public void ReParentNode(DataHierarchyNode node, DataHierarchyNode newParent)
		{

		}

		public void PushChangesToFileSystem()
		{
			foreach (DataHierarchyNode child in root.Children)
			{
				PushHierarchyNodeChangesToFiles(child);
			}
		}

		public void PushHierarchyNodeChangesToFiles(DataHierarchyNode node)
		{

		}*/
	}
}
