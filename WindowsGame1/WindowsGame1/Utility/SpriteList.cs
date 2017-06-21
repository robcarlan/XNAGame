using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{

    public class SpriteList
    {
        public Dictionary<string, Rectangle[]> list = new Dictionary<string, Rectangle[]>();

        public Rectangle getFrame( string key, byte pos )
        {
            return list[key][pos];
        }

        public void setFrames(ref Rectangle startRec, int sprW, int sprH, byte nColumns, string key)
        {
            Rectangle[] frame = new Rectangle[nColumns];

                for (short i = 0; i < nColumns; i++)
                {

                    frame[i].X = (i * sprW) + startRec.X;
                    frame[i].Y = startRec.Y;
                    frame[i].Width = sprW;
                    frame[i].Height = sprH;
                }
            list.Add(key, frame);

        }

        public void setFrames(ref Rectangle[] recs, string key)
        {
            ArrayList frame = new ArrayList();
            int i = 0;
            foreach (Rectangle X in recs)
            {
                frame.Add(recs[i]);
                i++;
            }
            list.Add(key, frame.Cast<Rectangle>().ToArray<Rectangle>());        

        }

    }
}
