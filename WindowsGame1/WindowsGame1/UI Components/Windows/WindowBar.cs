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

namespace WindowsGame1.UI_Components.Windows
{
    public class WindowBar
    {
        public const int spriteWidth = 16;

        public Vector2 scale;
        public Vector2 barL, barM, barR;
        public Rectangle absoluteArea;
        public Vector2 barMScale;
        public string spriteSuffix;

        //Text
        public bool hasText;
        public Text text;

        public WindowBar( Vector2 origin, Vector2 area, string spriteName, Vector2 scale)
        {
            hasText = false;
            this.scale = scale;
            spriteSuffix = spriteName;

            barL = origin;
            barM.Y = barR.Y = origin.Y;
            barM.X = barL.X + spriteWidth * scale.X;
            barR.X = origin.X + area.X - spriteWidth * scale.X;

            barMScale.X = (barR.X - barL.X + spriteWidth * scale.X ) / spriteWidth;
            barMScale.Y = scale.Y;

            absoluteArea.X = (int)barL.X;
            absoluteArea.Y = (int)barL.Y;

            absoluteArea.Width = (int)area.X;
            absoluteArea.Height = (int)area.Y;

        }

        public void incrementPositions(Point movement)
        {
            absoluteArea.X += movement.X;
            absoluteArea.Y += movement.Y;
            barL.X += movement.X;
            barL.Y += movement.Y;
            barM.X += movement.X;
            barM.Y += movement.Y;
            barR.X += movement.X;
            barR.Y += movement.Y;
        }

        public void addText(Text _text)
        {
            text = _text;
            hasText = true;
        }

    }
}
