using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DataLoader;
using WindowsGame1.Utility;

namespace WindowsGame1
{
	public partial class Game1 : Microsoft.Xna.Framework.Game
	{
		Debug_Form form;
		MethodInfo temp;
		//Game Content
		Thread contentLoader;
		string tileFilePath = "Object Databases//World//Tiles";
		string cellDataFilePath = "Object Databases//World//CellData";
		public string fullContentPath;
		string contentExtension = ".xnb";
		string directoryLevel = "//";
		List<Point> cellsToUnload = new List<Point>();
		bool isPanMode = false;
		public ScriptEngine script;

		//Save File
		//StorageContainer saveFile;
		//StorageDevice storage;
		IAsyncResult storageSelectResult;
		Stream saveFileStream;
		XmlSerializer fileSerializer = new XmlSerializer(typeof(PlayerLoader));
		const long MIN_FREE_SPACE = 2 * (1024) ^ 2;

		//FPS Stuff
		int totalUpdateTime;
		float currentFPS;
		int msPerFrame;
		int updateCounter;
		short delta;
		public const bool limitFramerate = false;

		//Counters
		float debugCollectTimer;
		const float debugCollectTimerMax = 1f;

		//Consts
		const string gameVer = "22 June 0.0.6";
		const short graphicsCycleSpeed = 200;
		const float scale = Declaration.Scale;
		const short spriteWidth = 24;
		const short spriteHeight = 28;
		const float gameSpriteWidth = spriteWidth * scale;
		const float gameSpriteHeight = spriteHeight * scale;

		Rectangle screen = new Rectangle(0, 0, 1000, 700);
		Random rnd = new Random();

		//Main classes
		//public Declaration filenames = new Declaration();
		public Weather weather;
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public ObjManager ObjectManager;
		public TileManager tileManager;
		public Entity_Manager.InventoryManager inventoryManager;
		public Entity_Manager.EffectManager effectManager;
		public Camera camera;
		public Player player;
		public UI UI;
		public KeyboardState prevKeyState;
		public KeyboardState keyboardState;
		public MouseState prevMouseState;
		public MouseState mouseState;
		public Point mousePos;

		//Drawing stuff
		DrawingEffects.DeferredRenderer renderer;
		PostProcessEffects postProcess;
		Vector4 col;
		Color ambientColorTemp;
		Vector2 viewportSize;
		SpriteFont font;
		RenderTarget2D renderTarget;
		DepthStencilState depthStencilState;
		Texture2D PostProcessTexture;
		Texture2D gameTex;
		ShadowmapResolver shadowResolver;
		QuadRenderComponent quadRender;
		Entity_Components.Dynamic.ParticleSystem particle;
		bool drawRenderTargetsDebug;
		bool drawDebugText = false;
		bool drawUI = true;
		public bool drawNavmeshDebug = false;
		public bool drawEntityPath = true;
		Matrix projection;
		Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
		Matrix transformed;
		Effect navmeshEffect;

		//Game quality (Read from a settings / xml file)
		const Settings.particleCount particleQualityDefault = Settings.particleCount.full;

		//Globals
		public enum gameState
		{
			MainMenu,
			Game,
			Loading
		}
		gameState state;


		public Game1()
		{
			this.IsMouseVisible = false;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
			this.Exiting += new EventHandler<EventArgs>(Game1_Exiting);
			drawRenderTargetsDebug = false;
			script = new ScriptEngine(this);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			if (!limitFramerate)
			{
				graphics.SynchronizeWithVerticalRetrace = false;
				IsFixedTimeStep = false;
			}

			Window.ClientBounds.Offset(0, 0);
			//IsMouseVisible = false;
			
			graphics.PreferredBackBufferWidth = screen.Width;
			graphics.PreferredBackBufferHeight = screen.Height;
			graphics.ApplyChanges();

			viewportSize = new Vector2(screen.Width, screen.Height);
			projection = Matrix.CreateOrthographicOffCenter(0, viewportSize.X, viewportSize.Y, 0, 0, 1);
			transformed = halfPixelOffset * projection;
			quadRender = new QuadRenderComponent(this);
			this.Components.Add(quadRender);
			shadowResolver = new ShadowmapResolver(GraphicsDevice, quadRender, 256, 1024);

			depthStencilState = new DepthStencilState();
			depthStencilState.StencilFunction = CompareFunction.LessEqual;
			ambientColorTemp = Color.White;
			base.Initialize();

			Console.WriteLine("Initialised");
		}

