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
    }

    public static class Functions
    {
        public const int tilesPerSector = 128;
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

        public static Vector2 getDirection(Vector2 position, Vector2 target)
        {
            return Vector2.Normalize(Vector2.Subtract(target, position));
        }

        public static Entity_Components.Dynamic.ParticleEmitter toParticle(DataLoader.ParticleLoader value)
        {
            Entity_Components.Dynamic.ParticleEmitter temp = new Entity_Components.Dynamic.ParticleEmitter(
                value.SecPerSpawn, value.SpawnDirection, value.SpawnNoiseAngle * (MathHelper.Pi / 180f ), value.startScale, value.endScale,
                value.startSpeed, value.endSpeed, value.startColour1, value.startColour2, value.endColour1, value.endColour2,
                value.maxParticles, value.relPosition, value.startLife, value.timed, value.duration, value.gravity, null, null);
            temp.texture = value.particleTex;
            temp.parentEntityID = -1;
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

    }
}