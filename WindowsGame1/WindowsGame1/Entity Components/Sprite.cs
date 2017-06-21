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
    public class Sprite : StaticSprite 
    {
        public float speed = 1;
       // public float Offset.X;
       // public float Offset.Y;
		public Vector2 Offset;

        public byte FramePosX;
        public byte FramePosXMax;
        public string animID;
        public bool advance;
        public bool isSolid;

        //Overriden by other sprites!
        public virtual bool handleCollision() //Returns true if the object has stopped
        {   
            return false;
        }

        public void setAnimation(string AnimName, byte noFrames)
        {
            animID = AnimName;
            FramePosXMax = noFrames;
        }
        public void queueAdvance()
        {
            advance = true;
        }
        public void frameAdvance()
        {
            if (!advance) return;
            if (FramePosX < FramePosXMax)
            {
                FramePosX++;
                advance = false;
            }
            else FramePosX = 0;
        }

		public void offsetPosition( short displacement )
		{
			localSpace.X -= displacement;
			localSpace.Y -= displacement;
		}

        public Sprite ( Point _tilePos ,float xAcc, float yAcc, string objName, Rectangle _localSpace)
        {
			tilePos = _tilePos;
            acc.Y = yAcc;
            acc.X = xAcc;
            name = objName;
            FramePosXMax = 0;
            localSpace = _localSpace;
        }

		//protected void getGlobalPosition(char X, char Y)
		//{
		//    tilePosX += Functions.toGlobalValue(X);
		//    tilePosY += Functions.toGlobalValue(Y);
		//}


        public Sprite()
        {
            name = "Null";
        }

        public Sprite( Sprite temp )
        {
            setValues(temp);
        }

        public Sprite(Sprite copy, Point tilePos)
        {
            setValues(copy);
            this.tilePos = tilePos;
            
        }

        public void setValues(Sprite temp)
        {
            this.Offset = temp.Offset;
            this.tilePos = temp.tilePos;
            this.name = temp.name;
            this.localSpace = temp.localSpace;
            this.animID = temp.animID;
            this.FramePosXMax = temp.FramePosXMax;
        }

        //Returns true if position has changed
        public bool UpdatePosition()
        {
            bool changed = new bool();
            Offset.X += vel.X;
            Offset.Y += vel.Y;

            while (Offset.X > Declaration.tileGameSize)
            {
                Offset.X -= Declaration.tileGameSize;
                tilePos.X++;
                changed = true;
            }
            while (Offset.X < 0)
            {
                Offset.X += Declaration.tileGameSize;
                tilePos.X--;
                changed = true;
            }
            while (Offset.Y > Declaration.tileGameSize)
            {
                Offset.Y -= Declaration.tileGameSize;
                tilePos.Y++;
                changed = true;
            }
            while (Offset.Y < 0)
            {
                Offset.Y += Declaration.tileGameSize;
                tilePos.Y--;
                changed = true;
            }
            return changed;
        }

        public bool UpdatePosition(short frames)
        {
            bool changed = new bool();
            Offset.X += vel.X * frames;
            Offset.Y += vel.Y * frames;

            while (Offset.X > Declaration.tileGameSize)
            {
                Offset.X -= Declaration.tileGameSize;
                tilePos.X++;
                changed = true;
            }
            while (Offset.X < 0)
            {
                Offset.X += Declaration.tileGameSize;
                tilePos.X--;
                changed = true;
            }
            while (Offset.Y > Declaration.tileGameSize)
            {
                Offset.Y -= Declaration.tileGameSize;
                tilePos.Y++;
                changed = true;
            }
            while (Offset.Y < 0)
            {
                Offset.Y += Declaration.tileGameSize;
                tilePos.Y--;
                changed = true;
            }
            return changed;
        }
       
    }
}
