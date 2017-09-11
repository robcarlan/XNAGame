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

namespace DataLoader
{
    //Split to abstract class effect base (which contains the basic control variables)
    //-->CharacterEffect 
    //-->Extended Effect
    //-->Damage Effect
	[Serializable]
	public struct KeyValueXML<K, V>
	{
		public K Key;
		public V Value;

		public KeyValueXML (K _key, V _value)
		{
			Key = _key;
			Value = _value;
		}
	}

    abstract public class BaseEffectLoader
    {
        public string effectName;
        public short effectID;
        public Point spriteSource;
        public bool permanent;
        public bool stacks;
        public bool consumable;
        public short maxStacks;
        public short stacksPerUse;
        public int durationMS;
    }

    public class EffectLoader
    {
        public string effectName;
        public short effectID;
		public string description;
		public Point spriteSource;
        public bool permanent;
        public bool stacks;
        public bool consumable;
        public short maxStacks;
		public short stacksPerUse;
        public int durationMS;
        public int tickSpeed;

        public short attackPowerIncrease;
        public short magicPowerIncrease;
        public short healPowerIncrease;
        public short defenseIncrease;
        public short evadeChanceIncrease;
        public short magicDefenseIncrease;

        public short hpIncrease;
        public short hpRegenPer5increase;

        public short manaIncrease;
        public short manaPer5Increase;

        public short hpMaxIncrease;
        public short manaMaxIncrease;

        public short movementSpeedIncrease;
        //Or split effects into sub classses so variable created only if needed.
        //Variables for lifetime of sprite
        //type of effect, hit chances
    }

    public class ExtendedEffectLoader : EffectLoader
    {
        public float attackPowerPercentIncrease;
        public float magicPowerPercentIncrease;
        public float healPowerPercentIncrease;
        public float defensePercentIncrease;
        public float evadeChancePercentIncrease;
        public float magicDefensePercentIncrease;

        public float hpPercentIncrease;
        public float hpRegenPer5PercentIncrease;

        public float manaPercentIncrease;
        public float manaPer5PercentIncrease;

        public short hpMaxPercentIncrease;
        public short manaMaxPercentIncrease;
    }
}
