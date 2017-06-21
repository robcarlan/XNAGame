using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Entity_Manager
{
    class CharacterTypes
    {
        //Contains the default characters used to generate creatures / characters etc.
        //Search lists for enemy types / levels / regional
        Random rand;

        public List<Character_Components.Character> Characters;
        CharacterBank bank;

        public Character_Components.Character createRandomEnemy( enums.characterType _type, enums.faction _faction, bool allowUnique, short regionID )
        {
            return null;
        }

        /// <summary>
        /// returns an id of an object which satisfies conditions given in the parameters
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_faction"></param>
        /// <param name="allowUnique"></param>
        /// <param name="regionID"></param>
        /// <returns> An id of the object. -1 represents an entity which cannot be found with those conditions</returns>
        public short getID(enums.characterType _type, enums.faction _faction, bool allowUnique, short regionID)
        {
            short typeID = (short)_type;

            if (bank.factions[_faction].Count == 0 && _faction != null)
                return -1;
            else
            {
                //Search for an entity
            }

            return -1;
        }

        public CharacterTypes()
        {
            rand = new Random();
            bank = new CharacterBank();
			Characters = new List<Character_Components.Character>();
        }

        public CharacterTypes(List<templateCharacter> _characters) : base()
        {
            addEntities(_characters);
        }

        public void addEntities( List<templateCharacter> _characters)
        {
            foreach (templateCharacter X in _characters)
            {
                Characters.Add(X.character);

                foreach (enums.faction listCounter in X._factions)
                {
                    bank.factions[listCounter].Add(X.character.ID);
                }


                foreach (short listCounter in X._regions)
                {
                    bank.regions[listCounter].Add(X.character.ID);
                }


                foreach (enums.characterType listCounter in X._types)
                {
                    bank.types[listCounter].Add(X.character.ID);
                }
            }
        }

    }

    //Stores a list of id's for each matching entity
    class CharacterBank
    {
        //f = faction, t = type
        public const short tHumanID = 0;
        public const short tCreatureID = 1;

        public const short fNoneID = 0;

        public const short rNoneID = 0;

        public Dictionary<enums.characterType, List<short>> types;
        public Dictionary<short, List<short>> regions;
        public Dictionary<enums.faction, List<short>> factions;

        public CharacterBank()
        {
            types = new Dictionary<enums.characterType, List<short>>();
            regions = new Dictionary<short, List<short>>();
            factions = new Dictionary<enums.faction, List<short>>();
        }
    }

    public class templateCharacter
    {
        public Character_Components.Character character;
        public List<enums.characterType> _types;
        public List<enums.faction> _factions;
        public List<int> _regions;
    }
}
