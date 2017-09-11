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
    public class Quadtree
    {
        private Rectangle screenBounds;
        public QuadTreeNode root;
        private Rectangle rootDimensions = new Rectangle(0, 0, 2000, 1600);
        private const short maxDepth = 3;
        private short totalTreeID = 0;
        private Rectangle screenDifference;
        private List<short> collidingQuads;
        public Dictionary<short, QuadTreeNode> NodeList = new Dictionary<short,QuadTreeNode>();

        public Quadtree()
        {
            createTree(root, maxDepth);
        }

        //Make function to get id of all nodes a rectangle intersects

        public Quadtree(bool _initialiseOnCreation, Rectangle _screenBounds)
        {
            updateScreen(_screenBounds);
            constructRoot(_screenBounds);
            collidingQuads = new List<short>();
            createTree(root, 0);
            if (_initialiseOnCreation)
            {

            }
            else
            {

            }
        }

        //Queries whether a rectangle fits inside a rectangle, and returns a child if it fits in that. Returns the int ID to the node.
        public int queryRectangle(ref Rectangle recQuery, ref QuadTreeNode testNode)
        {
            if (NodeList[testNode.BLNodeID].dimensions.Contains(recQuery))
            {
                return testNode.BLNodeID;
            }
            else
                if (NodeList[testNode.BRNodeID].dimensions.Contains(recQuery))
                {
                    return testNode.BRNodeID;
                }
                else
                    if (NodeList[testNode.TLNodeID].dimensions.Contains(recQuery))
                    {
                        return testNode.TLNodeID;
                    }
                    else
                        if (NodeList[testNode.TRNodeID].dimensions.Contains(recQuery))
                        {
                            return testNode.TRNodeID;
                        }
                        else
                            return testNode.nodeID;
                            //Returns the tested node if all tests fail, as it overlaps the child nodes.
        }

        //Make function to insert object in correct node
        /// <param name="IDTest">Use 0 to start from root node (default)</param>
        private short getNode( Rectangle ObjectBounds, short IDTest)
        {
            if (NodeList[IDTest].depth == maxDepth)
            {             
                return IDTest ;
            }

            if (NodeList[NodeList[IDTest].BLNodeID].dimensions.Contains(ObjectBounds))
                IDTest = getNode( ObjectBounds, NodeList[IDTest].BLNodeID);
            else
                if (NodeList[NodeList[IDTest].BRNodeID].dimensions.Contains(ObjectBounds))
                    IDTest =  getNode( ObjectBounds, NodeList[IDTest].BRNodeID);
                else
                    if (NodeList[NodeList[IDTest].TLNodeID].dimensions.Contains(ObjectBounds))
                        IDTest =  getNode( ObjectBounds, NodeList[IDTest].TLNodeID);
                    else
                        if (NodeList[NodeList[IDTest].TRNodeID].dimensions.Contains(ObjectBounds))
                            IDTest = getNode( ObjectBounds, NodeList[IDTest].TRNodeID);

            //If no child contains the object, assign it to the list
            return IDTest;
        }

        public void insertObject(Collidable_Sprite Object)
        {
            Rectangle temp = new Rectangle(Object.localSpace.X + (rootDimensions.Width - screenBounds.Width) / 2, Object.localSpace.Y + (rootDimensions.Height - screenBounds.Height) / 2, Object.localSpace.Width
                , Object.localSpace.Height);
            Object.collisionNode = getNode(temp, 0);
            NodeList[Object.collisionNode].contents.Add(Object);
        }

        // Used when an item goes out of range, pushes it up or down the quadtree


        private short pushItemUp(Rectangle bounds, short _NodeID)
        {

            if (NodeList[_NodeID].dimensions.Contains(bounds))
            {
                _NodeID = pushItemUp(bounds, NodeList[_NodeID].parentNodeID);
            }
            return _NodeID;
        }

        public short startPushItemUp(Rectangle bounds, short _NodeID)
        {
            Rectangle temp = new Rectangle((int)(bounds.X + screenDifference.X), (int)(bounds.Y + screenDifference.Y),
                bounds.Width, bounds.Height);
            return pushItemUp(temp, _NodeID);
        }

		public List<int> getCollidingObjects(Vector2 point, float radius)
		{
			//Returns all items colliding with this object.
			List<int> objects = new List<int>();
			return objects;
		}

        public void updateItem( Collidable_Sprite entity)
        {
            /* Only called on moving objects - if the object cannot be contained by its quad, keeps pushing it up until it can,
               then pushes it back down as far as possible!
             */

            Rectangle temp = new Rectangle( (int)(entity.localSpace.X + screenDifference.X), (int)(entity.localSpace.Y + screenDifference.Y), 
                entity.localSpace.Width, entity.localSpace.Height);
            int tempNode;
            if ((tempNode = getNode(temp, 0)) != entity.collisionNode)
            {
                NodeList[entity.collisionNode].contents.Remove(entity);
                entity.collisionNode = getNode(temp, 0);
                NodeList[entity.collisionNode].contents.Add(entity);
            }

        }

        public void testCollisions( Collidable_Sprite entity)
        {
            short tempNode = entity.collisionNode;

            do
            {
                for (int counter = 0; counter < NodeList[tempNode].contents.Count; counter++)
                {
					if (entity.ID == NodeList[tempNode].contents[counter].ID)
                            continue; //Check to make sure it's not testing with itself

					if (NodeList[tempNode].contents[counter].useCollisionBox == false && entity.useCollisionBox == false)
					{	//Evaluate with circle / circle
						if (entity.doesCollide( NodeList[tempNode].contents[counter].circleOrigin, NodeList[tempNode].contents[counter].collisionCircleRadius ))
							entity.ResolveCircleCollision( ref NodeList[tempNode].contents[counter].circleOrigin, 
								ref NodeList[tempNode].contents[counter].localSpace,
								ref NodeList[tempNode].contents[counter].vel,
								ref entity.collisionCircleRadius );
						return;
					}
                    if (NodeList[tempNode].contents[counter].localSpace.Intersects(entity.localSpace)) // Only if both rectangles 
                    {   //Rectangular collision has occured!
                             
                      entity.ResolveRectangleCollision(ref NodeList[tempNode].contents[counter].localSpace,
                                                             ref NodeList[tempNode].contents[counter].vel);
                    }
                }
                //Advance up the tree
                //Add methods to go further down the tree, while it intersects.
                tempNode = NodeList[tempNode].parentNodeID;
            } while (tempNode >= 0);
        }

		public List<short> getCollidingEntities(Vector2 origin, float radius)
		{
			List<short> lol = new List<short>();
			return lol;
		}

		public short getItemFromCursorPos(Point CursorPosition)
		{
			//Transform queryPoint
			CursorPosition.Y += screenDifference.Y;
			CursorPosition.X += screenDifference.X;
			return getNode(CursorPosition, root.nodeID, 0).ID;
		}

		private struct objectDepthID
		{
			public float depth;
			public short ID;
		}

		private objectDepthID getNode( Point queryPoint, short nodeID, short depth )
		{
			objectDepthID currentObject;
			//Set error values to start
			currentObject.depth = 2f;
			currentObject.ID = -1;

			if (!NodeList[nodeID].dimensions.Contains(queryPoint))
				return currentObject;

			float lowestDepth = 1f;
			short itemIndexCounter = 0;
			objectDepthID functionReturnObject;
			Point relativePoint = new Point(queryPoint.X - screenDifference.X, queryPoint.Y - screenDifference.Y);

			for (itemIndexCounter = 0; itemIndexCounter < NodeList[nodeID].contents.Count; itemIndexCounter++)
			{	//Iterate through each item, check for items containing point
				if ( NodeList[nodeID].contents[itemIndexCounter].localSpace.Contains(relativePoint) )
					//Rectangle contains Point
				{	//Get depth, compare against current best
					float objectDepth = NodeList[nodeID].contents[itemIndexCounter].localSpace.Y / screenBounds.Height;
					if ( objectDepth < lowestDepth)
					{	//Replace Object, change lowest depth
						currentObject.depth = objectDepth;
						currentObject.ID = NodeList[nodeID].contents[itemIndexCounter].ID;
						lowestDepth = objectDepth;
					}
				}
			}

			if (depth >= maxDepth)
				return currentObject;
			else depth++;

			//Recursively check each child node
			functionReturnObject = getNode(queryPoint, NodeList[nodeID].BLNodeID, depth);
			if (functionReturnObject.depth < lowestDepth)
				currentObject = functionReturnObject;

			functionReturnObject = getNode(queryPoint, NodeList[nodeID].BRNodeID, depth);
			if (functionReturnObject.depth < lowestDepth)
				currentObject = functionReturnObject;

			functionReturnObject = getNode(queryPoint, NodeList[nodeID].TLNodeID, depth);
			if (functionReturnObject.depth < lowestDepth)
				currentObject = functionReturnObject;

			functionReturnObject = getNode(queryPoint, NodeList[nodeID].TRNodeID, depth);
			if (functionReturnObject.depth < lowestDepth)
				currentObject = functionReturnObject;


			return currentObject;
		}

		private void createNode(ref QuadTreeNode _Node)
        {
            //initialise children

            //Top Left
            NodeList.Add( totalTreeID++, new QuadTreeNode(new Rectangle(_Node.dimensions.X, _Node.dimensions.Y, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.TLNodeID = totalTreeID;
            //Top Right
            NodeList.Add( totalTreeID++, new QuadTreeNode(new Rectangle(_Node.dimensions.X + _Node.dimensions.Width / 2, _Node.dimensions.Y, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.TRNodeID = totalTreeID;
            //Bottom Left
            NodeList.Add( totalTreeID++, new QuadTreeNode(new Rectangle(_Node.dimensions.X, _Node.dimensions.Y + _Node.dimensions.Height / 2, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.BLNodeID = totalTreeID;
            //Bottom Right
            NodeList.Add( totalTreeID++, new QuadTreeNode(new Rectangle(_Node.dimensions.X + _Node.dimensions.Width / 2, _Node.dimensions.Y + _Node.dimensions.Height / 2, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.BRNodeID = totalTreeID;
            _Node.isNode = true;
        }

        private void createTree(QuadTreeNode _Node, int depth)
        {
            //initialise children
            if (depth == maxDepth) return;

            //Top Left
            NodeList.Add(++totalTreeID, new QuadTreeNode(new Rectangle(_Node.dimensions.X, _Node.dimensions.Y, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.TLNodeID = totalTreeID;

            createTree(NodeList[totalTreeID], depth + 1);
            //Top Right
            NodeList.Add(++totalTreeID, new QuadTreeNode(new Rectangle(_Node.dimensions.X + _Node.dimensions.Width / 2, _Node.dimensions.Y, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.TRNodeID = totalTreeID;

            createTree(NodeList[totalTreeID], depth + 1);
            //Bottom Left
            NodeList.Add(++totalTreeID, new QuadTreeNode(new Rectangle(_Node.dimensions.X, _Node.dimensions.Y + _Node.dimensions.Height / 2, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.BLNodeID = totalTreeID;

            createTree(NodeList[totalTreeID], depth + 1);
            //Bottom Right
            NodeList.Add(++totalTreeID, new QuadTreeNode(new Rectangle(_Node.dimensions.X + _Node.dimensions.Width / 2, _Node.dimensions.Y + _Node.dimensions.Height / 2, _Node.dimensions.Width / 2,
                _Node.dimensions.Height / 2), _Node.nodeID, ref totalTreeID, _Node.depth));
            _Node.BRNodeID = totalTreeID;

            createTree(NodeList[totalTreeID], depth + 1);
            _Node.isNode = true;
        }

        public void constructRoot(Rectangle _screenBounds)  
        {
            screenBounds = _screenBounds;
            root = new QuadTreeNode();
            root.dimensions = rootDimensions;
            root.nodeID = 0;
            root.parentNodeID = -1;
            root.depth = 0;
            NodeList.Add(totalTreeID, root);
        }

        public void updateScreen(Rectangle _screenBounds)
        {
            screenBounds = _screenBounds;
            screenDifference.X = (rootDimensions.Width - screenBounds.Width) / 2;
            screenDifference.Y = (rootDimensions.Height - screenBounds.Height) / 2;
        }
    }
}
