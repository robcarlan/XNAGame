﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGame1.Entity_Components.Dynamic
{
    public class Particle
    {
        public float lifeLeft;
        public float startLife;
        public float lifePhase;
        public float startScale;
        public float endScale;
		public bool hasGravity;
        public Vector2 initialDirection;
        public Vector2 endDirection;
        public Vector2 position;
		public float zPos;
        public Vector2 velocity;
        public Color initialColour;
        public Color endColour;
        public ParticleEmitter parent;

        public Particle(
            Vector2 _position, float _zPos, bool hasG, Vector2 _initalDireciton, Vector2 _endDirection, float _startLife,
            float _startScale, float _endScale, Color _startColour, Color _endColour, ParticleEmitter _parent)
        {
            position = _position;
			zPos = _zPos;
			hasGravity = hasG;
            initialDirection = _initalDireciton;
            endDirection = _endDirection;
            startLife = _startLife;
            startScale = _startScale;
            endScale = _endScale;
            initialColour = _startColour;
            endColour = _endColour;
            parent = _parent;
            lifeLeft = _startLife;
            velocity = Vector2.Zero;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msPassed"></param>
        /// <returns>True if the particle is alive</returns>
        public virtual bool update(float msPassed)
        {
            lifeLeft -= msPassed;
            if (lifeLeft <= 0)
                return false;

            lifePhase = lifeLeft / startLife;
            position += Vector2.Lerp(endDirection, initialDirection, lifePhase) * msPassed;
            position += velocity;
			if (hasGravity)
			{
				zPos -= msPassed * 10;
				if (zPos < 0) return false; //Kill the particle once it hits the ground
			}

            return true;
        }

        public void Draw(SpriteBatch spriteBatch, int Scale, Vector2 Offset, Rectangle source)
        {
            float currScale = Functions.LinearInterpolate(startScale, endScale, lifePhase);
            Color currCol = Color.Lerp(endColour, initialColour, lifePhase);
            spriteBatch.Draw(parent.particleImage,
                     new Rectangle((int)((position.X - 0.5f * currScale) * Scale + Offset.X),
                              (int)((position.Y - 0.5f * currScale) * Scale + Offset.Y + zPos * Declaration.verticalScale ),
                              (int)(currScale * Scale),
                              (int)(currScale * Scale)),
                     source, currCol, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }

}
