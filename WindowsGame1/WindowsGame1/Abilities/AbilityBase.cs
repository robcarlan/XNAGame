using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Abilities
{
    public abstract class AbilityBase
    {
        public int ownerID;
        public int targetID;
        public int ID;
        public string name;

        public bool timed;
        public float duration;

        public List<short> activeParticleIDs = new List<short>();
        public List<short> castParticleID = new List<short>();

        public List<short> useEffects = new List<short>();

        //When the ability is destroyed, should the effects be removed
        public bool removeUseEffectsWhenFinished;

        public AbilityBase(AbilityBase temp)
        {

        }

        /// <summary>
        /// Updates the ability
        /// </summary>
        /// <param name="msPassed"></param>
        /// <returns>True if the ability should be destroyed</returns>
        public virtual bool update( float msPassed )
        {
            if (timed)
            {
                duration -= msPassed;

                if (duration <= 0)
                    return true;
            }

            return false;
        }
    }
}
