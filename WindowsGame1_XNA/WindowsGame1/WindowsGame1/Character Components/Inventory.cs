using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Character_Components
{
    public class Inventory
    {
        public int nextFreeSpace;
        public int maxSpace;
        public List<int> quantity = new List<int>();
        public List<short> contents = new List<short>();

        public Inventory(int _maxSpace)
        {
			quantity.Capacity = contents.Capacity = maxSpace = _maxSpace;
			nextFreeSpace = 0;
			//Initialise the contents of the inventory
			for (int i = 0; i < maxSpace; i++)
			{
				quantity.Add(0);
				contents.Add(0);
			}
        }

		public Inventory(int _maxSpace, List<short> _contents, List<int> _quantity)
            : this(_maxSpace)
        {
            contents = _contents;
			quantity = _quantity;
        }

		public int calculateNextFreeSpace()
		{
			for (int i = 0; i < maxSpace; i++)
			{
				if (contents[i] == -1)
				{
					nextFreeSpace = i;
					return i;
				}
			}
			nextFreeSpace = -1;
			return nextFreeSpace;
		}

		public void exchangeItems(short IDnew, short IDold)
		{

		}

		public bool giveItem(short itemID, int _quantity)
		{
			if (contents.Contains(itemID))
			{
				int pos = contents.IndexOf(itemID);
				quantity[pos] += _quantity;
				return true;
			}
			else
			{
				if (nextFreeSpace == -1)
					return false;
				else
				{
					contents[nextFreeSpace] = itemID;
					quantity[nextFreeSpace] = _quantity;
					calculateNextFreeSpace();
					return true;
				}
			}
		}

		public bool takeItem(short itemID, int _quantity)
		{
			if (contents.Contains(itemID))
			{
				int itemSlot = contents.IndexOf(itemID);
				if (quantity[itemSlot] >= _quantity)
				{
					quantity[itemSlot] -= _quantity;
					if (quantity[itemSlot] == 0)
					{
						contents[itemSlot] = 0;
					}
					calculateNextFreeSpace();
					return true;
				}
				else return false;
			}
			else return false;
		}

		public bool takeAll(short itemID, int _quantity)
		{
			if (contents.Contains(itemID))
			{
				int itemSlot = contents.IndexOf(itemID);
				quantity[itemSlot] = 0;
				contents[itemSlot] = 0;
				calculateNextFreeSpace();
				return true;
			}
			else return false;
		}

		public bool hasItem(short itemID, int _quantity)
		{
			if (contents.Contains(itemID))
			{
				int itemSlot = contents.IndexOf(itemID);
				return (quantity[itemSlot] >= _quantity);
			}
			else return false;
		}
    }

    //Drop table extends this class - adds a variable for percentage chances
}
