namespace WindowsGame1
{
	partial class Debug_Form
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
			this.cmdRefreshAllCells = new System.Windows.Forms.Button();
			this.lstCells = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdRefreshCell = new System.Windows.Forms.Button();
			this.lstEntities = new System.Windows.Forms.ListView();
			this.headerID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uniqueNPCsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.abilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.effectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sceneryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.navmeshEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.cmdDeleteObject = new System.Windows.Forms.Button();
			this.cmdAddObject = new System.Windows.Forms.Button();
			this.cmdEditObject = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cmdMoveObject = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdRefreshAllCells
			// 
			this.cmdRefreshAllCells.Location = new System.Drawing.Point(12, 330);
			this.cmdRefreshAllCells.Name = "cmdRefreshAllCells";
			this.cmdRefreshAllCells.Size = new System.Drawing.Size(92, 23);
			this.cmdRefreshAllCells.TabIndex = 0;
			this.cmdRefreshAllCells.Text = "Refresh Cells";
			this.cmdRefreshAllCells.UseVisualStyleBackColor = true;
			this.cmdRefreshAllCells.Click += new System.EventHandler(this.button1_Click);
			// 
			// lstCells
			// 
			this.lstCells.FormattingEnabled = true;
			this.lstCells.Location = new System.Drawing.Point(12, 60);
			this.lstCells.Name = "lstCells";
			this.lstCells.Size = new System.Drawing.Size(149, 264);
			this.lstCells.TabIndex = 1;
			this.lstCells.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Cell Data:";
			// 
			// cmdRefreshCell
			// 
			this.cmdRefreshCell.Location = new System.Drawing.Point(12, 359);
			this.cmdRefreshCell.Name = "cmdRefreshCell";
			this.cmdRefreshCell.Size = new System.Drawing.Size(110, 23);
			this.cmdRefreshCell.TabIndex = 3;
			this.cmdRefreshCell.Text = "Refresh This Cell";
			this.cmdRefreshCell.UseVisualStyleBackColor = true;
			// 
			// lstEntities
			// 
			this.lstEntities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerID,
            this.headerName});
			this.lstEntities.Location = new System.Drawing.Point(183, 60);
			this.lstEntities.Name = "lstEntities";
			this.lstEntities.ShowItemToolTips = true;
			this.lstEntities.Size = new System.Drawing.Size(170, 264);
			this.lstEntities.TabIndex = 4;
			this.lstEntities.UseCompatibleStateImageBehavior = false;
			// 
			// headerID
			// 
			this.headerID.Text = "ID";
			// 
			// headerName
			// 
			this.headerName.Text = "Name";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem,
            this.navmeshEditorToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(771, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// windowsToolStripMenuItem
			// 
			this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uniqueNPCsToolStripMenuItem,
            this.abilitiesToolStripMenuItem,
            this.effectsToolStripMenuItem,
            this.sceneryToolStripMenuItem,
            this.lightsToolStripMenuItem});
			this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
			this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
			this.windowsToolStripMenuItem.Text = "Windows";
			// 
			// uniqueNPCsToolStripMenuItem
			// 
			this.uniqueNPCsToolStripMenuItem.Name = "uniqueNPCsToolStripMenuItem";
			this.uniqueNPCsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.uniqueNPCsToolStripMenuItem.Text = "Unique NPC\'s";
			this.uniqueNPCsToolStripMenuItem.Click += new System.EventHandler(this.uniqueNPCsToolStripMenuItem_Click);
			// 
			// abilitiesToolStripMenuItem
			// 
			this.abilitiesToolStripMenuItem.Name = "abilitiesToolStripMenuItem";
			this.abilitiesToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.abilitiesToolStripMenuItem.Text = "Abilities";
			this.abilitiesToolStripMenuItem.Click += new System.EventHandler(this.abilitiesToolStripMenuItem_Click);
			// 
			// effectsToolStripMenuItem
			// 
			this.effectsToolStripMenuItem.Name = "effectsToolStripMenuItem";
			this.effectsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.effectsToolStripMenuItem.Text = "Effects";
			this.effectsToolStripMenuItem.Click += new System.EventHandler(this.effectsToolStripMenuItem_Click);
			// 
			// sceneryToolStripMenuItem
			// 
			this.sceneryToolStripMenuItem.Name = "sceneryToolStripMenuItem";
			this.sceneryToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.sceneryToolStripMenuItem.Text = "Scenery";
			// 
			// lightsToolStripMenuItem
			// 
			this.lightsToolStripMenuItem.Name = "lightsToolStripMenuItem";
			this.lightsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.lightsToolStripMenuItem.Text = "Lights";
			this.lightsToolStripMenuItem.Click += new System.EventHandler(this.lightsToolStripMenuItem_Click);
			// 
			// navmeshEditorToolStripMenuItem
			// 
			this.navmeshEditorToolStripMenuItem.Name = "navmeshEditorToolStripMenuItem";
			this.navmeshEditorToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
			this.navmeshEditorToolStripMenuItem.Text = "Navmesh Editor";
			this.navmeshEditorToolStripMenuItem.Click += new System.EventHandler(this.navmeshEditorToolStripMenuItem_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(360, 60);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(114, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Go To This Object";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// cmdDeleteObject
			// 
			this.cmdDeleteObject.Location = new System.Drawing.Point(360, 159);
			this.cmdDeleteObject.Name = "cmdDeleteObject";
			this.cmdDeleteObject.Size = new System.Drawing.Size(114, 23);
			this.cmdDeleteObject.TabIndex = 7;
			this.cmdDeleteObject.Text = "Delete This Object";
			this.cmdDeleteObject.UseVisualStyleBackColor = true;
			this.cmdDeleteObject.Click += new System.EventHandler(this.cmdDeleteObject_Click);
			// 
			// cmdAddObject
			// 
			this.cmdAddObject.Location = new System.Drawing.Point(360, 189);
			this.cmdAddObject.Name = "cmdAddObject";
			this.cmdAddObject.Size = new System.Drawing.Size(114, 23);
			this.cmdAddObject.TabIndex = 8;
			this.cmdAddObject.Text = "Add New Object";
			this.cmdAddObject.UseVisualStyleBackColor = true;
			this.cmdAddObject.Click += new System.EventHandler(this.cmdAddObject_Click);
			// 
			// cmdEditObject
			// 
			this.cmdEditObject.Location = new System.Drawing.Point(360, 130);
			this.cmdEditObject.Name = "cmdEditObject";
			this.cmdEditObject.Size = new System.Drawing.Size(114, 23);
			this.cmdEditObject.TabIndex = 9;
			this.cmdEditObject.Text = "Edit This Object";
			this.cmdEditObject.UseVisualStyleBackColor = true;
			this.cmdEditObject.Click += new System.EventHandler(this.cmdEditObject_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(180, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Objects in this cell:";
			// 
			// cmdMoveObject
			// 
			this.cmdMoveObject.Location = new System.Drawing.Point(359, 89);
			this.cmdMoveObject.Name = "cmdMoveObject";
			this.cmdMoveObject.Size = new System.Drawing.Size(114, 23);
			this.cmdMoveObject.TabIndex = 11;
			this.cmdMoveObject.Text = "Move Object";
			this.cmdMoveObject.UseVisualStyleBackColor = true;
			this.cmdMoveObject.Click += new System.EventHandler(this.cmdMoveObject_Click);
			// 
			// Debug_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(771, 409);
			this.Controls.Add(this.cmdMoveObject);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmdEditObject);
			this.Controls.Add(this.cmdAddObject);
			this.Controls.Add(this.cmdDeleteObject);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lstEntities);
			this.Controls.Add(this.cmdRefreshCell);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstCells);
			this.Controls.Add(this.cmdRefreshAllCells);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Debug_Form";
			this.Text = "Debug_Form";
			this.Load += new System.EventHandler(this.Debug_Form_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdRefreshAllCells;
		private System.Windows.Forms.ListBox lstCells;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdRefreshCell;
		private System.Windows.Forms.ListView lstEntities;
		private System.Windows.Forms.ColumnHeader headerID;
		private System.Windows.Forms.ColumnHeader headerName;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uniqueNPCsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem abilitiesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem effectsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sceneryToolStripMenuItem;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button cmdDeleteObject;
		private System.Windows.Forms.Button cmdAddObject;
		private System.Windows.Forms.Button cmdEditObject;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cmdMoveObject;
		private System.Windows.Forms.ToolStripMenuItem navmeshEditorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lightsToolStripMenuItem;

	}
}