namespace BeefThemeEditor
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenTheme = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenToml = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenPNG = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.Save = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.colorEditor1 = new Cyotek.Windows.Forms.ColorEditor();
			this.errMsg = new System.Windows.Forms.Label();
			this.menuStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Theme";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1105, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.Save,
            this.SaveAs,
            this.toolStripSeparator1,
            this.toolStripSeparator2,
            this.Exit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenTheme,
            this.OpenToml,
            this.OpenPNG});
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openToolStripMenuItem.Text = "&Open";
			// 
			// OpenTheme
			// 
			this.OpenTheme.Name = "OpenTheme";
			this.OpenTheme.Size = new System.Drawing.Size(110, 22);
			this.OpenTheme.Text = "Theme";
			// 
			// OpenToml
			// 
			this.OpenToml.Name = "OpenToml";
			this.OpenToml.Size = new System.Drawing.Size(110, 22);
			this.OpenToml.Text = "TOML";
			// 
			// OpenPNG
			// 
			this.OpenPNG.Name = "OpenPNG";
			this.OpenPNG.Size = new System.Drawing.Size(110, 22);
			this.OpenPNG.Text = "PNG";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(177, 6);
			// 
			// Save
			// 
			this.Save.Image = ((System.Drawing.Image)(resources.GetObject("Save.Image")));
			this.Save.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Save.Name = "Save";
			this.Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.Save.Size = new System.Drawing.Size(180, 22);
			this.Save.Text = "&Save";
			// 
			// SaveAs
			// 
			this.SaveAs.Name = "SaveAs";
			this.SaveAs.Size = new System.Drawing.Size(180, 22);
			this.SaveAs.Text = "Save &As";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
			// 
			// Exit
			// 
			this.Exit.Name = "Exit";
			this.Exit.Size = new System.Drawing.Size(180, 22);
			this.Exit.Text = "E&xit";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Location = new System.Drawing.Point(0, 73);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1105, 612);
			this.tabControl1.TabIndex = 3;
			this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1097, 586);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Template";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// colorEditor1
			// 
			this.colorEditor1.Location = new System.Drawing.Point(806, 156);
			this.colorEditor1.Name = "colorEditor1";
			this.colorEditor1.Size = new System.Drawing.Size(242, 260);
			this.colorEditor1.TabIndex = 5;
			this.colorEditor1.Visible = false;
			this.colorEditor1.ColorChanged += new System.EventHandler(this.colorEditor1_ColorChanged);
			// 
			// errMsg
			// 
			this.errMsg.AutoSize = true;
			this.errMsg.ForeColor = System.Drawing.Color.Red;
			this.errMsg.Location = new System.Drawing.Point(7, 57);
			this.errMsg.Name = "errMsg";
			this.errMsg.Size = new System.Drawing.Size(0, 13);
			this.errMsg.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1105, 685);
			this.Controls.Add(this.errMsg);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Theme Color Editor for Beef IDE";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem Save;
		private System.Windows.Forms.ToolStripMenuItem SaveAs;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem Exit;
		private System.Windows.Forms.ToolStripMenuItem OpenTheme;
		private System.Windows.Forms.ToolStripMenuItem OpenToml;
		private System.Windows.Forms.ToolStripMenuItem OpenPNG;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label errMsg;
		private Cyotek.Windows.Forms.ColorEditor colorEditor1;
	}
}