		//Tidy up by loading inside the new functions, and passing the content manager
		protected override void LoadContent()
		{
			//Load Debug Features
			initialiseDebug();

			camera = new Camera(new Point(screen.Width, screen.Height));
			fullContentPath = Content.RootDirectory; 
			//Create entity managers
			effectManager = new Entity_Manager.EffectManager(Content);
			ObjectManager = new ObjManager(screen, Content, GraphicsDevice, effectManager, script);
			ObjectManager.loadSettings(particleQualityDefault);
			tileManager = new TileManager(Content, screen);
			ObjectManager.lightManager.cellData = new Dictionary<Point, List<short>>();
			ObjectManager.lightManager.gameCamera = camera;

			//Create Rendering systems
			renderer = new DrawingEffects.DeferredRenderer
				(GraphicsDevice, Content);
			shadowResolver.LoadContent(Content);
			postProcess = new PostProcessEffects(Content);
			postProcess.onScreenChange(screen, GraphicsDevice);

			player = new Player(new Rectangle(288, 188, 24, 24), ObjectManager);
			font = this.Content.Load<SpriteFont>("Font1");
			navmeshEffect = this.Content.Load<Effect>("Graphical Effects\\Effect Files\\NavDraw");
			rectTex = this.Content.Load<Texture2D>("Graphical Effects\\blankTex");

			UI = new UI(Window.ClientBounds,
				new Rectangle(0, 0, this.GraphicsDevice.DisplayMode.Width, this.GraphicsDevice.DisplayMode.Height),
				font, this);
			weather = new Weather(screen, Content);
			weather.onResize(new Vector2(screen.Width, screen.Height), GraphicsDevice);

			//Item manager
			inventoryManager = new Entity_Manager.InventoryManager( Content,UI.itemSprites);
			while (inventoryManager.errorMessages.Count > 0)
			{
				UI.addMessage(inventoryManager.errorMessages[0]);
				inventoryManager.errorMessages.RemoveAt(0);
			}

			while (effectManager.messages.Count > 0)
			{
				UI.addMessage(effectManager.messages[0]);
				effectManager.messages.RemoveAt(0);
			}
			//Add each template light to the lightManager

			renderTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			spriteBatch = new SpriteBatch(GraphicsDevice);

			player.Hero.setAnimation(AnimName.mainChar, AnimID.walkNorth, (byte)(ObjectManager.SpriteList.list[player.Hero.animName][AnimID.walkNorth].Length - 1));
			//May be -1

			ObjectManager.textureList.Add("chara01_a", this.Content.Load<Texture2D>("chara01_a"));
			ObjectManager.textureList.Add("tileset", this.Content.Load<Texture2D>("tileset"));
			ObjectManager.textureList.Add("soft", this.Content.Load<Texture2D>("Sprite Content\\Particle"));
			ObjectManager.textureList.Add(Declaration.DEBUG_TEX, this.Content.Load<Texture2D>("Sprite Content\\DebugTex"));
			ObjectManager.setParticleTextures();
			ObjectManager.gameCamera = camera;

			player.tileLength = Declaration.tileLength * Declaration.Scale;

			//Add hero data
			player.Hero.localSpace.Width = (int)(24f * scale);
			player.Hero.localSpace.Height = (int)(28f * scale);
			player.Hero.localSpace.X = (screen.Width - player.Hero.localSpace.Width) / 2;
			player.Hero.localSpace.Y = (screen.Height - player.Hero.localSpace.Height) / 2;

			ObjectManager.hero = player;
			ObjectManager.quadtree.insertObject(player.Hero);
			player.Hero.ID = 999;
			ObjectManager.Characters[999] = player.Hero;
			ObjectManager.Entities[999] = player.Hero;
			ObjectManager.localEntity.Add(999);
			ObjectManager.drawingEntity.Add(999);

			//tilePos is not being kept
			Character_Components.Character nubHead = 
				new Character_Components.Character(new Point(4, 5), 0, 0, "chara01_a", new Rectangle(399, 40, 24, 30), ObjectManager.navmesh);
			nubHead.zPos = 0;
			ObjectManager.addChar(ref nubHead);
			ObjectManager.Entities[998].useCollisionBox = false;
			player.Hero.useCollisionBox = false;
			Vector2 origin = new Vector2((int)(ObjectManager.Entities[998].localSpace.X + 30), (int)(ObjectManager.Entities[998].localSpace.Y + 35));
			ObjectManager.Entities[998].setCircleCollision(origin, 32);
			origin = new Vector2((ObjectManager.hero.Hero.localSpace.X + 30), (ObjectManager.hero.Hero.localSpace.Y + 35));
			ObjectManager.hero.Hero.setCircleCollision(origin, 32);

			ObjectManager.Entities[998].tilePos.X = 500;
			ObjectManager.Entities[998].tilePos.Y = 312;
			//Add the inventories of each object
			foreach (short tempID in ObjectManager.Characters.Keys)
			{
				inventoryManager.inventories.Add(tempID, ObjectManager.Characters[tempID].inventory);
			}

			loadCharacter();

			//Set camera focus
			//camera.setFocus(999);
			state = gameState.Game;
			keyboardState = prevKeyState;
			mouseState = prevMouseState;
			ObjectManager.lightManager.addPointLight(new Color((int)Color.Orange.R, Color.Orange.G, Color.Orange.B, 100), 0f,
				Point.Zero, new Point(490, 312), Vector2.Zero, 600, 2);
			ObjectManager.lightManager.addPointLight(new Color((int)Color.Orange.R, Color.Orange.G, Color.Orange.B, 100), 0f,
				Point.Zero, new Point(508, 320), Vector2.Zero, 300, 2);
			//ObjectManager.lightManager.addPointLight(ObjectManager.lightLibrary[0], Point.Zero, new Point(512, 324), Vector2.Zero, 3);
			ObjectManager.attachLight(999, 1);
			//ObjectManager.attachParticleEffect(999, 2);
			//ObjectManager.attachParticleEffect(998, 2);
			short pID = ObjectManager.addParticleEffect(1, player.playerTilePos, Vector2.Zero, 0);
			ObjectManager.particles[pID].timed = true;
			ObjectManager.particles[pID].duration = 4.0f;
			ObjectManager.particles[pID].hasGravity = false;

			player.Hero.stats.applyEffect(effectManager.effects[1]);

			buildGameScripts(script);
			temp = script.functions["someFunction"];
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here

		}

