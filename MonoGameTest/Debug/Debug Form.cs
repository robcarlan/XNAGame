using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame1
{
	public partial class Debug_Form : Form
	{
		public Game1 game;
		public Graphics gfx;
		public Bitmap itemTexture;
		public Form effectForm;
		public Form navmeshForm;
		public Form lightForm;

		public Debug_Form()
		{
			InitializeComponent();
			gfx = this.CreateGraphics();
			System.Windows.Forms.Application.EnableVisualStyles();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			listCells();
		}

		public void listCells()
		{
			lstCells.Items.Clear();

			foreach(Microsoft.Xna.Framework.Point itr in game.tileManager.cellData.Keys)
			{
				lstCells.Items.Add(itr);
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Update list boxes to contain data from this cell
		
		}

		private void uniqueNPCsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Open NPC Window
		}

		private void abilitiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Open Ability Window
		}

		private void effectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Open Effect Window
			if (effectForm != null)
				effectForm.Show();
			else
			{
				effectForm = new EffectWindow(game);
				effectForm.Show();
			}
		}

		private void button1_Click_1(object sender, EventArgs e)
		{

		}

		private void cmdEditObject_Click(object sender, EventArgs e)
		{

		}

		private void cmdDeleteObject_Click(object sender, EventArgs e)
		{
			//Remove from relevant lists
		}

		private void cmdAddObject_Click(object sender, EventArgs e)
		{
			//Open a dialog to add a tpye of object from pre existing libraries of objects

			//Dynamic objects
				//Initialise triggers, i.e. door teleports / chest loot
		}

		private void cmdMoveObject_Click(object sender, EventArgs e)
		{
			//Hand control back to the game. Clicking on a space will move the object there
		}

		private void navmeshEditorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Open Navmesh Window
			if (navmeshForm != null)
				navmeshForm.Show();
			else
			{
				navmeshForm = new Debug.navmeshForm(game);
				navmeshForm.Show();
			}
		}

		private void lightsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Open Light Window
			if (lightForm != null)
				lightForm.Show();
			else
			{
				lightForm = new Debug.LightEditor(game);
				lightForm.Show();
			}
		}

		private void Debug_Form_Load(object sender, EventArgs e)
		{

		}
	}
}
