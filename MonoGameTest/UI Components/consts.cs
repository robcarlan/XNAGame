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
    public sealed class consts
    {
        //Tooltip colours
        public static Color TEXT_COLOUR = Color.White;
        public static Color NAME_COLOUR = Color.Gold;
        public static Color DATA_COLOUR = Color.LightGray;

        public const string scrollPrefix = "scrl";

        //Window regions
        public const string topLeft = "TLCnr";
        public const string topRight = "TRCnr";
        public const string bottomLeft = "BLCnr";
        public const string bottomRight = "BRCnr";
        public const string left = "LSide";
        public const string right = "RSide";
        public const string bottom = "DSide";
        public const string top = "TSide";
        public const string fill = "BackCtr";

        //Reserved Control ID's
        public const int CLOSE_BTN_ID = 0;

        //Icon sprite layout
        public const int BUTTON_DEFAULT = 1;
        public const int BUTTON_HELD = 2;
        public const int BUTTON_CHECKED = 3;
        public const int BUTTON_UNCHECKED = 1;
        public const int BUTTON_HOVER = 4;

        public const float UI_CONTROL_SCALE = 1.2f;
        public const float wiindowTextScale = 2f;
        public const short iconDimensions = 32;
        public const short maxLetters = 40;
        public const float abilityIconScale = 1.6f;

        private consts()
        {

        }

        public static List<String> parseText(string text)
        {
            List<string> lines = new List<string>();
            List<String> words = new List<string>();

            //Split the text into seperate words
            words.AddRange(text.Split(' '));
            //Add the words onto each line until it reaches a maximum size, then move onto the next line
            int letterCount = 0;
            int lineCount = 0;

            while (words.Count > 0)
            {
                lines.Add("");
                while (letterCount + words[0].Length <= maxLetters)
                {
                    lines[lineCount] += words[0] + " ";
                    letterCount += words[0].Length + 1;
                    words.RemoveAt(0);
                    if (words.Count == 0) break;
                }
                lineCount++;
                letterCount = 0;
            }

            return lines;
        }

        public static void drawOutlinedText(string msg, float thickness, Vector2 pos, ref SpriteBatch spriteBatch, Color colour, float scale, ref SpriteFont font)
        {

            pos.X += thickness;
            spriteBatch.DrawString(font, msg, pos, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            pos.X -= thickness;
            pos.Y += thickness;
            spriteBatch.DrawString(font, msg, pos, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            pos.Y -= thickness;

            spriteBatch.DrawString(font, msg, pos, colour, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void drawText(string text, float thickness, Vector2 pos, ref SpriteBatch spriteBatch, Color col, Vector2 scale, ref SpriteFont font)
        {
            if (thickness > 0)
            {
                pos.X += thickness;
                spriteBatch.DrawString(font, text, pos, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                pos.X -= thickness;
                pos.Y += thickness;
                spriteBatch.DrawString(font, text, pos, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                pos.Y -= thickness;
            }

            spriteBatch.DrawString(font, text, pos, col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
