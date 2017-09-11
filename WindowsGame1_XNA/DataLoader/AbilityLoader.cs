using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLoader
{
    //Container loader
    class AbilityLoader
    {
        public bool passive;
        public bool channeled;
        public int manaCost;
        public int cooldown;
        public int castTime;
        public string name;
        public string description;
    }

    //Base ability loader
    class AbilityBaseLoader
    {
        public int ownerID;
        public int targetID;
        public int ID;
        public string name;

        public bool timed;
        public float duration;

        public List<int> particleIDs = new List<int>();
        public List<int> castParticleID = new List<int>();

        public List<int> useEffects = new List<int>();

        public bool removeUseEffectsWhenFinished;

    }
}
