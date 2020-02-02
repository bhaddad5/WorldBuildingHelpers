using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using NameGenerator;

public class NameGeneratorTableView : TreeView
{
	public NameGeneratorTableView(TreeViewState state) : base(state)
	{
	}

	public NameGeneratorTableView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
	{
	}

	protected override TreeViewItem BuildRoot()
	{
		throw new System.NotImplementedException();
	}
}
