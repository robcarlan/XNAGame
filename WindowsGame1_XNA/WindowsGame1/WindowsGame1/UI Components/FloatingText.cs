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
    public class FloatingText
    {
        //public enumDataTypes. types.enums.textType { damage, poison, fire, ice, healing, money, item, exp, statGain, statLoss };
        public Color textColour;

        bool critical;
        public string text;
        float scale;

        //Mathy Stuff
        public Vector2 position;
        int Velocity;
        int duration;
        public int fadeValue;

        public FloatingText()
        {
            textColour = Color.White;
        }
        public FloatingText(string Text, Rectangle Source, bool critical, enums.textType type, Random rand)
        {
            textColour = getTextColour(type, critical);
            text = Text;
            generateTextPosition(new Vector2(Source.X + Source.Width / 2, Source.Y + Source.Height / 2), critical, 1
                , rand.Next(-50, 50), rand.Next(-50, 50));
            duration = (critical == true) ? 1500 : 1000;
        }

        public FloatingText(string Text, Vector2 Source, bool critical,  enums.textType type, Random rand)
        {
            textColour = getTextColour(type, critical);
            text = Text;
            generateTextPosition( Source, critical, 1
                , rand.Next(-50, 50), 0);
            duration = (critical == true) ? 1500 : 1000;
        }

        //rand pos should be between -50 and 50
        private void generateTextPosition(Vector2 source, bool critical, int randVel, int randPosX, int randPosY)
        {
            if (critical)
            {
                scale = 1.5f;
                Velocity = 0;
            }
            else
            {
                scale = 1f;
                Velocity = randVel;
            }

            position.X = source.X + randPosX;
            position.Y = source.Y + randPosY;

        }

        /// <summary>
        /// Used to update variables each frame
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>true to indicate the text has passed its duration</returns>
        public bool updateText(int gameTime)
        {
            position.Y -= Velocity;
            duration -= gameTime;
            if (duration < 255) fadeValue += gameTime;

            if (duration > 0) return false; else return true;
        }

        /// <summary>
        /// Gets the colour of the text based on the nature of the text:
        /// </summary>
        /// <returns>A color, lol</returns>
        public Color getTextColour( enums.textType type, bool critical )
        {
            switch (type)
            {
                case  enums.textType.damage:
                    if (critical)
                        return Color.Gainsboro;
                    else return Color.Red;
                case  enums.textType.fire:
                    return Color.Firebrick;                    
                case  enums.textType.ice:
                    return Color.LightSkyBlue;      
                case  enums.textType.healing:
                    if (critical)
                        return Color.Chartreuse;
                    else return Color.LawnGreen;
                case  enums.textType.money:
                    return Color.Gold;                    
                case  enums.textType.poison:
                    return Color.Green;                    
                case  enums.textType.exp:
                    return Color.DarkViolet;
                case enums.textType.mana:
                    return Color.Blue;
                case  enums.textType.statGain:
                    return Color.RoyalBlue;
                case  enums.textType.statLoss:
                    return Color.LightSkyBlue;
                case enums.textType.effectGain:
                    return Color.Purple;
                case enums.textType.effectLost:
                    return Color.SteelBlue;
                default:
                    return Color.White;
            }
        }
    }

    public class floatingTextData
    {
        public string text;
        public enums.textType textType;
        public bool critical;

        //Light class used for storing data of floating text to be made

        public floatingTextData()
        {

        }

        public floatingTextData(string _text, enums.textType _textType)
        {
            text = _text;
            textType = _textType;
        }

    }
}
