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
		public short entityID;
		Point focus;
		Vector2 focusVec;
		public bool isFollowing;
		public bool isPanning;
		Point panTarget;
		Vector2 panTargetVec;
		Vector2 translationPerMs;
		float msCounter;

		public Camera()
		{

		}

		public void setFocus(Point tile, Vector2 offset)
		{
			focus = tile;
			focusVec = offset;
		}

		public void setFocus(short entityID)
		{
			this.entityID = entityID;
			isFollowing = true;
		}

		public void releaseFocus()
		{
			entityID = -1;
			isFollowing = isPanning = false;
		}

		public void Pan(Point targetPoint, Vector2 targetVec, float Time)
		{
			Vector2 translation = new Vector2((targetPoint.X - focus.X) * Declaration.tileGameSize + targetVec.X - focusVec.X,
				(targetPoint.Y - focus.Y) * Declaration.tileGameSize + targetVec.Y - focusVec.Y);
			msCounter = 0;

			translationPerMs.X = translation.X / (Time * 1000.0f);
			translationPerMs.Y = translation.Y / (Time * 1000.0f);
			isPanning = true;
		}

		public void Update(Sprite focusSprite)
		{
			focus = focusSprite.tilePos;
			focusVec = focusSprite.Offset;
		}

		public void UpdatePan(float _msCounter)
		{
			msCounter += _msCounter;

			while (msCounter > 0.001f)
			{
				msCounter -= 0.001f;

				focusVec += translationPerMs;
				simplifyTarget();
			}
		}

		private void simplifyTarget()
		{
			if (focusVec.X > Declaration.tileGameSize)
			{
				focus.X += (int)(Math.Floor(focusVec.X / Declaration.tileGameSize));
				focus.X = focus.X % Declaration.tileGameSize;
			}
			else
			{
				while (focusVec.X < 0)
				{
					focus.X--;
					focusVec.X += Declaration.tileGameSize;
				}
			}

			if (focusVec.Y > Declaration.tileGameSize)
			{
				focus.Y += (int)(Math.Floor(focusVec.Y / Declaration.tileGameSize));
				focus.Y = focus.Y % Declaration.tileGameSize;
			}
			else
			{
				while (focusVec.Y < 0)
				{
					focus.Y--;
					focusVec.Y += Declaration.tileGameSize;
				}
			}
		}

	}
}
