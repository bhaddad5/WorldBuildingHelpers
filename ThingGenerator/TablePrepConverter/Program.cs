using System;
using System.IO;

namespace TablePrepConverter
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] data = File.ReadAllLines("../../../data/TablePrep.txt");

			string res = "";

			foreach (string d in data)
			{
				var ln = d.Replace("–", "-");
				ln = ln.Replace("\"", "'");

				bool isValid = ln.Length > 0 && Int32.TryParse(ln[0].ToString(), out int tmp1);

				if (!isValid)
				{
					res += "\n" + ln;
					continue;
				}

				var valSplit = ln.Split('\t');

				if (valSplit.Length != 2)
				{
					res += "\n" + ln;
					continue;
				}

				string probStr = valSplit[0];
				string value = valSplit[1];

				int prob = 0;
				var probs = probStr.Split('-');
				if (probs.Length == 1)
					prob = 1;
				else if (probs.Length == 2)
				{
					Int32.TryParse(probs[0], out int prob1);
					Int32.TryParse(probs[1], out int prob2);
					prob = prob2 - prob1 + 1;
				}

				res += $"\n{prob} - \"{value}\"";

				File.WriteAllText("../../../data/TablePrep.txt", res);
			}
		}
	}
}
