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
	private NameGeneratorWrapper nameGenerator = new NameGeneratorWrapper();

	public void GenerateName()
	{
		string dropdownValue = Dropdown.options[Dropdown.value].text;
		DisplayText.text = nameGenerator.GenerateNames(dropdownValue, 1).FirstOrDefault();
	}
}
