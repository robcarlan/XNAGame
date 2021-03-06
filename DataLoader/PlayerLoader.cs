﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DataLoader
{
	public class PlayerLoader : IProxyObject 
	{
		//Data format of XML File.
		//Contains Stats/Items/Location/Quests info 
		//Quest details
		//	-Completed / stage
		//Store location via ID of where the player last slept
		public int tileX;
		public int tileY;
		public float offsetX;
		public float offsetY;

		//Equipped Items

		//Inventory
		public List<short> inventory;
		public List<int> quantity;

		//Primary Stats
		
		public PlayerLoader(Point Tile, Vector2 Offset, 
			List <short> inventory, List<int> quantity)
		{
			tileX = Tile.X;
			tileY = Tile.Y;
			offsetX = Offset.X;
			offsetY = Offset.Y;
			this.inventory = inventory;
			this.quantity = quantity;
		}

		public PlayerLoader()
		{

		}
	}
}
