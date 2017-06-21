using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    enum direction { north, northeast, east, southeast, south, southwest, west, northwest };

    public class NPC : Character_Components.Character
    {
        public List<enums.characterType> Type;
        public enums.faction Faction;
        public Brain brain = new Brain();
        public bool unique;

        void walkNorth()
        {
            setDirection('n');
            queueAdvance();
            vel.Y = -2;
        }
        void walkSouth()
        {
            setDirection('s');
            queueAdvance();
            vel.Y = 2;
        }
        void walkEast()
        {
            setDirection('e');
            queueAdvance();
            vel.X = 2;
        }
        void walkWest()
        {
            setDirection('w');
            queueAdvance();
            vel.X = -2;
        }
    }

    class CombatNPC : NPC
    {
        Combat combat;

        //Ratehr than actually storing the effects, contain a list of strings of active effects. When an effect is
        //Applied, find the stats from a file, and then change the npc's stats from there.
        public void RemoveEffect(string effect)
        {
            
        }

        public void ReduceEffectQuantity(string effect)
        {

        }
    }

    class Enemy : CombatNPC
    {
        bool hostile;

    }
}
