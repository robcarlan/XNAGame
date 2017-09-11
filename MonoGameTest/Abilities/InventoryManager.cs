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

namespace WindowsGame1.Entity_Manager
{
    public class InventoryManager
    {
        /// <summary>
        /// Updated every time a new item is inserted or an item is removed
        /// </summary>
        const short itemMinimumRange = 0;
        const short consumableItemMinimumRange = 1000;
        const short armourMinimumRange = 2000;
        const short weaponMinimumRange = 3000;
        const short maxItemID = 4000;

        public List<String> errorMessages;

        public ItemManager items;
        public Dictionary<short, Character_Components.Inventory> inventories = new Dictionary<short, Character_Components.Inventory>();

        public InventoryManager(ContentManager content, Dictionary<short, Point> spriteLocations)
        {
            errorMessages = new List<string>();
			
			DataLoader.ItemLoader[] _items;// = content.Load<DataLoader.ItemLoader[]>("Object Databases\\Items\\ItemParser");
			DataLoader.ConsumableItemLoader[] _consumableItems;// = content.Load<DataLoader.ConsumableItemLoader[]>("Object Databases\\Items\\ConsumableItemLoader");
			DataLoader.ArmourItemLoader[] _armour;// = content.Load<DataLoader.ArmourItemLoader[]>("Object Databases\\Items\\ArmourLoader");
			DataLoader.WeaponItemLoader[] _weapons;// = content.Load<DataLoader.WeaponItemLoader[]>("Object Databases\\Items\\WeaponLoader");

			_items = Game1.LoadContent<DataLoader.ItemLoader[]>(Declaration.itemLoaderPath + ".xml");
			_consumableItems = Game1.LoadContent<DataLoader.ConsumableItemLoader[]>(Declaration.ConsumableItemLoaderPath + ".xml");
			_armour = Game1.LoadContent<DataLoader.ArmourItemLoader[]>(Declaration.ArmourLoaderPath + ".xml");
			_weapons = Game1.LoadContent<DataLoader.WeaponItemLoader[]>(Declaration.WeaponLoaderPath + ".xml");

            items = new ItemManager(_items, _consumableItems, _armour, _weapons, errorMessages);
						
            //Populate the spriteLocations list
            foreach (Item tempItem in items.Items.Values) 
            {
                spriteLocations.Add(tempItem.ID, tempItem.itemSpriteSource);
            }

            foreach (Item tempItem in items.Armour.Values)
            {
                spriteLocations.Add(tempItem.ID, tempItem.itemSpriteSource);
            }

            foreach (Item tempItem in items.ConsumableItems.Values)
            {
                spriteLocations.Add(tempItem.ID, tempItem.itemSpriteSource);
            }

            foreach (Item tempItem in items.Armour.Values)
            {
                spriteLocations.Add(tempItem.ID, tempItem.itemSpriteSource);
            }
        }

        public string getItemName(short itemID)
        {
            try
            {
                if (itemID >= itemMinimumRange && itemID < consumableItemMinimumRange)
                //itemID belongs to an item
                {
                    return items.Items[itemID].name;
                }
                if (itemID >= consumableItemMinimumRange && itemID < armourMinimumRange)
                //itemID belongs to a consumable
                {
                    return items.ConsumableItems[itemID].name;
                }
                if (itemID >= armourMinimumRange && itemID < weaponMinimumRange)
                //itemID belongs to a armour
                {
                    return items.Armour[itemID].name;
                }
                if (itemID >= weaponMinimumRange && itemID < maxItemID)
                //itemID belongs to a weapon
                {
                    return items.Weapons[itemID].name;
                }
                else return "Null";
            }
            catch (IndexOutOfRangeException)
            {
                errorMessages.Add("Could not find ID: " + itemID);
                return "Null";
            }
        }

