using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WindowsGame1.Entity_Manager
{
    public class ItemManager
    {
        public Dictionary<short, Item> Items;
        public Dictionary<short, Item> ConsumableItems;
        public Dictionary<short, Item> Armour;
        public Dictionary<short, Item> Weapons;

        public ItemManager( DataLoader.ItemLoader[] _items,
                            DataLoader.ConsumableItemLoader[] _consumableItems,
                            DataLoader.ArmourItemLoader[] _armor,
                            DataLoader.WeaponItemLoader[] _weapons,
                            List<string> errorMessages )
        {
            errorMessages = new List<string>();
            Items = new Dictionary<short, Item>();
            ConsumableItems = new Dictionary<short, Item>();
            Armour = new Dictionary<short, Item>();
            Weapons = new Dictionary<short, Item>();

            try
            {
                foreach (DataLoader.ItemLoader tempItem in _items)
                {
                    Item item = new Item(
                        tempItem.itemSpriteSource, tempItem.ID, tempItem.itemName,
                           tempItem.levelRequirement, tempItem.itemValue, tempItem.tradeable, tempItem.description);
                           
                    Items.Add(tempItem.ID, item);
    
                }
                errorMessages.Add("Loaded " + Items.Count + " items!");
            }
            catch(Exception)
            {
                errorMessages.Add("Could not load items from the item loader! Make sure the filename is correct.");
            }

            try
            {
                foreach (DataLoader.ConsumableItemLoader tempConsumableItem in _consumableItems)
                {
                    ConsumableItems.Add(tempConsumableItem.ID,
                            new Consumable(
                                tempConsumableItem.itemSpriteSource, tempConsumableItem.ID, tempConsumableItem.itemName,
                                tempConsumableItem.levelRequirement, tempConsumableItem.itemValue, tempConsumableItem.tradeable,
                                tempConsumableItem.useEffect, tempConsumableItem.unlimited, tempConsumableItem.cooldownSeconds, tempConsumableItem.description)
                        );
                }
                errorMessages.Add("Loaded " + ConsumableItems.Count + " consumable items!");
            }
            catch (Exception)
            {
                errorMessages.Add("Could not load consumable items from the item loader! Make sure the filename is correct.");
            }
        }
    }
}
