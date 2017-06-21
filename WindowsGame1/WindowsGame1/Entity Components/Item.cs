using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class Item
    {
        //Added when the item is in the inventory
        List<CharacterEffect> activeEffect;
        public Point itemSpriteSource;

        int levelRequirement, itemValue;
        public readonly String name;
        public readonly string description;
        public readonly short ID;
        bool tradeable;

        public Item() { }
        //Constructor, data loaded from XML
        public Item(
                    Point _itemSpriteSource,
                    short _ID, string _itemName,
                    int _levelRequirement, int _itemValue,
                    bool _tradeable, string _description
                   )
        {
            itemSpriteSource = _itemSpriteSource;
            name = _itemName;
            ID = _ID;
            levelRequirement = _levelRequirement;
            itemValue = _itemValue;
            tradeable = _tradeable;
            description = _description;
        }

        Item getItem()
        {
            return this;
        }

        public virtual List<string> getInfo()
        {
            List<string> info = new List<string>();
            short counter = 0;

            info.Add("Item Name: " + name);
            info.Add("ID: " + ID);
            info.Add("Item Sprit source-  X: " + itemSpriteSource.X + " Y: " + itemSpriteSource.Y);
            info.Add("Effects:");
            //while ( counter < activeEffect.Count)
            //{
            //    info.Add(activeEffect[counter].ToString());
            //}

            info.Add("Level requirement:" + levelRequirement);
            info.Add("Value: " + itemValue);
            info.Add("Tradeable: " + Convert.ToString(tradeable));

            return info;
        }
        
    }

    class Consumable : Item
    {
        //Added when the item is consumed
        List<String> useEffect;

        Boolean unlimited;
        short cooldown;

        public Consumable( Point _itemSpriteSource,
                    short _ID, string _itemName,
                    int _levelRequirement, int _itemValue,
                    bool _tradeable,
                    List<String> _useEffect, Boolean _unlimited, short _cooldown, string _description)
            : base(_itemSpriteSource, _ID, _itemName, _levelRequirement, _itemValue, _tradeable, _description)
        {
            useEffect = _useEffect;
            unlimited = _unlimited;
            cooldown = _cooldown;
        }

        public Consumable()
        {

        }

        public override List<string> getInfo()
        {
            List<string> info = base.getInfo();
            short counter = 0;

            info.Add("Use effects:");
            while (counter < useEffect.Count)
            {
                info.Add("   " + useEffect[counter]);
                    counter++;
            }
            
            info.Add("Unlimited: " + Convert.ToString(unlimited));
            info.Add("Cooldown: " + cooldown + " seconds.");

            return info;
        }
    }
}
