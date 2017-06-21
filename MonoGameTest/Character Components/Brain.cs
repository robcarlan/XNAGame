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
using WindowsGame1.Utility;

namespace WindowsGame1
{
    public class Brain
    {
        //Contains AI information, 
		public Navmesh navmesh; //Referemce for functions
		public Character_Components.Character entity;
        public bool isWalking;
        public bool hasGoal;
        public Vector2 targetPos;
        public short targetEntity;
		public float angle = 0;
		public Vector2 walkDirection = Vector2.Zero;
		public string aiBehaviour = null;

        LinkedList<Vector2> path = new LinkedList<Vector2>();
		public bool drawPath = true;
        //Have a function to generate a goal, i.e target a player
        //Have subgoals, i.e use a spell

		public Brain(Character_Components.Character character, Navmesh _navmesh)
		{
			entity = character;
			navmesh = _navmesh;
		}

		//Tick is per 0.1 seconds
		public void updateAI(ObjManager world)
		{
			//If behaviour = null, look for ai function
			if (path.Count > 0)
				calculateWalkDirection();
		}

		//Once per frame
		public void updateAIFrame(ObjManager world)
		{
			if (path.Count > 0)
			{
				isWalking =	followPath();
			}
		}

		public bool followPath()
		{
			//Returns false if path is completed
			//Go in direction of walkDirection, display animation
			return true;
		}

		public void Draw(GraphicsDevice device, Effect navEffect)
		{
			//Draw path
			if (path.Count > 0)
			{
				VertexPositionColor[] verts = new VertexPositionColor[path.Count + 1];
				//Initial node is character position
				verts[0] = new VertexPositionColor(new Vector3(entity.circleOrigin.X, entity.circleOrigin.Y, 0), Color.Red);
				int i = 1;
				for(LinkedListNode<Vector2> itr = path.First; itr != null; itr = itr.Next)
				{
					verts[i] = new VertexPositionColor(new Vector3(itr.Value.X, itr.Value.Y, 0), Color.Red);
					i++;
				}

				device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, verts, 0, verts.Length - 1);
			}
		}

		public bool moveTo(Vector2 destination)
		{
			LinkedList<Vector2> temp = navmesh.getPath(entity.circleOrigin, destination);
			if (temp == null)
			{
				Functions.WriteDebugLine("Error generating path for entity " + entity.ID + ".");
				return false;
			}
			else
			{
				path = temp;
				hasGoal = true;
				isWalking = true;
				calculateWalkDirection();
				return true;
			}
		}

		public void calculateWalkDirection()
		{
			Vector2 difference = path.First.Value - entity.circleOrigin;
			angle = (float)Math.Atan2(difference.Y, difference.X);
			walkDirection = Functions.toUnitDirection(angle);

			//Calculate walk animation
		}

		public void onTranslatation(Vector2 translation)
		{
			for (LinkedListNode<Vector2> i = path.First; i != null; i = i.Next)
				i.Value -= translation;
		}
    }
}
