using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.Utility
{

	class NavMeshExtraData
	{
		//min x max x etc
		public float minX;
		public float maxX;
		public float minY;
		public float maxY;
		public float leftX;
		public float rightX;
		public float topY;
		public float bottomY;
		public float width; // Determines whether to check each edge distance when computing the cell's score
		public float height;

		public Vector2 midLeft;
		public Vector2 midRight;
		public Vector2 midTop;
		public Vector2 midBottom;

		public Vector2 normLeft;
		public Vector2 normRight;
		public Vector2 normTop;
		public Vector2 normBottom;

		public void onScreenChange(Vector2 halfChange)
		{
			minX += halfChange.X;
			maxX += halfChange.X;
			minY += halfChange.Y;
			maxY += halfChange.Y;
			leftX += halfChange.X;
			rightX += halfChange.X;
			topY += halfChange.Y;
			bottomY += halfChange.Y;
		}
	}

	[Serializable]
	public class NavMeshVert
	{
		public Point cellPos;
		public Point tilePos;
		public Vector2 Offset;
		[ContentSerializerIgnore]
		public bool valid = true;

		public NavMeshVert()
		{
			cellPos = Point.Zero;
			tilePos = Point.Zero;
			Offset = Vector2.Zero;
			valid = false;
		}

		public void calculateValid()
		{
			valid = !(cellPos == Point.Zero && tilePos == Point.Zero && Offset == Vector2.Zero);
		}
	}

	[Serializable]
	public class Segment
	{
		//Each segment describes a connection between two navmeshcells. This will be two of the four vertices 
		//which form the boundary between two cells
		public Vector2 p1Local;
		public Vector2 p2Local;
		public Vector2 norm;

		public Vector2 getNode(Vector2 source)
		{
			//Returns a node indicating the ideal way to travel to the next cell

			return Vector2.Zero;
		}

		public Segment(Vector2 start, Vector2 end)
		{
			//Convert to local space
			p1Local = start;
			p2Local = end;
			norm = new Vector2(-(start.Y - end.Y), start.X - end.X);
		}
	}

	public class NavMeshCell
	{
		//Cell / tile / offset are stored here
		public NavMeshVert TLpos;
		public NavMeshVert TRpos;
		public NavMeshVert BLpos;
		public NavMeshVert BRpos;

		public int polyID;
		public List<int> connectedCells;	//Contains a list of all cells connected to this one

		[ContentSerializerIgnore]
		Navmesh mesh;
		//public Dictionary<int, Segment> cellConnection = new Dictionary<int,Segment>();	
			//Key = ID of connected cell
		//Cell's are calculated to local space for performance

		[ContentSerializerIgnore]
		public Vector2 TLVert;
		[ContentSerializerIgnore] 
		public Vector2 TRVert;
		[ContentSerializerIgnore]
		public Vector2 BLVert;
		[ContentSerializerIgnore]
		public Vector2 BRVert;
		[ContentSerializerIgnore] 
		public Rectangle bounding;

		NavMeshExtraData data;
		Vector2 Midpoint;

		const float MIN_DISTANCE_CHECK_ALL = 50;
		bool checkEdges;
		[ContentSerializerIgnore]
		public bool isReady = false;

		public NavMeshCell()
		{	//Used by editor, probably not good to use this in game!!!
			connectedCells = new List<int>();
			TLpos = new NavMeshVert();
			TRpos = new NavMeshVert();
			BLpos = new NavMeshVert();
			BRpos = new NavMeshVert();
		}

		public NavMeshCell(List<int> _connected, NavMeshVert _tl, NavMeshVert _tr,
			NavMeshVert _bl, NavMeshVert _br, int _id, Camera cam, Navmesh _mesh)
		{
			mesh = _mesh;
			connectedCells = _connected;
			polyID = _id;
			TLpos = _tl;
			TRpos = _tr;
			BLpos = _bl;
			BRpos = _br;
			TLpos.calculateValid();
			TRpos.calculateValid();
			BLpos.calculateValid();
			BRpos.calculateValid();
			calculateLocalSpace(cam);
			calculateSegments(mesh);
		}

		public int getValidVerts()
		{
			int validVerts = 0;
			if (TLpos.valid) validVerts++;
			if (TRpos.valid) validVerts++;
			if (BLpos.valid) validVerts++;
			if (BRpos.valid) validVerts++;
			return validVerts;
		}

		public NavMeshVert getVert(int i)
		{
			if (i == 0) return TLpos;
			if (i == 1) return TRpos;
			if (i == 2) return BLpos;
			if (i == 3) return BRpos;
			else return null;
		}

		//used to calculate local space and such of the poly
		public void calculateLocalSpace(Camera cam)
		{
			TLVert = cam.toLocalSpace(TLpos.cellPos, TLpos.tilePos, TLpos.Offset);
			TRVert = cam.toLocalSpace(TRpos.cellPos, TRpos.tilePos, TRpos.Offset);
			BLVert = cam.toLocalSpace(BLpos.cellPos, BLpos.tilePos, BLpos.Offset);
			BRVert = cam.toLocalSpace(BRpos.cellPos, BRpos.tilePos, BRpos.Offset);

			float minX = Math.Min(TLVert.X, BLVert.X);
			float maxX = Math.Max(TRVert.X, BRVert.X);
			float minY = Math.Min(TLVert.Y, TRVert.Y);
			float maxY = Math.Max(BLVert.Y, BRVert.Y);

			float width = maxX - minX;
			float height = maxY - minY;

			bounding = new Rectangle((int)minX, (int)minY, (int)(width), (int)(height));

			if (width > MIN_DISTANCE_CHECK_ALL || height > MIN_DISTANCE_CHECK_ALL)
			{
				//Cell is of sufficient size to check each individual edge
				checkEdges = true;
				data = new NavMeshExtraData();
				data.minX = minX;
				data.maxX = maxX;
				data.minY = minY;
				data.maxY = maxY;
				data.width = width;
				data.height = height;
				data.leftX = Math.Max(TLVert.X, BLVert.X);
				data.rightX = Math.Min(TRVert.X, BRVert.X);
				data.topY = Math.Max(TLVert.Y, TRVert.Y);
				data.bottomY = Math.Min(BLVert.Y, BRVert.Y);
				getNormals();
			}
			else checkEdges = false;

			isReady = true;
		}

		public void translate(Vector2 translation)
		{
			bounding.X += (int)translation.X;
			bounding.Y += (int)translation.Y;
			BLVert += translation;
			BRVert += translation;
			TLVert += translation;
			TRVert += translation;

			if (checkEdges)
			{
				//Update extended data
				data.onScreenChange(translation);
			}
		}

		public void getNormals()
		{
			//Generate midpoints for each line
			//Calculate normal vectors for these
			data.midLeft = new Vector2((TLVert.X + BLVert.X) / 2, (TLVert.Y + BLVert.Y) / 2);
			data.midRight = new Vector2((TRVert.X + BRVert.X) / 2, (TRVert.Y + BRVert.Y) / 2);
			data.midTop = new Vector2((TLVert.X + TRVert.X) / 2, (TLVert.Y + TRVert.Y) / 2);
			data.midBottom = new Vector2((BLVert.X + BRVert.X) / 2, (BLVert.Y + BRVert.Y) / 2);

			//left normal will point to the right - scale so x is 1
			Vector2 edgeDirection = new Vector2(data.midLeft.X - TLVert.X, data.midLeft.Y - TLVert.Y);
			edgeDirection.Y = edgeDirection.Y / edgeDirection.X;
			edgeDirection.X = 1.0f;
			data.normLeft = new Vector2(edgeDirection.Y, edgeDirection.X);
			//Top normal will point downwards - scale so y is one
			edgeDirection.X = data.midTop.X - TRVert.X;
			edgeDirection.Y = data.midTop.Y - TRVert.Y;
			edgeDirection.Y = 1.0f;
			edgeDirection.X = edgeDirection.X / edgeDirection.Y;
			data.normTop = new Vector2(edgeDirection.Y, edgeDirection.X);
			//Right normal will point left - scale so x is 1
			edgeDirection.X = data.midRight.X - BRVert.X;
			edgeDirection.Y = data.midRight.Y - BRVert.Y;
			edgeDirection.Y = edgeDirection.Y / edgeDirection.X;
			edgeDirection.X = 1.0f;
			data.normRight = new Vector2(edgeDirection.Y, edgeDirection.X);
			//Bottom normal will point upwards - scale so y is one
			edgeDirection.X = data.midBottom.X - BLVert.X;
			edgeDirection.Y = data.midBottom.Y - BLVert.Y;
			edgeDirection.Y = 1.0f;
			edgeDirection.X = edgeDirection.X / edgeDirection.Y;
			data.normBottom = new Vector2(edgeDirection.Y, edgeDirection.X);
		}

		public void calculateSegments(Navmesh mesh)
		{
			//Called on polygon load
			const float threshold = 15 * MathHelper.TwoPi / 360;
			float maxCos = (float)Math.Cos(threshold);

			foreach (int pairID in connectedCells)
			{
				Point key = mesh.getKey(pairID, polyID);
				if (mesh.cells.ContainsKey(pairID) || mesh.segments.ContainsKey(key))
				{
					List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
					NavMeshCell connectedPoly = mesh.cells[pairID];
					Vector2[] _lines1 = new Vector2[4];
					Vector2[] _lines2 = new Vector2[4];
					float[] angles = new float[8];
					_lines1[0] = new Vector2(TRVert.X - TLVert.X, TRVert.Y - TLVert.Y);
					_lines1[1] = new Vector2(BRVert.X - TRVert.X, BRVert.Y - TRVert.Y);
					_lines1[2] = new Vector2(BRVert.X - BLVert.X, BRVert.Y - BLVert.Y);
					_lines1[3] = new Vector2(BLVert.X - TLVert.X, BLVert.Y - TLVert.Y);

					_lines2[0] = new Vector2(connectedPoly.TRVert.X - connectedPoly.TLVert.X, 
						connectedPoly.TRVert.Y - connectedPoly.TLVert.Y);
					_lines2[1] = new Vector2(connectedPoly.BRVert.X - connectedPoly.TRVert.X,
						connectedPoly.BRVert.Y - connectedPoly.TRVert.Y);
					_lines2[2] = new Vector2(connectedPoly.BLVert.X - connectedPoly.BRVert.X, 
						connectedPoly.BLVert.Y - connectedPoly.BRVert.Y);
					_lines2[3] = new Vector2(connectedPoly.TLVert.X - connectedPoly.BLVert.X, 
						connectedPoly.TLVert.Y - connectedPoly.BLVert.Y);

					//bool isPairLeft = (Midpoint.X > connectedPoly.Midpoint.X);
					//bool isPairAbove = (Midpoint.Y > connectedPoly.Midpoint.Y);

					angles[0] = (float)Math.Atan2(_lines1[0].Y, _lines1[0].X) + MathHelper.Pi;
					angles[1] = (float)Math.Atan2(_lines1[1].Y, _lines1[1].X) + MathHelper.Pi;
					angles[2] = (float)Math.Atan2(_lines1[2].Y, _lines1[2].X) + MathHelper.Pi;
					angles[3] = (float)Math.Atan2(_lines1[3].Y, _lines1[3].X) + MathHelper.Pi;
					angles[4] = (float)Math.Atan2(_lines2[0].Y, _lines2[0].X) + MathHelper.Pi;
					angles[5] = (float)Math.Atan2(_lines2[1].Y, _lines2[1].X) + MathHelper.Pi;
					angles[6] = (float)Math.Atan2(_lines2[2].Y, _lines2[2].X) + MathHelper.Pi;
					angles[7] = (float)Math.Atan2(_lines2[3].Y, _lines2[3].X) + MathHelper.Pi;
					for (int i = 0; i < 4; i++)
					{
						for (int x = 0; x < 4; x++)
						{
							float val = Math.Abs((angles[i] - angles[4 + x]) % MathHelper.TwoPi);
							if (val < threshold || (MathHelper.TwoPi - val) < threshold)
							{
								//Angles are within reason!
								pairs.Add(new Tuple<int, int>(i, x));
							}
						}
					}

					//Plan - create a box surrounding each line w/ 
					if (pairs.Count > 1)
					{
						for (int i = pairs.Count - 1; i >= 0; i--)
						{
							//Get angles of both liens - difference should be pi rads

							//Need to reduce matches
							Vector2 l1 = _lines1[pairs[i].Item1];
							Vector2 l2 = _lines2[pairs[i].Item2];
							Vector2 c1 = getStart(pairs[i].Item1);
							Vector2 c2 = getStart((pairs[i].Item1 + 1) % 4);
							Vector2 c3 = connectedPoly.getStart(pairs[i].Item2);
							Vector2 c4 = connectedPoly.getStart((pairs[i].Item2 + 1) % 4);
							float minDistSqrd = float.PositiveInfinity;
							//minDistSqrd = Math.Min((c4 - c1).LengthSquared(), minDistSqrd);

							//Check if second position lies along first line
							Vector2 del = c2 - c1;
							Vector2 del2 = c4 - c3;
							float ang1, ang2;
							ang1 = (float)Math.Atan2(del.Y, del.X);
							ang2 = (float)Math.Atan2(del2.Y, del2.X);
							float differenceAngle = (ang2 - ang1) % MathHelper.Pi;
							//Angle between starting points is incorrect
							float directionAngle = (angles[pairs[i].Item1] - 
								angles[4 + pairs[i].Item2]) % MathHelper.TwoPi;
							float positionAngle;// = (float)Math.Atan2(del.X, del.Y);
							positionAngle = Math.Abs(Vector2.Dot(del, Vector2.UnitY) / del.Length());
							//Math.Abs(directionAngle - positionAngle) % MathHelper.TwoPi > threshold
							if (!(Math.Abs(differenceAngle) < threshold)) pairs.RemoveAt(i);
						}
					}
					if (pairs.Count == 0)
					{
						Functions.WriteDebugLine("Error calcualting navmesh connection between ID " + pairID + " and " + polyID
							+ ". No Matching line segments found.");
						continue;
					}

					if (pairs.Count > 1)
					{
						Functions.WriteDebugLine("Error calcualting navmesh connection between ID " + pairID + " and " + polyID
							+ ". Multiple pairs found!.");
					}

					//Two matching directions have been found, create segemnt from the minimum of the four positions
					Vector2 v1, v2, v3, v4;
					float v1l, v2l, v3l, v4l;
					v1 = getStart(pairs[0].Item1);
					v2 = getStart((pairs[0].Item1 + 1) % 4);
					v3 = getStart(pairs[0].Item2);
					v4 = getStart((pairs[0].Item2 + 1) % 4);
					v2l = Functions.getLengthSqrd(v1, v2);
					v4l = Functions.getLengthSqrd(v3, v1);
					Vector2 firstSeg = (v2l < v4l) ? v2 : v4;
					v3l = Functions.getLengthSqrd(firstSeg, v3);
					v1l = Functions.getLengthSqrd(firstSeg, v1);
					Vector2 secondSeg = (v3l < v1l) ? v3 : v1;

					Functions.WriteDebugLine("Calculated navmesh connection between ID " + pairID + " and " + polyID);

					Segment newSeg = new Segment(firstSeg, secondSeg);
					mesh.segments.Add(key, newSeg);
					//cellConnection.Add(pairID, newSeg);
				}
				else
				{
					//Poly hasn't been loaded - will be calculated when the other poly is loaded, or has already been 
					//calculated
					continue;
				}
			}
		}

		private Vector2 getStart(int vert)
		{
			if (vert >= 2)
			{
				if (vert == 2) return BRVert;
				else return BLVert;
			}
			else
			{
				if (vert == 0) return TLVert;
				else return TRVert;
			}
		}

		private bool isWithinThreshold(Vector2 _line1, Vector2 _line2, Vector2 _p1, Vector2 _p2)
		{
			return false;
		}

		//Lower score is better
		public float getScore(Vector2 target)
		{
			Vector2 target2D;
			target2D.X = target.X;
			target2D.Y = target.Y;

			//Check if target is inside this node, if so, return best score
			if (isPointInside(target2D)) return 0;

			float currentScore = Functions.getLengthSqrd(Midpoint, target);

			if (checkEdges)
			{
				//Gets the squared length between the closest point on the quad to the target
				//This will be a better score than length from the midpoint, as the target is not in the quad
				currentScore = Functions.getLengthSqrd(getClosestPoint(target), target);
			}

			return currentScore;
		}

		public bool isPointInside(Vector2 target)
		{
			if (target.X < data.minX || target.X > data.maxX || target.Y < data.minY || target.Y > data.maxY)
				return false;

			//for all normal vectors - n.p > 0
			if (Vector2.Dot(data.normLeft, Vector2.Subtract(target, data.midLeft)) > 0)
				return false;
			if (Vector2.Dot(data.normTop, Vector2.Subtract(target, data.midTop)) > 0)
				return false;
			if (Vector2.Dot(data.normRight, Vector2.Subtract(target, data.midRight)) > 0)
				return false;
			if (Vector2.Dot(data.normBottom, Vector2.Subtract(target, data.midBottom)) > 0)
				return false;
			return true;
		}

		#region copyright
//        Copyright (c) 1970-2003, Wm. Randolph Franklin

//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

//    Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimers.
//    Redistributions in binary form must reproduce the above copyright notice in the documentation and/or other materials provided with the distribution.
//    The name of W. Randolph Franklin may not be used to endorse or promote products derived from this Software without specific prior written permission. 

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
#endregion
		public bool pnpoly(Vector2 testPoint)
		{
			Vector2[] vert = new Vector2[4];
			vert[0] = TLVert;
			vert[1] = TRVert;
			vert[2] = BRVert;
			vert[3] = BLVert;

			int i, j = 0;
			bool c = false;
			for (i = 0, j = 3; i < 4; j = i++)
			{
				if (((vert[i].Y > testPoint.Y) != (vert[j].Y > testPoint.Y)) &&
				 (testPoint.X < (vert[j].X - vert[i].X) * (testPoint.Y - vert[i].Y) / (vert[j].Y - vert[i].Y) + vert[i].X))
					c = !c;
			}
			return c;
		}

		//Gets closes point along the edge of the quad to the target vector,
		//used to judge score. Only called for polys with extra data
		public Vector2 getClosestPoint(Vector2 target)
		{
			bool above, below;
			above = below = true;
			if (target.X < data.leftX)
			{
				//Definitely on the left of the polygon
				float tlY = (TLVert.X - target.X) * data.normLeft.Y + TLVert.Y;
				float blY = (BLVert.X - target.X) * data.normLeft.Y + BLVert.Y;

				if (target.Y > tlY && target.Y < blY)
				{
					float frac = (target.Y - tlY) / (blY - tlY);
					return new Vector2(BLVert.X + frac * (TLVert.X - BLVert.X), BLVert.Y + frac * (TLVert.Y - BLVert.Y));
				}

				if (target.Y < tlY) above = true;
				else below = true;
			}
			else if (target.X > data.rightX)
			{
				//Definitely on the right of the polygon
				float trY = (target.X - TRVert.X) * data.normRight.Y + TRVert.Y;
				float brY = (target.X - BRVert.X) * data.normRight.Y + BRVert.Y;

				if (target.Y > trY && target.Y < brY)
				{
					float frac = (target.Y - trY) / (brY - trY);
					return new Vector2(BRVert.X + frac * (TRVert.X - BRVert.X), BRVert.Y + frac * (TRVert.Y - BRVert.Y));
				}

				if (target.Y < trY) above = true;
				else below = true;
			}

			if (target.Y < data.topY)
			{
				//Definitely above the polygon
				float tlX = (TLVert.Y - target.Y) * data.normTop.X + TLVert.Y;
				float trX = (TRVert.Y - target.Y) * data.normTop.X + TRVert.Y;

				if (target.X > tlX && target.X < trX)
				{
					float frac = (target.X - tlX) / (trX - tlX);
					return new Vector2(TLVert.X + frac * (TRVert.X - TLVert.X), TLVert.Y + frac * (TRVert.Y - TLVert.Y));
				}

				if (target.X < tlX)
				{
					//Is left
					return TLVert;
				}
				else
				{
					//Is right
					return TRVert;
				}
			}
			else if (target.Y > data.bottomY)
			{
				//Definitely below the polygon
				float blX = (target.Y - BLVert.Y) * data.normTop.X + BLVert.Y;
				float brX = (target.Y - BRVert.Y) * data.normTop.X + BRVert.Y;

				if (target.X > blX && target.X < brX)
				{
					float frac = (target.X - blX) / (brX - blX);
					return new Vector2(BLVert.X + frac * (BRVert.X - BLVert.X), BLVert.Y + frac * (BRVert.Y - BLVert.Y));
				}

				if (target.X < blX)
				{
					//Is left
					return BLVert;
				}
				else
				{
					//Is right
					return BRVert;
				}
			}

			return Vector2.Zero;
		}
	}

	public class Navmesh
	{
		public Dictionary<Point, Segment> segments
			= new Dictionary<Point, Segment>();
		public Dictionary<int, NavMeshCell> cells 
			= new Dictionary<int,NavMeshCell>(); //Contains cells from all loaded cells!
		public Dictionary<Point, List<int>> cellNavmesh 
			= new Dictionary<Point,List<int>>(); //Contains all navmeshes in the cell. Used to bulk load / unload
		public const int maxIterations = 1000;

		//Temporary variables, cleared when next used.
		int finalNodeID;
		Vector2 target;
		Stack<int> openList = new Stack<int>();
		Stack<int> closedList = new Stack<int>();
		SortedSet<int> checkedCells = new SortedSet<int>();
		public int loadedCells = 0;
		public const int MAX_LOADED_CELLS = 25;

		public Navmesh()
		{
			cells = new Dictionary<int, NavMeshCell>();
		}

		public Point getKey(int node1, int node2)
		{
			if (node1 < node2) return new Point(node1, node2);
			else return new Point(node2, node1);
		}

		public LinkedList<Vector2> getPath(Vector2 start, Vector2 goal)
		{

			//Get the id of the final node
			int startNodeID = getContainingNode(start);
			finalNodeID = getContainingNode(goal);
			target = goal;

			if (startNodeID == -1)
			{
				//This shouldn't happen
				Functions.WriteDebugLine("Pathfinding Error - getPath(): start position is invalid.");
				return null;
			}

			if (finalNodeID == -1)	//Target is impossible to get to
				return null;


			if (startNodeID != finalNodeID)
			{	//Target located in a different cell, so search must be done
				//Reinitialise variables
				checkedCells.Clear();
				openList.Clear();
				closedList.Clear();
				//Begin search
				closedList.Push(startNodeID);
				checkedCells.Add(startNodeID);
				search(startNodeID);

				if (closedList.Count == 1)
				{
					//Path could not be found
					return null;
				}
				else if (closedList.Peek() != finalNodeID)
				{
					//Final node is not connected
					Functions.WriteDebugLine("Could not generate path, nodes are not connected.");
					return null;
				}
				else
				{
					//Build path from cell list
					LinkedList<Vector2> path = new LinkedList<Vector2>();
					//Get nearest vector to edge of connecting cell
					//add to path
					//repeat for the connecting cells
					int startNode = closedList.Pop();
					Vector2 currentPosition = start;
					try
					{
						while (closedList.Count > 0)
						{
							int nextNode = closedList.Pop();
							currentPosition = getBoundaryPosition(currentPosition, startNode, nextNode);
							path.AddFirst(currentPosition);
							startNode = nextNode;
						}
						path.AddLast(goal);
					}
					catch (Exception ex)
					{
						Functions.WriteDebugLine("Could not construct path, does segment exist?");
					}
					return path;
				}
			}
			else
			{	//Target is in the same cell, so the path is trivial
				LinkedList<Vector2> path = new LinkedList<Vector2>();
				path.AddFirst(goal);
				return path;
			}
		}

		public Vector2 getBoundaryPosition(Vector2 pos, int startCell, int nextCell)
		{
			//I'll do this later
			//Get segment, calculate closest position
			Vector2 position;
			Segment boundary = segments[getKey(startCell, nextCell)];
			return (boundary.p1Local + boundary.p2Local) / 2;
		}

		public void search(int cellID)
		{
			//Get neighbours
			SortedList<int, float> neighbours = new SortedList<int, float>();

			for (int neighbourItr = 0; neighbourItr < cells[cellID].connectedCells.Count; neighbourItr++)
			{
				int neighbourID = cells[cellID].connectedCells[neighbourItr];
				if (neighbourID == finalNodeID)
				{
					//Target found
					closedList.Push(neighbourID);
					return;
				}
				else
				{
					if (checkedCells.Contains(neighbourID))
					{
						continue;
					}
					else
					{
						neighbours.Add(neighbourID, cells[neighbourID].getScore(target));
						checkedCells.Add(neighbourID);
					}
				}
			}

			if (neighbours.Count > 0)
			{
				//Search the initial node
				closedList.Push(neighbours.Keys[0]);
				search(neighbours.Keys[0]);
			}

			//Search the neighbouring cells, starting with the lowest score
			for (int i = 1; i < neighbours.Count; i++)
			{
				if (closedList.Peek() == finalNodeID)
				{
					//target has been found, so return
					return;
				}
				else
				{
					//Search the next neighbour
					closedList.Pop();
					closedList.Push(neighbours.Keys[i]);
					search(neighbours.Keys[i]);
				}
			}

			//Target was not found at this node or any children, so return
			return;
		}

		public int getContainingNode(Vector2 pos)
		{
			foreach (NavMeshCell tempItr in cells.Values)
			{
				if(tempItr.pnpoly(pos))
				{
					return tempItr.polyID;
				}
			}

			return -1;
		}

		//used to set initial positions on camera transitions
		public void calculateLocalPositions(Vector2 cameraTranslation)
		{

		}

		//Remove cells which are no longer necessary
		public void unloadCellData(Point cell)
		{
			//Remove each entry from dictionary
			foreach (int itr in cellNavmesh[cell])
			{
				cells.Remove(itr);
			}
			//Finally remove list from loadedCells
			cellNavmesh.Remove(cell);
		}

		//Called by load content every second. Returns a list of points of navmesh data which should be loaded!!!
		public List<Point> queryRequestedCells(Point centerCell)
		{
			List<Point> lol = new List<Point>();
			return lol;
		}

		public void addNavmeshCell(List<NavMeshCell> dataToLoad, Point cell, Camera cam)
		{
			List<int> temp = new List<int>();
			//Create each poly, assign data i.e. id
			foreach (NavMeshCell itr in dataToLoad)
			{
				NavMeshCell newCell = 
					new NavMeshCell(itr.connectedCells, itr.TLpos, itr.TRpos, itr.BLpos, itr.BRpos, itr.polyID, cam,
						this);
				temp.Add(itr.polyID);
				cells.Add(itr.polyID, newCell);
			}

			cellNavmesh.Add(cell, temp);
		}

		public void onResize(int halfWChange, int halfHChange)
		{
			Vector2 offset = new Vector2(halfWChange, halfHChange);
			//Dictionary<Segment, bool> changedSegments = new Dictionary<Segment,bool>();
			foreach (NavMeshCell tempCell in cells.Values)
			{
				tempCell.translate(offset);
			}

			foreach (Segment seg in segments.Values)
			{
				//Segment is referenced multipel times over varying cells - dictionary only processes each segment once.
				//if (!changedSegments.ContainsKey(seg)) 
				//    changedSegments.Add(seg, true);

				seg.p1Local += offset;
				seg.p2Local += offset;
			}

			//Change each segment
			//foreach (Segment seg in changedSegments.Keys)
			//{
			//    seg.p1Local += offset;
			//    seg.p2Local += offset;
			//}
		}

		public void onTranslation(Vector2 translation)
		{
			foreach (NavMeshCell tempCell in cells.Values)
			{
				tempCell.translate(translation);
			}

			foreach (Segment seg in segments.Values)
			{
				seg.p1Local += translation;
				seg.p2Local += translation;
			}
		}
	}
}
