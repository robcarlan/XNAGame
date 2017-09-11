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
    public class StaticSprite
    {
        //public long tilePosX { get; set; }
        //public long tilePosY { get; set; }
		public Point tilePos;
		public Vector2 Offset;
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

		//Returns true if position has changed
		/// <summary>
		/// Applies the velocity of an object
		/// </summary>
		/// <returns>True if the object has moved into another tile</returns>
		public virtual bool UpdatePosition()
		{
			Offset.X += vel.X;
			Offset.Y += vel.Y;

			return simplifyPosition();
		}

		/// <summary>
		/// Moves the entity by the specified translation. Does not move local position of the object
		/// </summary>
		/// <param name="translation"></param>
		public virtual bool TranslatePosition(Vector2 translation)
		{
			Offset += translation;
			return simplifyPosition();
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

		/// <summary>
		/// Converts offset into tilePos when it is <0 or >tileSize
		/// </summary>
		public bool simplifyPosition()
		{
			bool changed = false;

			if (Offset.X > Declaration.tileGameSize)
			{
				tilePos.X += (int)(Math.Floor(Offset.X / Declaration.tileGameSize));
				Offset.X = Offset.X % Declaration.tileGameSize;
				changed = true;
			}
			else
			{
				if (Offset.X < 0)
				{
					tilePos.X += (int)(Math.Floor(Offset.X / Declaration.tileGameSize));
					Offset.X = Declaration.tileGameSize - ((Offset.X*-1) % Declaration.tileGameSize); //convert offset from -ve to corresponding +ve
					changed = true;
				}
			}

			if (Offset.Y > Declaration.tileGameSize)
			{
				tilePos.Y += (int)(Math.Floor(Offset.Y / Declaration.tileGameSize));
				Offset.Y = Offset.Y % Declaration.tileGameSize;
				changed = true;
			}
			else
			{
				if (Offset.Y < 0)
				{
					tilePos.Y += (int)(Math.Floor(Offset.Y / Declaration.tileGameSize));
					Offset.Y = Declaration.tileGameSize - ((Offset.Y*-1) % Declaration.tileGameSize);
					changed = true;
				}
			}

			return changed;
		}
    }
}