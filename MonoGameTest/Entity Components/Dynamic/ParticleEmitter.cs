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
using System.Xml;


namespace WindowsGame1.Entity_Components.Dynamic
{
	//Add derived class which allows for circular emitter


	public class ParticleEmitter
	{
        public bool hasLight;
        public PointLight light;

        public ParticleSystem parent;
        LinkedList<Particle> particles;
        LinkedListNode<Particle> nodeIterator;
        public Random random;
        public string texture;
        public Texture2D particleImage;
		public int spriteID;
		public Dictionary<int, Rectangle> sprites;
        public int maxParticles;
        float secPassed;
        float nextSpawnIn;
        public bool timed;
        public bool readyToDestroy;
        public bool hasGravity;
        public float duration;
        public int parentEntityID;
        public int ID;

        Vector2 startLife;
        public Vector2 relPosition;
		Vector2 spawnJitter;
        Vector2 SecPerSpawn;
        Vector2 SpawnDirection;
        Vector2 SpawnNoiseAngle;
        Vector2 startScale;
        Vector2 endScale;
        Vector2 startSpeed;
        Vector2 endSpeed;
        public float gravity;
		public float zPos;
        float elapsedTime;
        Color startColour1;
        Color startColour2;
        Color endColour1;
        Color endColour2;

		public int attachedEntity;
		public Collidable_Sprite attachedEntityObj;

        public ParticleEmitter( Vector2 _secPerSpawn, Vector2 _spawnJitter, Vector2 _spawnDirection, Vector2 _spawnNoiseAngle, Vector2 _startScale, Vector2 _endScale,
            Vector2 _startSpeed, Vector2 _endSpeed, Color _startColour1, Color _startColour2, Color _endColour1, Color _endColour2,
            int _maxParticles, Vector2 _relPosition, Vector2 _startLife, bool _timed, float _duration, float _gravity, int _spriteID, Texture2D _particleImage, 
            ParticleSystem _parent)
        {
            random = new Random();
            particles = new LinkedList<Particle>();

            if (_gravity != 0)
            {
                gravity = _gravity;
                hasGravity = true;
            }

            SecPerSpawn = _secPerSpawn;
			spawnJitter = _spawnJitter;
            SpawnDirection = _spawnDirection;
            SpawnNoiseAngle = _spawnNoiseAngle;
            startLife = _startLife;
            startScale = _startScale;
            endScale = _endScale;
            startSpeed = _startSpeed;
            endSpeed = _endSpeed;
            maxParticles = _maxParticles;
            relPosition = _relPosition;
            particleImage = _particleImage;
            parent = _parent;
            endColour1 = _endColour1;
            endColour2 = _endColour2;
            startColour1 = _startColour1;
            startColour2 = _startColour2;

            timed = _timed;
            duration = _duration;

            nextSpawnIn = Functions.LinearInterpolate(SecPerSpawn.X, SecPerSpawn.Y, random.NextDouble());
            secPassed = 0f;
            readyToDestroy = false;
			attachedEntity = -1;
			spriteID = _spriteID;
        }

        public ParticleEmitter( ParticleEmitter temp )
        {
            random = new Random();
            particles = new LinkedList<Particle>();


            gravity = temp.gravity;
			hasGravity = (gravity != 0);
            SecPerSpawn = temp.SecPerSpawn;
            SpawnDirection = temp.SpawnDirection;
			spawnJitter = temp.spawnJitter;
            SpawnNoiseAngle = temp.SpawnNoiseAngle;
            startLife = temp.startLife;
            startScale = temp.startScale;
            endScale = temp.endScale;
            startSpeed = temp.startSpeed;
            endSpeed = temp.endSpeed;
            maxParticles = temp.maxParticles;
            relPosition = temp.relPosition;
            particleImage = temp.particleImage;
            parent = temp.parent;
            endColour1 = temp.endColour1;
            endColour2 = temp.endColour2;
            startColour1 = temp.startColour1;
            startColour2 = temp.startColour2;

            timed = temp.timed;
            duration = temp.duration;

            nextSpawnIn = Functions.LinearInterpolate(SecPerSpawn.X, SecPerSpawn.Y, random.NextDouble());
            secPassed = 0f;
            readyToDestroy = false;
			attachedEntity = -1;
			spriteID = temp.spriteID;
			sprites = temp.sprites;
        }

