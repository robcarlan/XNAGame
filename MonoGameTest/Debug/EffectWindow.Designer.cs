namespace WindowsGame1
{
	partial class EffectWindow
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
			this.lstEffects = new System.Windows.Forms.ListView();
			this.columnID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmdRefresh = new System.Windows.Forms.Button();
			this.lblName = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.cmdAddEffect = new System.Windows.Forms.Button();
			this.cmdRemoveEffect = new System.Windows.Forms.Button();
			this.grpInfo = new System.Windows.Forms.GroupBox();
			this.txtStacksPerUse = new System.Windows.Forms.TextBox();
			this.txtMaxStacks = new System.Windows.Forms.TextBox();
			this.pbSpritePreview = new System.Windows.Forms.PictureBox();
			this.label38 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.txtSpriteY = new System.Windows.Forms.TextBox();
			this.txtSpriteX = new System.Windows.Forms.TextBox();
			this.txtTicks = new System.Windows.Forms.TextBox();
			this.txtTickFreq = new System.Windows.Forms.TextBox();
			this.txtFXDuration = new System.Windows.Forms.TextBox();
			this.chkPermanent = new System.Windows.Forms.CheckBox();
			this.chkStacks = new System.Windows.Forms.CheckBox();
			this.chkConsumable = new System.Windows.Forms.CheckBox();
			this.txtFXDesc = new System.Windows.Forms.TextBox();
			this.txtFXName = new System.Windows.Forms.TextBox();
			this.grpAbs = new System.Windows.Forms.GroupBox();
			this.txtHealPowAbs = new System.Windows.Forms.TextBox();
			this.txtMagPowAbs = new System.Windows.Forms.TextBox();
			this.txtAtkPowAbs = new System.Windows.Forms.TextBox();
			this.txtMoveSpeedAbs = new System.Windows.Forms.TextBox();
			this.txtEvadeAbs = new System.Windows.Forms.TextBox();
			this.txtDefAbs = new System.Windows.Forms.TextBox();
			this.txtMagDefAbs = new System.Windows.Forms.TextBox();
			this.txtManaRegenAbs = new System.Windows.Forms.TextBox();
			this.txtManaMaxAbs = new System.Windows.Forms.TextBox();
			this.txtManaAbs = new System.Windows.Forms.TextBox();
			this.txtHPRegenAbs = new System.Windows.Forms.TextBox();
			this.txtHPMaxAbs = new System.Windows.Forms.TextBox();
			this.txtHPAbs = new System.Windows.Forms.TextBox();
			this.grpPercent = new System.Windows.Forms.GroupBox();
			this.txtHealPowPer = new System.Windows.Forms.TextBox();
			this.label36 = new System.Windows.Forms.Label();
			this.txtMagPowPer = new System.Windows.Forms.TextBox();
			this.label37 = new System.Windows.Forms.Label();
			this.txtAtkPowPer = new System.Windows.Forms.TextBox();
			this.label35 = new System.Windows.Forms.Label();
			this.txtMoveSpeedPer = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.txtEvadePer = new System.Windows.Forms.TextBox();
			this.label33 = new System.Windows.Forms.Label();
			this.txtDefPer = new System.Windows.Forms.TextBox();
			this.label32 = new System.Windows.Forms.Label();
			this.txtMagDefPer = new System.Windows.Forms.TextBox();
			this.label31 = new System.Windows.Forms.Label();
			this.txtManaRegPer = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.txtManaMaxPer = new System.Windows.Forms.TextBox();
			this.label29 = new System.Windows.Forms.Label();
			this.txtManaPer = new System.Windows.Forms.TextBox();
			this.label28 = new System.Windows.Forms.Label();
			this.txtHPRegPer = new System.Windows.Forms.TextBox();
			this.label27 = new System.Windows.Forms.Label();
			this.txtHPMaxPer = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.txtHPPer = new System.Windows.Forms.TextBox();
			this.label25 = new System.Windows.Forms.Label();
			this.cmdSave = new System.Windows.Forms.Button();
			this.label39 = new System.Windows.Forms.Label();
			this.cmbType = new System.Windows.Forms.ComboBox();
			this.grpInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSpritePreview)).BeginInit();
			this.grpAbs.SuspendLayout();
			this.grpPercent.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstEffects
			// 
			this.lstEffects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnID,
            this.columnName,
            this.columnType});
			this.lstEffects.Location = new System.Drawing.Point(12, 12);
			this.lstEffects.Name = "lstEffects";
			this.lstEffects.Size = new System.Drawing.Size(293, 406);
			this.lstEffects.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstEffects.TabIndex = 0;
			this.lstEffects.UseCompatibleStateImageBehavior = false;
			this.lstEffects.View = System.Windows.Forms.View.Details;
			this.lstEffects.SelectedIndexChanged += new System.EventHandler(this.lstEffects_SelectedIndexChanged);
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
			// cmdRefresh
			// 
			this.cmdRefresh.Location = new System.Drawing.Point(12, 425);
			this.cmdRefresh.Name = "cmdRefresh";
			this.cmdRefresh.Size = new System.Drawing.Size(75, 23);
			this.cmdRefresh.TabIndex = 1;
			this.cmdRefresh.Text = "Refresh";
			this.cmdRefresh.UseVisualStyleBackColor = true;
			this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(7, 16);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(35, 13);
			this.lblName.TabIndex = 2;
			this.lblName.Text = "Name";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Description";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(173, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Sprite Source";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(413, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Permanent";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Max Stacks";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(406, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(63, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Does Stack";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(406, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Consumable";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(131, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(81, 13);
			this.label7.TabIndex = 9;
			this.label7.Text = "Stacks Per Use";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(7, 70);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 13);
			this.label8.TabIndex = 10;
			this.label8.Text = "Duration (MS)";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(178, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(77, 13);
			this.label9.TabIndex = 11;
			this.label9.Text = "Tick Freq (MS)";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(309, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(33, 13);
			this.label10.TabIndex = 12;
			this.label10.Text = "Ticks";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(9, 109);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(71, 13);
			this.label12.TabIndex = 14;
			this.label12.Text = "Attack Power";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(8, 133);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(69, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "Magic Power";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(9, 156);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(62, 13);
			this.label14.TabIndex = 16;
			this.label14.Text = "Heal Power";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(348, 48);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(47, 13);
			this.label15.TabIndex = 17;
			this.label15.Text = "Defense";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(316, 74);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(78, 13);
			this.label16.TabIndex = 18;
			this.label16.Text = "Evade Chance";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(316, 22);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(79, 13);
			this.label17.TabIndex = 19;
			this.label17.Text = "Magic Defense";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(7, 25);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(25, 13);
			this.label18.TabIndex = 20;
			this.label18.Text = "HP ";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(7, 68);
			this.label19.MaximumSize = new System.Drawing.Size(60, 0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(60, 26);
			this.label19.TabIndex = 21;
			this.label19.Text = "HP Regen Per 5";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(137, 68);
			this.label20.MaximumSize = new System.Drawing.Size(65, 0);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(61, 39);
			this.label20.TabIndex = 22;
			this.label20.Text = "Mana Regen Per 5";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(137, 25);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(34, 13);
			this.label21.TabIndex = 23;
			this.label21.Text = "Mana";
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(7, 48);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(45, 13);
			this.label22.TabIndex = 24;
			this.label22.Text = "HP Max";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(137, 48);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(57, 13);
			this.label23.TabIndex = 25;
			this.label23.Text = "Mana Max";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(303, 100);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(91, 13);
			this.label24.TabIndex = 26;
			this.label24.Text = "Movement Speed";
			// 
			// cmdAddEffect
			// 
			this.cmdAddEffect.Location = new System.Drawing.Point(12, 464);
			this.cmdAddEffect.Name = "cmdAddEffect";
			this.cmdAddEffect.Size = new System.Drawing.Size(75, 23);
			this.cmdAddEffect.TabIndex = 41;
			this.cmdAddEffect.Text = "Add New";
			this.cmdAddEffect.UseVisualStyleBackColor = true;
			this.cmdAddEffect.Click += new System.EventHandler(this.cmdAddEffect_Click);
			// 
			// cmdRemoveEffect
			// 
			this.cmdRemoveEffect.Location = new System.Drawing.Point(12, 504);
			this.cmdRemoveEffect.Name = "cmdRemoveEffect";
			this.cmdRemoveEffect.Size = new System.Drawing.Size(75, 23);
			this.cmdRemoveEffect.TabIndex = 42;
			this.cmdRemoveEffect.Text = "Remove";
			this.cmdRemoveEffect.UseVisualStyleBackColor = true;
			this.cmdRemoveEffect.Click += new System.EventHandler(this.cmdRemoveEffect_Click);
			// 
			// grpInfo
			// 
			this.grpInfo.Controls.Add(this.cmbType);
			this.grpInfo.Controls.Add(this.label39);
			this.grpInfo.Controls.Add(this.txtStacksPerUse);
			this.grpInfo.Controls.Add(this.txtMaxStacks);
			this.grpInfo.Controls.Add(this.pbSpritePreview);
			this.grpInfo.Controls.Add(this.label38);
			this.grpInfo.Controls.Add(this.label11);
			this.grpInfo.Controls.Add(this.txtSpriteY);
			this.grpInfo.Controls.Add(this.txtSpriteX);
			this.grpInfo.Controls.Add(this.txtTicks);
			this.grpInfo.Controls.Add(this.txtTickFreq);
			this.grpInfo.Controls.Add(this.txtFXDuration);
			this.grpInfo.Controls.Add(this.chkPermanent);
			this.grpInfo.Controls.Add(this.chkStacks);
			this.grpInfo.Controls.Add(this.chkConsumable);
			this.grpInfo.Controls.Add(this.txtFXDesc);
			this.grpInfo.Controls.Add(this.txtFXName);
			this.grpInfo.Controls.Add(this.label8);
			this.grpInfo.Controls.Add(this.label4);
			this.grpInfo.Controls.Add(this.label10);
			this.grpInfo.Controls.Add(this.label7);
			this.grpInfo.Controls.Add(this.label2);
			this.grpInfo.Controls.Add(this.label6);
			this.grpInfo.Controls.Add(this.label1);
			this.grpInfo.Controls.Add(this.label9);
			this.grpInfo.Controls.Add(this.lblName);
			this.grpInfo.Controls.Add(this.label3);
			this.grpInfo.Controls.Add(this.label5);
			this.grpInfo.Location = new System.Drawing.Point(311, 12);
			this.grpInfo.Name = "grpInfo";
			this.grpInfo.Size = new System.Drawing.Size(532, 133);
			this.grpInfo.TabIndex = 43;
			this.grpInfo.TabStop = false;
			this.grpInfo.Text = "Effect General";
			// 
			// txtStacksPerUse
			// 
			this.txtStacksPerUse.Location = new System.Drawing.Point(218, 101);
			this.txtStacksPerUse.Name = "txtStacksPerUse";
			this.txtStacksPerUse.Size = new System.Drawing.Size(44, 20);
			this.txtStacksPerUse.TabIndex = 27;
			this.txtStacksPerUse.TextChanged += new System.EventHandler(this.txtStacksPerUse_TextChanged);
			// 
			// txtMaxStacks
			// 
			this.txtMaxStacks.Location = new System.Drawing.Point(76, 101);
			this.txtMaxStacks.Name = "txtMaxStacks";
			this.txtMaxStacks.Size = new System.Drawing.Size(44, 20);
			this.txtMaxStacks.TabIndex = 26;
			this.txtMaxStacks.TextChanged += new System.EventHandler(this.txtMaxStacks_TextChanged);
			// 
			// pbSpritePreview
			// 
			this.pbSpritePreview.BackColor = System.Drawing.SystemColors.ControlLight;
			this.pbSpritePreview.Location = new System.Drawing.Point(270, 93);
			this.pbSpritePreview.Name = "pbSpritePreview";
			this.pbSpritePreview.Size = new System.Drawing.Size(40, 40);
			this.pbSpritePreview.TabIndex = 25;
			this.pbSpritePreview.TabStop = false;
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.Location = new System.Drawing.Point(320, 70);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(17, 13);
			this.label38.TabIndex = 24;
			this.label38.Text = "Y:";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(247, 70);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(17, 13);
			this.label11.TabIndex = 23;
			this.label11.Text = "X:";
			// 
			// txtSpriteY
			// 
			this.txtSpriteY.Location = new System.Drawing.Point(343, 67);
			this.txtSpriteY.Name = "txtSpriteY";
			this.txtSpriteY.Size = new System.Drawing.Size(45, 20);
			this.txtSpriteY.TabIndex = 22;
			this.txtSpriteY.TextChanged += new System.EventHandler(this.txtSpriteY_TextChanged);
			// 
			// txtSpriteX
			// 
			this.txtSpriteX.Location = new System.Drawing.Point(270, 67);
			this.txtSpriteX.Name = "txtSpriteX";
			this.txtSpriteX.Size = new System.Drawing.Size(44, 20);
			this.txtSpriteX.TabIndex = 21;
			this.txtSpriteX.TextChanged += new System.EventHandler(this.txtSpriteX_TextChanged);
			// 
			// txtTicks
			// 
			this.txtTicks.Location = new System.Drawing.Point(347, 12);
			this.txtTicks.Name = "txtTicks";
			this.txtTicks.Size = new System.Drawing.Size(53, 20);
			this.txtTicks.TabIndex = 20;
			this.txtTicks.TextChanged += new System.EventHandler(this.txtTicks_TextChanged);
			// 
			// txtTickFreq
			// 
			this.txtTickFreq.Location = new System.Drawing.Point(254, 13);
			this.txtTickFreq.Name = "txtTickFreq";
			this.txtTickFreq.Size = new System.Drawing.Size(53, 20);
			this.txtTickFreq.TabIndex = 19;
			this.txtTickFreq.TextChanged += new System.EventHandler(this.txtTickFreq_TextChanged);
			// 
			// txtFXDuration
			// 
			this.txtFXDuration.Location = new System.Drawing.Point(85, 67);
			this.txtFXDuration.Name = "txtFXDuration";
			this.txtFXDuration.Size = new System.Drawing.Size(79, 20);
			this.txtFXDuration.TabIndex = 18;
			this.txtFXDuration.TextChanged += new System.EventHandler(this.txtFXDuration_TextChanged);
			// 
			// chkPermanent
			// 
			this.chkPermanent.AutoSize = true;
			this.chkPermanent.Location = new System.Drawing.Point(475, 64);
			this.chkPermanent.Name = "chkPermanent";
			this.chkPermanent.Size = new System.Drawing.Size(15, 14);
			this.chkPermanent.TabIndex = 17;
			this.chkPermanent.UseVisualStyleBackColor = true;
			this.chkPermanent.CheckedChanged += new System.EventHandler(this.chkPermanent_CheckedChanged);
			// 
			// chkStacks
			// 
			this.chkStacks.AutoSize = true;
			this.chkStacks.Location = new System.Drawing.Point(475, 39);
			this.chkStacks.Name = "chkStacks";
			this.chkStacks.Size = new System.Drawing.Size(15, 14);
			this.chkStacks.TabIndex = 16;
			this.chkStacks.UseVisualStyleBackColor = true;
			this.chkStacks.CheckedChanged += new System.EventHandler(this.chkStacks_CheckedChanged);
			// 
			// chkConsumable
			// 
			this.chkConsumable.AutoSize = true;
			this.chkConsumable.Location = new System.Drawing.Point(475, 15);
			this.chkConsumable.Name = "chkConsumable";
			this.chkConsumable.Size = new System.Drawing.Size(15, 14);
			this.chkConsumable.TabIndex = 15;
			this.chkConsumable.UseVisualStyleBackColor = true;
			this.chkConsumable.CheckedChanged += new System.EventHandler(this.chkConsumable_CheckedChanged);
			// 
			// txtFXDesc
			// 
			this.txtFXDesc.Location = new System.Drawing.Point(73, 37);
			this.txtFXDesc.Name = "txtFXDesc";
			this.txtFXDesc.Size = new System.Drawing.Size(327, 20);
			this.txtFXDesc.TabIndex = 14;
			this.txtFXDesc.TextChanged += new System.EventHandler(this.txtFXDesc_TextChanged);
			// 
			// txtFXName
			// 
			this.txtFXName.Location = new System.Drawing.Point(44, 16);
			this.txtFXName.Name = "txtFXName";
			this.txtFXName.Size = new System.Drawing.Size(133, 20);
			this.txtFXName.TabIndex = 13;
			this.txtFXName.TextChanged += new System.EventHandler(this.txtFXName_TextChanged);
			// 
			// grpAbs
			// 
			this.grpAbs.Controls.Add(this.txtHealPowAbs);
			this.grpAbs.Controls.Add(this.txtMagPowAbs);
			this.grpAbs.Controls.Add(this.txtAtkPowAbs);
			this.grpAbs.Controls.Add(this.txtMoveSpeedAbs);
			this.grpAbs.Controls.Add(this.txtEvadeAbs);
			this.grpAbs.Controls.Add(this.txtDefAbs);
			this.grpAbs.Controls.Add(this.txtMagDefAbs);
			this.grpAbs.Controls.Add(this.txtManaRegenAbs);
			this.grpAbs.Controls.Add(this.txtManaMaxAbs);
			this.grpAbs.Controls.Add(this.txtManaAbs);
			this.grpAbs.Controls.Add(this.txtHPRegenAbs);
			this.grpAbs.Controls.Add(this.txtHPMaxAbs);
			this.grpAbs.Controls.Add(this.txtHPAbs);
			this.grpAbs.Controls.Add(this.label13);
			this.grpAbs.Controls.Add(this.label12);
			this.grpAbs.Controls.Add(this.label24);
			this.grpAbs.Controls.Add(this.label14);
			this.grpAbs.Controls.Add(this.label23);
			this.grpAbs.Controls.Add(this.label15);
			this.grpAbs.Controls.Add(this.label22);
			this.grpAbs.Controls.Add(this.label16);
			this.grpAbs.Controls.Add(this.label21);
			this.grpAbs.Controls.Add(this.label17);
			this.grpAbs.Controls.Add(this.label20);
			this.grpAbs.Controls.Add(this.label18);
			this.grpAbs.Controls.Add(this.label19);
			this.grpAbs.Location = new System.Drawing.Point(311, 151);
			this.grpAbs.Name = "grpAbs";
			this.grpAbs.Size = new System.Drawing.Size(532, 181);
			this.grpAbs.TabIndex = 44;
			this.grpAbs.TabStop = false;
			this.grpAbs.Text = "Absolute Increases";
			// 
			// txtHealPowAbs
			// 
			this.txtHealPowAbs.Location = new System.Drawing.Point(86, 153);
			this.txtHealPowAbs.Name = "txtHealPowAbs";
			this.txtHealPowAbs.Size = new System.Drawing.Size(62, 20);
			this.txtHealPowAbs.TabIndex = 40;
			this.txtHealPowAbs.TextChanged += new System.EventHandler(this.txtHealPowAbs_TextChanged);
			// 
			// txtMagPowAbs
			// 
			this.txtMagPowAbs.Location = new System.Drawing.Point(86, 130);
			this.txtMagPowAbs.Name = "txtMagPowAbs";
			this.txtMagPowAbs.Size = new System.Drawing.Size(62, 20);
			this.txtMagPowAbs.TabIndex = 39;
			this.txtMagPowAbs.TextChanged += new System.EventHandler(this.txtMagPowAbs_TextChanged);
			// 
			// txtAtkPowAbs
			// 
			this.txtAtkPowAbs.Location = new System.Drawing.Point(86, 106);
			this.txtAtkPowAbs.Name = "txtAtkPowAbs";
			this.txtAtkPowAbs.Size = new System.Drawing.Size(62, 20);
			this.txtAtkPowAbs.TabIndex = 38;
			this.txtAtkPowAbs.TextChanged += new System.EventHandler(this.txtAtkPowAbs_TextChanged);
			// 
			// txtMoveSpeedAbs
			// 
			this.txtMoveSpeedAbs.Location = new System.Drawing.Point(409, 97);
			this.txtMoveSpeedAbs.Name = "txtMoveSpeedAbs";
			this.txtMoveSpeedAbs.Size = new System.Drawing.Size(62, 20);
			this.txtMoveSpeedAbs.TabIndex = 37;
			this.txtMoveSpeedAbs.TextChanged += new System.EventHandler(this.txtMoveSpeedAbs_TextChanged);
			// 
			// txtEvadeAbs
			// 
			this.txtEvadeAbs.Location = new System.Drawing.Point(409, 71);
			this.txtEvadeAbs.Name = "txtEvadeAbs";
			this.txtEvadeAbs.Size = new System.Drawing.Size(62, 20);
			this.txtEvadeAbs.TabIndex = 36;
			this.txtEvadeAbs.TextChanged += new System.EventHandler(this.txtEvadeAbs_TextChanged);
			// 
			// txtDefAbs
			// 
			this.txtDefAbs.Location = new System.Drawing.Point(409, 45);
			this.txtDefAbs.Name = "txtDefAbs";
			this.txtDefAbs.Size = new System.Drawing.Size(62, 20);
			this.txtDefAbs.TabIndex = 35;
			this.txtDefAbs.TextChanged += new System.EventHandler(this.txtDefAbs_TextChanged);
			// 
			// txtMagDefAbs
			// 
			this.txtMagDefAbs.Location = new System.Drawing.Point(409, 19);
			this.txtMagDefAbs.Name = "txtMagDefAbs";
			this.txtMagDefAbs.Size = new System.Drawing.Size(62, 20);
			this.txtMagDefAbs.TabIndex = 34;
			this.txtMagDefAbs.TextChanged += new System.EventHandler(this.txtMagDefAbs_TextChanged);
			// 
			// txtManaRegenAbs
			// 
			this.txtManaRegenAbs.Location = new System.Drawing.Point(208, 71);
			this.txtManaRegenAbs.Name = "txtManaRegenAbs";
			this.txtManaRegenAbs.Size = new System.Drawing.Size(56, 20);
			this.txtManaRegenAbs.TabIndex = 33;
			this.txtManaRegenAbs.TextChanged += new System.EventHandler(this.txtManaRegenAbs_TextChanged);
			// 
			// txtManaMaxAbs
			// 
			this.txtManaMaxAbs.Location = new System.Drawing.Point(206, 45);
			this.txtManaMaxAbs.Name = "txtManaMaxAbs";
			this.txtManaMaxAbs.Size = new System.Drawing.Size(56, 20);
			this.txtManaMaxAbs.TabIndex = 32;
			this.txtManaMaxAbs.TextChanged += new System.EventHandler(this.txtManaMaxAbs_TextChanged);
			// 
			// txtManaAbs
			// 
			this.txtManaAbs.Location = new System.Drawing.Point(206, 18);
			this.txtManaAbs.Name = "txtManaAbs";
			this.txtManaAbs.Size = new System.Drawing.Size(56, 20);
			this.txtManaAbs.TabIndex = 31;
			this.txtManaAbs.TextChanged += new System.EventHandler(this.txtManaAbs_TextChanged);
			// 
			// txtHPRegenAbs
			// 
			this.txtHPRegenAbs.Location = new System.Drawing.Point(73, 71);
			this.txtHPRegenAbs.Name = "txtHPRegenAbs";
			this.txtHPRegenAbs.Size = new System.Drawing.Size(56, 20);
			this.txtHPRegenAbs.TabIndex = 30;
			this.txtHPRegenAbs.TextChanged += new System.EventHandler(this.txtHPRegenAbs_TextChanged);
			// 
			// txtHPMaxAbs
			// 
			this.txtHPMaxAbs.Location = new System.Drawing.Point(73, 45);
			this.txtHPMaxAbs.Name = "txtHPMaxAbs";
			this.txtHPMaxAbs.Size = new System.Drawing.Size(56, 20);
			this.txtHPMaxAbs.TabIndex = 29;
			this.txtHPMaxAbs.TextChanged += new System.EventHandler(this.txtHPMaxAbs_TextChanged);
			// 
			// txtHPAbs
			// 
			this.txtHPAbs.Location = new System.Drawing.Point(73, 19);
			this.txtHPAbs.Name = "txtHPAbs";
			this.txtHPAbs.Size = new System.Drawing.Size(56, 20);
			this.txtHPAbs.TabIndex = 28;
			this.txtHPAbs.TextChanged += new System.EventHandler(this.txtHPAbs_TextChanged);
			// 
			// grpPercent
			// 
			this.grpPercent.Controls.Add(this.txtHealPowPer);
			this.grpPercent.Controls.Add(this.label36);
			this.grpPercent.Controls.Add(this.txtMagPowPer);
			this.grpPercent.Controls.Add(this.label37);
			this.grpPercent.Controls.Add(this.txtAtkPowPer);
			this.grpPercent.Controls.Add(this.label35);
			this.grpPercent.Controls.Add(this.txtMoveSpeedPer);
			this.grpPercent.Controls.Add(this.label34);
			this.grpPercent.Controls.Add(this.txtEvadePer);
			this.grpPercent.Controls.Add(this.label33);
			this.grpPercent.Controls.Add(this.txtDefPer);
			this.grpPercent.Controls.Add(this.label32);
			this.grpPercent.Controls.Add(this.txtMagDefPer);
			this.grpPercent.Controls.Add(this.label31);
			this.grpPercent.Controls.Add(this.txtManaRegPer);
			this.grpPercent.Controls.Add(this.label30);
			this.grpPercent.Controls.Add(this.txtManaMaxPer);
			this.grpPercent.Controls.Add(this.label29);
			this.grpPercent.Controls.Add(this.txtManaPer);
			this.grpPercent.Controls.Add(this.label28);
			this.grpPercent.Controls.Add(this.txtHPRegPer);
			this.grpPercent.Controls.Add(this.label27);
			this.grpPercent.Controls.Add(this.txtHPMaxPer);
			this.grpPercent.Controls.Add(this.label26);
			this.grpPercent.Controls.Add(this.txtHPPer);
			this.grpPercent.Controls.Add(this.label25);
			this.grpPercent.Location = new System.Drawing.Point(311, 338);
			this.grpPercent.Name = "grpPercent";
			this.grpPercent.Size = new System.Drawing.Size(532, 169);
			this.grpPercent.TabIndex = 44;
			this.grpPercent.TabStop = false;
			this.grpPercent.Text = "Percent Increases";
			// 
			// txtHealPowPer
			// 
			this.txtHealPowPer.Location = new System.Drawing.Point(88, 146);
			this.txtHealPowPer.Name = "txtHealPowPer";
			this.txtHealPowPer.Size = new System.Drawing.Size(62, 20);
			this.txtHealPowPer.TabIndex = 66;
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Location = new System.Drawing.Point(9, 18);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(25, 13);
			this.label36.TabIndex = 47;
			this.label36.Text = "HP ";
			// 
			// txtMagPowPer
			// 
			this.txtMagPowPer.Location = new System.Drawing.Point(88, 123);
			this.txtMagPowPer.Name = "txtMagPowPer";
			this.txtMagPowPer.Size = new System.Drawing.Size(62, 20);
			this.txtMagPowPer.TabIndex = 65;
			// 
			// label37
			// 
			this.label37.AutoSize = true;
			this.label37.Location = new System.Drawing.Point(9, 61);
			this.label37.MaximumSize = new System.Drawing.Size(60, 0);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(60, 26);
			this.label37.TabIndex = 48;
			this.label37.Text = "HP Regen Per 5";
			// 
			// txtAtkPowPer
			// 
			this.txtAtkPowPer.Location = new System.Drawing.Point(88, 99);
			this.txtAtkPowPer.Name = "txtAtkPowPer";
			this.txtAtkPowPer.Size = new System.Drawing.Size(62, 20);
			this.txtAtkPowPer.TabIndex = 64;
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Location = new System.Drawing.Point(139, 61);
			this.label35.MaximumSize = new System.Drawing.Size(65, 0);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(61, 39);
			this.label35.TabIndex = 49;
			this.label35.Text = "Mana Regen Per 5";
			// 
			// txtMoveSpeedPer
			// 
			this.txtMoveSpeedPer.Location = new System.Drawing.Point(411, 90);
			this.txtMoveSpeedPer.Name = "txtMoveSpeedPer";
			this.txtMoveSpeedPer.Size = new System.Drawing.Size(62, 20);
			this.txtMoveSpeedPer.TabIndex = 63;
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Location = new System.Drawing.Point(318, 15);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(79, 13);
			this.label34.TabIndex = 46;
			this.label34.Text = "Magic Defense";
			// 
			// txtEvadePer
			// 
			this.txtEvadePer.Location = new System.Drawing.Point(411, 64);
			this.txtEvadePer.Name = "txtEvadePer";
			this.txtEvadePer.Size = new System.Drawing.Size(62, 20);
			this.txtEvadePer.TabIndex = 62;
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(139, 18);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(34, 13);
			this.label33.TabIndex = 50;
			this.label33.Text = "Mana";
			// 
			// txtDefPer
			// 
			this.txtDefPer.Location = new System.Drawing.Point(411, 38);
			this.txtDefPer.Name = "txtDefPer";
			this.txtDefPer.Size = new System.Drawing.Size(62, 20);
			this.txtDefPer.TabIndex = 61;
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Location = new System.Drawing.Point(318, 67);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(78, 13);
			this.label32.TabIndex = 45;
			this.label32.Text = "Evade Chance";
			// 
			// txtMagDefPer
			// 
			this.txtMagDefPer.Location = new System.Drawing.Point(411, 12);
			this.txtMagDefPer.Name = "txtMagDefPer";
			this.txtMagDefPer.Size = new System.Drawing.Size(62, 20);
			this.txtMagDefPer.TabIndex = 60;
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Location = new System.Drawing.Point(9, 41);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(45, 13);
			this.label31.TabIndex = 51;
			this.label31.Text = "HP Max";
			// 
			// txtManaRegPer
			// 
			this.txtManaRegPer.Location = new System.Drawing.Point(210, 64);
			this.txtManaRegPer.Name = "txtManaRegPer";
			this.txtManaRegPer.Size = new System.Drawing.Size(56, 20);
			this.txtManaRegPer.TabIndex = 59;
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(350, 41);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(47, 13);
			this.label30.TabIndex = 44;
			this.label30.Text = "Defense";
			// 
			// txtManaMaxPer
			// 
			this.txtManaMaxPer.Location = new System.Drawing.Point(208, 38);
			this.txtManaMaxPer.Name = "txtManaMaxPer";
			this.txtManaMaxPer.Size = new System.Drawing.Size(56, 20);
			this.txtManaMaxPer.TabIndex = 58;
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(139, 41);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(57, 13);
			this.label29.TabIndex = 52;
			this.label29.Text = "Mana Max";
			// 
			// txtManaPer
			// 
			this.txtManaPer.Location = new System.Drawing.Point(208, 11);
			this.txtManaPer.Name = "txtManaPer";
			this.txtManaPer.Size = new System.Drawing.Size(56, 20);
			this.txtManaPer.TabIndex = 57;
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(11, 149);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(62, 13);
			this.label28.TabIndex = 43;
			this.label28.Text = "Heal Power";
			// 
			// txtHPRegPer
			// 
			this.txtHPRegPer.Location = new System.Drawing.Point(75, 64);
			this.txtHPRegPer.Name = "txtHPRegPer";
			this.txtHPRegPer.Size = new System.Drawing.Size(56, 20);
			this.txtHPRegPer.TabIndex = 56;
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(305, 93);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(91, 13);
			this.label27.TabIndex = 53;
			this.label27.Text = "Movement Speed";
			// 
			// txtHPMaxPer
			// 
			this.txtHPMaxPer.Location = new System.Drawing.Point(75, 38);
			this.txtHPMaxPer.Name = "txtHPMaxPer";
			this.txtHPMaxPer.Size = new System.Drawing.Size(56, 20);
			this.txtHPMaxPer.TabIndex = 55;
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(11, 102);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(71, 13);
			this.label26.TabIndex = 41;
			this.label26.Text = "Attack Power";
			// 
			// txtHPPer
			// 
			this.txtHPPer.Location = new System.Drawing.Point(75, 12);
			this.txtHPPer.Name = "txtHPPer";
			this.txtHPPer.Size = new System.Drawing.Size(56, 20);
			this.txtHPPer.TabIndex = 54;
			this.txtHPPer.TextChanged += new System.EventHandler(this.txtHPPer_TextChanged);
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(10, 126);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(69, 13);
			this.label25.TabIndex = 42;
			this.label25.Text = "Magic Power";
			// 
			// cmdSave
			// 
			this.cmdSave.Location = new System.Drawing.Point(230, 425);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(75, 23);
			this.cmdSave.TabIndex = 45;
			this.cmdSave.Text = "Save...";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// label39
			// 
			this.label39.AutoSize = true;
			this.label39.Location = new System.Drawing.Point(320, 101);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(34, 13);
			this.label39.TabIndex = 28;
			this.label39.Text = "Type:";
			// 
			// cmbType
			// 
			this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Location = new System.Drawing.Point(360, 98);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new System.Drawing.Size(121, 21);
			this.cmbType.TabIndex = 29;
			this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
			// 
			// EffectWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(855, 591);
			this.Controls.Add(this.cmdSave);
			this.Controls.Add(this.cmdRemoveEffect);
			this.Controls.Add(this.cmdAddEffect);
			this.Controls.Add(this.cmdRefresh);
			this.Controls.Add(this.lstEffects);
			this.Controls.Add(this.grpInfo);
			this.Controls.Add(this.grpAbs);
			this.Controls.Add(this.grpPercent);
			this.Name = "EffectWindow";
			this.Text = "EffectWindow";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EffectWindow_FormClosing);
			this.Load += new System.EventHandler(this.EffectWindow_Load);
			this.grpInfo.ResumeLayout(false);
			this.grpInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSpritePreview)).EndInit();
			this.grpAbs.ResumeLayout(false);
			this.grpAbs.PerformLayout();
			this.grpPercent.ResumeLayout(false);
			this.grpPercent.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstEffects;
		private System.Windows.Forms.ColumnHeader columnID;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnType;
		private System.Windows.Forms.Button cmdRefresh;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Button cmdAddEffect;
		private System.Windows.Forms.Button cmdRemoveEffect;
		private System.Windows.Forms.GroupBox grpInfo;
		private System.Windows.Forms.GroupBox grpAbs;
		private System.Windows.Forms.GroupBox grpPercent;
		private System.Windows.Forms.TextBox txtFXName;
		private System.Windows.Forms.TextBox txtFXDesc;
		private System.Windows.Forms.TextBox txtFXDuration;
		private System.Windows.Forms.CheckBox chkPermanent;
		private System.Windows.Forms.CheckBox chkStacks;
		private System.Windows.Forms.CheckBox chkConsumable;
		private System.Windows.Forms.TextBox txtSpriteY;
		private System.Windows.Forms.TextBox txtSpriteX;
		private System.Windows.Forms.TextBox txtTicks;
		private System.Windows.Forms.TextBox txtTickFreq;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.PictureBox pbSpritePreview;
		private System.Windows.Forms.TextBox txtStacksPerUse;
		private System.Windows.Forms.TextBox txtMaxStacks;
		private System.Windows.Forms.TextBox txtHPRegenAbs;
		private System.Windows.Forms.TextBox txtHPMaxAbs;
		private System.Windows.Forms.TextBox txtHPAbs;
		private System.Windows.Forms.TextBox txtMagDefAbs;
		private System.Windows.Forms.TextBox txtManaRegenAbs;
		private System.Windows.Forms.TextBox txtManaMaxAbs;
		private System.Windows.Forms.TextBox txtManaAbs;
		private System.Windows.Forms.TextBox txtHealPowAbs;
		private System.Windows.Forms.TextBox txtMagPowAbs;
		private System.Windows.Forms.TextBox txtAtkPowAbs;
		private System.Windows.Forms.TextBox txtMoveSpeedAbs;
		private System.Windows.Forms.TextBox txtEvadeAbs;
		private System.Windows.Forms.TextBox txtDefAbs;
		private System.Windows.Forms.TextBox txtHealPowPer;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.TextBox txtMagPowPer;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.TextBox txtAtkPowPer;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.TextBox txtMoveSpeedPer;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.TextBox txtEvadePer;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.TextBox txtDefPer;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.TextBox txtMagDefPer;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.TextBox txtManaRegPer;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.TextBox txtManaMaxPer;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.TextBox txtManaPer;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.TextBox txtHPRegPer;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.TextBox txtHPMaxPer;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox txtHPPer;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.ComboBox cmbType;
		private System.Windows.Forms.Label label39;

	}
}