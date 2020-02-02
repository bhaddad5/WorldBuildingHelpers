using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using NameGenerator;
using UnityEditor;

public class NameGeneratorTablePickerTree : TreeView
{
	private List<HierarchyObject> fileHierarchy;

	public NameGeneratorTablePickerTree(TreeViewState state, List<HierarchyObject> fileHierarchy) : base(state)
	{
		this.fileHierarchy = fileHierarchy;
		this.Reload();
	}
	
	private List<TreeViewItem> allItems = new List<TreeViewItem>();

	protected override TreeViewItem BuildRoot()
	{
		var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
		allItems = new List<TreeViewItem>();

		foreach (HierarchyObject hierarchyObject in fileHierarchy)
			allItems.Add(new TreeViewItem(hierarchyObject.Id, hierarchyObject.Depth, hierarchyObject.DisplayName));
		
		SetupParentsAndChildrenFromDepths(root, allItems);

		return root;
	}

	protected override bool CanMultiSelect(TreeViewItem item)
	{
		return false;
	}
}
