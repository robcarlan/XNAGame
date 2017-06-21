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
using WindowsGame1;
using WindowsGame1.Character_Components;

namespace WindowsGame1.Entity_Components
{
	public class Projectile : Collidable_Sprite
	{
		//Lifetime
		//Fixed direction / variable
		//Has target (implies variable dir)
		//speed
		//

		//Projectile which follows a direction until it collides or exceeds local space
		public Character parent; //ID of character which caused the particle
		public Character target;
			//Determines who is damaged by it

		public bool homing = false;
		public float velocityMagnitude;
		public float durationLeft = 0;
		public const float defaultDuration = 1000000;
		public short libraryID;

		public List<CharacterEffect> effects;	//List of effects which occur on hit to enemy
		//List<short> particleEffects;
		public List<short> onCollideParticleEffects;
		public short lightID; //For a library projectile, indicates the type of light to add.
			//For an existing projectile, indicates the attached light.

		Character_Components.Character owner; //Used to get damage etc.

		public float angle;	//0 = upwards, measured in radians
		public float anglePerMS = 0; //Indicated the change in angle per ms

		public Projectile(Projectile _base, short id, Character Parent, Character Target, float startDir,
			List<short> _particles)
		{
			effects = _base.effects;
			particles = _particles;
			onCollideParticleEffects = _base.onCollideParticleEffects; 
			lightID = _base.lightID;
			homing = _base.homing;
			velocityMagnitude = _base.velocityMagnitude;
			collides = _base.collides;
			collisionCircleRadius = _base.collisionCircleRadius;
			ID = id;
			libraryID = _base.ID;
			animID = _base.animID;
			shadowID = 1;
			anglePerMS = _base.anglePerMS;
			name = _base.name;

			if (Target != null) target = Target;
			parent = Parent;
			angle = startDir;

			zPos = Declaration.projZPos;
			durationLeft = defaultDuration;
			localSpace.X = (int)parent.circleOrigin.X;
			localSpace.Y = (int)parent.circleOrigin.Y;
			tilePos = parent.tilePos;
			Offset = parent.Offset;
		}

		public Projectile(float vel, short lightEntityID, Character parent, Character target) 
		{
			//vel = new Vector2((float)Math.Sin(vel), (float)Math.Cos(vel));
		}

		public Projectile() { 
			zPos = Declaration.projZPos;
			lightID = -1;
		}

		//Event handlers

		public void onCollideEntity(short entID)
		{	//Currently called from objManager
			//If collides = true, this may not be the target entity
		}

		public void onCreateEntity(short entID)
		{

		}

		public void initialise()
		{
			//Set common variables
			//Set direction to be current facing / mouse position
			useCollisionBox = false;
			collides = true;
			collisionCircleRadius = 10;

			if (durationLeft == 0)
				durationLeft = defaultDuration;
		}

		public short Update(float msPassed, ObjManager obj)
		{
			//Update position
			if (homing)
			{
				Vector2 targetDirection = new Vector2(localSpace.X - target.localSpace.X, localSpace.Y - target.localSpace.Y);
				float angleTarget = (float)Math.Atan2(-targetDirection.X, -targetDirection.Y);
				//Alter direction
				float angleChange = msPassed * anglePerMS;
				float difference = MathHelper.WrapAngle(angleTarget - angle);
				difference = MathHelper.Clamp(difference, -angleChange, angleChange);
				angle = MathHelper.WrapAngle(angle + difference);

				//Update direction
				Offset.X += (int)(velocityMagnitude * Math.Sin(angle));//0 rads = upwards,
				Offset.Y += (int)(velocityMagnitude * Math.Cos(angle));
			}
			else
			{
				//Carry on in same direction
				Offset.X += (int)(velocityMagnitude * Math.Sin(angle));//0 rads = upwards,
				Offset.Y += (int)(velocityMagnitude * Math.Cos(angle));
			}
			circleOrigin.X = localSpace.X;
			circleOrigin.Y = localSpace.Y;
			durationLeft -= msPassed;
			//Check for collisions
			if ((circleOrigin - Functions.toVector2(target.localSpace.Center)).LengthSquared() < 25 * 25)
			{
				circleOrigin = Functions.toVector2(target.localSpace.Center);
				return target.ID;
			}
			//return colliding entity
			//If collided, apply effects and particles. Time out current particles
			
			//Check to see if the projectile is still valid
			if (durationLeft <= 0 || ((int)(circleOrigin.LengthSquared()) > 2000 * 2000))
			{
				//Projectile needs to be destroyed
				return -1;
			}

			//Update base, i..e animation

			//return result;
			return -2;
		}
	}
}
