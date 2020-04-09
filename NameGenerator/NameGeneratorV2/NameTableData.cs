using System;
using System.Collections.Generic;
using System.Linq;

public class NameTableData
{
	public string NameTableId;
	public List<string> NameOptions = new List<string>();

	public string ResolveNameTableToString()
	{
		string chosenNameOption = NameOptions[new Random().Next(0, NameOptions.Count)];

		string res = "";
		string resolvingTableRefernce = null;
		for (int i = 0; i < chosenNameOption.Length; i++)
		{
			if (chosenNameOption[i] == '{')
			{
				resolvingTableRefernce = "";
			}
			else if (chosenNameOption[i] == '}')
			{
				var table = Generator.AvailableNameTables.FirstOrDefault(nt =>
					nt.NameTableId == resolvingTableRefernce);
				if (table == null)
					throw new Exception($"Could not find table with id {resolvingTableRefernce}");
				res += table.ResolveNameTableToString();
				resolvingTableRefernce = null;
			}
			else
			{
				if (resolvingTableRefernce != null)
					resolvingTableRefernce += chosenNameOption[i];
				else
					res += chosenNameOption[i];
			}
		}

		return res;
	}
}