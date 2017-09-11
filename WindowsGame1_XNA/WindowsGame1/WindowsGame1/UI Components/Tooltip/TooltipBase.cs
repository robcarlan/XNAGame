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
    /// <summary>
    /// this provides the base tooltip, with text, but the class is derived with more specific tooltips,
    /// ie a tooltip for abilities, which contains the necessary information to build
    /// </summary>
    public class TooltipBase
    {
        public const byte horizontalTextOffset = 14;
        public const byte verticalTextOffset = 9;
        public const byte maxLetters = 40;
        public const byte textGap = 18;
        public const float fontScale = 0.7f;
        public const float tooltipScale = 1f;

        protected readonly Color mainTextColour = Color.White;
        protected readonly Color descriptionTextColour = Color.Gold;
        protected readonly Color hotkeyColour = Color.Gold;


        /* Drawing Vars */
        protected SpriteFont font;
        public bool drawHorizontalToRight;
        public bool drawVerticalDownwards;
        public readonly float tileSize = 16 * tooltipScale;
        public Vector2 sourcePos;
        public Vector2 Dimensions;

        protected string hotkey;
        public List<String> text { get; set; }
        bool visible; //For when the mouse is hovering over its parent thingy
        byte noOfLines; //The number of lines for the main body of the tooltip text

        public TooltipBase( Vector2 source, ref SpriteFont _font, string _text, string _hotkey, int maxScreenHeight, int maxScreenWidth )
        {
            int stringCount = 0;

            sourcePos = source;
            font = _font;
            text = UI_Components.consts.parseText(_text);

            if (_hotkey != null)
            {
                hotkey = _hotkey;
                stringCount++;
            }

            stringCount += text.Count;

            //Measure each string to get the longest length
            int lineCounter;
            int longestStringX = 0;

            for (lineCounter = 0; lineCounter < text.Count; lineCounter++)
            {
                int temp = (int)font.MeasureString(text[lineCounter]).X;
                if (temp > longestStringX)
                    longestStringX = temp;
            }

            getDimensions(maxScreenHeight, maxScreenWidth, longestStringX, stringCount );
            //Measure the string to find out if the tooltip appears above/below/left/right
        }

        public virtual void draw( ref SpriteBatch spriteBatch)
        {
            int lineCounter = 0;
            Vector2 drawPos = new Vector2(sourcePos.X, sourcePos.Y);

            drawPos.X += horizontalTextOffset;
            drawPos.Y += verticalTextOffset;

            for (lineCounter = 0; lineCounter < text.Count; lineCounter++)
            {
                drawTextOutlined(ref spriteBatch, text[lineCounter], mainTextColour, ref drawPos);
                drawPos.Y += textGap;
            }

            drawTextOutlined(ref spriteBatch, "Hotkey: (" + hotkey + ")", hotkeyColour, ref drawPos);
        }

        protected void drawTextOutlined(ref SpriteBatch batch, string Text, Color Colour, ref Vector2 Position)
        {
            Color darkened = new Color(Colour.R - 100, Colour.G - 100, Colour.B - 100, Colour.A);
            Position.Y += 1f;
            batch.DrawString(font, Text, Position, Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0f);
            Position.X += 1f;
            batch.DrawString(font, Text, Position, Color.Black, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0f);
            Position.X -= 1f;
            Position.Y -= 1f;
            batch.DrawString(font, Text, Position, Colour, 0, Vector2.Zero, fontScale, SpriteEffects.None, 0f);
        }

        public TooltipBase()
        {
            text = new List<string>();
        }

        protected void getDimensions( int maxHeight, int maxWidth, int longestString, int stringCount )
        {
            //Calculate the total necessary width/height, and test this against the screen resolution
            //To see if certain parameters are needed
            Dimensions.X = longestString + horizontalTextOffset;
            if (maxWidth - sourcePos.X < Dimensions.X)
            {
                //Not enough space, should draw to the left
                drawHorizontalToRight = false;
                sourcePos.X -= Dimensions.X;
            }
            else drawHorizontalToRight = true;
                
                //The extra number makes sure the text doesnt sit on the border
            Dimensions.Y = verticalTextOffset + 3 + stringCount * textGap;
            if (maxHeight - sourcePos.Y < Dimensions.Y)
            {
                //Not enough space, should draw higher up
                drawVerticalDownwards = false;
                sourcePos.Y -= Dimensions.Y;
            }
            else drawVerticalDownwards = true;

            return;
        }

        public float getScale() { return tooltipScale; }
    }
}