		protected void loadCharacter()
		{
			//Multiple save files
			//load character data from xml
			/* Contains:
			 * Position
			 * quest info
			 * invecntory
			 * stored items
			 * factions
			 */

			//Load the current cell
			player.currentCell = new Point(8, 5);
			player.playerTilePos.X = player.currentCell.X * Functions.tilesPerSector + 5;
			player.playerTilePos.Y = player.currentCell.Y * Functions.tilesPerSector + 5;
			player.Hero.tilePos = player.playerTilePos;
			player.Hero.zPos = 0;
			player.Hero.gameName = "Hero";
			//ObjectManager.tileManager.tileOriginX = (int)(player.playerTilePos.X - 0.5 * (Window.ClientBounds.Width / Declaration.tileGameSize));
			//ObjectManager.tileManager.tileOriginY = (int)(player.playerTilePos.Y + 0.5 * (Window.ClientBounds.Height / Declaration.tileGameSize));

			//Camera setFocus
			camera.setFocus(player.Hero.ID, player.Hero.tilePos,
				new Vector2(player.Hero.Offset.X, player.Hero.Offset.Y),
				new Vector2(gameSpriteWidth / 2, gameSpriteHeight / 2));
			camera.Update(ObjectManager.Entities[camera.entityID]);
			tileManager.setCells(player.Hero.tilePos, player.Hero.Offset);


			//Apply to objectmanager4
			ObjectManager.onCharacterLoad();
		}

