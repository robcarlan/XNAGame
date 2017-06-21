using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    public class Brain
    {
        //Contains AI information, 
        public bool isWalking;
        public bool hasGoal;
        public Vector3 targetPos;
        public short targetEntity;
        LinkedList<Vector3> path = new LinkedList<Vector3>();

        //Have a function to generate a goal, i.e target a player
        //Have subgoals, i.e use a spell
    }
}
