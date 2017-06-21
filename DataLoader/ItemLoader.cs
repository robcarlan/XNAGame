using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DataLoader
{
	public class ItemLoader : IProxyObject
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
