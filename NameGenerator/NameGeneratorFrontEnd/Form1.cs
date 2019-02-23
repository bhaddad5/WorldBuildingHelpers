using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NameGenerator;

namespace NameGeneratorFrontEnd
{
	public partial class NameGen : Form
	{
		public NameGen()
		{
			InitializeComponent();

			this.treeView1.BeforeSelect += TreeViewAboutToMakeNewSelection;
			this.treeView1.AfterSelect += FileSelected;
			this.button3.Click += SaveOpenFile;
			this.KeyDown += CheckSave;
			this.KeyPreview = true;
		}

		private void SelectFolder_Click(object sender, EventArgs e)
		{
			//Hacky, but MUCH better UX than shitty folder selector
			using (OpenFileDialog openFolderDialog = new OpenFileDialog())
			{
				openFolderDialog.ValidateNames = false;
				openFolderDialog.CheckFileExists = false;
				openFolderDialog.CheckPathExists = true;
				openFolderDialog.FileName = "Folder Selection";

				DialogResult result = openFolderDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFolderDialog.SafeFileName))
				{
					string chosenFolder = Path.GetDirectoryName(openFolderDialog.FileName);

					if (!Directory.Exists(chosenFolder))
						MessageBox.Show("No Folder Selected", "Please select a folder.", MessageBoxButtons.OK, MessageBoxIcon.Error);
					else
						SelectRootFolder(chosenFolder);
				}
			}
		}

		private void GenerateNamesList_Click(object sender, EventArgs e)
		{
			richTextBox2.Text = "";
			var currentSelectedFile = treeView1.SelectedNode?.Tag;
			if (currentSelectedFile == null || !(currentSelectedFile is FileInfo))
			{
				MessageBox.Show("No Table Selected", "Please select a name table to generate from.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int num = 0;
			if (!Int32.TryParse(textBox1.Text, out num))
			{
				MessageBox.Show("Cannot parse entry count", "Could not parse the number of specified entries: " + textBox1.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (num <= 0)
			{
				MessageBox.Show("Number of entries must be > 0", "Please enter a number of entries to print greater than 0", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			NameRequestHandler ReqHandler = new NameRequestHandler();
			List<string> vals = ReqHandler.HandleNameRequest((currentSelectedFile as FileInfo).FullName, num);
			for (int i = 0; i < vals.Count; i++)
			{
				richTextBox2.Text += vals[i];
				if (i != vals.Count - 1)
					richTextBox2.Text += ",\n";
			}
		}

		private void SelectRootFolder(string path)
		{
			treeView1.Nodes.Clear();
			DisplayDirectory(new DirectoryInfo(path), null);
		}

		private TreeNode DisplayDirectory(DirectoryInfo dir, TreeNode parent)
		{
			TreeNode node = new TreeNode(dir.Name);
			node.Tag = dir;
			if (parent != null)
				parent.Nodes.Add(node);
			else
				treeView1.Nodes.Add(node);
			node.ContextMenu = DirectoryContextMenu(dir, node);
			foreach (DirectoryInfo childDirectory in dir.GetDirectories())
				DisplayDirectory(childDirectory, node);
			foreach (FileInfo file in dir.GetFiles("*.txt"))
				DisplayFileNode(file, node);
			return node;
		}

		private ContextMenu DirectoryContextMenu(DirectoryInfo dir, TreeNode dirNode)
		{
			MenuItem newFolderButton = new MenuItem("Create New Folder", (sender, args) => HandleMakeNewFolder(dir, dirNode));
			MenuItem newFileButton = new MenuItem("Create New Table", (sender, args) => HandleMakeNewTable(dir, dirNode));
			MenuItem deleteFolderButton = new MenuItem("Delete Folder", (sender, args) => HandleDeleteTableOrDirectory(dirNode));
			var cm = new ContextMenu();
			cm.MenuItems.Add(newFolderButton);
			cm.MenuItems.Add(newFileButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(deleteFolderButton);
			return cm;
		}

		private ContextMenu FileContextMenu(TreeNode fileNode)
		{
			MenuItem deleteFolderButton = new MenuItem("Delete Table", (sender, args) => HandleDeleteTableOrDirectory(fileNode));
			var cm = new ContextMenu();
			cm.MenuItems.Add(deleteFolderButton);
			return cm;
		}

		private void HandleDeleteTableOrDirectory(TreeNode node)
		{
			string name = "";
			if (node.Tag is FileInfo)
				name = (node.Tag as FileInfo).Name;
			if (node.Tag is DirectoryInfo)
				name = (node.Tag as DirectoryInfo).Name;

			DialogResult dialogResult = MessageBox.Show("Are you sure?", $"Are you sure you want to delete {name}?" + node.Tag, MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				if (node.Tag is FileInfo)
					(node.Tag as FileInfo).Delete();
				if (node.Tag is DirectoryInfo)
					DeleteDirectory((node.Tag as DirectoryInfo).FullName);
				node.Remove();
			}
		}

		//From: https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true
		public static void DeleteDirectory(string target_dir)
		{
			string[] files = Directory.GetFiles(target_dir);
			string[] dirs = Directory.GetDirectories(target_dir);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs)
			{
				DeleteDirectory(dir);
			}

			Directory.Delete(target_dir, false);
		}

		private void HandleMakeNewFolder(DirectoryInfo dir, TreeNode dirNode)
		{
			string newFolderName = FileDialog("Make a new Folder", "New Folder Name:");
			if (String.IsNullOrEmpty(newFolderName))
				return;

			string path = Path.Combine(dir.FullName, newFolderName);

			var invalidChar = newFolderName.IndexOfAny(Path.GetInvalidFileNameChars());
			if (invalidChar >= 0 ||
			    File.Exists(path))
			{
				MessageBox.Show("Please enter a valid new file name.", "Invalid new file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var newDir = Directory.CreateDirectory(path);
			DisplayDirectory(newDir, dirNode);
		}

		private void HandleMakeNewTable(DirectoryInfo dir, TreeNode dirNode)
		{
			string newTableName = FileDialog("Make a new Table", "New Table Name:");

			if (String.IsNullOrEmpty(newTableName))
				return;

			if (!newTableName.EndsWith(".txt"))
				newTableName += ".txt";
			string path = Path.Combine(dir.FullName, newTableName);

			var invalidChar = newTableName.IndexOfAny(Path.GetInvalidFileNameChars());
			if (invalidChar >= 0 ||
			    File.Exists(path))
			{
				MessageBox.Show("Please enter a valid new file name.", "Invalid new file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var stream = File.Create(path);
			stream.Close();
			var node = DisplayFileNode(new FileInfo(path), dirNode);
			treeView1.SelectedNode = node;
		}

		private string FileDialog(string text, string caption)
		{
			Form prompt = new Form()
			{
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
			TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
			Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
			confirmation.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(textLabel);
			prompt.AcceptButton = confirmation;

			return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
		}

		private TreeNode DisplayFileNode(FileInfo file, TreeNode parent)
		{
			TreeNode node = new TreeNode(file.Name);
			node.Tag = file;
			if (parent != null)
				parent.Nodes.Add(node);
			else
				treeView1.Nodes.Add(node);
			node.ContextMenu = FileContextMenu(node);
			return node;
		}

		private void SaveOpenFile(object sender, EventArgs e)
		{
			var currentSelectedFile = treeView1.SelectedNode?.Tag;
			if (currentSelectedFile == null || !(currentSelectedFile is FileInfo))
			{
				MessageBox.Show("No Table Selected", "Not currently editing any name table.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			File.WriteAllText((currentSelectedFile as FileInfo).FullName, selectedFileContents.Text);
		}

		private FileInfo currentSelectedFile = null;
		private void FileSelected(object sender, TreeViewEventArgs e)
		{
			if (!(e.Node.Tag is FileInfo))
				return;
			currentSelectedFile = (e.Node.Tag as FileInfo);
			var stream = currentSelectedFile.OpenText();
			selectedFileContents.Text = stream.ReadToEnd();
			stream.Close();
		}

		private void TreeViewAboutToMakeNewSelection(object sender, TreeViewCancelEventArgs e)
		{
			if (currentSelectedFile == null || !currentSelectedFile.Exists)
				return;

			var stream = currentSelectedFile.OpenText();
			string currFileText = stream.ReadToEnd();
			currFileText = currFileText.Replace("\r", "");
			stream.Close();
			if (currFileText.Equals(selectedFileContents.Text))
			{
				return;
			}

			DialogResult dialogResult = MessageBox.Show($"Save {currentSelectedFile.Name}?", "Save Changes?", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				File.WriteAllText(currentSelectedFile.FullName, selectedFileContents.Text);
			}
		}

		private void CheckSave(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
			{
				SaveOpenFile(null, null);
			}
		}
	}
}
