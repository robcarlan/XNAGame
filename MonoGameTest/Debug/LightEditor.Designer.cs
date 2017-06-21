namespace WindowsGame1.Debug
{
	partial class LightEditor
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
			this.lstLights = new System.Windows.Forms.ListView();
			this.columnID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmdSave = new System.Windows.Forms.Button();
			this.cmdRemoveEffect = new System.Windows.Forms.Button();
			this.cmdAddEffect = new System.Windows.Forms.Button();
			this.cmdRefresh = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtRadius = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.pbColour = new System.Windows.Forms.PictureBox();
			this.cmdColourPick = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtOscPeriod = new System.Windows.Forms.TextBox();
			this.txtRadiusIncrease = new System.Windows.Forms.TextBox();
			this.txtIntensity = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.chkOscillates = new System.Windows.Forms.CheckBox();
			this.chkFlicker = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbColour)).BeginInit();
			this.SuspendLayout();
			// 
			// lstLights
			// 
			this.lstLights.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnID,
            this.columnName,
            this.columnType});
			this.lstLights.Location = new System.Drawing.Point(12, 12);
			this.lstLights.Name = "lstLights";
			this.lstLights.Size = new System.Drawing.Size(293, 291);
			this.lstLights.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lstLights.TabIndex = 1;
			this.lstLights.UseCompatibleStateImageBehavior = false;
			this.lstLights.View = System.Windows.Forms.View.Details;
			this.lstLights.SelectedIndexChanged += new System.EventHandler(this.lstEffects_SelectedIndexChanged);
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
			// cmdSave
			// 
			this.cmdSave.Location = new System.Drawing.Point(641, 155);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(75, 23);
			this.cmdSave.TabIndex = 49;
			this.cmdSave.Text = "Save...";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cmdRemoveEffect
			// 
			this.cmdRemoveEffect.Location = new System.Drawing.Point(474, 155);
			this.cmdRemoveEffect.Name = "cmdRemoveEffect";
			this.cmdRemoveEffect.Size = new System.Drawing.Size(75, 23);
			this.cmdRemoveEffect.TabIndex = 48;
			this.cmdRemoveEffect.Text = "Remove";
			this.cmdRemoveEffect.UseVisualStyleBackColor = true;
			this.cmdRemoveEffect.Click += new System.EventHandler(this.cmdRemoveEffect_Click);
			// 
			// cmdAddEffect
			// 
			this.cmdAddEffect.Location = new System.Drawing.Point(393, 155);
			this.cmdAddEffect.Name = "cmdAddEffect";
			this.cmdAddEffect.Size = new System.Drawing.Size(75, 23);
			this.cmdAddEffect.TabIndex = 47;
			this.cmdAddEffect.Text = "Add New";
			this.cmdAddEffect.UseVisualStyleBackColor = true;
			this.cmdAddEffect.Click += new System.EventHandler(this.cmdAddEffect_Click);
			// 
			// cmdRefresh
			// 
			this.cmdRefresh.Location = new System.Drawing.Point(312, 155);
			this.cmdRefresh.Name = "cmdRefresh";
			this.cmdRefresh.Size = new System.Drawing.Size(75, 23);
			this.cmdRefresh.TabIndex = 46;
			this.cmdRefresh.Text = "Refresh";
			this.cmdRefresh.UseVisualStyleBackColor = true;
			this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtRadius);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.pbColour);
			this.groupBox1.Controls.Add(this.cmdColourPick);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txtOscPeriod);
			this.groupBox1.Controls.Add(this.txtRadiusIncrease);
			this.groupBox1.Controls.Add(this.txtIntensity);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.chkOscillates);
			this.groupBox1.Controls.Add(this.chkFlicker);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(312, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(404, 136);
			this.groupBox1.TabIndex = 50;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Base Settings";
			// 
			// txtRadius
			// 
			this.txtRadius.Location = new System.Drawing.Point(80, 100);
			this.txtRadius.Name = "txtRadius";
			this.txtRadius.Size = new System.Drawing.Size(100, 20);
			this.txtRadius.TabIndex = 16;
			this.txtRadius.TextChanged += new System.EventHandler(this.txtRadius_TextChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(186, 75);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(52, 13);
			this.label8.TabIndex = 15;
			this.label8.Text = "Direction:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 103);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(43, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "Radius:";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(80, 72);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(100, 20);
			this.txtName.TabIndex = 13;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Light Name:";
			// 
			// pbColour
			// 
			this.pbColour.Location = new System.Drawing.Point(111, 44);
			this.pbColour.Name = "pbColour";
			this.pbColour.Size = new System.Drawing.Size(19, 21);
			this.pbColour.TabIndex = 11;
			this.pbColour.TabStop = false;
			// 
			// cmdColourPick
			// 
			this.cmdColourPick.Location = new System.Drawing.Point(52, 42);
			this.cmdColourPick.Name = "cmdColourPick";
			this.cmdColourPick.Size = new System.Drawing.Size(52, 23);
			this.cmdColourPick.TabIndex = 10;
			this.cmdColourPick.Text = "Pick";
			this.cmdColourPick.UseVisualStyleBackColor = true;
			this.cmdColourPick.Click += new System.EventHandler(this.cmdColourPick_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(365, 43);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(20, 13);
			this.label5.TabIndex = 9;
			this.label5.Text = "ms";
			// 
			// txtOscPeriod
			// 
			this.txtOscPeriod.Location = new System.Drawing.Point(303, 40);
			this.txtOscPeriod.Name = "txtOscPeriod";
			this.txtOscPeriod.Size = new System.Drawing.Size(61, 20);
			this.txtOscPeriod.TabIndex = 8;
			this.txtOscPeriod.TextChanged += new System.EventHandler(this.txtOscPeriod_TextChanged);
			// 
			// txtRadiusIncrease
			// 
			this.txtRadiusIncrease.Location = new System.Drawing.Point(303, 17);
			this.txtRadiusIncrease.Name = "txtRadiusIncrease";
			this.txtRadiusIncrease.Size = new System.Drawing.Size(77, 20);
			this.txtRadiusIncrease.TabIndex = 7;
			this.txtRadiusIncrease.TextChanged += new System.EventHandler(this.txtIntensityRange_TextChanged);
			// 
			// txtIntensity
			// 
			this.txtIntensity.Location = new System.Drawing.Point(61, 17);
			this.txtIntensity.Name = "txtIntensity";
			this.txtIntensity.Size = new System.Drawing.Size(77, 20);
			this.txtIntensity.TabIndex = 6;
			this.txtIntensity.TextChanged += new System.EventHandler(this.txtIntensity_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(213, 47);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(91, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Oscillation Period:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(213, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Radius Increase:";
			// 
			// chkOscillates
			// 
			this.chkOscillates.AutoSize = true;
			this.chkOscillates.Location = new System.Drawing.Point(145, 46);
			this.chkOscillates.Name = "chkOscillates";
			this.chkOscillates.Size = new System.Drawing.Size(71, 17);
			this.chkOscillates.TabIndex = 3;
			this.chkOscillates.Text = "Oscillates";
			this.chkOscillates.UseVisualStyleBackColor = true;
			this.chkOscillates.CheckedChanged += new System.EventHandler(this.chkOscillates_CheckedChanged);
			// 
			// chkFlicker
			// 
			this.chkFlicker.AutoSize = true;
			this.chkFlicker.Location = new System.Drawing.Point(145, 19);
			this.chkFlicker.Name = "chkFlicker";
			this.chkFlicker.Size = new System.Drawing.Size(62, 17);
			this.chkFlicker.TabIndex = 2;
			this.chkFlicker.Text = "Flickers";
			this.chkFlicker.UseVisualStyleBackColor = true;
			this.chkFlicker.CheckedChanged += new System.EventHandler(this.chkFlicker_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Colour:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Intensity:";
			// 
			// LightEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(728, 309);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmdSave);
			this.Controls.Add(this.cmdRemoveEffect);
			this.Controls.Add(this.cmdAddEffect);
			this.Controls.Add(this.cmdRefresh);
			this.Controls.Add(this.lstLights);
			this.Name = "LightEditor";
			this.Text = "LightEditor";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbColour)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lstLights;
		private System.Windows.Forms.ColumnHeader columnID;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnType;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.Button cmdRemoveEffect;
		private System.Windows.Forms.Button cmdAddEffect;
		private System.Windows.Forms.Button cmdRefresh;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkOscillates;
		private System.Windows.Forms.CheckBox chkFlicker;
		private System.Windows.Forms.PictureBox pbColour;
		private System.Windows.Forms.Button cmdColourPick;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtOscPeriod;
		private System.Windows.Forms.TextBox txtRadiusIncrease;
		private System.Windows.Forms.TextBox txtIntensity;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.TextBox txtRadius;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
	}
}