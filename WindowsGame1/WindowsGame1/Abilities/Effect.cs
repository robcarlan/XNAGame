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

namespace WindowsGame1
{
    public enum damageType { Magic, Fire, Physical };

    public class CharacterEffect
    {

        //For floating text, could have a description of the effect instead of the effect name 

        //Create a class for damaging effects
        public short effectID;
        public string name;
		public string description;
		public Point spriteSource;
        //If permanent, the effect cannot be undone (values remain on the character)
        public bool permanent;
        public bool stacks;
        public bool consumable; //For things like potions (effect is not stored by character)
		public short stacksPerUse;
        public short maxStacks;

        //The effects duration in milliseconds
        public int durationMS;
        //How often per tick
        public int tickSpeed;
        public int tickTimer;
        public int ticks;
     
        //Absolute
        public short attackPowerIncrease;
        public short magicPowerIncrease;
        public short healPowerIncrease;
        public short defenseIncrease;
        public short evadeChanceIncrease;
        public short magicDefenseIncrease;


        //Absolute
        public short hpIncrease;
        public short hpRegenPer5increase;

        public short manaIncrease;
        public short manaPer5Increase;

        public short hpMaxIncrease;
        public short manaMaxIncrease;

        public short movementSpeedIncrease;

        public CharacterEffect(CharacterEffect copy)
        {
            effectID = copy.effectID;
            name = copy.name;
			description = copy.description;
		    spriteSource = copy.spriteSource;
            permanent = copy.permanent;
            stacks = copy.stacks;
            consumable = copy.consumable; 
		    stacksPerUse = copy.stacksPerUse;
            maxStacks = copy.maxStacks;
            durationMS = copy.durationMS;
            tickSpeed = copy.tickSpeed;
            tickTimer= copy.tickTimer;
            ticks = 0;
            attackPowerIncrease = copy.attackPowerIncrease;
            magicPowerIncrease = copy.magicPowerIncrease;
            healPowerIncrease = copy.healPowerIncrease;
            defenseIncrease = copy.defenseIncrease;
            evadeChanceIncrease = copy.evadeChanceIncrease;
            magicDefenseIncrease = copy.magicDefenseIncrease;
            hpIncrease = copy.hpIncrease;
            hpRegenPer5increase = copy.hpRegenPer5increase;
            manaIncrease = copy.manaIncrease;
            manaPer5Increase = copy.manaPer5Increase;
            hpMaxIncrease = copy.hpMaxIncrease;
            manaMaxIncrease = copy.manaMaxIncrease;
            movementSpeedIncrease = copy.movementSpeedIncrease;
        }

        public CharacterEffect() { }

    }

    public class EffectExtended : CharacterEffect
    {
        //Gives percentage values

        //Percent
        public float attackPowerPercentIncrease;
        public float magicPowerPercentIncrease;
        public float healPowerPercentIncrease;
        public float defensePercentIncrease;
        public float evadeChancePercentIncrease;
        public float magicDefensePercentIncrease;

        //Percent
        public float hpPercentIncrease;
        public float hpRegenPer5PercentIncrease;

        public float manaPercentIncrease;
        public float manaPer5PercentIncrease;

        public short hpMaxPercentIncrease;
        public short manaMaxPercentIncrease;

        public EffectExtended(EffectExtended copy)
            : base(copy)
        {

        attackPowerPercentIncrease = copy.attackPowerPercentIncrease;
        magicPowerPercentIncrease = copy.magicPowerPercentIncrease;
        healPowerPercentIncrease = copy.healPowerPercentIncrease;
        defensePercentIncrease = copy.defensePercentIncrease;
        evadeChancePercentIncrease = copy.evadeChancePercentIncrease;
        magicDefensePercentIncrease = copy.magicDefensePercentIncrease;

        hpPercentIncrease = copy.hpPercentIncrease;
        hpRegenPer5PercentIncrease = copy.hpRegenPer5PercentIncrease;

        manaPercentIncrease = copy.manaPercentIncrease;
        manaPer5PercentIncrease = copy.manaPer5PercentIncrease;

        hpMaxPercentIncrease = copy.hpMaxPercentIncrease;
        manaMaxPercentIncrease = copy.manaMaxPercentIncrease;

        }

        public EffectExtended() { }
    }

    public class DamageEffect : CharacterEffect
    {
        public short tickMS;

        public short damagePerTick;
        public List<damageType> type;
    }
}