        public Item getItem(short itemID)
        {
            try
            {
                if (itemID >= itemMinimumRange && itemID < consumableItemMinimumRange)
                //itemID belongs to an item
                {
                    return items.Items[itemID];
                }
                if (itemID >= consumableItemMinimumRange && itemID < armourMinimumRange)
                //itemID belongs to a consumable
                {
                    return items.ConsumableItems[itemID];
                }
                if (itemID >= armourMinimumRange && itemID < weaponMinimumRange)
                //itemID belongs to a armour
                {
                    return items.Armour[itemID];
                }
                if (itemID >= weaponMinimumRange && itemID < maxItemID)
                //itemID belongs to a weapon
                {
                    return items.Weapons[itemID];
                }
                else return null;
            }
            catch (KeyNotFoundException)
            {
                errorMessages.Add("Could not find ID: " + itemID);
                return null;
            }
        }

        //Search Functions

        /// <summary>
        /// Finds an item in an inventory
        /// </summary>
        /// <param name="objectID">Object whose inventory is to be searched! returns the location of the item</param>
        /// <param name="searchID"></param>
        /// <returns>An int to represent the location of the item, -1 if not found</returns>
        public short findItem(short objectID, short searchID)
        {
            for (short i = 0; i < inventories[objectID].contents.Count; i++)
            {
                if (inventories[objectID].contents[i] == searchID)
                    return i;       
            }
            return -1;
        }

        //Get the quantity of a specific item
        public int getItemQuantity(short itemID, short objectID)
        {
            for (int i = 0; i < inventories[objectID].contents.Capacity; i++)
            {
                if (inventories[objectID].contents[i] == itemID)
                    return inventories[objectID].quantity[i];
            }
            return -1;
        }


        /// <summary>
        /// Removes an item from the inventory, the quantity of an item in the inventory should be known beforehand with getItemQuantity()
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="quantity">The amount of the item to take. quantity as 0 means to take all of the specified item</param>
        /// <returns>boolean depending on wether the item could be found</returns>
        public bool removeItem(short itemID, short _quantity, short objectID)
        {
            for (int i = 0; i < inventories[objectID].contents.Capacity; i++)
            {
                if (inventories[objectID].contents[i] == itemID)
                {
                    //Object can be found
                    if (inventories[objectID].quantity[i] == _quantity)
                    {
                        //Remove the item from the list, as none is left
						inventories[objectID].contents[i] = 0;
						inventories[objectID].quantity[i] = 0;
						getFreeSpace(objectID);
                    }
                    else if (inventories[objectID].quantity[i] < _quantity)
                    {
                        //Not enough
                        return false;
                    }
                    else inventories[objectID].quantity[i] -= _quantity;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Inserts an item into a players inventory
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="_quantity">Number of items to give</param>
        /// <returns>Boolean value depending on whetehr the item could be inserted into the inventory or not</returns>
        public bool giveItem(short objectID, short itemID, short _quantity)
        {
            if (checkSpace(objectID))
            {
                short itemLocation = findItem(objectID, itemID );
                if (itemLocation == -1)
                {
                    inventories[objectID].contents[inventories[objectID].nextFreeSpace] = itemID;
                    inventories[objectID].quantity[inventories[objectID].nextFreeSpace] = _quantity;

                    getFreeSpace(objectID);
                }
                else inventories[objectID].quantity[itemLocation] += _quantity;
                return true;
            }
            else return false;
        }

        public void getFreeSpace(short objectID)
        {
            for (int i = 0; i < inventories[objectID].contents.Capacity; i++)
            {
                //If empty
                if ( inventories[objectID].contents[i] <= 0 )
                {
                    inventories[objectID].nextFreeSpace = i;
                    return;
                }
            }
            return;
        }

        public bool checkSpace(short objectID)
        {
            for (int i = 0; i < inventories[objectID].contents.Capacity; i++)
            {
                //If empty
                if ( inventories[objectID].contents[i] == 0 )
                {
                    return true;
                }
            }
            return false;
        }
        //Give item, item type function, swap slots
    }
}
