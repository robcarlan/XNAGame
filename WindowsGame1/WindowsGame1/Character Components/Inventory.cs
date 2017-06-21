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
        public List<short> quantity = new List<short>();
        public List<short> contents = new List<short>();

        public Inventory(int _maxSpace)
        {
			quantity.Capacity = contents.Capacity = maxSpace = _maxSpace;
		
			//Initialise the contents of the inventory
			for (int i = 0; i < maxSpace; i++)
			{
				quantity.Add(0);
				contents.Add(0);
			}
        }

		public Inventory(int _maxSpace, List<short> _contents, List<short> _quantity)
            : this(_maxSpace)
        {
            contents = _contents;
			quantity = _quantity;
        }

		public void exchangeItems(short IDnew, short IDold)
		{

		}
    }

    //Drop table extends this class - adds a variable for percentage chances
}