        /// <summary>
        /// Updates the particles, deletes and adds when necessary
        /// </summary>
        /// <param name="msPassed"></param>
        public void update(float msPassed)
        {
            duration -= msPassed;
            secPassed += msPassed;

            while (secPassed > nextSpawnIn)
            {
                if (particles.Count < maxParticles )
                {
                    //Can spawn a particle

					Vector2 StartDirection = Vector2.Transform(SpawnDirection,
						Matrix.CreateRotationZ(Functions.LinearInterpolate(SpawnNoiseAngle.X, SpawnNoiseAngle.Y, random.NextDouble())));
					StartDirection.Normalize();
					Vector2 EndDirection = StartDirection * Functions.LinearInterpolate(endSpeed.X, endSpeed.Y, random.NextDouble());
					StartDirection *= Functions.LinearInterpolate(startSpeed.X, startSpeed.Y, random.NextDouble());
					Vector2 _spawnJitter = Vector2.Zero;
					if (spawnJitter.X != 0 || spawnJitter.Y != 0)
					{
						_spawnJitter = new Vector2((float)random.NextDouble() * spawnJitter.X - spawnJitter.X / 2,
							(float)random.NextDouble() * spawnJitter.Y - spawnJitter.Y / 2);
					}
					particles.AddLast(new Particle(
						Vector2.Lerp(parent.LastPos, parent.Position, secPassed / msPassed) + _spawnJitter,
						zPos, hasGravity,
						StartDirection,
						EndDirection,
						Functions.LinearInterpolate(startLife.X, startLife.Y, random.NextDouble()),
						Functions.LinearInterpolate(startScale.X, startScale.Y, random.NextDouble()),
						Functions.LinearInterpolate(endScale.X, endScale.Y, random.NextDouble()),
						Color.Lerp(startColour1, startColour2, (float)random.NextDouble()),
						Color.Lerp(endColour1, endColour2, (float)random.NextDouble()),
						this)
						);

					particles.Last.Value.update(secPassed);

                }
                secPassed -= nextSpawnIn;
                nextSpawnIn = Functions.LinearInterpolate(SecPerSpawn.X, SecPerSpawn.Y, random.NextDouble());
            }

            //Update gravity
            float newGravity = gravity * msPassed;
            nodeIterator = particles.First;
            while (nodeIterator != null)
            {
                nodeIterator.Value.velocity.Y += newGravity;
                nodeIterator = nodeIterator.Next;
            }

            nodeIterator = particles.First;
            while (nodeIterator != null)
            {
                bool isAlive = nodeIterator.Value.update(msPassed);
                nodeIterator = nodeIterator.Next;
                if (!isAlive)
                {
                    if (nodeIterator == null)
                    {
                        particles.RemoveLast();
                    }
                    else
                    {
                        particles.Remove(nodeIterator.Previous);
                    }
                }
            }

            if (timed)
            {
                if (duration <= 0)
                {
                    maxParticles -= 5;
                //    particles.RemoveLast();
                  
                    if (particles.Count == 0)
                    {
                        parent.EmitterList.Remove(this);
                        readyToDestroy = true;
                    }
					else particles.RemoveFirst();
                }
            }

			if (attachedEntity != -1)
			{
				relPosition = Functions.toVector2(attachedEntityObj.localSpace, true);
			}
        }

        public void draw(SpriteBatch spriteBatch, int Scale, Vector2 Offset)
        {
            nodeIterator = particles.First;
			Rectangle spriteRec = sprites[spriteID];
            while (nodeIterator != null)
            {
                nodeIterator.Value.Draw(spriteBatch, Scale, Offset, spriteRec);
                nodeIterator = nodeIterator.Next;
            }
        }

        public void clear()
        {
            particles.Clear();
        }

        public void setDirection(Vector2 newDirection)
        {
            SpawnDirection = -newDirection;
        }

		public void attachTo(Collidable_Sprite _toAttach)
		{
			attachedEntity = _toAttach.ID;
			attachedEntityObj = _toAttach;
			relPosition = Functions.toVector2(attachedEntityObj.localSpace, true);
		}
       
	}


}
