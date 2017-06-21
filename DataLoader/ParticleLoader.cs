using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DataLoader
{
	public class ParticleLoader : IProxyObject 
    {
        public string name;
        public string particleTex;
        public int maxParticles;
        public bool timed;
        public float duration;
		public int spriteID;

        public Vector2 startLife;
        public Vector2 spawnJitter;
        public Vector2 SecPerSpawn;
        public Vector2 SpawnDirection;
        public Vector2 SpawnNoiseAngle;
        public Vector2 startScale;
        public Vector2 endScale;
        public Vector2 startSpeed;
        public Vector2 endSpeed;
        public float gravity;
        public Color startColour1;
        public Color startColour2;
        public Color endColour1;
        public Color endColour2;
    }
}
