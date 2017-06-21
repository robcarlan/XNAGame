using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLoader
{
    //Every type of character
    public enum characterType{ Human, Undead, Animal, Magic};
	public enum factions { Imperial, Beast, Rebel };

    class CharacterLoader : CollidableLoader
    {
		public string characterTemplateID;
		public string baseAnimation;
		public string gameName;

		public Boolean invincible;

		//Character Stats
       // public List<characterType> characterType;
		public List<factions> factions;
    }

    class UniqueCharacterLoader
    {
        string ID;
    }

	class NPCLoader : CharacterLoader
	{

	}

	class EnemyLoader : NPCLoader
	{

	}
}
