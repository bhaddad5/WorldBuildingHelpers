using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NameGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			string dataFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\Data");

			NameRequestHandler reqHandler = new NameRequestHandler();

			string path = null;

			while (path == null)
			{
				Console.WriteLine("Input a file to randomly print from:");
				string fileToOpen = Console.ReadLine();
				path = Path.Combine(dataFolderPath, fileToOpen);
				if (!File.Exists(path))
					path = null;
			}
			
			int num = 0;
			while (num <= 0)
			{
				Console.WriteLine("Input number of names to print:");
				string numStr = Console.ReadLine();
				Int32.TryParse(numStr, out num);
			}
			List<string> results = reqHandler.HandleNameRequest(path, num);

			Console.WriteLine("Results:");
			foreach (string result in results)
			{
				Console.WriteLine(result);
			}

			Console.ReadLine();
		}

		
	}
}
