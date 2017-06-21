using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Abilities
{
    class AbilityData
    {
        //Contains information for the player about what each ability does, and holds management data such as cooldown
		//Data used by UI tooltips
        public bool passive;
        public bool channeled;
        public int manaCost;
        public int cooldown;
        public int castTime;
        public string name;
        public string description;

		public AbilityData(
			bool _passive, bool _channeled, int _manaCost,
			int _cooldown, int _castTime, string _name,
			string _description)
		{
			passive = _passive;
			channeled = _channeled;
			manaCost = _manaCost;
			cooldown = _cooldown;
			castTime = _castTime;
			name = _name;
			description = _description;
		}

    }
}
