using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName3.Render;
using Microsoft.Xna.Framework;

namespace GameName3.Entities.Components.Render {
	/// <summary>
	/// A LinearAnimationComponent represents rendering one animation with a given amount of sprites. 
	/// </summary>
	class LinearAnimationComponent : StaticRenderComponent  {
		protected byte framePos = 0;
		public bool cycleBackwards { get; set; }
		protected bool forwards = true;
		protected int animLength;

		public LinearAnimationComponent(GraphicalEntity entity, GameRender render, int spriteID, int texID) : base(entity, render, spriteID, texID) {
			framePos = 0;
			animLength = render.simpleAnimations.getAnimLength(spriteID);
		}

		public override void calcHalfSpace(GameRender render) {
			Rectangle sprite = render.simpleAnimations.getFrame(spriteID, framePos);
			halfSize = new Vector2(sprite.Width / 2, sprite.Height / 2);
		}

		public override void IRenderable.Draw(ComponentRenderer render) {
			render.Render(render.toLocalSpace(entity.position), textureID, spriteID, 0);
		}

		public override void Update(Game.Game1 game) {

		}

		/// <summary>
		/// Called whenever sprite positions need to be updated
		/// </summary>
		public virtual void onGraphicsTick() {
			if (framePos <= 0 && !forwards) {
				//Start reached -> cycle back forwards
				forwards = true;
				framePos++;
			} else if (framePos >= animLength - 1) {
				if (cycleBackwards) {
					forwards = false;
					framePos--;
				} else {
					framePos = 0;
				}
			}
		}
	}
}
