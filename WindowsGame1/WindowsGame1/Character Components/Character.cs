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
    public class Character : Collidable_Sprite 
    {
        public List<Abilities.AbilityBase> abilities; 
		public Inventory inventory;
        public List<enums.faction> factions;
        public string gameName; //Represents the name used by the game
        public bool invincible;
        public short level;
        public Stats stats;

        public Character(Point tilePos, float xAcc, float yAcc, string objName, Rectangle _localSpace ) 
                    : base( tilePos, xAcc, yAcc, objName, _localSpace )
        {
            ID = -1;
            animID = ".walkNorth";
            stats = new Stats();
        }

        public Character(ref Character temp)
        {
            animID = ".walkNorth";
            this.Offset = temp.Offset;
            this.tilePos = temp.tilePos;
            this.name = temp.name;
            this.localSpace = temp.localSpace;
            this.animID = temp.animID;
            this.FramePosXMax = temp.FramePosXMax;
            stats = new Stats();
            abilities = new List<Abilities.AbilityBase>();

        }

		//public void setLocalSpace( long currentTileX, long currentTileY, int Offset.X, int Offset.Y )
		//{
		//    this.Offset.X = Offset.X;
		//    this.Offset.Y = Offset.Y;
		//}

        public Character() { }

        public bool isHostile( List<enums.faction> queryFaction )
        {
            //Compare factions to determine if two entities are hostile
            return false;
        }

        public void setDirection(char Dir)
        {
            switch (Dir)
            {
                case 'n':
                    animID = ".walkNorth";
                    queueAdvance();
                    vel.Y = -speed;
                    break;
                case 'e':
                    animID = ".walkEast";
                    queueAdvance();
                    vel.X = speed;
                    break;
                case 's':
                    animID = ".walkSouth";
                    queueAdvance();
                    vel.Y = speed;
                    break;
                case 'w':
                    animID = ".walkWest";
                    queueAdvance();
                    vel.X = -speed;
                    break;
            }

        }
    }
}
