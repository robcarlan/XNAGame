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

namespace WindowsGame1
{
	namespace Settings
	{
		public enum particleCount
		{
			very_low,
			low,
			medium,
			high,
			full
		}

		public static class funcs
		{
			public static float getParticleCountModifier( particleCount count )
			{
				switch (count)
				{
					case particleCount.very_low:
						return 0.2f;
					case particleCount.low:
						return 0.35f;
					case particleCount.medium:
						return 0.6f;
					case particleCount.high:
						return 0.8f;
					default:
						return 1.0f;
				}
			}
		}
	}


    //Contains global declarations
    public static class enums
    {
        public enum textType { damage, poison, fire, ice, healing, money, item, exp, mana, statGain, statLoss, effectGain, effectLost };
        public enum faction { imperial, none, enemy };
        public enum characterType { human, creature, ghost }
        public enum textLocation { above, below, left, right, inside };

		public enum weaponType { sword, bow, spear, staff }
    }

    public static class Functions
    {
		public const float tileLength = Declaration.tileGameSize;
        public const int tilesPerSector = 64;
        public const int rainYSpawn = -40;
        public const int rainYDespawn = 800;
        public const int rainXDespawn = 1200;
        public const float rainSplashMidX = 6.5f;
        public const float rainSplashMidY = 6f;
        public const float rainSplashScale = 1.5f;

        public static Color _32BitToColour(UInt32 val)
        {
            Color temp = new Color(0,0,0,0);

            temp.R = (byte)((val & 0xff000000) >> 6);
            temp.G = (byte)((val & 0x00ff0000) >> 4);
            temp.B = (byte)((val & 0x0000ff00) >> 4);
            temp.A = (byte)((val & 0x000000ff));
            return temp;
        }

        public static byte LinearInterpolate(byte a, byte b, double t)
        {
            return (byte)(a * (1 - t) + b * t);
        }
        public static float LinearInterpolate(float a, float b, double t)
        {
            return (float)(a * (1 - t) + b * t);
        }

        public static int toGlobalValue(char val)
        {
            return ((int)(val) - 64) * tilesPerSector;
        }

        public static float getRandom(float min, float max, Random rand)
        {
            return min + (float)(rand.NextDouble() * (max - min));
        }

        public static Vector2 getRandom(Vector2 min, Vector2 max, Random rand)
        {
            return new Vector2(
                getRandom(min.X, max.X, rand),
                getRandom(min.Y, max.Y, rand));
        }

        public static Vector2 toGlobalValue(char[] val)
        {
            Vector2 temp;
            temp.X = ((int)(val[0]) - 64) * tilesPerSector;
            temp.Y = ((int)(val[1]) - 64) * tilesPerSector;

            return temp;
        }

        public static float getLengthSqrd(Vector2 origin, Vector2 target)
        {
            return ((origin.X - target.X) * (origin.X - target.X) + (origin.Y - target.Y) * (origin.Y - target.Y));
        }

		public static float getLengthSqrd(Vector3 origin, Vector3 target)
		{
			return ((origin.X - target.X) * (origin.X - target.X) + (origin.Y - target.Y) * (origin.Y - target.Y)
				+ (origin.Z - target.Z) * (origin.Z - target.Z));
		}

        public static Vector2 getDirection(Vector2 position, Vector2 target)
        {
            return Vector2.Normalize(Vector2.Subtract(target, position));
        }

        public static Entity_Components.Dynamic.ParticleEmitter toParticle(DataLoader.ParticleLoader value)
        {
            Entity_Components.Dynamic.ParticleEmitter temp = new Entity_Components.Dynamic.ParticleEmitter(
                value.SecPerSpawn, value.spawnJitter, value.SpawnDirection, value.SpawnNoiseAngle * (MathHelper.Pi / 180f ), value.startScale, value.endScale,
                value.startSpeed, value.endSpeed, value.startColour1, value.startColour2, value.endColour1, value.endColour2,
                value.maxParticles, Vector2.Zero, value.startLife, value.timed, value.duration, value.gravity, value.spriteID,
				null, null);
            temp.texture = value.particleTex;
            temp.parentEntityID = -1;
            return temp;
        }

		public static Entity_Components.Projectile toProjectile(DataLoader.ProjectileLoader value, short id, ObjManager obj)
		{
			Entity_Components.Projectile temp = new Entity_Components.Projectile();
			temp.particles = value.particleEffects;
			temp.ID = id;
			temp.libraryID = id;
			temp.name = value.projName;
			temp.lightID = value.lightID;
			temp.velocityMagnitude = value.velocity;
			temp.anglePerMS = value.anglePerMS;
			temp.collides = value.collidable;
			temp.homing = value.homing;
			temp.effects = new List<CharacterEffect>();
			temp.collisionCircleRadius = value.collisionRadius;
			foreach (short _id in value.characterEffectIDs)
			{
				temp.effects.Add(obj.effectManager.effects[_id]);
			}
			temp.onCollideParticleEffects = value.onCollideParticleEffects;
			temp.animID = value.spriteID;
			return temp;
		}

        public static string WriteDebugLine( string line )
        {
            Console.WriteLine(line);
            return line;
        }

		public static Random seedRand(int seed)
		{
			return new Random(seed);
		}

		public static Random seedRand()
		{
			return new Random();
		}

		public static Vector2 getLocalSpace(Point camOrigin, Vector2 camOriginVec, Point entityLoc, Vector2 entityLocVec )
		{
			return new Vector2(
				(entityLoc.X - camOrigin.X) * tileLength + (entityLocVec.X - camOriginVec.X),
				(entityLoc.Y - camOrigin.Y) * tileLength + (entityLocVec.Y - camOriginVec.Y));
		}

		public static Point getPointWSP(Point cell, Point relativePosition)
		{
			return new Point(
				cell.X * tilesPerSector + relativePosition.X,
				cell.Y * tilesPerSector + relativePosition.Y);
		}

		public static Vector2 toVector2(Rectangle rec, bool middle)
		{
			if (middle) return new Vector2(rec.X + rec.Width / 2, rec.Y + rec.Height / 2);
			else return new Vector2(rec.X, rec.Y);
		}

		public static Vector2 toVector2(Point vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		public static Vector2 toVector2(Vector3 vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		public static Vector3 toVector3(Vector2 vec)
		{
			return new Vector3(vec.X, vec.Y, 0);
		}

		public static Point addPoint(Point one, Point two)
		{
			return new Point(one.X + two.X, one.Y + two.Y);
		}

		public static Point removePoint(Point one, Point toMinus)
		{
			return new Point(one.X - toMinus.X, one.Y - toMinus.Y);
		}

		public static Vector2 toUnitDirection(float angle)
		{
			return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
		}
    }
}