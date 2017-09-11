using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.Character_Components
{
    public class Stats
    {
        //Add a list which calculates effects done, and adds them to a floating text list per tick/application
        //For all stats attributes have a different colour ( for +ve and -Ve ) than damage
        public short level;
        
        public short attackPower;
        public short magicPower;
        public short healPower;
        public short defense;
        public short evadeChance;
        public short magicDefense;

        public short hpCurrent;
        public float hpMaxPercentIncr;
        public short hpMax;
        public short hpRegenPer5;

        public short manaCurrent;
        public short manaMax;
        public float manaMaxPercentIncr;
        public short manaPer5;

        public short movementSpeed;

        public short MSPerHP;
        public short MSPerMana;
        public short currentManaTimer;
        public short currentHealthTimer;

		public short dexterity;
		public short strength;
		public short intelligence;


        //Have current values to avoid recalculation, changed everytime an effect is applied / lost

		public Dictionary< int, CharacterEffect > effects;
		public Dictionary<int, short> effectStacks;
        public List<int> effectKillQueue;
        public List<floatingTextData> floatingTextList;
        public bool newMessages;

        public Stats()
        {
            effects = new Dictionary<int, CharacterEffect>();
            effectStacks = new Dictionary<int, short>();
            effectKillQueue = new List<int>();
            floatingTextList = new List<floatingTextData>();

            hpMaxPercentIncr = manaMaxPercentIncr = 1f;
            
            //Temp
            hpCurrent = 50;
            hpMax = 100;
            hpRegenPer5 = 10;
            manaPer5 = 5;
            manaCurrent = 70;
            manaMax = 250;

            calculateValues();
        }

        public void calculateValues()
        {
            //ms per x = total ms / x in total ms
            MSPerHP = (short)(Math.Max(5000f / hpRegenPer5, 1));
            MSPerMana = (short)Math.Max(5000f / manaPer5, 1);

            //Clamp values
            currentHealthTimer = (short)MathHelper.Clamp(currentHealthTimer, 0f, MSPerHP);
            currentManaTimer = (short)MathHelper.Clamp(currentManaTimer, 0, MSPerMana);

        }

        public void updateValues( short msPassed )
        {
            currentManaTimer += msPassed;

			//Fix this shit
			//while (currentManaTimer > MSPerMana)
			//{
			//    currentManaTimer -= MSPerMana;
			//    manaCurrent++;
			//    manaCurrent = (short)MathHelper.Clamp(manaCurrent, 0, getMaxMana());
			//}

            currentHealthTimer += msPassed;

			//while (currentHealthTimer > MSPerHP)
			//{
			//    currentHealthTimer -= MSPerHP;
			//    hpCurrent++;
			//    hpCurrent = (short)MathHelper.Clamp(hpCurrent, 0, getMaxHP());
			//}

            foreach (int effectID in effects.Keys)
            {
                effects[effectID].durationMS -= msPassed;
                effects[effectID].tickTimer += msPassed;
                if (effects[effectID].tickTimer >= effects[effectID].tickSpeed)
                {
                    //Tick has occured
                    effects[effectID].tickTimer -= effects[effectID].tickSpeed;
                    tickEffect(effectID);
                }

                //Test to see if the effect hsa passed its duration

                if (effects[effectID].durationMS <= 0)
                {
                    effectKillQueue.Add(effectID);
                }
            }

            for (int i = 0; i < effectKillQueue.Count; i++)
            {
				if (effects[effectKillQueue[i]].stacks)
					discardEffect( effectKillQueue[i], effectStacks[effectKillQueue[i]]);
				else discardEffect(effectKillQueue[i], 0);
                effectKillQueue.RemoveAt(i);
            }
        }

		public short getQuantity(int ID)
		{
			if (effectStacks.ContainsKey(ID))
				return effectStacks[ID];
			else return 0;
		}

        public int getHP()
        {
            return (int)(hpCurrent);
        }

        public int getMaxHP()
        {
            return (int)(hpMax * hpMaxPercentIncr);
        }

        public int getMana()
        {
            return (int)(manaCurrent);
        }

        public int getMaxMana()
        {
            return (int)(manaMax * manaMaxPercentIncr);
        }

        public void applyEffect(CharacterEffect _effect)
        {
            CharacterEffect effect = new CharacterEffect(_effect);
			short effectMultiplier = 1;

			if (effects.ContainsKey(effect.effectID) && effect.stacks == true)
			{
				//Add to the stacks quantity
				effectMultiplier = (short)(MathHelper.Clamp(effect.stacksPerUse, 1, effect.maxStacks - effectStacks[effect.effectID]));
				effectStacks[effect.effectID] += effectMultiplier;
			}
			else if (effects.ContainsKey(effect.effectID) && effect.stacks == false)
			{
				//Reset duration
				effects[effect.effectID].durationMS = effect.durationMS;
				return;
			}
			else
			{
				//Effect doesn't already exist
				//Include two types of permanent
				if (!effect.consumable)
					effects.Add(effect.effectID, effect);

				if (effect.stacks == true)
					effectStacks.Add(effect.effectID, effect.maxStacks);
				else
				{
					//Tick directly and return
					tickEffect(effect);
					return;
				}
			}

            tickEffect(effect.effectID);

            calculateValues();

            //Update message list
            floatingTextList.Add( new floatingTextData( "Gained Effect: " + effect.name, enums.textType.effectGain ));
            newMessages = true;
       
        }

        public string getTimeConcise(int effectID)
        {
            string text = "";
            if (effects[effectID].durationMS <= 0)
            {
                //Effect is not timed
            }
            if (effects[effectID].durationMS <= 60000)
            {
                //No minutes, just get seconds
                text = (effects[effectID].durationMS) / 1000 + "s";
            }
            else
                text = (effects[effectID].durationMS) / 60000 + "m";

            return text;

        }

        public string getTimeFull(short effectID)
        {
            string text = "";

            return text;
        }


        private void tickEffect(int effectID)
        {
            effects[effectID].ticks++;
            tickEffect(effects[effectID]);
        }

        private void tickEffect(CharacterEffect effect)
        {

			hpCurrent += (short)(effect.hpIncrease);
			if (hpCurrent <= 0)
			{
				//Character killed!
				hpCurrent -= (short)(effect.hpIncrease);
				effects[effect.effectID].durationMS = -1;
				return;
			}
			hpCurrent = (short)MathHelper.Clamp(hpCurrent, -1f, hpMax);

            hpMax += (short)(effect.hpMaxIncrease);
            if (hpMax <= 0)
            {
                //Undo effect
                hpMax -= (short)(effect.hpMaxIncrease);
                effects[effect.effectID].durationMS = -1;
                //Character should probably die here
                return;
            }
            manaMax += (short)(effect.manaMaxIncrease);
            if (manaMax <= 0)
            {
                //Undo effect
                manaMax -= (short)(effect.manaMaxIncrease);
                effects[effect.effectID].durationMS = -1;
                return;
            }

            attackPower += (short)(effect.attackPowerIncrease);
            magicPower += (short)(effect.magicPowerIncrease);
            healPower += (short)(effect.healPowerIncrease);
            defense += (short)(effect.defenseIncrease);
            evadeChance += (short)(effect.evadeChanceIncrease);
            magicDefense += (short)(effect.magicDefenseIncrease);

            hpRegenPer5 += (short)(effect.hpRegenPer5increase);

            manaCurrent += (short)(effect.manaIncrease);
            manaCurrent = (short)MathHelper.Clamp(manaCurrent, 1f, manaMax);
            manaPer5 += (short)(effect.manaPer5Increase);

            //Add floating text
            if (effect.hpIncrease != 0)
            {
                if (effect.hpIncrease > 0)
                    floatingTextList.Add(new floatingTextData("+" + effect.hpIncrease, enums.textType.healing));
                else floatingTextList.Add(new floatingTextData(effect.hpIncrease.ToString(), enums.textType.damage));
                newMessages = true;
            }
            if (effect.manaIncrease != 0)
            {
                floatingTextList.Add(new floatingTextData(effect.hpIncrease.ToString() + " mana", enums.textType.mana));
                newMessages = true;
            }

            movementSpeed += (short)(effect.movementSpeedIncrease);
        }

        public void applyEffect(EffectExtended _effect)
        {
            applyEffect((CharacterEffect)_effect);

            hpMaxPercentIncr += _effect.hpMaxPercentIncrease;
            manaMaxPercentIncr += _effect.manaMaxPercentIncrease;
        }

        public void removeEffect(int effectID)
        {
			discardEffect(effectID, effects[effectID].maxStacks);
        }

        public void removeEffect(List<int> effectID)
        {
			for (short i = 0; i < effectID.Count; i++)
			{
				discardEffect(effectID[i], effects[i].maxStacks);
			}

        }

		public void removeEffect(short effectID, short quantity)
        {
			discardEffect(effectID, quantity);
        }

		public bool hasEffect(short effectID)
		{
			if (effects.ContainsKey(effectID))
				return true;
			else return false;
		}

		private void discardEffect(int effectID, short quantity)
		{

            undoValues(effects[effectID]);
            newMessages = true;

            if (effects[effectID].stacks)
            {
                effectStacks[effectID] -= (quantity);

                if (effectStacks[effectID] <= 0)
                {
                    floatingTextList.Add( new floatingTextData("Lost Effect: " + effects[effectID].name, enums.textType.effectLost ));
                    effects.Remove(effectID);
                    effectStacks.Remove(effectID);
                }
                else
                    floatingTextList.Add(new floatingTextData(" -1 stack of " + effects[effectID].name, enums.textType.effectLost));
            }
            else
            {
                floatingTextList.Add(new floatingTextData("Lost Effect: " + effects[effectID].name, enums.textType.effectLost));
                effects.Remove(effectID);
            }
		}

        private void undoValues(CharacterEffect effect)
        {
            effect.ticks = effects[effect.effectID].ticks ;
			attackPower -= (short) (effect.attackPowerIncrease * effect.ticks);
			magicPower -= (short) (effect.magicPowerIncrease * effect.ticks);
			healPower -= (short) (effect.healPowerIncrease * effect.ticks);
			defense -= (short) (effect.defenseIncrease * effect.ticks);
			evadeChance -= (short) (effect.evadeChanceIncrease * effect.ticks);
			magicDefense -= (short) (effect.magicDefenseIncrease * effect.ticks);

			hpMax -= (short) (effect.hpMaxIncrease * effect.ticks);
			hpRegenPer5 -= (short) (effect.hpRegenPer5increase * effect.ticks);

			manaMax -= (short) (effect.manaMaxIncrease * effect.ticks);
			manaPer5 -= (short) (effect.manaPer5Increase * effect.ticks);

			movementSpeed -= (short) (effect.movementSpeedIncrease * effect.ticks);

        }

        public List<floatingTextData> takeMessages()
        {
            List<floatingTextData> tempList = new List<floatingTextData>(floatingTextList);
            floatingTextList = new List<floatingTextData>();

            newMessages = false;
            return tempList;
        }
    }
}
