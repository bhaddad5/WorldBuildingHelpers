using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NameGeneratorFrontEnd
{
	public class TreeUiHandler
	{
		private ShittyMoveWatcher moveWatcher;
		private FileSystemWatcher fileSystem;
		private TreeView treeView;
		private Func<string, ContextMenu> dirMenuFactory;
		private Func<string, ContextMenu> fileMenuFactory;

		public void Setup(string path, TreeView tree, ShittyMoveWatcher moveWatcher, Func<string, ContextMenu> dirNodeFactory, Func<string, ContextMenu> fileNodeFactory)
		{
			treeView = tree;
			this.dirMenuFactory = dirNodeFactory;
			this.fileMenuFactory = fileNodeFactory;
			this.moveWatcher = moveWatcher;

			fileSystem = new FileSystemWatcher(path);
			fileSystem.EnableRaisingEvents = true;
			fileSystem.IncludeSubdirectories = true;
			fileSystem.Created += FileSystemOnCreated;
			fileSystem.Renamed += FileSystemOnRenamed;
			fileSystem.Deleted += FileSystemOnDeleted;
			moveWatcher.FileMoved += FileSystemOnMoved;

			DisplayDirectory(path, null);
			GetByTag(path).Expand();
		}

		private void FileSystemOnMoved(string newPath, string oldPath)
		{
			HandleMoveOrRename(newPath, oldPath);
		}

		private void DisplayDirectory(string dir, TreeNode parent)
		{
			TreeNode node = new TreeNode(Path.GetFileName(dir));
			node.Tag = dir;
			node.ContextMenu = dirMenuFactory(dir);
			if (parent != null)
				parent.Nodes.Add(node);
			else
				treeView.Nodes.Add(node);
			foreach (DirectoryInfo childDirectory in new DirectoryInfo(dir).GetDirectories())
				DisplayDirectory(childDirectory.FullName, node);
			foreach (FileInfo file in new DirectoryInfo(dir).GetFiles("*.txt"))
				DisplayFileNode(file.FullName, node);
		}

		private void DisplayFileNode(string file, TreeNode parent)
		{
			TreeNode node = new TreeNode(Path.GetFileNameWithoutExtension(file));
			node.Tag = file;
			node.ContextMenu = fileMenuFactory(file);
			if (parent != null)
				parent.Nodes.Add(node);
			else
				treeView.Nodes.Add(node);
		}

		private void FileSystemOnCreated(object sender, FileSystemEventArgs e)
		{
			treeView.Invoke(new Action(() =>
			{
				if (GetByTag(e.FullPath) != null)
					return;
				var parNode = GetByTag(Path.GetDirectoryName(e.FullPath));
				if (Directory.Exists(e.FullPath))
					DisplayDirectory(e.FullPath, parNode);
				if(File.Exists(e.FullPath) && e.FullPath.EndsWith(".txt"))
					DisplayFileNode(e.FullPath, parNode);

				treeView.Sort();
			}));
		}

		private void FileSystemOnRenamed(object sender, RenamedEventArgs e)
		{
			treeView.Invoke(new Action(() => HandleMoveOrRename(e.FullPath, e.OldFullPath)));
		}

		private void HandleMoveOrRename(string newPath, string oldPath)
		{
			var oldNode = GetByTag(oldPath);
			oldNode.Remove();
			var dirPath = Path.GetDirectoryName(newPath);
			var parentNode = GetByTag(dirPath);
			if(File.Exists(newPath))
				DisplayFileNode(newPath, parentNode);
			if(Directory.Exists(newPath))
				DisplayDirectory(newPath, parentNode);

			TableManagementHelpers.UpdateNameAcrossAllFiles(treeView.TopNode.Tag as string, oldPath, newPath);

			treeView.Sort();
		}

		private void FileSystemOnDeleted(object sender, FileSystemEventArgs e)
		{
			treeView.Invoke(new Action(() =>
			{
				var oldNode = GetByTag(e.FullPath);
				oldNode?.Remove();
			}));
		}

		private TreeNode GetByTag(object tag)
		{
			return GetByTagHelper(tag, treeView.Nodes[0]);
		}

		private TreeNode GetByTagHelper(object tag, TreeNode node)
		{
			if (tag.Equals(node.Tag as string))
				return node;
			foreach (TreeNode n in node.Nodes.OfType<TreeNode>())
			{
				var subNode = GetByTagHelper(tag, n);
				if (subNode != null)
					return subNode;
			}
			return null;
		}

		public void Shutdown()
		{
			treeView.Nodes.Clear();
			fileSystem.Created -= FileSystemOnCreated;
			fileSystem.Renamed -= FileSystemOnRenamed;
			fileSystem.Deleted -= FileSystemOnDeleted;
			moveWatcher.FileMoved -= FileSystemOnMoved;
		}
	}
}
