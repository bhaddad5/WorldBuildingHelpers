using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NameGenerator;
using UnityEngine;

public static class NameGeneratorUnity
{
	public static string GenerateName(string nameTable)
	{
		return GenerateNames(nameTable, 1).FirstOrDefault();
	}

	public static List<string> GenerateNames(string nameTable, int numToGenerate)
	{
		if (!nameTable.EndsWith(".txt"))
			nameTable += ".txt";
		NameRequestHandler gen = new NameRequestHandler();
		string path = Path.Combine(Application.streamingAssetsPath, Path.Combine("Name Generator/Name Tables/", nameTable));
		return gen.HandleNameRequest(path, numToGenerate);
	}
}
