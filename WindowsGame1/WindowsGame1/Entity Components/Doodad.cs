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

        public Doodad(Doodad temp)
            : base (temp)
        {
            floats = temp.floats;
            canSeeThrough = temp.canSeeThrough;
            castsShadow = temp.castsShadow;
        }

        public Doodad() { }
    }

    public class AmbientLight : Doodad
    {
        public Color lightColor;
        public float intensity;

    }

    public class PointLight : AmbientLight
    {
        public float radius;
        public Vector3 pos;

    }

    public class DirectionalLight : PointLight
    {
        public Vector3 Direction;
        public float lightDropOff;
    }
}
