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

namespace DataLoader
{
    public class MapData
    {
        public List<EnitityObject> Characters;
        public List<LightObject> Lights;
        public List<DoodadObject> Doodads;
        public List<ParticleObject> Particles;
    }

    public class EnitityObject
    {
        public short ID;
        public Point relTilePos;
        public float zPos;
        public Vector2 offset;
    }

    public class LightObject
    {
        public short ID;
        public Point relTilePos;
        public float zPos;
        public Vector2 offset;
    }

    public class DoodadObject
    {
        public short ID;
        public Point relTilePos;
        public float zPos;
        public Vector2 offset;
    }

    public class ParticleObject
    {
        public short ID;
        public Point relTilePos;
        public float zPos;
        public Vector2 offset;
    }

    //Class for dynamic object which stores the objects state?
    public class DynamicObjectData : DoodadObject
    {
        //Enum state
    }

    //can be overloaded for other objects which require more customisation
}