		protected bool saveCharacter()
		{
            return true;

            /*
             * 
             * 
             *  Removed after monogame covnersion due to no GamerServices
             * 
             * 
             */

			//DataLoader.PlayerLoader playerData = new PlayerLoader(player.Hero.tilePos,
			//	player.Hero.Offset, player.Hero.inventory.contents,
			//	player.Hero.inventory.quantity);

			////Data eventually replaced by a player.getData() method

			//storageSelectResult = StorageDevice.BeginShowSelector(null, null);
			//storageSelectResult.AsyncWaitHandle.WaitOne();
			//storage = StorageDevice.EndShowSelector(storageSelectResult);
			//storageSelectResult.AsyncWaitHandle.Close();

			//storageSelectResult = storage.BeginOpenContainer("Player Data", null, null);
			//storageSelectResult.AsyncWaitHandle.WaitOne();

			//saveFile = storage.EndOpenContainer(storageSelectResult);
			//storageSelectResult.AsyncWaitHandle.Close();

			//string filename = player.Hero.gameName + ".sav";
			//if (saveFile.FileExists(filename))
			//{
			//	if (saveFile.StorageDevice.FreeSpace > MIN_FREE_SPACE)
			//		saveFile.DeleteFile(filename);
			//	else
			//	{
			//		UI.addMessage(Functions.WriteDebugLine("Error: could not save file, not enough space. " + saveFile.StorageDevice.FreeSpace
			//			+ " free, " + MIN_FREE_SPACE + " required."), Message.msgType.System);
			//		return false;
			//	}
			//}

			//saveFileStream = saveFile.CreateFile(filename);
			//fileSerializer.Serialize(saveFileStream, playerData);
			//saveFileStream.Close();
			//saveFileStream.Dispose();

			//UI.addMessage(Functions.WriteDebugLine("Saved character " + player.Hero.gameName + " to file " +
			//	saveFile.StorageDevice.ToString()), Message.msgType.System);
			//return true;
		}

		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			//Vector2 halfChange =
			//    new Vector2(Window.ClientBounds.Width - screen.Width, Window.ClientBounds.Height - screen.Height);
			Vector2 halfChange =
				new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth - screen.Width, 
					GraphicsDevice.PresentationParameters.BackBufferHeight - screen.Height);
			halfChange.X /= 2;
			halfChange.Y /= 2;
			translateVerts(halfChange);
			screen.Width = Window.ClientBounds.Width;
			screen.Height = Window.ClientBounds.Height;
			viewportSize = new Vector2(screen.Width, screen.Height);

			ObjectManager.screenSizeChange(Window.ClientBounds.Width, Window.ClientBounds.Height);
			tileManager.onScreenChange(screen);
			ObjectManager.scrWidth = Window.ClientBounds.Width;
			ObjectManager.scrHeight = Window.ClientBounds.Height;
			ObjectManager.lightManager.blankTex = new Texture2D(GraphicsDevice, screen.Width, screen.Height);
			renderTarget.Dispose();
			renderer.renewRenderTargets(GraphicsDevice);
			camera.onScreenChange(new Point(screen.Width, screen.Height), halfChange);
			UI.updateElementPositions(Window.ClientBounds);
			postProcess.onScreenChange(screen, GraphicsDevice);
			weather.onResize(viewportSize, GraphicsDevice);
			onScreenResizeDebug();

			//Reset the matrices
			projection = Matrix.CreateOrthographicOffCenter(0, viewportSize.X, viewportSize.Y, 0, 0, 1);
			transformed = halfPixelOffset * projection;

