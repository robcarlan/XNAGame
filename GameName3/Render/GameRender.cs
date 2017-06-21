using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName3.Game;
using GameName3.Render;
using GameName3.Render.DrawingEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameName3.Render {

	/// <summary>
	/// This class is responsible for rendering all components
	/// </summary>
	public class GameRender {
		//There will be one SimpleSpriteList and one SpriteList
		//They are only used here, each render component needs only know it's animation id's etc. no point giving one to each manager
		public Game1 game;
		public Rectangle screen;
		Vector2 viewportSize;
		
		public SpriteBatch spriteBatch;
		GraphicsDevice graphics;
		GraphicsDeviceManager manager;

		SpriteFont font;
		RenderTarget2D renderTarget;
		DepthStencilState depthStencilState;
		Texture2D PostProcessTexture;
		Texture2D gameTex;

		//Render sub classes
		DeferredRenderer renderer;
		PostProcessEffects postProcess;
		ShadowmapResolver shadowResolver;
		QuadRenderComponent quadRender;

		public SpriteListSimple simpleAnimations;
		public SpriteList Animations;

		bool drawRenderTargetsDebug;
		bool drawDebugText = false;
		bool drawUI = true;
		bool drawNavmeshDebug = false;
		bool drawEntityPath = true;

		Matrix projection;
		Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
		Matrix transformed;

		Effect navmeshEffect;

		public void GameRender(Game1 _game) {
			game = _game;
			manager = new GraphicsDeviceManager(game);
			graphics = game.GraphicsDevice;
			spriteBatch = new SpriteBatch(graphics);
			
		}

		public void Initialize() {

		}

		public void Load() {
			//Load the sprite list, applying all sprite ids.
		}

		//Update only handles Render specific material - i.e. render components will probably already be updated
		public void Update(float msSinceLastFrame) {

		}

		public void Render(GameTime gameTime) {
			graphics.Clear(Color.CornflowerBlue);
		}

		public Vector2 getLocalSpace(Vector2 wsp) {
			//Uses Camera to convert to local space
			return Vector2.Zero;
		}

		public void onScreenChange(Rectangle newWindow) {
			GameWindow Window = game.getWindow();
			screen.Width = Window.ClientBounds.Width;
			screen.Height = Window.ClientBounds.Height;
			viewportSize = new Vector2(screen.Width, screen.Height);

			renderTarget.Dispose();
			renderer.renewRenderTargets(graphics);
			postProcess.onScreenChange(screen, graphics);

			//Reset the matrices
			projection = Matrix.CreateOrthographicOffCenter(0, viewportSize.X, viewportSize.Y, 0, 0, 1);
			transformed = halfPixelOffset * projection;

			if (screen.Width > 0 && screen.Height > 0) {
				renderTarget = new RenderTarget2D(graphics, screen.Width, screen.Height, false, SurfaceFormat.Color, DepthFormat.None);
			}
		}

	}
}
