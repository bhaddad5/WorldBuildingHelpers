using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NameGenerator;
using UnityEngine;
using UnityEngine.UI;

public class NameGeneratorExample : MonoBehaviour
{
	public Dropdown Dropdown;
	public Text DisplayText;

	public void GenerateName()
	{
		string dropdownValue = Dropdown.options[Dropdown.value].text;
		DisplayText.text = NameGeneratorUnity.GenerateName(dropdownValue);
	}
}
