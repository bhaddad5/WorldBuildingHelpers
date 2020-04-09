using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThingGeneratorInteractive
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

			List<ThingData> things = new List<ThingData>();
			List<AttributeData> attributes = new List<AttributeData>();
			List<NameTableData> names = new List<NameTableData>();

			object currTable = null;
			
			int lineNum = 0;
			foreach (string ln in data)
			{
				lineNum++;

				var s = ln.Trim();

				if (string.IsNullOrEmpty(s))
					continue;

				if (s.StartsWith("[") && s.EndsWith("]:"))
				{
					s = s.Replace("[", "");
					s = s.Replace("]", "");
					s = s.Replace(":", "");
					s = s.Replace("\"", "");
					var idAndName = s.Split("-", 2);
					if(idAndName.Length != 2)
						throw new Exception($"Invalid definition on line: {lineNum}");
					var currThingData = new ThingData();
					currThingData.ThingId = idAndName[0];
					currThingData.ThingName = idAndName[1];
					currTable = currThingData;
					things.Add(currThingData);
				}
				else if (s.StartsWith("(") && s.EndsWith("):"))
				{
					s = s.Replace("(", "");
					s = s.Replace(")", "");
					s = s.Replace(":", "");
					s = s.Replace("\"", "");
					var idAndName = s.Split("-", 2);
					if (idAndName.Length != 2)
						throw new Exception($"Invalid definition on line: {lineNum}");
					var currAttributeData = new AttributeData();
					currAttributeData.AttributeId = idAndName[0];
					currAttributeData.AttributeName = idAndName[1];
					currTable = currAttributeData;
					attributes.Add(currAttributeData);
				}
				else
				{
					s = s.Replace("\"", "");
					if (s.Contains("-"))
					{
						if (currTable is AttributeData)
						{
							var probAndStr = s.Split('-', 2);
							if (probAndStr.Length != 2)
								throw new Exception($"Invalid attribute option on line: {lineNum}");
							Int32.TryParse(probAndStr[0].Trim(), out int prob);
							if(prob <= 0)
								throw  new Exception($"Invalid probability on line: {lineNum}");
							var attrOpt = new AttributeOption();
							attrOpt.Probability = prob;
							attrOpt.AttributeString = probAndStr[1].Trim();

							(currTable as AttributeData).AttributeOptions.Add(attrOpt);
						}
					}
					else
					{
						if(currTable is ThingData)
							(currTable as ThingData).AttributesIds.Add(s);
					}
				}
			}

			Generator.AvailableAttributes = attributes;
			Generator.AvailableThings = things;

			Console.WriteLine(things[0].ResolveThingToString());
		}
	}
}
