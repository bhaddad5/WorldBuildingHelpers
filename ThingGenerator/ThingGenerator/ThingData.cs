using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ThingData
{
	public string ThingId;
	public string ThingName;

	public List<string> AttributesIds = new List<string>();

	public string ResolveThingToString()
	{
		string res = $"{ThingName}:";
		foreach (string attributeId in AttributesIds)
		{
			AttributeData attribute = ThingGenerator.AvailableAttributes.FirstOrDefault(attr => attr.AttributeId == attributeId);
			if (attribute == null)
				throw new Exception("Failed to find attribute with id: " + attributeId);

			res += $"\n\t{attribute.ResolveAttributeToString(true)}";
		}
		return res;
	}
}
