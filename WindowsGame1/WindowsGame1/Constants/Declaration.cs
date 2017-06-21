using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
	class Declaration
	{

		//UI
		public const string ItemParserFolder = "Object Databases\\Items\\";
		public const string ArmourLoaderFile = "ArmourLoader.xml";
		public const string ConsumableItemLoaderFile = "ConsumableItemLoader.xml";
		public const string ItemLoaderFile = "ItemParser.xml";
		public const string WeaponLoaderFile = "WeaponLoader.xml";

		public const string SpriteContentFolder = "Sprite Content\\";
		public const string itemSpriteFilename = "Items.png";
		public const string consumableItemSpriteFilename = "Consumables.png";
		public const string armourItemFilename = "Armour.png";
		public const string weaponItemFilename = "Weapons.png";
		public const string UITextureFilename = "UITex.png";

        enum faction { imperial, none, enemy };

        //Globals
        public const float Scale = 2.5f;
        public const short tileLength = 16;
        public const short tileGameSize = (int)(Scale * tileLength);

        //Player specifics
        public const int playerSpeed = 4;

        //Abilities
        public const float AbilityMaxRange = 600;

	}
}
