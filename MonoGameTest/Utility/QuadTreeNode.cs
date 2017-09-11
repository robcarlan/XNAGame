using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    public class QuadTreeNode
    {
        public bool isEmpty = true; //True if this node and all its children are empty. Used to prevent testing
        public bool isNode = false; //Else is a leaf!
        public Rectangle dimensions;
        
        // ID for child nodes
        public short BLNodeID;
        public short BRNodeID;
        public short TLNodeID;
        public short TRNodeID;
        public List<Collidable_Sprite> contents = new List<Collidable_Sprite>();

        public byte depth { get; set; }                     //The depth of this node
        public short parentNodeID { get; set; }  //Used to iterate through nodes
        public short nodeID { get; set; }        //The ID of this node

        public QuadTreeNode() { }
        public QuadTreeNode(Rectangle _dimensions, short _parentNodeID, ref short _totalTreeID, byte parentDepth)
        {
            dimensions = _dimensions;
            parentNodeID = _parentNodeID;

            //Get the next free ID, and assign it to the node
           // _totalTreeID++;
            nodeID = _totalTreeID;
            depth = ++parentDepth;
        }

        public bool addObject(ref Sprite entity)
        {
            //Compare bounding box with children nodes, then pass the function on to them, else add it to this node.
            return true;
        }
    }
}
