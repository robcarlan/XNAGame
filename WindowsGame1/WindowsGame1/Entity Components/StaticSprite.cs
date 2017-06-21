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
    public class StaticSprite
    {
        //public long tilePosX { get; set; }
        //public long tilePosY { get; set; }
		public Point tilePos;
        public short zPos { get; set; }
        public Rectangle localSpace = new Rectangle(0, 0, 0, 0);
        public Vector2 vel = new Vector2(0, 0);
        public Vector2 acc = new Vector2(0, 0);

        public string name;

        public StaticSprite(float xAcc, float yAcc, string objName, Rectangle _localSpace)
        {
            localSpace = _localSpace;

            acc.Y = yAcc;
            acc.X = xAcc;
            name = objName;
        }

        public StaticSprite()
        {
            name = "Null";
        }

        public void UpdatePosition(GameTime delta)
        {
        //    localpos.X += (vel.X / 1000);
        //    localpos.Y += (vel.Y / 1000);
        }

        public void setSpeed(float newX, float newY)
        {
            vel.X = newX;
            vel.Y = newY;
            return;
        }

        public void incrSpeed(float newX, float newY)
        {
            vel.X += newX;
            vel.Y += newY;
        }
    }
}