			if (screen.Width > 0 && screen.Height > 0)
			{
				renderTarget = new RenderTarget2D(graphics.GraphicsDevice, screen.Width, screen.Height, false, SurfaceFormat.Color, DepthFormat.None);
			}
		}

		//Handles all forms of input
		protected void handleInputGame(GameTime time)
		{
			if (!IsActive) return;
			prevMouseState = mouseState;
			prevKeyState = keyboardState;
			mouseState = Mouse.GetState();
			keyboardState = Keyboard.GetState();
			mousePos = new Point(mouseState.X, mouseState.Y);
			Vector2 mouseVec = new Vector2(mousePos.X, mousePos.Y);

			if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released
				&& keyboardState.IsKeyDown(Keys.LeftControl))
			{
				//Tuple<Point, Point, Point> position = getSelectedPosition(mousePos);
				ObjectManager.Characters[999].brain.moveTo(mouseVec);
			}

			if (form != null)
			{
				if (form.navmeshForm != null)
				{
					if (((Debug.navmeshForm)form.navmeshForm).isAwaitingInput)
					{
						if (((Debug.navmeshForm)form.navmeshForm).isGettingConnected)
						{
							if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
							{
								int selectedCell = ObjectManager.navmesh.getContainingNode(mouseVec);
								((Debug.navmeshForm)form.navmeshForm).connectPolygon(selectedCell);
								form.navmeshForm.Show();
							}
						}
						else
						{
							Utility.NavMeshCell selected = ((Debug.navmeshForm)form.navmeshForm).selectedCell;
							//FIXME :: Selected position is inaccurate (about two tiles to the left), no offset
							Tuple<Point, Point, Point> position = getSelectedPosition(mousePos);
							((Debug.navmeshForm)form.navmeshForm).setVerticePosition(
								position.Item1, position.Item2, Functions.toVector2(position.Item3));
							if (navmeshVertices.ContainsKey(selected.polyID))
							{
								navmeshVertices[selected.polyID][((Debug.navmeshForm)form.navmeshForm).verticeInput - 1].Position =
									Functions.toVector3(
									camera.toLocalSpace(position.Item1, position.Item2, Functions.toVector2(position.Item3)));
							}

							//if distance between a line segment is negligible (i.e. < 10), snap to that!

							if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
							{
								((Debug.navmeshForm)form.navmeshForm).onPositionSet();
								form.navmeshForm.Show();
							}
						}
					}
				}
			}
			if (awaitingMouseClick)
			{
				if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
				{
					//Button has been pressed
					onMouseDebugClick();
				}
			}
			if (camera.cinematicMode)
			{
				//Only allow next / pause buttons
			}
			else
			{
				UI.keyboard = keyboardState;
				UI.prevKeyboardState = prevKeyState;
				UI.mouse = mouseState;
				UI.prevMouseState = prevMouseState;
				UI.processInput();

				if (!string.IsNullOrEmpty(UI.textInputFinal))
				{
					parseTextCommand(ref UI.textInputFinal);
				}

				short hoverID = ObjectManager.quadtree.getItemFromCursorPos(mousePos);

				if (hoverID != -1)
					UI.getTooltip(ObjectManager.Characters[hoverID]);
				else
					UI.getTooltip(null);


				if (keyboardState.IsKeyDown(Keys.Up) && keyboardState.IsKeyUp(Keys.Down) 
					&& !UI.isCinematicMode)
				{
					player.Hero.setDirection('n');
					player.Hero.queueAdvance();
				}
				else if (keyboardState.IsKeyDown(Keys.Down) && keyboardState.IsKeyUp(Keys.Up)
					&& !UI.isCinematicMode)
				{
					player.Hero.setDirection('s');
					player.Hero.queueAdvance();
				}
				else player.Hero.vel.Y = 0;


				if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Right) 
					&& !UI.isCinematicMode)
				{
					player.Hero.setDirection('w');
					player.Hero.queueAdvance();
				}
				else if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Left)
					&& !UI.isCinematicMode)
				{
					player.Hero.setDirection('e');
					player.Hero.queueAdvance();
				}
				else player.Hero.vel.X = 0;

				if (keyboardState.IsKeyDown(Keys.Space))
				{
					//camera.setFocus(player.Hero.ID, player.Hero.tilePos,
					//    new Vector2(player.Hero.Offset.X, player.Hero.Offset.Y),
					//    new Vector2(gameSpriteWidth / 2, gameSpriteHeight / 2));
				}

				if (keyboardState.IsKeyDown(Keys.Home) && prevKeyState.IsKeyUp(Keys.Home))
				{
					if (form != null)
						return;
					form = new Debug_Form();
					form.Show();
					form.game = this;
				}

				if (keyboardState.IsKeyDown(Keys.Tab) && prevKeyState.IsKeyUp(Keys.Tab))
				{
					isPanMode = !isPanMode;
				}
				if (keyboardState.IsKeyDown(Keys.Q) && prevKeyState.IsKeyUp(Keys.Q))
				{
					float pointDirection = (float)Math.Atan2(mousePos.X - player.Hero.circleOrigin.X,
						mousePos.Y - player.Hero.circleOrigin.Y);
					ObjectManager.addProjectile(1, player.Hero,
					(Character_Components.Character)(ObjectManager.Entities[998]), pointDirection);
					script.functions["someFunction"].Invoke(null, new object[]{player.Hero, ObjectManager.Entities[998], 
						ObjectManager});
					script.setMainScript("dialogueScript", this);
				}
				if (keyboardState.IsKeyDown(Keys.E) && prevKeyState.IsKeyUp(Keys.E))
				{
					float pointDirection = (float)Math.Atan2(mousePos.X - player.Hero.circleOrigin.X,
						mousePos.Y - player.Hero.circleOrigin.Y);
					ObjectManager.addProjectile(2, player.Hero,
					(Character_Components.Character)(ObjectManager.Entities[998]), pointDirection);
					temp.Invoke(null, new object[] { player.Hero, ObjectManager.Entities[998], ObjectManager });
					script.callScript("someFunction", new object[] { player.Hero, ObjectManager.Entities[998], ObjectManager });
				}

				#region FunctionKeys
				else if (keyboardState.IsKeyDown(Keys.F1) && prevKeyState.IsKeyUp(Keys.F1))
				{
					drawDebugText = !drawDebugText;
				}
				else if (keyboardState.IsKeyDown(Keys.F2))
				{
					if (weather.setRain())
						UI.addMessage("Rain started", Message.msgType.System);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F3))
				{
					if (weather.disableRain())
						UI.addMessage("Stoppping rain", Message.msgType.System);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F4))
				{
					if (postProcess.beginBlur(4.0f, 400, 200, 1.0f / 128.0f))
						UI.addMessage("Beginning blur", Message.msgType.System);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F5))
				{
					drawRenderTargetsDebug = !drawRenderTargetsDebug;
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F6))
				{
					if (postProcess.startRadialFadeOut(2.0f, 2.0f))
						UI.addMessage("Beginning radial fade", Message.msgType.System);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F7))
				{
					if (postProcess.enableMonochromatic(4.0f, 6.0f, 1.0f))
						UI.addMessage("Beginning monochromatic overlay", Message.msgType.System);
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F8))
				{
					if (!weather.isFogActive)
					{
						weather.setFog(GraphicsDevice);
						UI.addMessage("Beginning monochromatic overlay", Message.msgType.System);
					}
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F9))
				{
					if (weather.isFogActive)
					{
						weather.disableFog();
						UI.addMessage("Stopping Fog", Message.msgType.System);
					}
				}
				else if (Keyboard.GetState().IsKeyDown(Keys.F11))
				{
					drawUI = !drawUI;
				}
				#endregion
			}
			handleInputDebug();
		}

		protected override void Update(GameTime gameTime)
		{
			//seperate states for notifications and dialog z cutscenes
			switch (state)
			{
				case gameState.Game:
					{
						updateGame(gameTime);
						break;
					}
				case gameState.Loading:
					{
						updateLoad(gameTime);
						break;
					}
				case gameState.MainMenu:
					{
						updateMainMenu(gameTime);
						break;
					}
			}
		}

		private void updateGame(GameTime gameTime)
		{

			//Calculate fps
			totalUpdateTime += gameTime.ElapsedGameTime.Milliseconds;
			debugCollectTimer += gameTime.ElapsedGameTime.Milliseconds / 1000f;

			updateCounter++;
			if (totalUpdateTime >= 1000)
			{
				msPerFrame = (totalUpdateTime / updateCounter);
				currentFPS = updateCounter;
				totalUpdateTime = updateCounter = 0;
			}

			updateAnimations((short)gameTime.ElapsedGameTime.Milliseconds);

			handleInputGame(gameTime);

			UI.updateUI(gameTime);

			//Update Objects
			ObjectManager.updateAll((short)gameTime.ElapsedGameTime.Milliseconds, GraphicsDevice, mousePos); //Will check collisions, so velocities will be final afterwards!
			player.UpdateCharacter();	//Currently does nothing worthwhile
			if (isPanMode)
			{
				camera.Pan(Point.Zero, getPanAmount(), 0f);
			}

			//Update Camera
			if (camera.isFollowing)
			{
				camera.Update(ObjectManager.Entities[camera.entityID]);
				ObjectManager.onCameraTranslation(camera.originVec, camera.origin, camera.translationThisUpdate, camera.entityID);
			}
			else if (camera.isPanning)
			{
				camera.UpdatePan(gameTime.ElapsedGameTime.Milliseconds / 1000f);
				ObjectManager.onCameraTranslation(camera.originVec, camera.origin, camera.translationThisUpdate);
			}
			else
			{
				//Normally set focus -> now only happens on ctrl press
			}

			translateVerts(-camera.translationThisUpdate);
			ObjectManager.navmesh.onTranslation(-camera.translationThisUpdate);
			base.Update(gameTime);

			//Can get accurate player movement by: last pos- new pos

			//Update weather
			weather.updateWeather((float)gameTime.ElapsedGameTime.Milliseconds, player.Hero.vel);
			postProcess.update(gameTime.ElapsedGameTime.Milliseconds / 1000f);

			tileManager.Update(camera.focus, camera.focusVec);

			collectDebugMessages();
			script.runScripts(gameTime.ElapsedGameTime.Milliseconds);

			//Handles loading of data
			UpdateGameData();
		}

		private void updateAnimations(short msPassed)
		{
			delta += msPassed;
			if (delta > graphicsCycleSpeed)
			{
				//Step game frames forwards
				delta -= graphicsCycleSpeed;
				ObjectManager.advanceFrames();
				tileManager.advanceFrames();
			}
		}

		private void collectDebugMessages()
		{
			//TODO :: Add collection for characters in objmanager
			//Get floating text from each entity
			//foreach ( int charID in ObjectManager.npc)
			//{
			//    Character_Components.Character character
			//    if (character.stats.newMessages == true)
			//        //Get messages
			//        UI.addFloatingTextRange(character.stats.takeMessages(), character.localSpace);
			//}

			//Get messages (updated at certain intervals)

			if (debugCollectTimer > debugCollectTimerMax)
			{
				UI.pushDebugTextSelf();
				UI.pushDebugText(ObjectManager.UIMessages);
				UI.pushDebugText(postProcess.Messages);

				postProcess.Messages.Clear();
				ObjectManager.UIMessages.Clear();

				debugCollectTimer -= debugCollectTimerMax;
			}
		}

		//This probably doesn't need to be here :(
		private void updateLoad(GameTime gameTime)
		{
			//Load all content brute force

			//Finish radial fade
			postProcess.startRadialFadeOut(1.5f, 0);
		}

		private void updateMainMenu(GameTime gameTime)
		{

		}

		//Shadows for z pos
		//zPos working
		//load / save character
		//Change tile data
		//finish doodads
		//Add doodads
		//Collision!

		//Then
		//AI

		//After core gameplay is complete:
		//Text
		//	Have an xml file of all text of an npc (and variations of the same text)
		//Quests
		//Trading
		//When an object is hurt, they could flash red / white etc.

		protected override void Draw(GameTime gameTime)
		{
			Vector2 temp = new Vector2(0, 0);

			// - reflection map for water / ice etc.
			renderer.setRenderTargets(GraphicsDevice);  //Initialise render targets
			GraphicsDevice.Clear(Color.Purple);
			tileManager.draw(ref spriteBatch, renderer.gBufferEffect, viewportSize);
			ObjectManager.drawShadows(spriteBatch);
			ObjectManager.drawAll(spriteBatch, player.tileOffsetX, player.tileOffsetY, renderer.gBufferEffect, viewportSize);
			//ObjectManager.drawParticles(spriteBatch);
			weather.drawWeather(ref spriteBatch);
			//The texture now contains color data from all sprites
			GraphicsDevice.SetRenderTarget(null);

			DrawLighting();

			//Combine lightmap onto colormap
			GraphicsDevice.SetRenderTarget(renderTarget);
			renderer.colourTexture.SetValue(renderer.targetColour);
			renderer.viewVecCombine.SetValue(viewportSize);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null,
				RasterizerState.CullNone, renderer.lightmapCombineEffect);
			renderer.lightmapCombineEffect.Parameters["sunTex"].SetValue(weather.sunPos);
			renderer.lightmapCombineEffect.Parameters["time"].SetValue(weather.time);
			//Ambient color can be used here
			ambientColorTemp = Color.Gray * 0.8f;
			ambientColorTemp = Color.Black;
			//Changed shader so it is dark lol
			spriteBatch.Draw(renderer.targetLight, screen, ambientColorTemp);
			//The texture now has lighting data
			spriteBatch.End();

			GraphicsDevice.SetRenderTarget(null);
			gameTex = (Texture2D)renderTarget;

			//Draw to render target, include effects
			//Draw post-process effects(under HUD)
			postProcess.drawEffectsUnderHUD(ref spriteBatch, ref gameTex, GraphicsDevice);

			GraphicsDevice.SetRenderTarget(renderTarget);
			gameTex = postProcess.getImage();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			spriteBatch.Draw(gameTex, Vector2.Zero, Color.White);
			spriteBatch.End();
			ObjectManager.drawParticles(spriteBatch);

			if (drawUI) UI.DrawUI(gameTime, ref spriteBatch);
			GraphicsDevice.SetRenderTarget(null);

			gameTex = (Texture2D)renderTarget;

			//draw final post-process effects
			postProcess.drawEffectsOverHUD(ref spriteBatch, ref gameTex, GraphicsDevice);

			//Draw final image to screen
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			spriteBatch.Draw(postProcess.getImage(), Vector2.Zero, Color.White);
			//Draw debug
			//ObjectManager.drawCollisionBoxes(spriteBatch, true, true, true);
			spriteBatch.End();

			if (drawRenderTargetsDebug)
				renderer.drawTargetsToScreen(spriteBatch);

			//Draw debug
			GraphicsDevice.BlendState = BlendState.Additive;
			drawDebug();

			Window.Title = "Game: " + gameVer;

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			//UI.drawCursor(ref spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);

		}

		public void DrawLighting()
		{
			foreach (LightArea temp in ObjectManager.lightManager.lightAreas.Values)
			{
				//Check to see if it's worth drawing
				//Rectangle lightInfluence = new Rectangle(temp.LightPosition.X - temp.LightAreaSize.X / 2,
				//    temp.LightPosition.Y - temp.lightID.Y / 2

				if (!ObjectManager.lightManager.lights[temp.lightID].isFlickering)
				{
					temp.BeginDrawingShadowCasters();
					float radiusSqrd = (temp.LightAreaSize.X / 2);
					radiusSqrd *= radiusSqrd;
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp,
						DepthStencilState.None, RasterizerState.CullNone);
					foreach (short id in ObjectManager.drawingEntity)
					{
						Collidable_Sprite caster = ObjectManager.Entities[id];
						if (!(caster is Collidable_Sprite)) continue;

						BasicLight light = ObjectManager.lightManager.lights[temp.lightID];
						if (caster.zPos + 5 >= light.zPos) continue;

						Vector2 loc = new Vector2(ObjectManager.Entities[id].localSpace.X, ObjectManager.Entities[id].localSpace.Y);
						loc = ObjectManager.getShadowPosition(caster);
						if (Functions.getLengthSqrd(loc, temp.LightPosition) > radiusSqrd)
							continue;

						if (light.attachedEntity != null)
						{
							if (light.attachedEntity.ID == id) continue;
							//Don't draw the entity which is holding the light
						}
						Vector2 relativePosition = temp.ToRelativePosition(loc);
						//relativePosition.Y -= caster.zPos;
						Rectangle spriteKey = ObjectManager.getSpriteKey(
							ObjectManager.Entities[id]);
						relativePosition.X -= Declaration.Scale * spriteKey.Width / 2;
						relativePosition.Y -= Declaration.Scale * spriteKey.Height / 2;
						spriteBatch.Draw(ObjectManager.shadowTex, relativePosition, ObjectManager.getShadowSprite(3), Color.Black, 0f,
							Vector2.Zero, scale, SpriteEffects.None, 0f);
					}
					spriteBatch.End();
					temp.EndDrawingShadowCasters();
					shadowResolver.ResolveShadows(temp.RenderTarget, temp.RenderTarget, temp.LightPosition);
				}
			}

			//Finally combine all shadows into the deferred renderer
			GraphicsDevice.SetRenderTarget(renderer.targetLight);
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			foreach (LightArea temp in ObjectManager.lightManager.lightAreas.Values)
			{
				if (ObjectManager.lightManager.lights[temp.lightID].isFlickering) continue;

				//spriteBatch.Draw(temp.RenderTarget, temp.LightPosition - temp.LightAreaSize * 0.5f,
				//    ObjectManager.lightManager.lights[temp.lightID].lightColor);

				spriteBatch.Draw(temp.RenderTarget, temp.LightPosition - temp.LightAreaSize * 0.5f, null,
					ObjectManager.lightManager.lights[temp.lightID].lightColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}

			//Draw large lights which don't cast shadows here!
			spriteBatch.End();
		}
	}
}
