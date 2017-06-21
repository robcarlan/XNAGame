using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework;
using DataLoader;

namespace WindowsGame1.Debug
{
	public partial class LightEditor : Form
	{
		public Game1 game;
		public short selectedLightID;
		public short selectedLightIndex;
		public DataLoader.LightLoader selectedLight;
		public List<short> freeLightID;
		public bool skipEvent = false;

		public LightEditor(Game1 game)
		{
			this.game = game;
			selectedLight = null;
			selectedLightID = -1;
			selectedLightIndex = -1;
			InitializeComponent();

			freeLightID = new List<short>();
			for (short i = 0; i < 1000; i++)
			{
				freeLightID.Add(i);
			}
			getLights();
		}

		public void selectLight(short ID)
		{
			skipEvent = true;
			selectedLightID = ID;
			selectedLight = game.ObjectManager.lightLibrary[ID];
			txtIntensity.Text = selectedLight.intensity.ToString();
			txtRadiusIncrease.Text = selectedLight.intensityRange.ToString();
			txtName.Text = selectedLight.lightName;
			txtOscPeriod.Text = selectedLight.periodMS.ToString();
			chkFlicker.Checked = selectedLight.flickers;
			chkOscillates.Checked = selectedLight.oscillates;
			txtRadius.Text = selectedLight.radius.ToString();
			if (selectedLight.oscillates)
				txtOscPeriod.Text = Convert.ToString(selectedLight.periodMS);
			skipEvent = false;
			pbColour.BackColor = System.Drawing.Color.FromArgb(selectedLight.lightColor.A, selectedLight.lightColor.R,
				selectedLight.lightColor.G, selectedLight.lightColor.B);
		}

		public void deselectLight(short ID)
		{
			skipEvent = true;
			selectedLightID = -1;
			selectedLight = null;
			txtIntensity.Text = "";
			txtRadiusIncrease.Text = "";
			txtName.Text = "";
			chkFlicker.Checked = false;
			chkOscillates.Checked = false;
			txtRadius.Text = "";
			txtOscPeriod.Text = "";
			skipEvent = false;
			pbColour.BackColor = System.Drawing.Color.Transparent;
		}

		public void getLights()
		{
			foreach (DataLoader.LightLoader light in game.ObjectManager.lightLibrary.Values)
			{
				addLight(light);
			}
		}

		private void addLight(DataLoader.LightLoader light)
		{
			String[] items = new String[3];
			items[0] = ("" + light.ID);
			items[1] = (light.lightName);
			items[2] = ("PointLight");
			freeLightID.Remove(light.ID);
			ListViewItem newItem = new ListViewItem(items);
			lstLights.Items.Add(newItem);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			//Serialise list to xml
			//Serialise this data
			string lightPath = Declaration.lightLoaderPath + ".xml";

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			using (XmlWriter writer = XmlWriter.Create(lightPath, settings))
			{
				IntermediateSerializer.Serialize(writer, game.ObjectManager.lightLibrary, null);
			}
		}

		private void cmdRemoveEffect_Click(object sender, EventArgs e)
		{
			//Remove effect, deselect
			if (selectedLightID == -1) return;
			game.ObjectManager.lightLibrary.Remove(selectedLightID);
			freeLightID.Insert(0, selectedLightID);
			lstLights.Items.RemoveAt(selectedLightIndex);
			selectedLightIndex = -1;
			deselectLight(selectedLightID);
		}

		private void cmdAddEffect_Click(object sender, EventArgs e)
		{
			//Create a new effect and select it
			DataLoader.LightLoader newlight = new DataLoader.LightLoader();
			newlight.ID = freeLightID[0];
			game.ObjectManager.lightLibrary.Add(newlight.ID, newlight);
			addLight(newlight);
		}

		private void cmdRefresh_Click(object sender, EventArgs e)
		{
			//Clear the list, get each element

			freeLightID.Clear();
			for (short i = 0; i < 1000; i++)
			{
				freeLightID.Add(i);
			}
			foreach (DataLoader.LightLoader light in game.ObjectManager.lightLibrary.Values)
			{
				addLight(light);
			}
		}

		private void lstEffects_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Change selected index if valid
			if (lstLights.SelectedIndices.Count == 1)
			{
				selectedLightIndex = (short)lstLights.SelectedIndices[0];
				selectedLightID = Convert.ToInt16(lstLights.Items[selectedLightIndex].SubItems[0].Text);
				selectedLight = game.ObjectManager.lightLibrary[selectedLightID];
				selectLight(selectedLightID);
			}
		}

		private void cmdColourPick_Click(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			DialogResult result;
			result = colorDialog1.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				System.Drawing.Color col = colorDialog1.Color;
				pbColour.BackColor = col;
				selectedLight.lightColor = new Microsoft.Xna.Framework.Color(col.R, col.G, col.B, col.A);
			}
		}

		private void txtIntensity_TextChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.intensity = (float)Convert.ToDecimal(txtIntensity.Text);
		}

		private void chkFlicker_CheckedChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.flickers = chkFlicker.Checked;
		}

		private void chkOscillates_CheckedChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.oscillates = chkOscillates.Checked;
		}

		private void txtIntensityRange_TextChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.intensityRange = (float)Convert.ToDecimal(txtRadiusIncrease.Text);
		}

		private void txtOscPeriod_TextChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.periodMS = (float)Convert.ToDecimal(txtOscPeriod.Text);
		}

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.lightName = txtName.Text;
			//Update name in list
			lstLights.Items[selectedLightIndex].SubItems[1].Text = txtName.Text;
		}

		private void txtRadius_TextChanged(object sender, EventArgs e)
		{
			if (selectedLightID == -1 || skipEvent) return;
			selectedLight.radius = (float)Convert.ToDecimal(txtRadius.Text);
		}
	}
}
