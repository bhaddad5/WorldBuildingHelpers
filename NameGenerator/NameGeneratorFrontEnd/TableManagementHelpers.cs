using System;
using System.Collections.Generic;
using System.IO;

namespace NameGeneratorFrontEnd
{
	public static class TableManagementHelpers
	{
		public static void UpdateNameAcrossAllFiles(string rootDir, string oldPath, string newPath)
		{
			if(newPath.EndsWith(".txt"))
				UpdateOutgoingRefs(oldPath, newPath);

			foreach (FileInfo file in GetAllFiles(new DirectoryInfo(rootDir)))
				UpdateRefsInFile(file, oldPath, newPath);
		}

		private static void UpdateOutgoingRefs(string oldPath, string newPath)
		{
			var refs = ParseAllRefsInFile(newPath);
			string text = File.ReadAllText(newPath);
			foreach (string oldRef in refs)
			{
				text = text.Replace(oldRef, UpdateOldRefToNewRef(oldRef, oldPath, newPath));
			}
			File.WriteAllText(newPath, text);
		}

		private static HashSet<string> ParseAllRefsInFile(string filePath)
		{
			HashSet<string> refs = new HashSet<string>();
			string text = File.ReadAllText(filePath);
			string currBuildingRef = null;
			foreach (char c in text)
			{
				if (c == '{')
				{
					currBuildingRef = "";
				}
				else if(c == '}')
				{
					refs.Add(currBuildingRef);
					currBuildingRef = null;
				}
				else if (currBuildingRef != null)
				{
					currBuildingRef += c;
				}
			}

			return refs;
		}

		private static string UpdateOldRefToNewRef(string oldRef, string oldFilePath, string newFilePath)
		{
			string oldRefFullDestination = Path.Combine(Path.GetDirectoryName(oldFilePath), oldRef);
			if (oldRefFullDestination.EndsWith(".txt"))
				oldRefFullDestination = oldRefFullDestination.Replace(".txt", "");

			string pathRelativeToNewFile = new Uri(newFilePath).MakeRelativeUri(new Uri(oldRefFullDestination)).ToString();
			pathRelativeToNewFile = pathRelativeToNewFile.Replace("%20", " ");
			if (pathRelativeToNewFile.EndsWith(".txt"))
				pathRelativeToNewFile = pathRelativeToNewFile.Replace(".txt", "");
			return pathRelativeToNewFile;
		}

		private static void UpdateRefsInFile(FileInfo file, string oldPath, string newPath)
		{
			string pathRelativeToOldFile = new Uri(file.FullName).MakeRelativeUri(new Uri(oldPath)).ToString();
			pathRelativeToOldFile = pathRelativeToOldFile.Replace("%20", " ");
			if (pathRelativeToOldFile.EndsWith(".txt"))
				pathRelativeToOldFile = pathRelativeToOldFile.Replace(".txt", "");
			else if (!pathRelativeToOldFile.EndsWith("/"))
				pathRelativeToOldFile += "/";
			string pathRelativeToNewFile = new Uri(file.FullName).MakeRelativeUri(new Uri(newPath)).ToString();
			pathRelativeToNewFile = pathRelativeToNewFile.Replace("%20", " ");
			if (pathRelativeToNewFile.EndsWith(".txt"))
				pathRelativeToNewFile = pathRelativeToNewFile.Replace(".txt", "");
			else if (!pathRelativeToNewFile.EndsWith("/"))
				pathRelativeToNewFile += "/";

			var reader = file.OpenText();
			string text = reader.ReadToEnd();
			reader.Close();
			if (text.Contains(pathRelativeToOldFile))
			{
				text = text.Replace(pathRelativeToOldFile, pathRelativeToNewFile);
				File.WriteAllText(file.FullName, text);
			}
		}

		public static List<FileInfo> GetAllFiles(this DirectoryInfo dir)
		{
			List<FileInfo> files = new List<FileInfo>();
			foreach (FileInfo file in dir.GetFiles())
			{
				if(file.Extension.Contains(".txt"))
					files.Add(file);
			}
			foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
			{
				foreach (FileInfo file in GetAllFiles(directoryInfo))
					files.Add(file);
			}

			return files;
		}
	}
}
