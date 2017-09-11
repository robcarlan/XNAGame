using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public enum direction { north, northeast, east, southeast, south, southwest, west, northwest };

    public class NPC : Character_Components.Character
    {
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
}
