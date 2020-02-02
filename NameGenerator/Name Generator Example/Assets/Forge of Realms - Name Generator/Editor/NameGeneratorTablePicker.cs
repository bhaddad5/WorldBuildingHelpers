using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public abstract class HierarchyObject
{
	public int Id;
	public int Depth;
	public string DisplayName;
	public string Path;

	public HierarchyObject(int id, int depth, string displayName, string path)
	{
		Id = id;
		Depth = depth;
		DisplayName = displayName;
		Path = path;
	}
}

public class HierarchyFile : HierarchyObject
{
	public HierarchyFile(int id, int depth, string displayName, string path) : base(id, depth, displayName, path){}
}

public class HierarchyFolder : HierarchyObject
{
	public HierarchyFolder(int id, int depth, string displayName, string path) : base(id, depth, displayName, path){}
}

public class NameGeneratorTablePicker : EditorWindow
{
	[SerializeField] TreeViewState m_TreeViewState;
	//The TreeView is not serializable, so it should be reconstructed from the tree data.
	NameGeneratorTablePickerTree m_SimpleTreeView;

	private List<HierarchyObject> currFileHierarchy = new List<HierarchyObject>();

	void OnEnable()
	{
		// Check whether there is already a serialized view state (state 
		// that survived assembly reloading)
		if (m_TreeViewState == null)
			m_TreeViewState = new TreeViewState(){};

		currFileHierarchy = GetFileHierarchy(0, Path.Combine(Application.streamingAssetsPath, "Name Generator/Name Tables"));

		m_SimpleTreeView = new NameGeneratorTablePickerTree(m_TreeViewState, currFileHierarchy);
		m_SimpleTreeView.ExpandAll();
	}

	void OnGUI()
	{
		m_SimpleTreeView.OnGUI(new Rect(0, 40, position.width, position.height - 40));
		
		var sel = m_SimpleTreeView.GetSelection();
		if (sel.Count > 0)
		{
			if(sel[0] >= currFileHierarchy.Count)
				Debug.Log(sel[0] + ", " + currFileHierarchy.Count);

			var selection = currFileHierarchy[sel[0]];
			if (selection is HierarchyFile)
			{
				if (GUILayout.Button("Edit", GUILayout.Width(80)))
				{
					NameTableEditor.OpenNameTableEditor(selection.Path);
				}
			}
			if (selection is HierarchyFolder)
			{
				if (GUILayout.Button("Add Folder", GUILayout.Width(80)))
				{
					Debug.Log("add f!");
				}

				if (GUILayout.Button("Add Table", GUILayout.Width(80)))
				{
					Debug.Log("add t!");
				}
			}

			if (GUILayout.Button("Delete", GUILayout.Width(80)))
			{
				Debug.Log("Nuke!");
			}
		}
	}

	[MenuItem("Forge of Realms/Name Table Editor")]
	static void OpenTreeView()
	{
		var window = GetWindow<NameGeneratorTablePicker>();
		window.titleContent = new GUIContent("Edit Name Tables");
		window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 400, 600);
		window.Show();
	}

	private static int currId;
	private List<HierarchyObject> GetFileHierarchy(int depth, string folder)
	{
		List<HierarchyObject> obs = new List<HierarchyObject>();
		foreach (var dir in Directory.GetDirectories(folder))
		{
			obs.Add(new HierarchyFolder(currId++, depth, Path.GetFileNameWithoutExtension(dir), dir));
			obs = obs.Concat(GetFileHierarchy(depth + 1, dir)).ToList();
		}

		foreach (string file in Directory.GetFiles(folder))
		{
			if(file.EndsWith(".txt"))
				obs.Add(new HierarchyFile(currId++, depth, Path.GetFileNameWithoutExtension(file), file));
		}

		return obs;
	}
}
