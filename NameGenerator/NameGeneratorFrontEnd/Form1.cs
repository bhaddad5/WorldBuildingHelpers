using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NameGenerator;

namespace NameGeneratorFrontEnd
{
	public partial class NameGen : Form
	{
		public NameGen()
		{
			InitializeComponent();
		}

		private void SelectFolder_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog openFolderDialog = new FolderBrowserDialog())
			{
				openFolderDialog.ShowNewFolderButton = true;

				DialogResult result = openFolderDialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFolderDialog.SelectedPath))
				{
					SelectRootFolder(openFolderDialog.SelectedPath);

					string[] files = Directory.GetFiles(openFolderDialog.SelectedPath);

					System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
				}
			}
		}

		//private string currentSelectedFilePath = null;

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

		private void DisplayDirectory(DirectoryInfo dir, TreeNode parent)
		{
			TreeNode node = new TreeNode(dir.Name);
			node.Tag = dir;
			if (parent != null)
				parent.Nodes.Add(node);
			else
				treeView1.Nodes.Add(node);
			foreach (DirectoryInfo childDirectory in dir.GetDirectories())
				DisplayDirectory(childDirectory, node);
			foreach (FileInfo file in dir.GetFiles("*.txt"))
				DisplayFileNode(file, node);
		}

		private void DisplayFileNode(FileInfo file, TreeNode parent)
		{
			TreeNode node = new TreeNode(file.Name);
			node.Tag = file;
			if (parent != null)
				parent.Nodes.Add(node);
			else
				treeView1.Nodes.Add(node);
		}
	}
}
