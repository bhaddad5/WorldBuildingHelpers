using System.Collections;
using System.Collections.Generic;
using System.IO;
using NameGenerator;
using UnityEngine;

public class NameGeneratorWrapper
{
	public List<string> GenerateNames(string nameTable, int numToGenerate)
	{
		if (!nameTable.EndsWith(".txt"))
			nameTable += ".txt";
		NameRequestHandler gen = new NameRequestHandler();
		string path = Path.Combine(Application.streamingAssetsPath, Path.Combine("Name Generator/Name Tables/", nameTable));
		return gen.HandleNameRequest(path, numToGenerate);
	}
}
