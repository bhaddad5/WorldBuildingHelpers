using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NameGenerator;

namespace NameGeneratorFrontEnd
{
	public partial class NameGen : Form
	{
		private TreeUiHandler treeHandler = null;
		private ShittyMoveWatcher moveWatcher = new ShittyMoveWatcher();

		public NameGen()
		{
			InitializeComponent();

			this.fileTreeView.TreeViewNodeSorter = new NodeSorter();

			this.fileTreeView.AfterSelect += FileSelected;

			this.fileTreeView.ItemDrag += ItemDragged;
			this.fileTreeView.DragEnter += ItemDragEnter;
			this.fileTreeView.DragDrop += MoveFile;

			this.button3.Click += SaveOpenFile;
			this.KeyDown += CheckSave;
			this.KeyPreview = true;
			this.button4.Click += ValidateCurrentDirectory;

			var lastFolder = Properties.Settings.Default["LastFolder"] as string;
			if (lastFolder != null && Directory.Exists(lastFolder))
				ShowDirectory(lastFolder);
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
						ShowDirectory(chosenFolder);
				}
			}
		}

		private void ShowDirectory(string path)
		{
			treeHandler?.Shutdown();
			treeHandler = new TreeUiHandler();
			treeHandler.Setup(path, fileTreeView, moveWatcher, DirectoryContextMenu, FileContextMenu);
			Properties.Settings.Default["LastFolder"] = path;
			Properties.Settings.Default.Save();
		}

		private void GenerateNamesList_Click(object sender, EventArgs e)
		{
			richTextBox2.Text = "";
			if (currentSelectedNode == null)
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
			List<string> vals = ReqHandler.HandleNameRequest(GetCurrSelectedFile().FullName, num);
			for (int i = 0; i < vals.Count; i++)
			{
				richTextBox2.Text += vals[i];
				if (i != vals.Count - 1)
					richTextBox2.Text += ",\n";
			}
		}

		private ContextMenu DirectoryContextMenu(string dir)
		{
			MenuItem newFolderButton = new MenuItem("Create New Folder", (sender, args) => HandleMakeNewFolder(dir));
			MenuItem newFileButton = new MenuItem("Create New Table", (sender, args) => HandleMakeNewTable(dir));
			MenuItem renameFolderButton = new MenuItem("Rename Folder", (sender, args) => HandleRenameTableOrDirectory(dir));
			MenuItem deleteFolderButton = new MenuItem("Delete Folder", (sender, args) => HandleDeleteTableOrDirectory(dir));
			var cm = new ContextMenu();
			cm.MenuItems.Add(newFolderButton);
			cm.MenuItems.Add(newFileButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(renameFolderButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(deleteFolderButton);
			return cm;
		}

		private ContextMenu FileContextMenu(string file)
		{
			MenuItem deleteFolderButton = new MenuItem("Delete Table", (sender, args) => HandleDeleteTableOrDirectory(file));
			MenuItem renameFileButton = new MenuItem("Rename File", (sender, args) => HandleRenameTableOrDirectory(file));
			var cm = new ContextMenu();
			cm.MenuItems.Add(renameFileButton);
			cm.MenuItems.Add("-");
			cm.MenuItems.Add(deleteFolderButton);
			return cm;
		}

		private void HandleRenameTableOrDirectory(string path)
		{
			string newName = FileDialog("Rename", "Enter new name:", Path.GetFileNameWithoutExtension(path));
			if (String.IsNullOrEmpty(newName))
				return;

			string newPath = Path.Combine(Path.GetDirectoryName(path), newName);
			if (File.Exists(path) && !newPath.EndsWith(".txt"))
				newPath += ".txt";

			if (!CheckValidNewPath(newPath))
				return;

			if (File.Exists(path))
				File.Move(path, newPath);
			if (Directory.Exists(path))
				Directory.Move(path, newPath);
		}

		private bool CheckValidNewPath(string newPath)
		{
			var invalidChar = Path.GetFileNameWithoutExtension(newPath).IndexOfAny(Path.GetInvalidFileNameChars());
			if (invalidChar >= 0 || File.Exists(newPath))
			{
				MessageBox.Show("Please enter a valid new file name.", "Invalid new file name.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

		private void HandleDeleteTableOrDirectory(string path)
		{
			DialogResult dialogResult = MessageBox.Show("Are you sure?", $"Are you sure you want to delete:\n\n{path}?", MessageBoxButtons.YesNo);
			if ((currentSelectedNode?.Tag as string) == path)
				currentSelectedNode = null;
			if (dialogResult == DialogResult.Yes)
			{
				if (File.Exists(path))
					new FileInfo(path).Delete();
				if (Directory.Exists(path))
					DeleteDirectory(path);
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

		private void HandleMakeNewFolder(string dir)
		{
			string newFolderName = FileDialog("Make a new Folder", "New Folder Name:", "");
			if (String.IsNullOrEmpty(newFolderName))
				return;

			string path = Path.Combine(dir, newFolderName);

			if (!CheckValidNewPath(path))
				return;

			Directory.CreateDirectory(path);
		}

		private void HandleMakeNewTable(string dir)
		{
			string newTableName = FileDialog("Make a new Table", "New Table Name:", "");

			if (String.IsNullOrEmpty(newTableName))
				return;

			if (!newTableName.EndsWith(".txt"))
				newTableName += ".txt";
			string path = Path.Combine(dir, newTableName);

			if (!CheckValidNewPath(path))
				return;

			var stream = File.Create(path);
			stream.Close();
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

		private void SaveOpenFile(object sender, EventArgs e)
		{
			if (currentSelectedNode == null)
			{
				MessageBox.Show("No Table Selected", "Not currently editing any name table.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			File.WriteAllText(GetCurrSelectedFile().FullName, selectedFileContents.Text);
		}

		private TreeNode currentSelectedNode = null;
		private FileInfo GetCurrSelectedFile()
		{
			return new FileInfo(currentSelectedNode?.Tag as string);
		}

		private void FileSelected(object sender, TreeViewEventArgs e)
		{
			CheckSaveOldFile();

			currentSelectedNode = null;
			selectedFileContents.Text = "";
			selectedFileContents.ReadOnly = true;
			if (!File.Exists(e.Node.Tag as string))
				return;
			currentSelectedNode = e.Node;
			var stream = GetCurrSelectedFile().OpenText();
			selectedFileContents.Text = stream.ReadToEnd();
			selectedFileContents.ReadOnly = false;
			stream.Close();
		}

		private void CheckSaveOldFile()
		{
			if (currentSelectedNode == null || !GetCurrSelectedFile().Exists)
				return;
			string currSelectedNodeName = currentSelectedNode.Text;
			
			string currFileText = File.ReadAllText(currentSelectedNode.Tag as string);
			currFileText = currFileText.Replace("\r", "");
			if (currFileText.Equals(selectedFileContents.Text))
				return;

			DialogResult dialogResult = MessageBox.Show($"Save {currSelectedNodeName}?", "Save Changes?", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
				SaveOpenFile(null, null);
		}

		private void CheckSave(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
				SaveOpenFile(null, null);
		}

		private void ValidateCurrentDirectory(object sender, EventArgs e)
		{
			NameTablesDirectoryValidator validator = new NameTablesDirectoryValidator();
			if (fileTreeView.Nodes.Count == 0)
				return;
			if(currentSelectedNode != null)
				SaveOpenFile(null, null);
			if (!validator.ValidateDirectory(fileTreeView.TopNode.Tag as string))
				MessageBox.Show(validator.errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else
				MessageBox.Show( "No problems detected with current tables :)", "All good!", MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		private void ItemDragged(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				DoDragDrop(e.Item, DragDropEffects.Move);
		}

		private void ItemDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void MoveFile(object sender, DragEventArgs e)
		{
			Point targetPoint = fileTreeView.PointToClient(new Point(e.X, e.Y));
			TreeNode targetNode = fileTreeView.GetNodeAt(targetPoint);
			if (targetNode == null || !Directory.Exists(targetNode.Tag as string))
				return;

			// Retrieve the node that was dragged.
			TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
			string oldPath = draggedNode.Tag as string;
			string newPath = Path.Combine(targetNode.Tag as string, Path.GetFileName(oldPath));
			if (File.Exists(oldPath))
				new FileInfo(oldPath).MoveTo(newPath);
			if (Directory.Exists(draggedNode.Tag as string))
				new DirectoryInfo(oldPath).MoveTo(newPath);
			moveWatcher.InvokeFileMove(newPath, oldPath);
		}
	}
}

public class ShittyMoveWatcher
{
	public event Action<string, string> FileMoved;
	public void InvokeFileMove(string newPath, string oldPath) => FileMoved?.Invoke(newPath, oldPath);
}

public class NodeSorter : IComparer
{
	public int Compare(object x, object y)
	{
		var xVal = (x as TreeNode)?.Tag as string;
		var yVal = (y as TreeNode)?.Tag as string;

		if (Directory.Exists(xVal) && !Directory.Exists(yVal))
			return -1;
		if (Directory.Exists(yVal) && !Directory.Exists(xVal))
			return 1;
		return String.Compare(Path.GetFileNameWithoutExtension(xVal), Path.GetFileNameWithoutExtension(yVal));
	}
}