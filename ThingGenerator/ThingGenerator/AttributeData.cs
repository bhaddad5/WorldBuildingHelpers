using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AttributeOption
{
	public int Probability;
	public string AttributeString;
	public string ResolveAttributeOptionToString()
	{
		string result = AttributeString;
		if (AttributeString.StartsWith("("))
		{
			var attributeId = AttributeString;
			attributeId = attributeId.Replace("(", "");
			attributeId = attributeId.Replace(")", "");
			AttributeData attribute = ThingGenerator.AvailableAttributes.FirstOrDefault(attr => attr.AttributeId == attributeId);
			if (attribute == null)
				throw new Exception("Failed to find attribute with id: " + attributeId);

			result = attribute.ResolveAttributeToString(false);
		}

		return ResolveNameReferences(AttributeString);
	}

	private string ResolveNameReferences(string str)
	{
		string res = "";
		string resolvingTableRefernce = null;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '{')
			{
				resolvingTableRefernce = "";
			}
			else if (str[i] == '}')
			{
				var table = NameGenerator.AvailableNameTables.FirstOrDefault(nt =>
					nt.NameTableId == resolvingTableRefernce);
				if (table == null)
					throw new Exception($"Could not find table with id {resolvingTableRefernce}");
				res += table.ResolveNameTableToString();
				resolvingTableRefernce = null;
			}
			else
			{
				if (resolvingTableRefernce != null)
					resolvingTableRefernce += str[i];
				else
					res += str[i];
			}
		}

		return res;
	}
}

public class AttributeData
{
	public string AttributeId;
	public string AttributeName;
	public List<AttributeOption> AttributeOptions = new List<AttributeOption>();

	public string ResolveAttributeToString(bool includeAttributeName)
	{
		int totalProb = 0;
		foreach (AttributeOption option in AttributeOptions)
		{
			totalProb += option.Probability;
		}

		int val = new Random().Next(0, totalProb);

		foreach (AttributeOption option in AttributeOptions)
		{
			if (option.Probability > val)
			{
				if (!includeAttributeName)
					return option.ResolveAttributeOptionToString();
				else
					return $"{AttributeName}: {option.ResolveAttributeOptionToString()}";
			}
			val -= option.Probability;
		}

		throw new Exception("Total probability error.");
	}
}