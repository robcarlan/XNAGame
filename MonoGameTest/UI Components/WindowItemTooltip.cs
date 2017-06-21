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
    public class WindowItemTooltip
    {
        public Rectangle iconRegion;
        public bool shown;
        public string Name;
        public List<string> description;
        public List<string> extras; //Shown only by tooltip
        public List<string> requirements; //Coloured based on users stats
        public int spriteID;

        public WindowItemTooltip(string _abilityName, string _description, int _manaCost, int _cooldown, int _castTime,
            bool _channeled, bool _passive, short levelRequirement )
        {
            description = extras = requirements = new List<string>();

            //Add ability name to first line
            setName(_abilityName);

            //Parse through details
            if (levelRequirement != 0)
                extras.Add("Level " + levelRequirement + " Required ");

            if (_channeled)
                extras.Add("Channelled");
            else if (_passive)
                extras.Add("Passive");

            if (_castTime != 0)
                extras.Add(_castTime + "s cast time");
            else extras.Add("Instant");

            if (_manaCost != 0)
                extras.Add(_manaCost + " mana");

            if (_cooldown != 0)
                extras.Add(_cooldown + "s cooldown");


            //Get main text body
            description = consts.parseText(_description);
        }

        public WindowItemTooltip(string _itemName, int value, string _description)
        {
            description = extras = requirements = new List<string>();

            iconRegion = new Rectangle(0, 0, 0, 0);
            setName(_itemName + ": " + value );
            description.Add(_description);
   
        }

        public void setName(string _name)
        {
            Name = _name;
        }

        public void setValues()
        {
            //Sets the values, called by window classs. Sets positional values and whether the item is hidden / shown

        }
        
    }
}
