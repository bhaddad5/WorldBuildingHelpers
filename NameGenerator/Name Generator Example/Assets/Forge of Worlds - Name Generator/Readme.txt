IMPORTANT:
DO AFTER INSTALL:
SERIOUSLY:
LIKE, IF YOU WANT IT TO WORK AT ALL:
	- Drag the contents of "Forge of Worlds - Name Generator/StreamingAssets" into your "Assets/StreamingAssets" folder.
OK, pheawph, you're good.

0. Glossary.
	- Name Chunk -	A string that can contain text as well as references to other Name Tables.

	- Name Table -	A list of Name Chunks.  These are stores in an external .txt file and are seperated by ',' characters.

1. How it works.

	Put simply, when getting a random name from the system you are simply getting a random Name Chunk from a specified Name Table.

	These Name Chunks are resolved when they are accessed, so if they contain a reference to another Name Table they will, in turn, get a random Name Chunk from it.

	They reference other tables by including a relative path to said table from their current table inside a pair of {}.  These references can, but need not, include the tables ".txt" extension.

	EXAMPLE:
		ExampleWeapons.txt
			{ExampleMetals} {ExampleSwords},
			{ExampleAdectives} {ExampleMetals} {ExampleSwords},
			The {ExampleAdectives} {ExampleSwords}

		ExampleMetals.txt
			Golden,
			Iron,
			Steel

		ExampleSwords.txt
			Sword,
			Blade,
			Scimitar

		ExampleAdjectives.txt
			Bright,
			Shining,
			Cursed

		Generate Names From (ExampleWeapons) => 
			"Cursed Iron Scimitar",
			"Golden Blade",
			"The Shining Sword",
			"Iron Sword"

	A Name Table assumes that all of its entries have an equal probability of being chosen.  Therefore, if you wish to give some names a higher probability you can include them multiple times.

	NOTE: You MAY NOT include circular references, where two Name Tables each reference the other.  This can cause infinite loops and StackOverflow exceptions.  The NameTableEditor (described below) includes tools to check for these, and other, problems.

	NOTE: New Line (\n) characters will not be included in parsed Name Chunks!

2. Accessing tables from Unity

	Since all the data/logic is kept in Streaming Assets, all you need to do to in your Unity code is call: 

		string myName = NameGeneratorUnity.GenerateName("myNameTable");

	"myNameTable" is a string that will search for a relevant table under "StreamingAssets/Name Generator/Name Tables/".  It can include a relative path.

3. Editing/creating tables in the NameTableEditor

	Since Name Tables are simply .txt files, you can edit and manage them through a simple text editor like Notepad.

	However, the project does come with a much more ueful/convenient solution for data management.  A shortcut to the Name Table Editor sits in StreamingAssets/Name Generator.

	Once you start the program, click "Select Folder" and pick the root folder of your Name Table Files (Probably: "StreamingAssets/Name Generator/Name Tables/").

	You can then expand your folders, as well as create/delete/rename folders and tables.
	
	Once you click a Name Table, it's contents will be displayed in the adjacent text field.  Here you can edit them at will.

	Once you have made your edits, hit "Save" to save the file. (NOTE: the save prompt will emerge in some cases if you stop editing, but not all.  Be careful not to lose work!)

	To test the file you currently have selected, click "Generate Names" and specify the number to create.  This will output a list of example names that the program could generate.

	To check if you made any mistakes in your work, click "Validate All Tables".  This will check for errors in all NameTables accessible from your current root folder.