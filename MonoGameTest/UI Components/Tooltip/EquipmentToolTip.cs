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
    enum quality { common, uncommon, rare, epic, legendary };
    enum armorSlot { helmet, neck, torso, legs, gloves, feet, primary, shield, bow, staff, none };

    class EquipmentToolTip : TooltipBase
    {
        Rectangle iconSourceRec;

        Color qualityColour;
        protected readonly Color effectColour = Color.Lime;

        //Change depending on qualities?
        protected readonly Color itemTextBodyColour = Color.LightGray;

        List<string> name;
        string dps;
        string equipmentType;

        public EquipmentToolTip( Vector2 source, ref SpriteFont textFont, string _name, string _dps, string textBody, int quantity,
            quality _weaponQuality, armorSlot armorSlot, int maxHeight, int maxWidth)
        {
            int longestStringX = 0;
            int lineCounter = 0;
            int stringCount = 0;

            sourcePos = source;
            font = textFont;
            qualityColour = getQualityColour(_weaponQuality);

            //Add the item Name 
            name = new List<string>();

            if ( quantity > 1 )
                name.AddRange(UI_Components.consts.parseText(_name + " (" + quantity + ")"));
            else name.AddRange(UI_Components.consts.parseText(_name));

            //Get equipment type
            if (armorSlot != armorSlot.none)
            {
                equipmentType = getItemType(armorSlot);
                stringCount++;
            }

            //Add the damage stats if present
            if (_dps != null)
            {
                dps = _dps;
                stringCount++;
            }

            //Add the text body
            text.AddRange(UI_Components.consts.parseText(textBody));

            for (lineCounter = 0; lineCounter < text.Count; lineCounter++)
            {
                int temp = (int)font.MeasureString(text[lineCounter]).X;
                if (temp > longestStringX)
                    longestStringX = temp;
            }

            for (lineCounter = 0; lineCounter < name.Count; lineCounter++)
            {
                int temp = (int)font.MeasureString(name[lineCounter]).X;
                if (temp > longestStringX)
                    longestStringX = temp;
            }
            stringCount += text.Count;
            stringCount += name.Count;

            getDimensions(maxHeight, maxWidth, longestStringX, stringCount);
            
        }

        /// <summary>
        /// Draws each string to the screen. Drawing the surrounding box should be handled by the UI class.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void draw(ref SpriteBatch spriteBatch)
        {
            int lineCounter = 0;
            Vector2 drawPos = new Vector2(sourcePos.X, sourcePos.Y);

            drawPos.X += horizontalTextOffset;
            drawPos.Y += verticalTextOffset;

            //Draw each line in the name strings
            for (lineCounter = 0; lineCounter < name.Count; lineCounter++)
            {
                drawTextOutlined( ref spriteBatch, name[lineCounter].ToString(), qualityColour, ref drawPos);
                drawPos.Y += textGap;
            }

            if (equipmentType != null)
            {
                drawTextOutlined(ref spriteBatch, equipmentType, mainTextColour, ref drawPos);
                drawPos.Y += textGap;
            }

            if (dps != null)
            {
                drawTextOutlined(ref spriteBatch, dps, mainTextColour, ref drawPos);
                drawPos.Y += textGap;
            }

            //Draw each line in the textBody strings
            for (lineCounter = 0; lineCounter < text.Count; lineCounter++)
            {
               
                drawTextOutlined(ref spriteBatch, text[lineCounter].ToString(), itemTextBodyColour, ref drawPos);
                drawPos.Y += textGap;
            }
        }

        #region getFunctions
        Color getQualityColour(quality quality)
        {
            switch (quality)
            {
                case quality.common:
                    return Color.White;
                case quality.uncommon:
                    return Color.Gray;
                case quality.rare:
                    return Color.CadetBlue;
                case quality.epic:
                    return Color.Purple;
                case quality.legendary:
                    return Color.OrangeRed;
                default:
                    return Color.Pink;
            }
        }

        string getItemType(armorSlot equipment)
        {
            switch (equipment)
            {
                case armorSlot.primary:
                    return "Primary Weapon";
                case armorSlot.bow:
                    return "Bow";
                case armorSlot.feet:
                    return "Feet";
                case armorSlot.gloves:
                    return "Gloves";
                case armorSlot.helmet:
                    return "Helmet";
                case armorSlot.legs:
                    return "Legs";
                case armorSlot.neck:
                    return "Neck";
                case armorSlot.shield:
                    return "Shield";
                case armorSlot.staff:
                    return "Staff";
                case armorSlot.torso:
                    return "Torso";
                default:
                    return "Misc.";
            }
        }

        #endregion 
    }
}
