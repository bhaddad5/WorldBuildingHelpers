namespace NameGeneratorFrontEnd
{
	partial class NameGen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.selectedFileContents = new System.Windows.Forms.RichTextBox();
			this.richTextBox2 = new System.Windows.Forms.RichTextBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(2, 0);
			this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(136, 35);
			this.button1.TabIndex = 0;
			this.button1.Text = "Select Folder";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.SelectFolder_Click);
			// 
			// selectedFileContents
			// 
			this.selectedFileContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.selectedFileContents.Location = new System.Drawing.Point(587, 46);
			this.selectedFileContents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.selectedFileContents.Name = "selectedFileContents";
			this.selectedFileContents.Size = new System.Drawing.Size(423, 581);
			this.selectedFileContents.TabIndex = 2;
			this.selectedFileContents.Text = "";
			// 
			// richTextBox2
			// 
			this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.richTextBox2.Location = new System.Drawing.Point(1022, 46);
			this.richTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.Size = new System.Drawing.Size(344, 581);
			this.richTextBox2.TabIndex = 4;
			this.richTextBox2.Text = "";
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.treeView1.Location = new System.Drawing.Point(2, 46);
			this.treeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(577, 581);
			this.treeView1.TabIndex = 5;
			this.treeView1.BeforeSelect += TreeViewAboutToMakeNewSelection;
			this.treeView1.AfterSelect += FileSelected;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(1218, 4);
			this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(148, 26);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "10";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(1022, 0);
			this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(186, 35);
			this.button2.TabIndex = 3;
			this.button2.Text = "Generate Names";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.GenerateNamesList_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(587, 0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(157, 35);
			this.button3.TabIndex = 7;
			this.button3.Text = "Save";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += SaveOpenFile;
			// 
			// NameGen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1386, 635);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.richTextBox2);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.selectedFileContents);
			this.Controls.Add(this.button1);
			this.Name = "NameGen";
			this.Text = "Name Generator";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox selectedFileContents;
		private System.Windows.Forms.RichTextBox richTextBox2;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
	}
}

