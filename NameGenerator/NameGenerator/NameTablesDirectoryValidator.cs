using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NameGenerator
{
	public class NameTablesDirectoryValidator
	{
		public string errorMsg = null;

		public bool ValidateDirectory(string path)
		{
			DirectoryInfo dir = new DirectoryInfo(path);
			foreach (DirectoryInfo subDir in dir.GetDirectories())
			{
				if (!ValidateDirectory(subDir.FullName))
					return false;
			}

			foreach (FileInfo file in dir.GetFiles())
			{
				if (file.Extension != ".txt")
					continue;
				var validator = new NameTableValidator();
				if (!validator.ValidateTable(file.FullName, new HashSet<string>()))
				{
					errorMsg = validator.errorMsg;
					return false;
				}
			}

			return true;
		}
	}

	public class NameTableValidator
	{
		public string errorMsg = null;

		//Does anybody else see a butt hole when they look at the term A* Algorithm?
		public bool ValidateTable(string myTablePath, HashSet<string> visitedTables)
		{
			visitedTables.Add(myTablePath);
			foreach (string entry in CommonTableHandlers.ParseNamesFromFile(myTablePath))
			{
				if (entry.Contains("{"))
				{
					var refs = GetRefsFromEntry(entry);
					if (errorMsg != null)
					{
						errorMsg += $"\n\nIn table:\n{myTablePath}";
						return false;
					}
					foreach (string r in refs)
					{
						var tableReferencePath = Path.Combine(Path.GetDirectoryName(myTablePath), r);
						if (visitedTables.Contains(tableReferencePath))
						{
							errorMsg = $"Circular dependency present.\n\nTable can loop back on itself:\n{tableReferencePath}";
							return false;
						}
						
						if (!File.Exists(tableReferencePath))
						{
							errorMsg = $"File not found.\n\nCould not find:\n{tableReferencePath}\n\nReferenced from:\n{myTablePath}";
							return false;
						}

						if (!ValidateTable(tableReferencePath, new HashSet<string>(visitedTables)))
							return false;
					}
				}
			}
			return true;
		}

		private List<string> GetRefsFromEntry(string entry)
		{
			List<string> refs = new List<string>();
			string currentBuilderStr = null;
			foreach (char c in entry)
			{
				if (c == '{')
				{
					if (currentBuilderStr != null)
					{
						errorMsg = $"Invalid brace formatting in entry:\n{entry}";
						return new List<string>();
					}

					currentBuilderStr = "";
				}
				else if (c == '}')
				{
					if (currentBuilderStr == null)
					{
						errorMsg = $"Invalid brace formatting in entry:\n{entry}";
						return new List<string>();
					}

					if (!currentBuilderStr.EndsWith(".txt"))
						currentBuilderStr += ".txt";

					refs.Add(currentBuilderStr);
					currentBuilderStr = null;
				}
				else if (currentBuilderStr != null)
				{
					currentBuilderStr += c;
				}
			}

			if (currentBuilderStr != null)
			{
				errorMsg = $"Unclosed '{{' in entry:\n{entry}";
			}
			return refs;
		}
	}
}
