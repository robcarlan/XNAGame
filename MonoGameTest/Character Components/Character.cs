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

namespace WindowsGame1.Character_Components
{
	//Character cannot be attacked, does not have skills or levels. Has basic goal planning and full dialogue
	//NPC is an extended character, and has combat goalplanning and stats. 
    public class Character : Collidable_Sprite 
    {
		//The hero is also this class (unless a hero class is needed
        public List<Abilities.AbilityBase> abilities; 
		public Inventory inventory;
        public List<enums.faction> factions;
		public List<enums.characterType> Type;
        public string gameName; //Represents the name used by the game
        public bool invincible;
        public Stats stats;
		public bool unique;
		public Brain brain;
		public int gold;

        public Character(Point tilePos, float xAcc, float yAcc, string objName, Rectangle _localSpace, Utility.Navmesh mesh ) 
                    : base( tilePos, xAcc, yAcc, objName, _localSpace )
        {
            ID = -1;
			animID = AnimID.walkNorth;
            stats = new Stats();
			brain = new Brain(this, mesh);
        }

        public Character(ref Character temp)
        {
			animID = AnimID.walkNorth;
            this.Offset = temp.Offset;
            this.tilePos = temp.tilePos;
            this.name = temp.name;
            this.localSpace = temp.localSpace;
            this.animID = temp.animID;
            this.FramePosXMax = temp.FramePosXMax;
            stats = new Stats();
            abilities = new List<Abilities.AbilityBase>();

        }

        public Character() { }

		//Event Handlers


		//Abstraction functions
		//i.e. getName, applyEffect, castAbility, isEnemy(Character query)

		//Called once per whenever
		public void updateAI()
		{

		}

        public bool isHostile( List<enums.faction> queryFaction )
        {
            //Compare factions to determine if two entities are hostile
            return false;
        }

		public override bool TranslatePosition(Vector2 translation)
		{
			brain.onTranslatation(translation);
			return base.TranslatePosition(translation);
		}

        public void setDirection(char Dir)
        {
            switch (Dir)
            {
                case 'n':
                    animID = AnimID.walkNorth;
                    queueAdvance();
                    vel.Y = -speed;
                    break;
                case 'e':
                    animID = AnimID.walkEast;
                    queueAdvance();
                    vel.X = speed;
                    break;
                case 's':
                    animID = AnimID.walkSouth;
                    queueAdvance();
                    vel.Y = speed;
                    break;
                case 'w':
                    animID = AnimID.walkWest;
                    queueAdvance();
                    vel.X = -speed;
                    break;
            }

        }
    }
}
