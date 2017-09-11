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
    public class Sprite : StaticSprite 
    {
        public float speed = 0.5f;
       // public float Offset.X;
       // public float Offset.Y;

        public byte FramePosX;
        public byte FramePosXMax;
        public int animID;
		public int animName;
        public bool advance;
        public bool isSolid;

		//Two functions for handling translation across sprite / static sprite and collidable sprite:
			//UpdatePosition() - Uses velocity
			//Translate(vector2 position) 
			//Each one moves the offset and tilePos
			//Local position calculated for each localEntity via camera origin - (One batch process instead of incrementing 
			// by a small amount for each function call, removes rounding errors that will occur over time)

        //Overriden by other sprites!
        public virtual bool handleCollision() //Returns true if the object has stopped
        {   
            return false;
        }

        public void setAnimation(int AnimName, int animID, byte noFrames)
        {
            this.animName = AnimName;
			this.animID = animID;
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

		//public void offsetPosition( short displacement )
		//{
		//    localSpace.X -= displacement;
		//    localSpace.Y -= displacement;
		//}

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

		//public bool UpdatePosition(short frames)
		//{
		//    Offset.X += vel.X * frames;
		//    Offset.Y += vel.Y * frames;

		//    return simplifyPosition();
		//}
       
    }
}
