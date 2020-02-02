using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using NameGenerator;

public class NameTableEditor : EditorWindow
{
	private static string currFile;

	public static void OpenNameTableEditor(string file)
	{
		currFile = file;
		GetFileContents();

		var window = GetWindow<NameTableEditor>("Edit: " + Path.GetFileNameWithoutExtension(file));
		window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 500, 600);
		window.Show();
	}

	private static string currentFileContents = "";
	private static Vector2 scroll;
	void OnGUI()
	{
		scroll = EditorGUILayout.BeginScrollView(scroll);
		currentFileContents = EditorGUILayout.TextArea(currentFileContents, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
		EditorGUILayout.EndScrollView();

		if (GUILayout.Button("Save", GUILayout.Width(50)))
		{
			Debug.Log("save");
			this.Close();
			System.IO.File.WriteAllText(MyFile(), currentFileContents);
		}

		if (GUILayout.Button("Done", GUILayout.Width(50)))
		{
			this.Close();
		}
	}

	private static void GetFileContents()
	{
		if (MyFile() == null)
			currentFileContents = "";

		var contents = System.IO.File.ReadAllText(MyFile());
		contents = contents.Replace('\n', '\0');
		contents = contents.Replace('\r', '\0');
		contents = contents.Replace(",", ",\n");

		currentFileContents = contents;
	}

	private static string MyFile()
	{
		if (String.IsNullOrEmpty(currFile))
			return "";

		var myDir = Path.Combine(Application.streamingAssetsPath, "Name Generator/Name Tables");
		return Path.Combine(myDir, currFile);
	}
}