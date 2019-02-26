using System;
using System.Collections.Generic;
using System.Drawing;
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

			this.treeView1.ItemDrag += ItemDragged;
			this.treeView1.DragEnter += ItemDragEnter;
			this.treeView1.DragDrop += MoveFile;

			this.button3.Click += SaveOpenFile;
			this.KeyDown += CheckSave;
			this.KeyPreview = true;
			this.textBox1.TextChanged += TypedInFile;
			this.button4.Click += ValidateCurrentDirectory;
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
			var rootDir = DisplayDirectory(new DirectoryInfo(path), null);
			rootDir.Expand();
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
			MenuItem renameFolderButton = new MenuItem("Rename Folder", (sender, args) => HandleRenameTableOrDirectory(dirNode));
			MenuItem deleteFolderButton = new MenuItem("Delete Folder", (sender, args) => HandleDeleteTableOrDirectory(dirNode));
			var cm = new ContextMenu();
			cm.MenuItems.Add(newFolderButton);
			cm.MenuItems.Add(newFileButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(renameFolderButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(deleteFolderButton);
			return cm;
		}

		private ContextMenu FileContextMenu(TreeNode fileNode)
		{
			MenuItem deleteFolderButton = new MenuItem("Delete Table", (sender, args) => HandleDeleteTableOrDirectory(fileNode));
			MenuItem renameFileButton = new MenuItem("Rename File", (sender, args) => HandleRenameTableOrDirectory(fileNode));
			var cm = new ContextMenu();
			cm.MenuItems.Add(renameFileButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(deleteFolderButton);
			return cm;
		}

		private void HandleRenameTableOrDirectory(TreeNode node)
		{
			string newName = FileDialog("Rename", "Enter new name:", node.Text);
			if (String.IsNullOrEmpty(newName))
				return;

			string startingPath = null;
			string newPath = null;
			if (node.Tag is FileInfo)
			{
				startingPath = Path.Combine((node.Tag as FileInfo).DirectoryName, (node.Tag as FileInfo).Name);
				newPath = Path.Combine((node.Tag as FileInfo).DirectoryName, newName);
				if (!newPath.EndsWith(".txt"))
					newPath += ".txt";
			}
			if (node.Tag is DirectoryInfo)
			{
				startingPath = (node.Tag as DirectoryInfo).FullName;
				newPath = Path.Combine((node.Tag as DirectoryInfo).Parent.FullName, newName);
			}
			
			var invalidChar = newName.IndexOfAny(Path.GetInvalidFileNameChars());
			if (invalidChar >= 0 || File.Exists(newPath))
			{
				MessageBox.Show("Please enter a valid new file name.", "Invalid new file name.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (node.Tag is FileInfo)
			{
				File.Move(startingPath, newPath);

				var newFileInfo = new FileInfo(newPath);
				if (currentSelectedFile == node.Tag)
					currentSelectedNode.Tag = newFileInfo;
				node.Tag = newFileInfo;
			}

			if (node.Tag is DirectoryInfo)
			{
				Directory.Move(startingPath, newPath);
				node.Tag = new DirectoryInfo(newPath);
			}
			node.Text = newName;
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
				{
					if (currentSelectedFile == (node.Tag as FileInfo))
						currentSelectedNode = null;
					(node.Tag as FileInfo).Delete();
				}
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
			string newFolderName = FileDialog("Make a new Folder", "New Folder Name:", "");
			if (String.IsNullOrEmpty(newFolderName))
				return;

			string path = Path.Combine(dir.FullName, newFolderName);

			var invalidChar = newFolderName.IndexOfAny(Path.GetInvalidFileNameChars());
			if (invalidChar >= 0 || File.Exists(path))
			{
				MessageBox.Show("Please enter a valid new file name.", "Invalid new file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var newDir = Directory.CreateDirectory(path);
			DisplayDirectory(newDir, dirNode);
		}

		private void HandleMakeNewTable(DirectoryInfo dir, TreeNode dirNode)
		{
			string newTableName = FileDialog("Make a new Table", "New Table Name:", "");

			if (String.IsNullOrEmpty(newTableName))
				return;

			if (!newTableName.EndsWith(".txt"))
				newTableName += ".txt";
			string path = Path.Combine(dir.FullName, newTableName);

			var invalidChar = newTableName.IndexOfAny(Path.GetInvalidFileNameChars());
			if (invalidChar >= 0 || File.Exists(path))
			{
				MessageBox.Show("Please enter a valid new file name.", "Invalid new file name", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var stream = File.Create(path);
			stream.Close();
			var node = DisplayFileNode(new FileInfo(path), dirNode);
			treeView1.SelectedNode = node;
		}

		private string FileDialog(string text, string caption, string startingStr)
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
			TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, Text = startingStr };
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

		private TreeNode currentSelectedNode = null;
		private FileInfo currentSelectedFile => currentSelectedNode?.Tag as FileInfo;
		private void FileSelected(object sender, TreeViewEventArgs e)
		{
			currentSelectedNode = null;
			selectedFileContents.Text = "";
			selectedFileContents.ReadOnly = true;
			if (!(e.Node.Tag is FileInfo))
				return;
			currentSelectedNode = e.Node;
			var stream = currentSelectedFile.OpenText();
			selectedFileContents.Text = stream.ReadToEnd();
			selectedFileContents.ReadOnly = false;
			stream.Close();
		}

		private void TreeViewAboutToMakeNewSelection(object sender, TreeViewCancelEventArgs e)
		{
			if (currentSelectedNode == null || !currentSelectedFile.Exists)
				return;

			var stream = currentSelectedFile.OpenText();
			string currFileText = stream.ReadToEnd();
			currFileText = currFileText.Replace("\r", "");
			stream.Close();
			if (currFileText.Equals(selectedFileContents.Text))
			{
				return;
			}

			DialogResult dialogResult = MessageBox.Show($"Save {currentSelectedNode.Name}?", "Save Changes?", MessageBoxButtons.YesNo);
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

		private void ValidateCurrentDirectory(object sender, EventArgs e)
		{
			NameTablesDirectoryValidator validator = new NameTablesDirectoryValidator();
			if (treeView1.Nodes.Count == 0)
				return;
			if(currentSelectedNode != null)
				SaveOpenFile(null, null);
			if (!validator.ValidateDirectory((treeView1.Nodes[0].Tag as DirectoryInfo).FullName))
			{
				MessageBox.Show(validator.errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				MessageBox.Show( "No problems detected with current tables :)", "All good!", MessageBoxButtons.OK, MessageBoxIcon.None);
			}
		}

		private void ItemDragged(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				DoDragDrop(e.Item, DragDropEffects.Move);
			}
		}

		private void ItemDragEnter(object sender, DragEventArgs e)
		{
			Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
			TreeNode targetNode = treeView1.GetNodeAt(targetPoint);
			//if (targetNode?.Tag is DirectoryInfo)
				e.Effect = DragDropEffects.Move;
		}

		private void MoveFile(object sender, DragEventArgs e)
		{
			Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
			TreeNode targetNode = treeView1.GetNodeAt(targetPoint);

			// Retrieve the node that was dragged.
			TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

			if (targetNode != null && targetNode.Tag is DirectoryInfo)
			{
				if (draggedNode.Tag is FileInfo)
				{
					string oldPath = (draggedNode.Tag as FileInfo).FullName;
					string newPath = Path.Combine((targetNode.Tag as DirectoryInfo).FullName, (draggedNode.Tag as FileInfo).Name);
					(draggedNode.Tag as FileInfo).MoveTo(newPath);
					if(treeView1.Nodes.Count > 0 && (treeView1.Nodes[0].Tag is DirectoryInfo))
						TableManagementHelpers.UpdateNameAcrossAllFiles((treeView1.Nodes[0].Tag as DirectoryInfo).FullName, oldPath, newPath);
				}
				if (draggedNode.Tag is DirectoryInfo)
				{
					string oldPath = (draggedNode.Tag as DirectoryInfo).FullName;
					string newPath = Path.Combine((targetNode.Tag as DirectoryInfo).FullName, (draggedNode.Tag as DirectoryInfo).Name);
					(draggedNode.Tag as DirectoryInfo).MoveTo(newPath);
					if (treeView1.Nodes.Count > 0 && (treeView1.Nodes[0].Tag is DirectoryInfo))
						TableManagementHelpers.UpdateNameAcrossAllFiles((treeView1.Nodes[0].Tag as DirectoryInfo).FullName, oldPath, newPath);

				}
				draggedNode.Remove();
				targetNode.Nodes.Add(draggedNode);
				targetNode.Expand();
			}
		}

		private void TypedInFile(object sender, EventArgs e)
		{

		}
	}
}
