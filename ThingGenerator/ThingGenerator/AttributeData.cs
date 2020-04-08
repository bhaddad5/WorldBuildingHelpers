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
		if (AttributeString.StartsWith("("))
		{
			var attributeId = AttributeString;
			attributeId = attributeId.Replace("(", "");
			attributeId = attributeId.Replace(")", "");
			AttributeData attribute = Generator.AvailableAttributes.FirstOrDefault(attr => attr.AttributeId == attributeId);
			if (attribute == null)
				throw new Exception("Failed to find attribute with id: " + attributeId);

			return attribute.ResolveAttributeToString(false);
		}

		return AttributeString;
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