using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName3.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameName3.Entities.Components.Render {
	/// <summary>
	/// Implements a static render component, i.e. the object to render is a given rectangle on a sprite sheet,
	/// with given scaling and constant position
	/// </summary>
	public class StaticRenderComponent : RenderComponent {
		protected GraphicalEntity entity;
		protected int textureID;
		protected int spriteID;

		public StaticRenderComponent(GraphicalEntity entity, GameRender render, int spriteID, int texID) {
			textureID = texID;
			this.spriteID = spriteID;
			this.entity = entity;
			
			//Calculate halfspace
			calcHalfSpace(render);
		}

		/// <summary>
		/// Calculates the current half space of the sprite
		/// </summary>
		public virtual void calcHalfSpace(GameRender render) {
			Rectangle sprite = render.simpleAnimations.getFrame(spriteID, 0);
			halfSize = new Vector2(sprite.Width / 2, sprite.Height / 2);
		}

		public virtual Vector2 getLocalPosition(ComponentRenderer render) {
			return render.toLocalSpace(entity.position);
		}

		public virtual float getDepth() {
			return 0;
		}

		public override void IRenderable.Draw(ComponentRenderer render) {
			render.Render(getLocalPosition(render), textureID, spriteID, 0);
		}

		public override void Update(Game.Game1 game) {
		}
	}
}
