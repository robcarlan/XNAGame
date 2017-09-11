using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameName3;

namespace GameName3.Render {
	//Passed to each component in order to render each object
	class ComponentRenderer {
		
		public GameRender render;
		SpriteBatch spriteBatch;
		SpriteListSimple spritesSimple;
		SpriteList sprites;
		Dictionary<int, Texture2D> texMap;

		public ComponentRenderer(GameRender _render) {
			render = _render;
			spriteBatch = render.spriteBatch;
			sprites = render.Animations;
			spritesSimple = render.simpleAnimations;
		}

		public Vector2 toLocalSpace(Vector2 worldSpace) {
			return render.game.Camera.toLocalSpace(worldSpace);
		}

		public void Render(Vector2 position, int texID, int animID, byte pos) {
			spriteBatch.Draw(texMap[texID], position, spritesSimple.getFrame(animID, pos), Color.White);
		}

		public void Render(Vector2 position, int texID, int animID, byte pos, float scale, float rotation, float depth) {
			spriteBatch.Draw(texMap[texID], position, spritesSimple.getFrame(animID, pos), Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, depth);
		}

		public void Render(Vector2 position, int texID, int animID, byte pos, float scale, float rotation, float depth, bool flipVert) {
			spriteBatch.Draw(texMap[texID], position, spritesSimple.getFrame(animID, pos), Color.White, rotation, Vector2.Zero, scale, 
				flipVert ? SpriteEffects.FlipVertically : SpriteEffects.None, 
				depth);
		}
	}
}
