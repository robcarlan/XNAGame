namespace WindowsGame1.Debug
{
	partial class navmeshForm
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
			this.lstPolys = new System.Windows.Forms.ListView();
			this.columnID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmdGotoPoly = new System.Windows.Forms.Button();
			this.cmdCreateNew = new System.Windows.Forms.Button();
			this.cmdRemovePoly = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdSelectVert4 = new System.Windows.Forms.Button();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.txtOffsetY4 = new System.Windows.Forms.TextBox();
			this.txtOffsetX4 = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.txtTileY4 = new System.Windows.Forms.TextBox();
			this.txtTileX4 = new System.Windows.Forms.TextBox();
			this.label37 = new System.Windows.Forms.Label();
			this.label38 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.txtCellY4 = new System.Windows.Forms.TextBox();
			this.txtCellX4 = new System.Windows.Forms.TextBox();
			this.label40 = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.cmdSelectVert3 = new System.Windows.Forms.Button();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.txtOffsetY3 = new System.Windows.Forms.TextBox();
			this.txtOffsetX3 = new System.Windows.Forms.TextBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.txtTileY3 = new System.Windows.Forms.TextBox();
			this.txtTileX3 = new System.Windows.Forms.TextBox();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.txtCellY3 = new System.Windows.Forms.TextBox();
			this.txtCellX3 = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.cmdSelectVert2 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtOffsetY2 = new System.Windows.Forms.TextBox();
			this.txtOffsetX2 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.txtTileY2 = new System.Windows.Forms.TextBox();
			this.txtTileX2 = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.txtCellY2 = new System.Windows.Forms.TextBox();
			this.txtCellX2 = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.cmdSelectVert1 = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.txtOffsetY1 = new System.Windows.Forms.TextBox();
			this.txtOffsetX1 = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.txtTileY1 = new System.Windows.Forms.TextBox();
			this.txtTileX1 = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtCellY1 = new System.Windows.Forms.TextBox();
			this.txtCellX1 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lstConnectedPolys = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmdSave = new System.Windows.Forms.Button();
			this.cmdRefresh = new System.Windows.Forms.Button();
			this.cmdAddConnected = new System.Windows.Forms.Button();
			this.cmdRemoveConnected = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstPolys
			// 
			this.lstPolys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnID,
            this.columnName,
            this.columnType});
			this.lstPolys.Location = new System.Drawing.Point(12, 12);
			this.lstPolys.Name = "lstPolys";
			this.lstPolys.Size = new System.Drawing.Size(293, 291);
			this.lstPolys.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstPolys.TabIndex = 2;
			this.lstPolys.UseCompatibleStateImageBehavior = false;
			this.lstPolys.View = System.Windows.Forms.View.Details;
			this.lstPolys.SelectedIndexChanged += new System.EventHandler(this.lstPolys_SelectedIndexChanged);
			// 
			// columnID
			// 
			this.columnID.Text = "ID";
			// 
			// columnName
			// 
			this.columnName.Text = "Name";
			this.columnName.Width = 113;
			// 
			// columnType
			// 
			this.columnType.Text = "Type";
			this.columnType.Width = 115;
			// 
			// cmdGotoPoly
			// 
			this.cmdGotoPoly.Location = new System.Drawing.Point(12, 309);
			this.cmdGotoPoly.Name = "cmdGotoPoly";
			this.cmdGotoPoly.Size = new System.Drawing.Size(130, 23);
			this.cmdGotoPoly.TabIndex = 3;
			this.cmdGotoPoly.Text = "Go To This Object";
			this.cmdGotoPoly.UseVisualStyleBackColor = true;
			this.cmdGotoPoly.Click += new System.EventHandler(this.cmdGotoPoly_Click);
			// 
			// cmdCreateNew
			// 
			this.cmdCreateNew.Location = new System.Drawing.Point(12, 338);
			this.cmdCreateNew.Name = "cmdCreateNew";
			this.cmdCreateNew.Size = new System.Drawing.Size(130, 23);
			this.cmdCreateNew.TabIndex = 4;
			this.cmdCreateNew.Text = "Add New";
			this.cmdCreateNew.UseVisualStyleBackColor = true;
			this.cmdCreateNew.Click += new System.EventHandler(this.cmdCreateNew_Click);
			// 
			// cmdRemovePoly
			// 
			this.cmdRemovePoly.Location = new System.Drawing.Point(12, 367);
			this.cmdRemovePoly.Name = "cmdRemovePoly";
			this.cmdRemovePoly.Size = new System.Drawing.Size(130, 23);
			this.cmdRemovePoly.TabIndex = 5;
			this.cmdRemovePoly.Text = "Remove";
			this.cmdRemovePoly.UseVisualStyleBackColor = true;
			this.cmdRemovePoly.Click += new System.EventHandler(this.cmdRemovePoly_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmdSelectVert4);
			this.groupBox1.Controls.Add(this.label32);
			this.groupBox1.Controls.Add(this.label33);
			this.groupBox1.Controls.Add(this.txtOffsetY4);
			this.groupBox1.Controls.Add(this.txtOffsetX4);
			this.groupBox1.Controls.Add(this.label34);
			this.groupBox1.Controls.Add(this.label35);
			this.groupBox1.Controls.Add(this.label36);
			this.groupBox1.Controls.Add(this.txtTileY4);
			this.groupBox1.Controls.Add(this.txtTileX4);
			this.groupBox1.Controls.Add(this.label37);
			this.groupBox1.Controls.Add(this.label38);
			this.groupBox1.Controls.Add(this.label39);
			this.groupBox1.Controls.Add(this.txtCellY4);
			this.groupBox1.Controls.Add(this.txtCellX4);
			this.groupBox1.Controls.Add(this.label40);
			this.groupBox1.Controls.Add(this.label41);
			this.groupBox1.Controls.Add(this.cmdSelectVert3);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.label23);
			this.groupBox1.Controls.Add(this.txtOffsetY3);
			this.groupBox1.Controls.Add(this.txtOffsetX3);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.label25);
			this.groupBox1.Controls.Add(this.label26);
			this.groupBox1.Controls.Add(this.txtTileY3);
			this.groupBox1.Controls.Add(this.txtTileX3);
			this.groupBox1.Controls.Add(this.label27);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Controls.Add(this.label29);
			this.groupBox1.Controls.Add(this.txtCellY3);
			this.groupBox1.Controls.Add(this.txtCellX3);
			this.groupBox1.Controls.Add(this.label30);
			this.groupBox1.Controls.Add(this.label31);
			this.groupBox1.Controls.Add(this.cmdSelectVert2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtOffsetY2);
			this.groupBox1.Controls.Add(this.txtOffsetX2);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.txtTileY2);
			this.groupBox1.Controls.Add(this.txtTileX2);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.txtCellY2);
			this.groupBox1.Controls.Add(this.txtCellX2);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.cmdSelectVert1);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.txtOffsetY1);
			this.groupBox1.Controls.Add(this.txtOffsetX1);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.txtTileY1);
			this.groupBox1.Controls.Add(this.txtTileX1);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.txtCellY1);
			this.groupBox1.Controls.Add(this.txtCellX1);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(312, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(376, 244);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Poly Data";
			// 
			// cmdSelectVert4
			// 
			this.cmdSelectVert4.Location = new System.Drawing.Point(78, 176);
			this.cmdSelectVert4.Name = "cmdSelectVert4";
			this.cmdSelectVert4.Size = new System.Drawing.Size(98, 23);
			this.cmdSelectVert4.TabIndex = 71;
			this.cmdSelectVert4.Text = "Select Position";
			this.cmdSelectVert4.UseVisualStyleBackColor = true;
			this.cmdSelectVert4.Click += new System.EventHandler(this.cmdSelectVert4_Click);
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Location = new System.Drawing.Point(320, 207);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(17, 13);
			this.label32.TabIndex = 70;
			this.label32.Text = "Y:";
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(277, 207);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(17, 13);
			this.label33.TabIndex = 69;
			this.label33.Text = "X:";
			// 
			// txtOffsetY4
			// 
			this.txtOffsetY4.Location = new System.Drawing.Point(335, 204);
			this.txtOffsetY4.Name = "txtOffsetY4";
			this.txtOffsetY4.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetY4.TabIndex = 68;
			this.txtOffsetY4.TextChanged += new System.EventHandler(this.txtOffsetY4_TextChanged);
			// 
			// txtOffsetX4
			// 
			this.txtOffsetX4.Location = new System.Drawing.Point(293, 204);
			this.txtOffsetX4.Name = "txtOffsetX4";
			this.txtOffsetX4.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetX4.TabIndex = 67;
			this.txtOffsetX4.TextChanged += new System.EventHandler(this.txtOffsetX4_TextChanged);
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Location = new System.Drawing.Point(244, 206);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(38, 13);
			this.label34.TabIndex = 66;
			this.label34.Text = "Offset:";
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Location = new System.Drawing.Point(197, 206);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(17, 13);
			this.label35.TabIndex = 65;
			this.label35.Text = "Y:";
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Location = new System.Drawing.Point(154, 206);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(17, 13);
			this.label36.TabIndex = 64;
			this.label36.Text = "X:";
			// 
			// txtTileY4
			// 
			this.txtTileY4.Location = new System.Drawing.Point(212, 203);
			this.txtTileY4.Name = "txtTileY4";
			this.txtTileY4.Size = new System.Drawing.Size(26, 20);
			this.txtTileY4.TabIndex = 63;
			this.txtTileY4.TextChanged += new System.EventHandler(this.txtTileY4_TextChanged);
			// 
			// txtTileX4
			// 
			this.txtTileX4.Location = new System.Drawing.Point(170, 203);
			this.txtTileX4.Name = "txtTileX4";
			this.txtTileX4.Size = new System.Drawing.Size(26, 20);
			this.txtTileX4.TabIndex = 62;
			this.txtTileX4.TextChanged += new System.EventHandler(this.txtTileX4_TextChanged);
			// 
			// label37
			// 
			this.label37.AutoSize = true;
			this.label37.Location = new System.Drawing.Point(130, 206);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(27, 13);
			this.label37.TabIndex = 61;
			this.label37.Text = "Tile:";
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.Location = new System.Drawing.Point(83, 206);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(17, 13);
			this.label38.TabIndex = 60;
			this.label38.Text = "Y:";
			// 
			// label39
			// 
			this.label39.AutoSize = true;
			this.label39.Location = new System.Drawing.Point(40, 206);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(17, 13);
			this.label39.TabIndex = 59;
			this.label39.Text = "X:";
			// 
			// txtCellY4
			// 
			this.txtCellY4.Location = new System.Drawing.Point(98, 203);
			this.txtCellY4.Name = "txtCellY4";
			this.txtCellY4.Size = new System.Drawing.Size(26, 20);
			this.txtCellY4.TabIndex = 58;
			this.txtCellY4.TextChanged += new System.EventHandler(this.txtCellY4_TextChanged);
			// 
			// txtCellX4
			// 
			this.txtCellX4.Location = new System.Drawing.Point(56, 203);
			this.txtCellX4.Name = "txtCellX4";
			this.txtCellX4.Size = new System.Drawing.Size(26, 20);
			this.txtCellX4.TabIndex = 57;
			this.txtCellX4.TextChanged += new System.EventHandler(this.txtCellX4_TextChanged);
			// 
			// label40
			// 
			this.label40.AutoSize = true;
			this.label40.Location = new System.Drawing.Point(16, 206);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(27, 13);
			this.label40.TabIndex = 56;
			this.label40.Text = "Cell:";
			// 
			// label41
			// 
			this.label41.AutoSize = true;
			this.label41.Location = new System.Drawing.Point(6, 180);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(67, 13);
			this.label41.TabIndex = 55;
			this.label41.Text = "Vertice Four:";
			// 
			// cmdSelectVert3
			// 
			this.cmdSelectVert3.Location = new System.Drawing.Point(78, 123);
			this.cmdSelectVert3.Name = "cmdSelectVert3";
			this.cmdSelectVert3.Size = new System.Drawing.Size(98, 23);
			this.cmdSelectVert3.TabIndex = 54;
			this.cmdSelectVert3.Text = "Select Position";
			this.cmdSelectVert3.UseVisualStyleBackColor = true;
			this.cmdSelectVert3.Click += new System.EventHandler(this.cmdSelectVert3_Click);
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(320, 154);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(17, 13);
			this.label22.TabIndex = 53;
			this.label22.Text = "Y:";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(277, 154);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(17, 13);
			this.label23.TabIndex = 52;
			this.label23.Text = "X:";
			// 
			// txtOffsetY3
			// 
			this.txtOffsetY3.Location = new System.Drawing.Point(335, 151);
			this.txtOffsetY3.Name = "txtOffsetY3";
			this.txtOffsetY3.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetY3.TabIndex = 51;
			this.txtOffsetY3.TextChanged += new System.EventHandler(this.txtOffsetY3_TextChanged);
			// 
			// txtOffsetX3
			// 
			this.txtOffsetX3.Location = new System.Drawing.Point(293, 151);
			this.txtOffsetX3.Name = "txtOffsetX3";
			this.txtOffsetX3.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetX3.TabIndex = 50;
			this.txtOffsetX3.TextChanged += new System.EventHandler(this.txtOffsetX3_TextChanged);
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(244, 153);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(38, 13);
			this.label24.TabIndex = 49;
			this.label24.Text = "Offset:";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(197, 153);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(17, 13);
			this.label25.TabIndex = 48;
			this.label25.Text = "Y:";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(154, 153);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(17, 13);
			this.label26.TabIndex = 47;
			this.label26.Text = "X:";
			// 
			// txtTileY3
			// 
			this.txtTileY3.Location = new System.Drawing.Point(212, 150);
			this.txtTileY3.Name = "txtTileY3";
			this.txtTileY3.Size = new System.Drawing.Size(26, 20);
			this.txtTileY3.TabIndex = 46;
			this.txtTileY3.TextChanged += new System.EventHandler(this.txtTileY3_TextChanged);
			// 
			// txtTileX3
			// 
			this.txtTileX3.Location = new System.Drawing.Point(170, 150);
			this.txtTileX3.Name = "txtTileX3";
			this.txtTileX3.Size = new System.Drawing.Size(26, 20);
			this.txtTileX3.TabIndex = 45;
			this.txtTileX3.TextChanged += new System.EventHandler(this.txtTileX3_TextChanged);
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(130, 153);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(27, 13);
			this.label27.TabIndex = 44;
			this.label27.Text = "Tile:";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(83, 153);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(17, 13);
			this.label28.TabIndex = 43;
			this.label28.Text = "Y:";
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(40, 153);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(17, 13);
			this.label29.TabIndex = 42;
			this.label29.Text = "X:";
			// 
			// txtCellY3
			// 
			this.txtCellY3.Location = new System.Drawing.Point(98, 150);
			this.txtCellY3.Name = "txtCellY3";
			this.txtCellY3.Size = new System.Drawing.Size(26, 20);
			this.txtCellY3.TabIndex = 41;
			this.txtCellY3.TextChanged += new System.EventHandler(this.txtCellY3_TextChanged);
			// 
			// txtCellX3
			// 
			this.txtCellX3.Location = new System.Drawing.Point(56, 150);
			this.txtCellX3.Name = "txtCellX3";
			this.txtCellX3.Size = new System.Drawing.Size(26, 20);
			this.txtCellX3.TabIndex = 40;
			this.txtCellX3.TextChanged += new System.EventHandler(this.txtCellX3_TextChanged);
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(16, 153);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(27, 13);
			this.label30.TabIndex = 39;
			this.label30.Text = "Cell:";
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Location = new System.Drawing.Point(6, 127);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(74, 13);
			this.label31.TabIndex = 38;
			this.label31.Text = "Vertice Three:";
			// 
			// cmdSelectVert2
			// 
			this.cmdSelectVert2.Location = new System.Drawing.Point(78, 70);
			this.cmdSelectVert2.Name = "cmdSelectVert2";
			this.cmdSelectVert2.Size = new System.Drawing.Size(98, 23);
			this.cmdSelectVert2.TabIndex = 37;
			this.cmdSelectVert2.Text = "Select Position";
			this.cmdSelectVert2.UseVisualStyleBackColor = true;
			this.cmdSelectVert2.Click += new System.EventHandler(this.cmdSelectVert2_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(320, 101);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(17, 13);
			this.label2.TabIndex = 36;
			this.label2.Text = "Y:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(277, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(17, 13);
			this.label3.TabIndex = 35;
			this.label3.Text = "X:";
			// 
			// txtOffsetY2
			// 
			this.txtOffsetY2.Location = new System.Drawing.Point(335, 98);
			this.txtOffsetY2.Name = "txtOffsetY2";
			this.txtOffsetY2.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetY2.TabIndex = 34;
			this.txtOffsetY2.TextChanged += new System.EventHandler(this.txtOffsetY2_TextChanged);
			// 
			// txtOffsetX2
			// 
			this.txtOffsetX2.Location = new System.Drawing.Point(293, 98);
			this.txtOffsetX2.Name = "txtOffsetX2";
			this.txtOffsetX2.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetX2.TabIndex = 33;
			this.txtOffsetX2.TextChanged += new System.EventHandler(this.txtOffsetX2_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(244, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 13);
			this.label4.TabIndex = 32;
			this.label4.Text = "Offset:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(197, 100);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(17, 13);
			this.label7.TabIndex = 31;
			this.label7.Text = "Y:";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(154, 100);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(17, 13);
			this.label16.TabIndex = 30;
			this.label16.Text = "X:";
			// 
			// txtTileY2
			// 
			this.txtTileY2.Location = new System.Drawing.Point(212, 97);
			this.txtTileY2.Name = "txtTileY2";
			this.txtTileY2.Size = new System.Drawing.Size(26, 20);
			this.txtTileY2.TabIndex = 29;
			this.txtTileY2.TextChanged += new System.EventHandler(this.txtTileY2_TextChanged);
			// 
			// txtTileX2
			// 
			this.txtTileX2.Location = new System.Drawing.Point(170, 97);
			this.txtTileX2.Name = "txtTileX2";
			this.txtTileX2.Size = new System.Drawing.Size(26, 20);
			this.txtTileX2.TabIndex = 28;
			this.txtTileX2.TextChanged += new System.EventHandler(this.txtTileX2_TextChanged);
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(130, 100);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(27, 13);
			this.label17.TabIndex = 27;
			this.label17.Text = "Tile:";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(83, 100);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(17, 13);
			this.label18.TabIndex = 26;
			this.label18.Text = "Y:";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(40, 100);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(17, 13);
			this.label19.TabIndex = 25;
			this.label19.Text = "X:";
			// 
			// txtCellY2
			// 
			this.txtCellY2.Location = new System.Drawing.Point(98, 97);
			this.txtCellY2.Name = "txtCellY2";
			this.txtCellY2.Size = new System.Drawing.Size(26, 20);
			this.txtCellY2.TabIndex = 24;
			this.txtCellY2.TextChanged += new System.EventHandler(this.txtCellY2_TextChanged);
			// 
			// txtCellX2
			// 
			this.txtCellX2.Location = new System.Drawing.Point(56, 97);
			this.txtCellX2.Name = "txtCellX2";
			this.txtCellX2.Size = new System.Drawing.Size(26, 20);
			this.txtCellX2.TabIndex = 23;
			this.txtCellX2.TextChanged += new System.EventHandler(this.txtCellX2_TextChanged);
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(16, 100);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(27, 13);
			this.label20.TabIndex = 22;
			this.label20.Text = "Cell:";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(6, 74);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(67, 13);
			this.label21.TabIndex = 21;
			this.label21.Text = "Vertice Two:";
			// 
			// cmdSelectVert1
			// 
			this.cmdSelectVert1.Location = new System.Drawing.Point(78, 17);
			this.cmdSelectVert1.Name = "cmdSelectVert1";
			this.cmdSelectVert1.Size = new System.Drawing.Size(98, 23);
			this.cmdSelectVert1.TabIndex = 20;
			this.cmdSelectVert1.Text = "Select Position";
			this.cmdSelectVert1.UseVisualStyleBackColor = true;
			this.cmdSelectVert1.Click += new System.EventHandler(this.cmdSelectVert1_Click);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(320, 48);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(17, 13);
			this.label13.TabIndex = 19;
			this.label13.Text = "Y:";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(277, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(17, 13);
			this.label14.TabIndex = 18;
			this.label14.Text = "X:";
			// 
			// txtOffsetY1
			// 
			this.txtOffsetY1.Location = new System.Drawing.Point(335, 45);
			this.txtOffsetY1.Name = "txtOffsetY1";
			this.txtOffsetY1.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetY1.TabIndex = 17;
			this.txtOffsetY1.TextChanged += new System.EventHandler(this.txtOffsetY1_TextChanged);
			// 
			// txtOffsetX1
			// 
			this.txtOffsetX1.Location = new System.Drawing.Point(293, 45);
			this.txtOffsetX1.Name = "txtOffsetX1";
			this.txtOffsetX1.Size = new System.Drawing.Size(26, 20);
			this.txtOffsetX1.TabIndex = 16;
			this.txtOffsetX1.TextChanged += new System.EventHandler(this.txtOffsetX1_TextChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(244, 47);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(38, 13);
			this.label15.TabIndex = 15;
			this.label15.Text = "Offset:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(197, 47);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(17, 13);
			this.label10.TabIndex = 14;
			this.label10.Text = "Y:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(154, 47);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(17, 13);
			this.label11.TabIndex = 13;
			this.label11.Text = "X:";
			// 
			// txtTileY1
			// 
			this.txtTileY1.Location = new System.Drawing.Point(212, 44);
			this.txtTileY1.Name = "txtTileY1";
			this.txtTileY1.Size = new System.Drawing.Size(26, 20);
			this.txtTileY1.TabIndex = 12;
			this.txtTileY1.TextChanged += new System.EventHandler(this.txtTileY1_TextChanged);
			// 
			// txtTileX1
			// 
			this.txtTileX1.Location = new System.Drawing.Point(170, 44);
			this.txtTileX1.Name = "txtTileX1";
			this.txtTileX1.Size = new System.Drawing.Size(26, 20);
			this.txtTileX1.TabIndex = 11;
			this.txtTileX1.TextChanged += new System.EventHandler(this.txtTileX1_TextChanged);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(130, 47);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(27, 13);
			this.label12.TabIndex = 10;
			this.label12.Text = "Tile:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(83, 47);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(17, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Y:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(40, 47);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(17, 13);
			this.label8.TabIndex = 8;
			this.label8.Text = "X:";
			// 
			// txtCellY1
			// 
			this.txtCellY1.Location = new System.Drawing.Point(98, 44);
			this.txtCellY1.Name = "txtCellY1";
			this.txtCellY1.Size = new System.Drawing.Size(26, 20);
			this.txtCellY1.TabIndex = 7;
			this.txtCellY1.TextChanged += new System.EventHandler(this.txtCellY1_TextChanged);
			// 
			// txtCellX1
			// 
			this.txtCellX1.Location = new System.Drawing.Point(56, 44);
			this.txtCellX1.Name = "txtCellX1";
			this.txtCellX1.Size = new System.Drawing.Size(26, 20);
			this.txtCellX1.TabIndex = 6;
			this.txtCellX1.TextChanged += new System.EventHandler(this.txtCellX1_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 47);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(27, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Cell:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(66, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Vertice One:";
			// 
			// lstConnectedPolys
			// 
			this.lstConnectedPolys.FormattingEnabled = true;
			this.lstConnectedPolys.Location = new System.Drawing.Point(168, 328);
			this.lstConnectedPolys.Name = "lstConnectedPolys";
			this.lstConnectedPolys.Size = new System.Drawing.Size(137, 108);
			this.lstConnectedPolys.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(168, 309);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(90, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Connected Polys:";
			// 
			// cmdSave
			// 
			this.cmdSave.Location = new System.Drawing.Point(12, 396);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(130, 23);
			this.cmdSave.TabIndex = 9;
			this.cmdSave.Text = "Save";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cmdRefresh
			// 
			this.cmdRefresh.Location = new System.Drawing.Point(12, 448);
			this.cmdRefresh.Name = "cmdRefresh";
			this.cmdRefresh.Size = new System.Drawing.Size(130, 23);
			this.cmdRefresh.TabIndex = 10;
			this.cmdRefresh.Text = "Refresh";
			this.cmdRefresh.UseVisualStyleBackColor = true;
			this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
			// 
			// cmdAddConnected
			// 
			this.cmdAddConnected.Location = new System.Drawing.Point(321, 328);
			this.cmdAddConnected.Name = "cmdAddConnected";
			this.cmdAddConnected.Size = new System.Drawing.Size(115, 23);
			this.cmdAddConnected.TabIndex = 11;
			this.cmdAddConnected.Text = "Add Connected Poly";
			this.cmdAddConnected.UseVisualStyleBackColor = true;
			this.cmdAddConnected.Click += new System.EventHandler(this.cmdAddConnected_Click);
			// 
			// cmdRemoveConnected
			// 
			this.cmdRemoveConnected.Location = new System.Drawing.Point(321, 391);
			this.cmdRemoveConnected.Name = "cmdRemoveConnected";
			this.cmdRemoveConnected.Size = new System.Drawing.Size(115, 45);
			this.cmdRemoveConnected.TabIndex = 12;
			this.cmdRemoveConnected.Text = "Remove Connected Poly";
			this.cmdRemoveConnected.UseVisualStyleBackColor = true;
			this.cmdRemoveConnected.Click += new System.EventHandler(this.cmdRemoveConnected_Click);
			// 
			// navmeshForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(836, 483);
			this.Controls.Add(this.cmdRemoveConnected);
			this.Controls.Add(this.cmdAddConnected);
			this.Controls.Add(this.cmdRefresh);
			this.Controls.Add(this.cmdSave);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lstConnectedPolys);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmdRemovePoly);
			this.Controls.Add(this.cmdCreateNew);
			this.Controls.Add(this.cmdGotoPoly);
			this.Controls.Add(this.lstPolys);
			this.Name = "navmeshForm";
			this.Text = "Navmesh_Form";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView lstPolys;
		private System.Windows.Forms.ColumnHeader columnID;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnType;
		private System.Windows.Forms.Button cmdGotoPoly;
		private System.Windows.Forms.Button cmdCreateNew;
		private System.Windows.Forms.Button cmdRemovePoly;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button cmdSelectVert4;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.TextBox txtOffsetY4;
		private System.Windows.Forms.TextBox txtOffsetX4;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.TextBox txtTileY4;
		private System.Windows.Forms.TextBox txtTileX4;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.TextBox txtCellY4;
		private System.Windows.Forms.TextBox txtCellX4;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Button cmdSelectVert3;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.TextBox txtOffsetY3;
		private System.Windows.Forms.TextBox txtOffsetX3;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox txtTileY3;
		private System.Windows.Forms.TextBox txtTileX3;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.TextBox txtCellY3;
		private System.Windows.Forms.TextBox txtCellX3;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Button cmdSelectVert2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtOffsetY2;
		private System.Windows.Forms.TextBox txtOffsetX2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TextBox txtTileY2;
		private System.Windows.Forms.TextBox txtTileX2;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox txtCellY2;
		private System.Windows.Forms.TextBox txtCellX2;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Button cmdSelectVert1;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox txtOffsetY1;
		private System.Windows.Forms.TextBox txtOffsetX1;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtTileY1;
		private System.Windows.Forms.TextBox txtTileX1;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtCellY1;
		private System.Windows.Forms.TextBox txtCellX1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lstConnectedPolys;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.Button cmdRefresh;
		private System.Windows.Forms.Button cmdAddConnected;
		private System.Windows.Forms.Button cmdRemoveConnected;
	}
}