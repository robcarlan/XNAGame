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
    class CharacterTooltip : TooltipBase
    {
        private readonly Color enemyColourStrong = Color.Red;
        private readonly Color enemyColourWeak = Color.Orange;
        private readonly Color friendColour = Color.YellowGreen;

        Color drawColourMain;
        public List<string> mainText = new List<string>();

        public CharacterTooltip(Vector2 source, Point screenDimensions, SpriteFont _font, Character_Components.Character character, Character_Components.Character Hero )
        {
            int lineCounter = 0;
            int longestStringX = 0;
            font = _font;
            sourcePos = source;
     
            //Determine main colour
            if (Hero.isHostile(character.factions))
            {
                //Compare levels
                if (Hero.stats.level >= character.stats.level)
                    drawColourMain = enemyColourWeak;
                else
                    drawColourMain = enemyColourStrong;
            }
            else drawColourMain = friendColour;

            mainText.AddRange(UI_Components.consts.parseText(character.name));
            mainText.Add("Level: " + character.stats.level);
            mainText.Add("ID: " + character.ID); 

            //Get longest string
            for (lineCounter = 0; lineCounter < text.Count; lineCounter++)
            {
                int temp = (int)font.MeasureString(text[lineCounter]).X;
                if (temp > longestStringX)
                    longestStringX = temp;
            }

            for (lineCounter = 0; lineCounter <  mainText.Count; lineCounter++)
            {
                int temp = (int)font.MeasureString(mainText[lineCounter]).X;
                if (temp > longestStringX)
                    longestStringX = temp;
            }

            getDimensions(screenDimensions.Y, screenDimensions.X, longestStringX, text.Count + mainText.Count );
        }

        public override void draw(ref SpriteBatch spriteBatch)
        {
            int lineCounter = 0;
            Vector2 drawPos = new Vector2(sourcePos.X, sourcePos.Y);

            drawPos.X += horizontalTextOffset;
            drawPos.Y += verticalTextOffset;

            for (lineCounter = 0; lineCounter < mainText.Count; lineCounter++)
            {
                drawTextOutlined(ref spriteBatch, mainText[lineCounter], mainTextColour, ref drawPos);
                drawPos.Y += textGap;
            }

            for (lineCounter = 0; lineCounter < text.Count; lineCounter++)
            {
                drawTextOutlined(ref spriteBatch, text[lineCounter], descriptionTextColour, ref drawPos);
                drawPos.Y += textGap;
            }
        }
    }
}
