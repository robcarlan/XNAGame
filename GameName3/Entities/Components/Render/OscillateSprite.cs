using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName3.Render;
using Microsoft.Xna.Framework;

namespace GameName3.Entities.Components.Render {
	/// <summary>
	/// Represents a static sprite which oscillates in scale
	/// </summary>
	class OscillateSprite : StaticRenderComponent {
		protected float t = 0;
		protected float scale, duration;

		public OscillateSprite(GraphicalEntity entity, GameRender render, int spriteID, int texID, float scale, float duration)
			: base(entity, render, spriteID, texID) {
			this.scale = scale;
			this.duration = duration;
		}

		//Render with custom scale
		public override void IRenderable.Draw(ComponentRenderer render) {
			float scaleThisFrame = scale + (float)Math.Sin(t / duration);
			render.Render(getLocalPosition(render), textureID, spriteID, 0, scaleThisFrame, 0f, 0f, false);
		}

		public override void Update(Game.Game1 game) {
			t += game.gameDelta;
		}
	}
}
