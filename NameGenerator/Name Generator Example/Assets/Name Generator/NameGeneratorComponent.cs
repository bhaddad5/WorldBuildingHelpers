using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NameGenerator;
using UnityEngine;
using UnityEngine.UI;

public class NameGeneratorComponent : MonoBehaviour
{
	public Dropdown Dropdown;
	public Text DisplayText;

	public void GenerateName()
	{
		NameRequestHandler gen = new NameRequestHandler();
		string path = Path.Combine(Application.streamingAssetsPath, Path.Combine("Name Generator/Name Tables/", Dropdown.options[Dropdown.value].text + ".txt"));
		DisplayText.text = gen.HandleNameRequest(path, 1).FirstOrDefault();
	}
}
