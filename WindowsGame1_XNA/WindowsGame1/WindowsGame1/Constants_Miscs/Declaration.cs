using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
	public class Declaration
	{

		//UI

		//Script prefixes;
		public const string onCollide = "onCollide_";
		public const string projectilePrefix = "p_";

		//Data files
		public const string SpriteContentFolder = "Sprite Content\\";
		public const string itemSpriteFilename = "Items.png";
		public const string consumableItemSpriteFilename = "Consumables.png";
		public const string armourItemFilename = "Armour.png";
		public const string weaponItemFilename = "Weapons.png";
		public const string UITextureFilename = "UITex.png";

		public const string contentDirectory = "C:\\Content\\";
		public const string tileDataFolder = contentDirectory + "World\\Tiles\\";
		public const string contentDataFolder = contentDirectory + "World\\CellData\\";
		public const string navmeshDataFolder = contentDirectory + "Navmesh\\";
		public const string itemDataFolder = contentDirectory + "Items\\";
		public const string scriptsFolder = contentDirectory + "Scripts\\";

			//Items
		public const string itemLoaderPath = itemDataFolder + "ItemParser";
		public const string ArmourLoaderPath = itemDataFolder + "ArmourLoader";
		public const string WeaponLoaderPath = itemDataFolder + "WeaponLoader";
		public const string ConsumableItemLoaderPath = itemDataFolder + "ConsumableItemLoader";
			//ObjManager
		public const string lightLoaderPath = contentDirectory  + "LightLibrary";
		public const string projectileLoaderPath = contentDirectory + "Projectiles";
		public const string particleLoaderPath = contentDirectory + "Particles";
		public const string particleSpriteLoaderPath = contentDirectory + "ParticleSprites";
		public const string projectileSpriteLoaderPath = contentDirectory + "ProjectileSprites";
		public const string characterSpriteLoaderPath = contentDirectory + "CharacterSprites";

		public const string effectLoaderPath = contentDirectory + "Effects\\EffectLoader";

        //Globals
        public const float Scale = 2.5f;
        public const short tileLength = 16;
        public const short tileGameSize = (int)(Scale * tileLength);
		public const float drawRecSize = 10.0f;
		public const float drawCircleDiameter = 10.0f;
		public static float verticalScale = (float)(1/Math.Cos(60));

        //Player specifics
        public const float playerBaseSpeed = 8.0f;

        //Abilities
        public const float AbilityMaxRange = 2000;
		public const short projZPos = 50;

		//Textures
		public const string DEBUG_TEX = "debug";

		//Tile Manager
		public const int tileNoID = -1;
	}
}
