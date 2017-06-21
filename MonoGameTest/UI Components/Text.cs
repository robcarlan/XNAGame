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

namespace WindowsGame1.UI_Components
{
    /// <summary>
    /// A class used to easily create a line of text
    /// </summary>
    public class Text
    {
        public const float ICON_TEXT_DISTANCE = 8f;

        public float outlineWidth; //If 0, not outlined
        public Vector2 position;
        Vector2 size;
        public int controlID;

        public string text;
        public Color colour;
        public Vector2 scale;
        public bool center;        //If centered, position is automatically determined given an area bound
        public bool isVisible;

        //public void recalculateArea();
        public void setID(int ID)
        {
            controlID = ID;
        }

        public void increasePosition(Point increase)
        {
            position.X += increase.X;
            position.Y += increase.Y;
        }

        public Text(String text, Color col, Vector2 origin, Vector2 scale, bool center, float outlineWidth, SpriteFont font)
        {
            this.text = text;
            colour = col;
            position = origin;
            this.outlineWidth = outlineWidth;
            this.center = center;
            this.scale = scale;
            isVisible = true;

            if (center)
            {
                size = font.MeasureString(text) * scale * 0.5f;
                position = origin - size;
            }
        }

        public Text(String _text, WindowIcon icon, Color col, Vector2 scale, SpriteFont font,
            float outlineWidth)
        {
            text = _text;
            size = font.MeasureString(text) * scale;
            Vector2 textMidPoint = size * 0.5f; //The middle x / y of the text, used for centering
            this.outlineWidth = outlineWidth;
            this.scale = scale;
            colour = col;
            isVisible = true;

            //Position text based on the setting defined in the icon
            switch (icon.textLoc)
            {
                case enums.textLocation.above:  //Use mid x
                    center = true;
                    position.Y = icon.absolutePosition.Y - ICON_TEXT_DISTANCE;
                    position.X = icon.absolutePosition.X + (icon.absoluteArea.Width * 0.5f) + textMidPoint.X;   
                        //Middle y of icon - half of text width
                    break;
                case enums.textLocation.below:  //Use mid x
                    center = true;
                    position.Y = icon.absolutePosition.Y + icon.absoluteArea.Height + ICON_TEXT_DISTANCE;
                    position.X = icon.absolutePosition.X + icon.absoluteArea.Width * 0.5f - textMidPoint.X;
                    break;
                case enums.textLocation.inside: //Use mid x and y
                    center = true;
                    position.X = icon.absolutePosition.X + (icon.absoluteArea.Width * 0.5f) + textMidPoint.X;
                    position.Y = icon.absolutePosition.Y + icon.absoluteArea.Height * 0.5f - textMidPoint.Y;
                    break;
                case enums.textLocation.left:   //use mid y
                    position.X = icon.absolutePosition.X - size.X - ICON_TEXT_DISTANCE;
                    position.Y = icon.absolutePosition.Y + icon.absoluteArea.Height * 0.5f - textMidPoint.Y;
                    break;
                case enums.textLocation.right:  //use mid y
                    position.X = icon.absolutePosition.X + icon.absoluteArea.Width + ICON_TEXT_DISTANCE;
                    position.Y = icon.absolutePosition.Y + icon.absoluteArea.Height * 0.5f - textMidPoint.Y;
                    break;
            }
        }

        public Text(string _text, Windows.WindowBar bar, Vector2 scale, Color col, SpriteFont font, float width, bool center)
        {
            text = _text;
            size = font.MeasureString(text) * scale;
            Vector2 textMidPoint = size * 0.5f; //The middle x / y of the text, used for centering
            outlineWidth = width;
            this.scale = scale;
            colour = col;
            isVisible = true;

            if (center)
            {
                position.X = bar.absoluteArea.X + 0.5f * bar.absoluteArea.Width - textMidPoint.X;
                position.Y = bar.absoluteArea.Y + 0.5f * bar.absoluteArea.Height - textMidPoint.Y;
            }
            else
            {
                position.X = bar.absoluteArea.X + 4;
                position.Y = bar.absoluteArea.Y + 0.5f * bar.absoluteArea.Height - textMidPoint.Y;
            }
        }


    }

    class MovingText
    {

    }

    class FadingText
    {
        float duration;
    }

    //Allignement, multiple lines
    class textBlock
    {

    }


    //Text within a windowBar
    class textHeader
    {

    }
}
