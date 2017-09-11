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


namespace WindowsGame1.Entity_Components.Dynamic
{
    public class ParticleSystem
    {
		public Dictionary<int, Rectangle> Sprites;
		public Texture2D particleTex;
        public List<ParticleEmitter> EmitterList;
        Random random;
        public Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { LastPos = position; position = value; }
        }
        public Vector2 LastPos;

        public ParticleSystem(Vector2 Position, ContentManager content)
        {
            this.position = Position;
            LastPos = position;
            random = new Random();
            EmitterList = new List<ParticleEmitter>();
			Sprites = content.Load<Dictionary<int, Rectangle>>("Object Databases\\ParticleSprites");
			particleTex = content.Load<Texture2D>("Sprite Content\\Particle");
			Sprites = Game1.LoadContent<Dictionary<int, Rectangle>>(Declaration.particleSpriteLoaderPath + ".xml");
        }

        public void Update(float msPassed)
        {
            for (int i = 0; i < EmitterList.Count; i++)
            {
                if (EmitterList[i].maxParticles > 0)
                {
                    EmitterList[i].update(msPassed);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int Scale, Vector2 Offset)
        {
            for (int i = 0; i < EmitterList.Count; i++)
            {
                if (EmitterList[i].maxParticles > 0)
                {
                    EmitterList[i].draw(spriteBatch, Scale, Offset);
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < EmitterList.Count; i++)
            {
                if (EmitterList[i].maxParticles > 0)
                {
                    EmitterList[i].clear();
                }
            }
        }

        public void AddEmitter(Vector2 SecPerSpawn, Vector2 spawnJitter, Vector2 SpawnDirection, Vector2 SpawnNoiseAngle, Vector2 StartLife, Vector2 StartScale,
                    Vector2 EndScale, Color StartColor1, Color StartColor2, Color EndColor1, Color EndColor2, Vector2 StartSpeed,
                    Vector2 EndSpeed, int maxParticles, Vector2 RelPosition, bool timed, float duration, float gravity, int sprID,  Texture2D ParticleSprite)
        {
            ParticleEmitter emitter = new ParticleEmitter(SecPerSpawn, spawnJitter, SpawnDirection, SpawnNoiseAngle,
                                        StartScale, EndScale, StartSpeed, EndSpeed, StartColor1, StartColor2, EndColor1, EndColor2, 
                                        maxParticles, RelPosition, StartLife, timed, duration, gravity, sprID, particleTex, this); 
            EmitterList.Add(emitter);
        }

        public void setDirection(Vector2 direction)
        {
            for (int i = 0; i < EmitterList.Count; i++)
            {
                EmitterList[i].setDirection(direction);
            }
        }
    }
}
