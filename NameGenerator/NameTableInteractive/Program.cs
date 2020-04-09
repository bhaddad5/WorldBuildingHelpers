using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameTableInteractive
{
	class Program
	{
		static void Main(string[] args)
		{
			List<string> data = new List<string>();

			foreach (string file in Directory.GetFiles("../../../data"))
			{
				if (Path.GetExtension(file) == ".txt")
				{
					data = data.Concat(File.ReadAllLines(file)).ToList();
				}
			}

			List<NameTableData> names = new List<NameTableData>();

			NameTableData currTable = null;

			int lineNum = 0;
			foreach (string ln in data)
			{
				lineNum++;

				var s = ln.Trim();

				if (string.IsNullOrEmpty(s))
					continue;

				if (s.StartsWith("{") && s.EndsWith("}:"))
				{
					s = s.Replace("{", "");
					s = s.Replace("}", "");
					s = s.Replace(":", "");
					s = s.Replace("\"", "");
					var currThingData = new NameTableData();
					currThingData.NameTableId = s;
					currTable = currThingData;
					names.Add(currThingData);
				}
				else
				{
					s = s.Replace("\"", "");
					currTable.NameOptions.Add(s);
				}
			}

			NameGenerator.AvailableNameTables = names;

			Console.WriteLine(names[0].ResolveNameTableToString());
		}
	}
}
