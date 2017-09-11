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


namespace WindowsGame1
{
    public class Doodad : Collidable_Sprite
    {
        /*
         * All physical static objects in the world
         * have a dynamic object which extends from this for 
         * things like doors
         */
        public bool floats; //Determines whether it can be walked under (If its z pos is greater than characters current)
        public bool canSeeThrough; //For LOS, such as windows
        public bool castsShadow; //Shadow is drawn at entities ypos;
		public bool isAttackable; //Indicates whether it can be destroyed

        public Doodad(Doodad temp)
            : base (temp)
        {
            floats = temp.floats;
            canSeeThrough = temp.canSeeThrough;
            castsShadow = temp.castsShadow;
			
        }

        public Doodad() { }

		public void onInteract(Character_Components.Character activator)
		{

		}
    }

    public class PointLight : BasicLight
    {
        public float radius;

		public bool oscillates;
		public float minRadius;
		public float maxRadius;
		public int periodMS;
		public int elapsedMS;
    }

    public class DirectionalLight : PointLight
    {
        public Vector3 Direction;
    }

	public enum flickerValues 
	{
		Quick = 100,
		Medium = 500,
		Long = 2000,
	}

	public class BasicLight
	{
		public string lightName;
		public Color lightColor;
		public Point tilePos;
		public float zPos;

		public Vector2 Offset;
		public float intensity;
		public short ID;
		public short baseID;

		//Controls ebbing etc.
		public bool flickers;
		public bool isFlickering = false;
		public float nextFlickerIn;
		public flickerValues flickerFreq;

		public Collidable_Sprite attachedEntity;
	}

}
