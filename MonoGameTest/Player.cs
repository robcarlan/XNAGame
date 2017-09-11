using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DataLoader;

namespace WindowsGame1
{
    public class Player
    {
		//public Lua lua;

        public int tilePosYIncreased = new int();
        public int tilePosXIncreased = new int();
        //public long playerTilePosX, playerTilePosY = new long();   //Loaded from save file
		public Point playerTilePos;
        public float tileOffsetX, tileOffsetY = new float();
        public Point currentCell;

        public float tileLength = new float();
        public float tileScale = new byte();
        public Character_Components.Character Hero; //Make a hero class
        public Dictionary<short, Item> heldItems = new Dictionary< short, Item>();
        
        //Load data from file
        public Player(Rectangle LocalSpace, ObjManager obj)
		{
            //These values should be loaded from a save file!
            playerTilePos = new Point(0,0);
            Hero = new Character_Components.Character(new Point(6, -1), 0, 0, "chara01_a", LocalSpace, obj.navmesh);
            Hero.speed = Declaration.playerBaseSpeed;
			Hero.inventory = new Character_Components.Inventory(20);
        }

		public void loadPlayer(DataLoader.PlayerLoader data)
		{
			Hero.tilePos.X = data.tileX;
			Hero.tilePos.Y = data.tileY;
			Hero.Offset.X = data.offsetX;
			Hero.Offset.Y = data.offsetY;
			Hero.inventory.contents = data.inventory;
			Hero.inventory.quantity = data.quantity;
		}

		public void setupLua()
		{
			//Registers functions so they can be used in scripts
		}

        public void UpdateCharacter()
        {
			tileOffsetX = Hero.Offset.X;
			tileOffsetY = Hero.Offset.Y;
			playerTilePos = Hero.tilePos;
			//tileOffsetX += Hero.vel.X;
			//tileOffsetY += Hero.vel.Y;

			//if (tileOffsetX > tileLength)
			//{
			//    tileOffsetX -= tileLength;
			//    playerTilePos.X++;
			//    tilePosXIncreased = 1;
			//}
			//else if (tileOffsetX < 0)
			//{
			//    tileOffsetX += tileLength;
			//    playerTilePos.X--;
			//    tilePosXIncreased = -1;
			//}
			//if (tileOffsetY > tileLength)
			//{
			//    tileOffsetY -= tileLength;
			//    playerTilePos.Y++;
			//    tilePosYIncreased = 1;
			//}
			//else if (tileOffsetY < 0)
			//{
			//    tileOffsetY += tileLength;
			//    playerTilePos.Y--;
			//    tilePosYIncreased = -1;
			//}
        }

        public bool giveItemToPlayer(short itemToGive, int quantity)
        {
			return Hero.inventory.giveItem(itemToGive, quantity);
        }
    }
}
