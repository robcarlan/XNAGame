using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Abilities
{
	//Derived classes include - projectile, channeled, attack (i.e. sword swing), aoe attack
	//Each ability could have a list of projectiles
	//How do defending?
	//Consult abilities.txt before working on this!!!
    public abstract class AbilityBase
    {
        public int ownerID;
        public int targetID;	//Only for a targeted spell.
        public int ID;

        public readonly string name;
		public readonly string description;

        public bool timed;
        public float duration;
		public float cooldown;

		//public readonly AbilityData data; //References data from library
				//MOve this out 

        //public List<short> activeProjectileIDs = new List<short>(); //Tracks projectiles cast
        public List<short> castParticleID = new List<short>();	//These particle effect occur when the spell is being cast

        public List<short> useEffects = new List<short>();

        //When the ability is destroyed, should the effects be removed
        public bool removeUseEffectsWhenFinished;

        public AbilityBase(AbilityBase temp)
        {

        }

		public AbilityBase()
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

		/// <summary>
		/// Called when the ability has begun being cast
		/// </summary>
		public virtual void useAbility()
		{

		}

		/// <summary>
		/// Called when the character moves.
		/// </summary>
		public virtual void onCharacterMove()
		{

		}

		/// <summary>
		/// Called when the button to cast the spell has been released. 
		/// </summary>
		public virtual void onClickRelease()
		{

		}
		
		/// <summary>
		/// Called when the ability has finished casting.
		/// </summary>
		public virtual void onAbilityCast()
		{
			//Remove particles
		}
    }
}
