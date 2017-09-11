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
	public class Camera
	{

		//Fix the origin vector 
		Point screenSize;
		public Point halfScreenSize;
		public Point origin;
		public Vector2 originVec;

		public Point lastLoc;
		public Vector2 lastLocVec;

		public bool cinematicMode;
		public Vector2 gameSpriteSizeOver2;
		public short entityID;
		public Point focus;
		public Vector2 focusVec;
		public bool isFollowing;
		public bool isPanning;
		Point panTarget;
		Vector2 panTargetVec;
		Vector2 translationPerMs;
		float msCounter;
		float panTime;
		public Vector2 translationThisUpdate;

		public Camera(Point screen)
		{
			isFollowing = isPanning = false;
			halfScreenSize = new Point(screen.X / 2, screen.Y / 2);
		}

		public void activateCinematicMode()
		{
			cinematicMode = true;
		}

		public void deactivateCinematicMode()
		{
			cinematicMode = false;
		}

		private void calculateScreenOrigin()
		{
			origin = new Point(focus.X - (int)(halfScreenSize.X / Declaration.tileGameSize),
				focus.Y - (int)(double)(halfScreenSize.Y / Declaration.tileGameSize));
	//        origin = new Point(focus.X - (int)Math.Ceiling((double)(halfScreenSize.X / Declaration.tileGameSize)),
	//focus.Y - (int)Math.Ceiling((double)(halfScreenSize.Y / Declaration.tileGameSize)));
			originVec = focusVec;
			originVec.X += halfScreenSize.X % Declaration.tileGameSize;
			originVec.Y += halfScreenSize.Y % Declaration.tileGameSize;
			origin.X += (int)(originVec.X / Declaration.tileGameSize);
			origin.Y += (int)(originVec.Y / Declaration.tileGameSize);
			originVec.X = originVec.X % Declaration.tileGameSize;
			originVec.Y = originVec.Y % Declaration.tileGameSize;
		}

		//Focus entity does not stay in exact middle after screen expand!?
		public void onScreenChange(Point newScreen, Vector2 halfChange)
		{
			screenSize = newScreen;
			halfScreenSize = new Point(newScreen.X / 2, newScreen.Y / 2);
			//calculateScreenOrigin();
			originVec.X -= halfChange.X;
			originVec.Y -= halfChange.Y;
		}

		public void setFocus(short entityID, Point entityPos, Vector2 entityPosVec, Vector2 halfSpriteSize)
		{
			this.entityID = entityID;
			gameSpriteSizeOver2 = halfSpriteSize;
			isFollowing = true;
			isPanning = false;
			lastLocVec = focusVec;
			lastLoc = focus;
			focus = entityPos;
			focusVec = entityPosVec;
			simplifyTarget();
			calculateScreenOrigin();

			//Calculate vector

			//Force calculate local spaces, update object manager groups
		}

		public void setFocus(Collidable_Sprite entity)
		{
			short entityID = entity.ID;
			Point entityPos = entity.tilePos;
			Vector2 entityPosVec = entity.Offset;
			setFocus(entityID, entityPos, entityPosVec, Vector2.Zero);
		}

		public void releaseFocus()
		{
			entityID = -1;
			isFollowing = isPanning = false;
		}

		/// <summary>
		/// Sets the camera to pan. 0 time = instant
		/// </summary>
		/// <param name="targetPoint"></param>
		/// <param name="targetVec"></param>
		/// <param name="Time"></param>
		public void Pan(Point targetPoint, Vector2 targetVec, float Time)
		{
				Vector2 translation = new Vector2((targetPoint.X - focus.X) * Declaration.tileGameSize + targetVec.X - focusVec.X,
				(targetPoint.Y - focus.Y) * Declaration.tileGameSize + targetVec.Y - focusVec.Y);
			msCounter = 0;

			if (Time > 0)
			{
				translationPerMs.X = translation.X / (Time * 1000.0f);
				translationPerMs.Y = translation.Y / (Time * 1000.0f);
			}
			else
			{
				translationPerMs = translation;
			}
			isPanning = true;
			isFollowing = false;
			entityID = -1;
			panTime = Time;
		}

		//If the camera is focused on an entity
		public void Update(Sprite focusSprite)
		{
			lastLoc = focus;
			lastLocVec = focusVec;
			focus = focusSprite.tilePos;
			focusVec = focusSprite.Offset +gameSpriteSizeOver2;
			translationThisUpdate = toLocalSpace(focus, focusVec) - toLocalSpace(lastLoc, lastLocVec);

			//Move the origin by the translation of the camera
			updateOrigin();	
			
		}

		private void updateOrigin()
		{
			//origin.X += (int)(Math.Floor(translationThisUpdate.X / Declaration.tileGameSize));
			//origin.Y += (int)(Math.Floor(translationThisUpdate.Y / Declaration.tileGameSize));
			//originVec.X += translationThisUpdate.X % Declaration.tileGameSize;
			//originVec.Y += translationThisUpdate.Y % Declaration.tileGameSize;

			originVec += translationThisUpdate;
			origin.X += (int)(Math.Floor(originVec.X / Declaration.tileGameSize));
			origin.Y += (int)(Math.Floor(originVec.Y / Declaration.tileGameSize));
			if (originVec.X >= 0) originVec.X = originVec.X % Declaration.tileGameSize;
			else originVec.X = Declaration.tileGameSize - (Math.Abs(originVec.X) % Declaration.tileGameSize);

			if (originVec.Y >= 0) originVec.Y = originVec.Y % Declaration.tileGameSize;
			else originVec.Y = Declaration.tileGameSize - (Math.Abs(originVec.Y) % Declaration.tileGameSize);
		}

		//If the camera is panning to a specific position
		public void UpdatePan(float _msCounter)
		{
			msCounter += _msCounter;
			//Check if pan has finished
			if (msCounter > panTime)
			{
				//Don't update over the pan time
				_msCounter -= msCounter - panTime;
				isPanning = false;
			}

			lastLoc = focus;
			lastLocVec = focusVec;

			if (panTime > 0)
				focusVec += translationPerMs * (_msCounter / 0.001f);
			else focusVec += translationPerMs;

			simplifyTarget();
			translationThisUpdate = toLocalSpace(focus, focusVec) - toLocalSpace(lastLoc, lastLocVec);
			updateOrigin();
		}


		private void simplifyTarget()
		{
			if (focusVec.X > Declaration.tileGameSize)
			{
				focus.X += (int)(Math.Floor(focusVec.X / Declaration.tileGameSize));
				focusVec.X = focusVec.X % Declaration.tileGameSize;
			}
			else
			{
				if (focusVec.X < 0)
				{
					focus.X += (int)(Math.Floor(focusVec.X / Declaration.tileGameSize));
					focusVec.X = Declaration.tileGameSize - (Math.Abs(focusVec.X) % Declaration.tileGameSize);
				}
			}

			if (focusVec.Y > Declaration.tileGameSize)
			{
				focus.Y += (int)(Math.Floor(focusVec.Y / Declaration.tileGameSize));
				focusVec.Y = focus.Y % Declaration.tileGameSize;
			}
			else
			{
				if (focusVec.Y < 0)
				{
					focus.Y += (int)(Math.Floor(focusVec.Y / Declaration.tileGameSize));
					focusVec.Y = Declaration.tileGameSize - (Math.Abs(focusVec.Y) % Declaration.tileGameSize);
				}
			}
		}

		public Vector2 toLocalSpace(Point trgt, Vector2 trgtVec)
		{
			Vector2 local;
			local.X = (trgt.X - origin.X) * Declaration.tileGameSize;
			local.X += trgtVec.X - originVec.X;
			local.Y = (trgt.Y - origin.Y) * Declaration.tileGameSize;
			local.Y += trgtVec.Y - originVec.Y;
			return local;
		}

		public Vector2 toLocalSpace(Point cell, Point trgt, Vector2 trgtVec)
		{
			Vector2 local;
			local.X = (trgt.X + cell.X * Functions.tilesPerSector - origin.X) * Declaration.tileGameSize;
			local.X += trgtVec.X - originVec.X;
			local.Y = (trgt.Y + cell.Y * Functions.tilesPerSector - origin.Y) * Declaration.tileGameSize;
			local.Y += trgtVec.Y - originVec.Y;
			return local;
		}

	}
}
