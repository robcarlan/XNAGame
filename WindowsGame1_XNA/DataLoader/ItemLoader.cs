using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DataLoader
{
    public class ItemLoader
    {
        public Point itemSpriteSource;

        public short ID;
        public string itemName;

        public int levelRequirement;
        public int itemValue;
        public bool tradeable;

        public string description;
    }

    public class ConsumableItemLoader : ItemLoader
    {
		public short nextState;
        public bool unlimited;
        public short cooldownSeconds;
        public List<String> useEffect;
    }

    public class EquipmentItemLoader : ItemLoader
    {
        //Slot
        public List<String> equipEffect;
        public List<String> hitEffect;
    }

    public class ArmourItemLoader : EquipmentItemLoader
    {
        //Damages, speed
    }

    public class WeaponItemLoader : EquipmentItemLoader
    {
        //Defenses
    }


}
