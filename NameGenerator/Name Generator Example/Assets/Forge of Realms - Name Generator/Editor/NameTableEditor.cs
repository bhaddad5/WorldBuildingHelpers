using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using NameGenerator;
using UnityEngine.Windows;

public class NameTableEditor : EditorWindow
{
	[MenuItem("Forge of Realms/Name Table Editor")]
	static void OpenNameTableEditor()
	{
		GetFileContents();


		NameTableEditor window = ScriptableObject.CreateInstance<NameTableEditor>();
		window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 400, 600);
		window.ShowPopup();
	}

	private static string currentFileContents = "";
	private static string currFile = "Old Ones/Lovecraft Name Prefixes";

	void OnGUI()
	{
		EditorGUILayout.TextArea(currentFileContents, GUILayout.Height(390), GUILayout.Width(200));

		if (GUILayout.Button("Save"))
		{
			Debug.Log("save");
			this.Close();
			System.IO.File.WriteAllText(MyFile(), currentFileContents);
		}

		if (GUILayout.Button("Cancel"))
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
		return Path.Combine(myDir, currFile + ".txt");
	}
}