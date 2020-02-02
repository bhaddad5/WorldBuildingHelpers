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

		latestTestResults = "";

		var window = GetWindow<NameTableEditor>("Edit: " + Path.GetFileNameWithoutExtension(file));
		window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 500, 600);
		window.Show();
	}

	private static string currentFileContents = "";
	private static Vector2 scroll;
	private static string latestTestResults = "";
	void OnGUI()
	{
		if (GUILayout.Button("Done", GUILayout.Width(50)))
		{
			this.Close();
		}

		scroll = EditorGUILayout.BeginScrollView(scroll);
		currentFileContents = EditorGUILayout.TextArea(currentFileContents, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
		EditorGUILayout.EndScrollView();

		if (GUILayout.Button("Save", GUILayout.Width(50)))
		{
			Debug.Log("save");
			this.Close();
			System.IO.File.WriteAllText(currFile, currentFileContents);
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Test", GUILayout.Width(50)))
		{
			var generatedNames = new NameRequestHandler().HandleNameRequest(currFile, 5);
			latestTestResults = "";
			foreach (string name in generatedNames)
				latestTestResults += name + ",\n";
		}

		if (latestTestResults != "")
		{
			GUILayout.Label("Example Results:");
			GUILayout.Label(latestTestResults);
		}
	}

	private static void GetFileContents()
	{
		var contents = System.IO.File.ReadAllText(currFile);
		contents = contents.Replace('\n', '\0');
		contents = contents.Replace('\r', '\0');
		contents = contents.Replace(",", ",\n");

		currentFileContents = contents;
	}
}