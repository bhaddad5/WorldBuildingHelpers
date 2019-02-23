using System;
using System.Collections.Generic;
using System.IO;

namespace NameGenerator
{
	public class NameRequestHandler
	{
		private Dictionary<string, string[]> CachedNameFiles = new Dictionary<string, string[]>();
		private Random Rand = new Random();

		public List<string> HandleNameRequest(string path, int numNames)
		{
			List<string> results = new List<string>();
			for (int i = 0; i < numNames; i++)
			{
				var name = GetNameFromCachedList(path);
				if (name != null)
					results.Add(name);
			}
			return results;
		}

		private void GetAndCacheNameFiles(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Could not find file: " + path);
				return;
			}

			string fileContents = File.ReadAllText(path);
			fileContents = fileContents.Replace("\n", "");
			fileContents = fileContents.Replace("\r", "");
			if (fileContents.EndsWith(","))
				fileContents = fileContents.Remove(fileContents.Length - 1);
			CachedNameFiles[path] = fileContents.Split(',');
		}

		private string GetNameFromCachedList(string path)
		{
			if (!CachedNameFiles.ContainsKey(path))
				GetAndCacheNameFiles(path);

			if (!CachedNameFiles.ContainsKey(path))
				return null;

			int randomNumber = Rand.Next(0, CachedNameFiles[path].Length);
			return ResolveName(CachedNameFiles[path][randomNumber], path);
		}

		private string ResolveName(string name, string nameFilePath)
		{
			string resolvedString = "";
			string currentRefString = "";
			bool lookingAtRefString = false;
			foreach (char c in name)
			{
				if (c == '{')
				{
					lookingAtRefString = true;
				}
				else if (c == '}')
				{
					if (!currentRefString.EndsWith(".txt"))
						currentRefString += ".txt";
					string refPath = Path.Combine(Path.GetDirectoryName(nameFilePath), currentRefString);
					string resolvedRefString = GetNameFromCachedList(refPath);
					resolvedString += resolvedRefString;
					lookingAtRefString = false;
					currentRefString = "";
				}
				else if (lookingAtRefString)
				{
					currentRefString += c;
				}
				else
				{
					resolvedString += c;
				}
			}
			return resolvedString;
		}
	}
}
