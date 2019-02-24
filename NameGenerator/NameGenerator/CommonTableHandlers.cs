using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NameGenerator
{
	public static class CommonTableHandlers
	{
		public static string[] ParseNamesFromFile(string path)
		{
			if (!File.Exists(path))
			{
				Console.WriteLine("Could not find file: " + path);
				return new string[0];
			}

			string fileContents = File.ReadAllText(path);
			fileContents = fileContents.Replace("\n", "");
			fileContents = fileContents.Replace("\r", "");
			if (fileContents.EndsWith(","))
				fileContents = fileContents.Remove(fileContents.Length - 1);
			return fileContents.Split(',');
		}
	}
}
