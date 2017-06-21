using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1.Render
{

    public class SpriteList
    {
		// list[animName][animID][framePos]
        public Dictionary<int, Dictionary<int, Rectangle[]>> list = new Dictionary<int, Dictionary<int, Rectangle[]>>();

        public Rectangle getFrame( int animName, int animID, byte pos )
        {
            return list[animID][animID][pos];
        }

        public void setFrames(ref Rectangle startRec, int sprW, int sprH, byte nColumns, int animName, int animID)
        {
            Rectangle[] frame = new Rectangle[nColumns];

                for (short i = 0; i < nColumns; i++)
                {

                    frame[i].X = (i * sprW) + startRec.X;
                    frame[i].Y = startRec.Y;
                    frame[i].Width = sprW;
                    frame[i].Height = sprH;
                }

			if (list.ContainsKey(animName))
			{
				list[animName].Add(animID, frame);
			}
			else
			{
				list.Add(animName, new Dictionary<int, Rectangle[]>());
				list[animName].Add(animID, frame);
			}
        }

        public void setFrames(ref Rectangle[] recs, int animName, int animID)
        {
			Rectangle[] frame = new Rectangle[recs.Length];
			int i = 0;
			foreach (Rectangle X in recs)
			{
				frame[i] = recs[i];
				i++;
			}

			if (list.ContainsKey(animName))
			{
				list[animName].Add(animID, frame);
			}
			else
			{
				list.Add(animName, new Dictionary<int, Rectangle[]>());
				list[animName].Add(animID, frame);
			}

        }

    }


	public class SpriteListSimple
	{
		// list[animName][animID][framePos]
		public Dictionary<int, Rectangle[]> list = new Dictionary<int, Rectangle[]>();

		public Rectangle getFrame(int animName, byte pos)
		{
			return list[animName][pos];
		}

		public void setFrames(ref Rectangle startRec, int sprW, int sprH, byte nColumns, int animName )
		{
			Rectangle[] frame = new Rectangle[nColumns];

			for (short i = 0; i < nColumns; i++)
			{

				frame[i].X = (i * sprW) + startRec.X;
				frame[i].Y = startRec.Y;
				frame[i].Width = sprW;
				frame[i].Height = sprH;
			}
			list.Add(animName, frame);
		}

		public void setFrames(ref Rectangle[] recs, int animName)
		{
			Rectangle[] frame = new Rectangle[recs.Length];
			int i = 0;
			foreach (Rectangle X in recs)
			{
				frame[i] = recs[i];
				i++;
			}

			list.Add(animName, frame);
		}

	}

}
