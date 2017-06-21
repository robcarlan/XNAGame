using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace WindowsGame1
{
	public partial class EffectWindow : Form
	{
		public bool isExtended = false;
		public bool handleEvent = true;
		public Game1 game;
		public Bitmap tex;
		public Bitmap buffer = new Bitmap(40, 40);
		public Graphics gfx;
		public Rectangle sourceRect;
		public Rectangle drawRec = new Rectangle(0, 0, 40, 40);
		public short selectedEffectID = -1;
		public int selectedListIndex = -1;
		public CharacterEffect selectedEffect;
		public string appDirectory = Application.StartupPath;
		public List<short> freeID = new List<short>(1000);

		public EffectWindow(Game1 game)
		{
			InitializeComponent();
			this.game = game;
			sourceRect.Width = sourceRect.Height = 40;
			gfx = Graphics.FromImage(buffer);
			tex = new Bitmap(game.fullContentPath + "Sprite Content\\SpellIcons.png");
			for (short i = 1; i <= 1000; i++)
			{
				freeID.Add(i);
			}
		}

		private void EffectWindow_Load(object sender, EventArgs e)
		{
			//Load effect combo box
			cmbType.Items.AddRange(Enum.GetNames(typeof(damageType)));
			listEffects();
		}

		private void cmdRefresh_Click(object sender, EventArgs e)
		{
			lstEffects.Items.Clear();
			listEffects();
		}

		private void listEffects()
		{
			// Get all effects
			foreach (CharacterEffect fxItr in game.effectManager.effects.Values)
			{
				String[] items = new String[3];
				items[0] = ("" + fxItr.effectID);
				items[1] = (fxItr.name);

				if (typeof(EffectExtended) == fxItr.GetType())
				{
					items[2] = ("EffectExtended");
				}
				else items[2] = ("CharacterEffect");

				freeID.Remove(fxItr.effectID);
				ListViewItem newItem = new ListViewItem(items);
				lstEffects.Items.Add(newItem);
			}

			lstEffects.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void EffectWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}

		public void updateSprite()
		{
			sourceRect.X = selectedEffect.spriteSource.X;
			sourceRect.Y = selectedEffect.spriteSource.Y;
			gfx.Clear(Color.AliceBlue);
			gfx.DrawImage(tex, drawRec, sourceRect, GraphicsUnit.Pixel);
			pbSpritePreview.Image = buffer;
		}

		private void lstEffects_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstEffects.SelectedItems.Count == 1)
			{
				selectedEffectID = Convert.ToInt16(lstEffects.SelectedItems[0].SubItems[0].Text);
				selectedListIndex = lstEffects.SelectedIndices[0];
				selectItem(selectedEffectID);
				updateSprite();
			}

		}

		public void selectItem(short index)
		{
			handleEvent = false;
			selectedEffect = (game.effectManager.effects[index]);
			//Default
			txtFXName.Text = selectedEffect.name;
			txtFXDesc.Text = selectedEffect.description;
			txtFXDuration.Text = selectedEffect.durationMS.ToString();
			chkConsumable.Checked = selectedEffect.consumable;
			chkPermanent.Checked = selectedEffect.permanent;
			chkStacks.Checked = selectedEffect.stacks;
			cmbType.SelectedItem = cmbType.Items[(int)selectedEffect.type];
			try
			{
				txtSpriteY.Text = game.UI.effectSprites[selectedEffect.effectID].Value.Y.ToString();
			}
			catch(Exception ex)
			{
				txtSpriteY.Text = "0";
			}
			try
			{
				txtSpriteX.Text = game.UI.effectSprites[selectedEffect.effectID].Value.X.ToString();
			}
			catch (Exception ex)
			{
				txtSpriteY.Text = "0";
			}
			txtTickFreq.Text = selectedEffect.tickSpeed.ToString();
			txtTicks.Text = selectedEffect.ticks.ToString();
			txtMaxStacks.Text = selectedEffect.maxStacks.ToString();
			txtStacksPerUse.Text = selectedEffect.stacksPerUse.ToString();

			//Absolute Increases
			txtHPAbs.Text = selectedEffect.hpIncrease.ToString();
			txtHPMaxAbs.Text = selectedEffect.hpMaxIncrease.ToString();
			txtHPRegenAbs.Text = selectedEffect.hpRegenPer5increase.ToString();
			txtManaAbs.Text = selectedEffect.manaIncrease.ToString();
			txtManaMaxAbs.Text = selectedEffect.manaMaxIncrease.ToString();
			txtManaRegenAbs.Text = selectedEffect.manaPer5Increase.ToString();
			txtMagDefAbs.Text = selectedEffect.magicDefenseIncrease.ToString();
			txtDefAbs.Text = selectedEffect.defenseIncrease.ToString();
			txtEvadeAbs.Text = selectedEffect.evadeChanceIncrease.ToString();
			txtMoveSpeedAbs.Text = selectedEffect.movementSpeedIncrease.ToString();
			txtAtkPowAbs.Text = selectedEffect.attackPowerIncrease.ToString();
			txtMagPowAbs.Text = selectedEffect.magicPowerIncrease.ToString();
			txtHealPowAbs.Text = selectedEffect.healPowerIncrease.ToString();

			//Percentage Increases
			if (typeof(EffectExtended) == selectedEffect.GetType())
			{
				isExtended = true;
				EffectExtended selectedEffectExtended = (EffectExtended)selectedEffect;
				txtHPPer.Text = selectedEffectExtended.hpPercentIncrease.ToString();
				txtHPMaxPer.Text = selectedEffectExtended.hpMaxPercentIncrease.ToString();
				txtHPRegPer.Text = selectedEffectExtended.hpRegenPer5PercentIncrease.ToString();
				txtManaPer.Text = selectedEffectExtended.manaPercentIncrease.ToString();
				txtManaMaxPer.Text = selectedEffectExtended.manaMaxPercentIncrease.ToString();
				txtManaRegPer.Text = selectedEffectExtended.manaPer5PercentIncrease.ToString();
				txtMagDefPer.Text = selectedEffectExtended.magicDefensePercentIncrease.ToString();
				txtDefPer.Text = selectedEffectExtended.defensePercentIncrease.ToString();
				txtEvadePer.Text = selectedEffectExtended.evadeChancePercentIncrease.ToString();
				txtMoveSpeedPer.Text = selectedEffectExtended.movementSpeedPercentIncrease.ToString();
				txtAtkPowPer.Text = selectedEffectExtended.attackPowerPercentIncrease.ToString();
				txtMagPowPer.Text = selectedEffectExtended.magicPowerPercentIncrease.ToString();
				txtHealPowPer.Text = selectedEffectExtended.healPowerPercentIncrease.ToString();
			}
			else
			{
				isExtended = false;
				foreach (Control ctrl in grpPercent.Controls)
				{
					if (typeof(TextBox) == ctrl.GetType())
						ctrl.Text = "";
					else continue;
				}
			}
			handleEvent = true;
		}

		public CharacterEffect createNew()
		{
			CharacterEffect temp = new CharacterEffect();
			//Initialise all variables to default, return this
			temp.effectID = freeID[0];
			game.effectManager.effects.Add(freeID[0], temp);
			freeID.RemoveAt(0);
			return temp;
		}

		private void cmdAddEffect_Click(object sender, EventArgs e)
		{
			//Add new effect to list, select it, add new effect to game
			CharacterEffect temp = createNew();
			String[] items = new String[3];
			items[0] = "" + temp.effectID;
			items[1] = "";
			items[2] = "CharacterEffect";
			ListViewItem newItem = new ListViewItem(items);
			lstEffects.Items.Add(newItem);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			//Recreate effect sprite parser, save xml somewhere 
			DataLoader.KeyValueXML<int, Microsoft.Xna.Framework.Rectangle>[] newList
				= new DataLoader.KeyValueXML<int, Microsoft.Xna.Framework.Rectangle>[game.effectManager.effects.Count];
			DataLoader.EffectLoader[] fxList = new DataLoader.EffectLoader[game.effectManager.effects.Count];

			#region setData
			for (short i = 1; i <= game.effectManager.effects.Count; i++)
			{
				CharacterEffect temp = game.effectManager.effects[i];
				DataLoader.EffectLoader data = new DataLoader.EffectLoader();
				data.attackPowerIncrease = temp.attackPowerIncrease;
				data.consumable = temp.consumable;
				data.defenseIncrease = temp.defenseIncrease;
				data.description = temp.description;
				data.durationMS = temp.durationMS;
				data.effectID = temp.effectID;
				data.effectName = temp.name;
				data.evadeChanceIncrease = temp.evadeChanceIncrease;
				data.healPowerIncrease = temp.healPowerIncrease;
				data.hpIncrease = temp.hpIncrease;
				data.hpMaxIncrease = temp.hpMaxIncrease;
				data.hpRegenPer5increase = temp.hpRegenPer5increase;
				data.magicDefenseIncrease = temp.magicDefenseIncrease;
				data.magicPowerIncrease = temp.magicPowerIncrease;
				data.manaIncrease = temp.manaIncrease;
				data.manaMaxIncrease = temp.manaMaxIncrease;
				data.manaPer5Increase = temp.manaPer5Increase;
				data.maxStacks = temp.maxStacks;
				data.movementSpeedIncrease = temp.movementSpeedIncrease;
				data.permanent = temp.permanent;
				data.spriteSource = temp.spriteSource;
				data.stacks = temp.stacks;
				data.stacksPerUse = temp.stacksPerUse;
				data.tickSpeed = temp.tickSpeed;

				if (temp.GetType() == typeof(EffectExtended))
				{
					EffectExtended eTemp = (EffectExtended)temp;
					DataLoader.ExtendedEffectLoader eData = (DataLoader.ExtendedEffectLoader)data;
					eData.attackPowerPercentIncrease = eTemp.attackPowerPercentIncrease;
					eData.defensePercentIncrease = eTemp.defensePercentIncrease;
					eData.evadeChancePercentIncrease = eTemp.evadeChancePercentIncrease;
					eData.healPowerPercentIncrease = eTemp.healPowerPercentIncrease;
					eData.hpMaxPercentIncrease = eTemp.hpMaxPercentIncrease;
					eData.hpPercentIncrease = eTemp.hpPercentIncrease;
					eData.hpRegenPer5PercentIncrease = eTemp.hpRegenPer5PercentIncrease;
					eData.magicDefensePercentIncrease = eTemp.magicDefensePercentIncrease;
					eData.magicPowerPercentIncrease = eTemp.magicPowerPercentIncrease;
					eData.manaMaxPercentIncrease = eTemp.manaMaxPercentIncrease;
					eData.manaPer5PercentIncrease = eTemp.manaPer5PercentIncrease;
					eData.manaPercentIncrease = eTemp.manaPercentIncrease;

					fxList[i - 1] = eData;
				}
				else fxList[i - 1] = data;
				Microsoft.Xna.Framework.Rectangle kek = new Microsoft.Xna.Framework.Rectangle();
				kek.Width = 40;
				kek.Height = 40;
				kek.X = temp.spriteSource.X;
				kek.Y = temp.spriteSource.Y;
				DataLoader.KeyValueXML<int, Microsoft.Xna.Framework.Rectangle> lol = 
					new DataLoader.KeyValueXML<int, Microsoft.Xna.Framework.Rectangle>(i, kek);
				newList[i - 1].Key = i;
				newList[i - 1].Value = kek;
			}
#endregion

			//Serialise this data
			string fxPath = Declaration.effectLoaderPath;
			string spritePath = game.fullContentPath + "Object Databases\\Effects\\EffectSpriteParser.xml";

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			using (XmlWriter writer = XmlWriter.Create(spritePath, settings))
			{
				IntermediateSerializer.Serialize(writer, newList, null);
			}
			using (XmlWriter writer = XmlWriter.Create(fxPath, settings))
			{
				IntermediateSerializer.Serialize(writer, fxList, null);
			}
		}

		private void cmdRemoveEffect_Click(object sender, EventArgs e)
		{
			//Remove item from list, free id, remove from game
			if (selectedEffectID == -1)
				return;

			freeID.Insert(0, selectedEffect.effectID);
			lstEffects.Items.RemoveAt(selectedListIndex);
			game.effectManager.effects.Remove(selectedEffect.effectID);

			selectedEffect = null;
			selectedEffectID = -1;
		}

		private void onExtendedValueRemove()
		{
			//If no extended values are present, set the object to a normal effect object
			isExtended = true;
			foreach (Control ctrl in grpPercent.Controls)
			{
				if (!(ctrl.Text == "" || ctrl.Text == "0"))
				{
					isExtended = false;
					break;
				}
			}

			if (!isExtended)
			{
				//Change object to normal effect
				CharacterEffect temp = new CharacterEffect(selectedEffect);
				game.effectManager.effects[selectedEffect.effectID] = temp;
				lstEffects.Items[selectedListIndex].SubItems[2].Text = temp.GetType().Name;
			}
		}

		private void onExtendedValueAdd()
		{
			//Change effect to an extended object
			isExtended = true;

			if (typeof(EffectExtended) != selectedEffect.GetType())
			{
				//Change object
				EffectExtended temp = new EffectExtended(selectedEffect);
				game.effectManager.effects[selectedEffect.effectID] = temp;
				lstEffects.Items[selectedListIndex].SubItems[2].Text = temp.GetType().Name;
			}
		}

		#region "BaseEffectSettings"
		private void txtFXName_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				selectedEffect.name = txtFXName.Text;
				lstEffects.Items[selectedListIndex].SubItems[1].Text = selectedEffect.name;
			}
		}

		private void txtTickFreq_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.tickSpeed = Convert.ToInt16(txtTickFreq.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtTicks_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.ticks = Convert.ToInt16(txtTicks.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void chkConsumable_CheckedChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				selectedEffect.consumable = chkConsumable.Checked;
			}
		}

		private void chkStacks_CheckedChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				selectedEffect.stacks = chkStacks.Checked;
			}
		}

		private void chkPermanent_CheckedChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				selectedEffect.permanent = chkPermanent.Checked;
			}
		}

		private void txtFXDesc_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				selectedEffect.description = txtFXDesc.Text;
			}
		}

		private void txtFXDuration_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.durationMS = Convert.ToInt16(txtFXDuration.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtSpriteX_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.spriteSource.X = Convert.ToInt16(txtSpriteX.Text);
					updateSprite();
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtSpriteY_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.spriteSource.Y = Convert.ToInt16(txtSpriteY.Text);
					updateSprite();
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtMaxStacks_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.maxStacks = Convert.ToInt16(txtMaxStacks.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtStacksPerUse_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.stacksPerUse = Convert.ToInt16(txtStacksPerUse.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		#endregion

		#region "EffectAbsoluteSettings"
		private void txtHPAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.hpIncrease = Convert.ToInt16(txtHPAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtHPMaxAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.hpMaxIncrease = Convert.ToInt16(txtHPMaxAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtHPRegenAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.hpRegenPer5increase = Convert.ToInt16(txtHPRegenAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtAtkPowAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.attackPowerIncrease = Convert.ToInt16(txtAtkPowAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtMagPowAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.magicPowerIncrease = Convert.ToInt16(txtMagPowAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtHealPowAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.healPowerIncrease = Convert.ToInt16(txtHealPowAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtManaAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.manaIncrease = Convert.ToInt16(txtManaAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtManaMaxAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.manaMaxIncrease = Convert.ToInt16(txtManaMaxAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtManaRegenAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.manaPer5Increase = Convert.ToInt16(txtManaRegenAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtMagDefAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.magicDefenseIncrease = Convert.ToInt16(txtMagDefAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtDefAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.defenseIncrease = Convert.ToInt16(txtDefAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtEvadeAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.evadeChanceIncrease = Convert.ToInt16(txtEvadeAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void txtMoveSpeedAbs_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					selectedEffect.movementSpeedIncrease = Convert.ToInt16(txtMoveSpeedAbs.Text);
				}
				catch (Exception ex)
				{

				}
			}
		}
		#endregion

		private void txtHPPer_TextChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1 || handleEvent == false)
				return;
			else
			{
				try
				{
					onExtendedValueAdd();
					EffectExtended temp = (EffectExtended)selectedEffect;
					temp.hpPercentIncrease = (float)Convert.ToDecimal(txtHPPer.Text);
					if (txtHPPer.Text == "" || txtHPPer.Text == "0")
						onExtendedValueRemove();
				}
				catch (Exception ex)
				{

				}
			}
		}

		private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (selectedEffectID == -1)
				return;

			selectedEffect.type = (damageType)cmbType.SelectedIndex;
		}


	}
}
