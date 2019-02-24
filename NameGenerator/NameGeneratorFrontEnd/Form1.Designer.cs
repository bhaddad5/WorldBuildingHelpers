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
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.button4 = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 0);
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
			this.selectedFileContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.selectedFileContents.BackColor = System.Drawing.SystemColors.MenuText;
			this.selectedFileContents.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.selectedFileContents.ForeColor = System.Drawing.SystemColors.Menu;
			this.selectedFileContents.Location = new System.Drawing.Point(398, 5);
			this.selectedFileContents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.selectedFileContents.Name = "selectedFileContents";
			this.selectedFileContents.ReadOnly = true;
			this.selectedFileContents.Size = new System.Drawing.Size(386, 662);
			this.selectedFileContents.TabIndex = 2;
			this.selectedFileContents.Text = "";
			// 
			// richTextBox2
			// 
			this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox2.BackColor = System.Drawing.SystemColors.MenuText;
			this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.richTextBox2.ForeColor = System.Drawing.SystemColors.Menu;
			this.richTextBox2.Location = new System.Drawing.Point(792, 5);
			this.richTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ReadOnly = true;
			this.richTextBox2.Size = new System.Drawing.Size(387, 662);
			this.richTextBox2.TabIndex = 4;
			this.richTextBox2.Text = "";
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.BackColor = System.Drawing.SystemColors.MenuText;
			this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.treeView1.ForeColor = System.Drawing.SystemColors.Menu;
			this.treeView1.ItemHeight = 20;
			this.treeView1.Location = new System.Drawing.Point(4, 5);
			this.treeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(386, 662);
			this.treeView1.TabIndex = 5;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(320, 0);
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
			this.button3.Location = new System.Drawing.Point(156, 0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(157, 35);
			this.button3.TabIndex = 7;
			this.button3.Text = "Save";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.selectedFileContents, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.richTextBox2, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 53);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 672F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1183, 672);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(514, 4);
			this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(150, 26);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "10";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.panel1.Controls.Add(this.button4);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.button3);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1183, 44);
			this.panel1.TabIndex = 9;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1189, 728);
			this.tableLayoutPanel2.TabIndex = 10;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(690, 0);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(177, 35);
			this.button4.TabIndex = 8;
			this.button4.Text = "Validate All Tables";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// NameGen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.ClientSize = new System.Drawing.Size(1189, 728);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Name = "NameGen";
			this.Text = "Name Generator";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RichTextBox selectedFileContents;
		private System.Windows.Forms.RichTextBox richTextBox2;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button button4;
	}
}

