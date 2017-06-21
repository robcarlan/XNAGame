using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DataLoader;

namespace WindowsGame1
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {

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
        const short graphicsCycleSpeed = 200;
        const float scale = Declaration.Scale;

        Rectangle screen = new Rectangle(0, 0, 1000, 700);
        Random rnd = new Random();

        //Main classes
		Declaration filenames = new Declaration();
        Weather weather;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ObjManager ObjectManager;
        Entity_Manager.InventoryManager inventoryManager;
        Entity_Manager.EffectManager effectManager;
		Camera camera;
        Player player;
        UI UI;

        //Drawing stuff
        DrawingEffects.DeferredRenderer renderer;
		PostProcessEffects postProcess;
        Vector4 col;
        SpriteFont font;
        RenderTarget2D renderTarget;
		Texture2D PostProcessTexture;
        Texture2D gameTex;
        Entity_Components.Dynamic.ParticleSystem particle;
		bool drawRenderTargetsDebug;
		bool drawDebugText = true;
		bool drawUI = true;

		//Game quality (Read from a settings / xml file)
		const Settings.particleCount particleQualityDefault = Settings.particleCount.full;

        public Game1()
        {
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
			drawRenderTargetsDebug = false;


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
            Window.Title = "Arr, 'me hearties!";
            IsMouseVisible = false;
 
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();

            base.Initialize();

            Console.WriteLine("Initialised");
            
        }

        protected override void LoadContent()
        {
            ObjectManager = new ObjManager(screen, this.Content.Load<Dictionary<string, Rectangle[]>>("Object Databases\\tiles"),
                this.Content.Load<Dictionary<string, SpriteListLoader>>("Object Databases\\CharacterSprites"),
                this.Content.Load<Dictionary<short, ParticleLoader>>("Object Databases\\Particles"));
			ObjectManager.loadSettings(particleQualityDefault);

            ObjectManager.getMapData(this.Content.Load<Dictionary<Point, DataLoader.MapData>>("Object Databases\\World\\WorldData"));

            renderer = new DrawingEffects.DeferredRenderer
                (GraphicsDevice, this.Content.Load<Effect>("Graphical Effects\\Effect Files\\gBufferWrite"));

            player = new Player(new Rectangle(288, 188, 24, 24));
			font = this.Content.Load<SpriteFont>("Font1");

            UI = new UI(
                this.Content.Load<Texture2D>("Sprite Content\\UITex"),
                this.Content.Load<Texture2D>("Sprite Content\\itemSprites"),
                this.Content.Load<Texture2D>("Sprite Content\\SpellIcons"),
                this.Content.Load<Texture2D>("Sprite Content\\WindowSprites"),
                Window.ClientBounds,
                new Rectangle(0, 0, this.GraphicsDevice.DisplayMode.Width, this.GraphicsDevice.DisplayMode.Height),
                this.Content.Load<Dictionary<string, DataLoader.SpriteListLoader>>("Object Databases\\UISpriteParser"),
                this.Content.Load<Dictionary<string, DataLoader.SpriteListLoader>>("Object Databases\\Effects\\EffectSpriteParser"),
                this.Content.Load<Dictionary<string, DataLoader.SpriteListLoader>>("Object Databases\\UIWindowSpritesParser"),
                font, player);

			postProcess = new PostProcessEffects (this.Content.Load<Effect>("Graphical Effects/Effect Files/RadialFog"),
				this.Content.Load<Effect>("Graphical Effects/Effect Files/RadialBlur"),
				this.Content.Load<Effect>("Graphical Effects/Effect Files/monochromaticEffect"));
			postProcess.onScreenChange(screen, GraphicsDevice);

            weather = new Weather(
                screen,
                this.Content.Load<Texture>("Graphical Effects/daySample"),
                this.Content.Load<Texture2D>("Graphical Effects/rain2"),
                this.Content.Load<Texture2D>("Graphical Effects/rainSplashImg"),
                this.Content.Load<Dictionary<string, DataLoader.SpriteListLoader>>("Graphical Effects/RainSplash"),
                this.Content.Load<Effect>("Graphical Effects/Effect Files/sunEffect"),
                this.Content.Load<Effect>("Graphical Effects/Effect Files/fogEffect"));
			weather.onResize(new Vector2(screen.Width, screen.Height), GraphicsDevice);

            //Item manager
            inventoryManager = new Entity_Manager.InventoryManager(
                this.Content.Load<DataLoader.ItemLoader[]>("Object Databases\\Items\\ItemParser"),
                this.Content.Load<DataLoader.ConsumableItemLoader[]>("Object Databases\\Items\\ConsumableItemLoader"),
                this.Content.Load<DataLoader.ArmourItemLoader[]>("Object Databases\\Items\\ArmourLoader"),
                this.Content.Load<DataLoader.WeaponItemLoader[]>("Object Databases\\Items\\WeaponLoader"),
                ref UI.itemSprites);
            while (inventoryManager.errorMessages.Count > 0)
            {
                UI.addMessage(inventoryManager.errorMessages[0]);
                inventoryManager.errorMessages.RemoveAt(0);
            }

            //Effect Manager
            effectManager = new Entity_Manager.EffectManager(
                this.Content.Load<DataLoader.EffectLoader[]>("Object Databases\\Effects\\EffectLoader"),
                null,
                null);
            while (effectManager.messages.Count > 0)
            {
                UI.addMessage(effectManager.messages[0]);
                effectManager.messages.RemoveAt(0);
            }

            renderTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            spriteBatch = new SpriteBatch(GraphicsDevice);
			camera = new Camera();

            player.Hero.setAnimation(".walkNorth", (byte)(ObjectManager.SpriteList.list[player.Hero.name + ".walkNorth"].Length - 1));

            ObjectManager.textureList.Add("chara01_a", this.Content.Load<Texture2D>("chara01_a"));
            ObjectManager.textureList.Add("tileset", this.Content.Load<Texture2D>("tileset"));
            ObjectManager.textureList.Add("soft", this.Content.Load<Texture2D>("Sprite Content\\Particle"));
            ObjectManager.setParticleTextures();

            for (int y = 0; y < ObjectManager.tileManager.arraySize; y++)
            {
                for (int x = 0; x < ObjectManager.tileManager.arraySize; x++)
                {
                    ObjectManager.tileManager.onscreenTiles[x, y] = new tile();
                    if (rnd.NextDouble() > 0.5f)
                        ObjectManager.tileManager.onscreenTiles[x, y].value = "greenBrick";
                    else ObjectManager.tileManager.onscreenTiles[x, y].value = "beigeBrick";
                }
            }

            player.tileLength = Declaration.tileLength * Declaration.Scale;

            //Add hero data
            player.Hero.localSpace.Width = (int)(24f * scale);
            player.Hero.localSpace.Height = (int)(28f * scale);
            player.Hero.localSpace.X = (screen.Width - player.Hero.localSpace.Width) / 2;
            player.Hero.localSpace.Y = (screen.Height - player.Hero.localSpace.Height) / 2;

            ObjectManager.hero.Add(player);
            ObjectManager.quadtree.insertObject(player.Hero);
            player.Hero.ID = 999;
            ObjectManager.Characters[999] = player.Hero;
            ObjectManager.Entities[999] = player.Hero;
            ObjectManager.localEntity.Add(999);

            Character_Components.Character nubHead = new Character_Components.Character(new Point(4, 5), 0, 0, "chara01_a", new Rectangle(399, 40, 24, 30));
            ObjectManager.addChar(ref nubHead);
            ObjectManager.Entities[998].useCollisionBox = false;
            player.Hero.useCollisionBox = false;
            Vector2 origin = new Vector2((int)(ObjectManager.Entities[998].localSpace.X + 30), (int)(ObjectManager.Entities[998].localSpace.Y + 35));
            ObjectManager.Entities[998].setCircleCollision(origin, 14);
            origin = new Vector2((ObjectManager.hero[0].Hero.localSpace.X + 30), (ObjectManager.hero[0].Hero.localSpace.Y + 35));
            ObjectManager.hero[0].Hero.setCircleCollision(origin, 14);

            //Add the inventories of each object
            foreach (short tempID in ObjectManager.Characters.Keys)
            {
                inventoryManager.inventories.Add(tempID, ObjectManager.Characters[tempID].inventory);
            }

            loadCharacter();

			//Set camera focus
			camera.setFocus(999);

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
            //Load the current cell
            player.currentCell = new Point(5, 5);
            player.playerTilePos.X = player.currentCell.X * Functions.tilesPerSector;
            player.playerTilePos.Y = player.currentCell.Y * Functions.tilesPerSector;
            player.Hero.tilePos = player.playerTilePos;
            ObjectManager.tileManager.tileOriginX = (int)(player.playerTilePos.X - 0.5 * (Window.ClientBounds.Width / Declaration.tileGameSize));
            ObjectManager.tileManager.tileOriginY = (int)(player.playerTilePos.Y + 0.5 * (Window.ClientBounds.Height / Declaration.tileGameSize));

            //Apply to objectmanager4
            ObjectManager.onCharacterLoad();
        }

        protected void saveCharacter()
        {
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            ObjectManager.screenSizeChange(Window.ClientBounds.Width, Window.ClientBounds.Height);
            ObjectManager.tileManager.screenChange(Window.ClientBounds.Height - ObjectManager.scrHeight, Window.ClientBounds.Width - ObjectManager.scrWidth , scale);
            ObjectManager.scrWidth = Window.ClientBounds.Width;
            ObjectManager.scrHeight = Window.ClientBounds.Height;
            ObjectManager.tileManager.tileOriginX = (int)(player.playerTilePos.X - 0.5 * Window.ClientBounds.Width / 40);
            ObjectManager.tileManager.tileOriginY = (int)(player.playerTilePos.Y - 0.5 * Window.ClientBounds.Height / 40);
            renderTarget.Dispose();
            renderer.renewRenderTargets(GraphicsDevice);

            UI.updateElementPositions(Window.ClientBounds);
            screen.Width = Window.ClientBounds.Width;
            screen.Height = Window.ClientBounds.Height;
			postProcess.onScreenChange(screen, GraphicsDevice);
            weather.onResize(new Vector2(screen.Width, screen.Height), GraphicsDevice);

			if (screen.Width > 0 && screen.Height > 0)
			{
				renderTarget = new RenderTarget2D(graphics.GraphicsDevice, screen.Width, screen.Height, false, SurfaceFormat.Color, DepthFormat.None);
			}
        }

        protected override void Update(GameTime gameTime)
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

            delta += (short) gameTime.ElapsedGameTime.Milliseconds;
            if (delta > graphicsCycleSpeed)
            {   
                //Step game frames forwards
                delta -= graphicsCycleSpeed;
                player.Hero.frameAdvance();
                ObjectManager.advanceFrames();
            }

            weather.time += 0.001f;
            if (weather.time > 1)
                weather.time = 0;

            UI.processInput();
            UI.updateUI(gameTime);   
            if (!string.IsNullOrEmpty(UI.textInputFinal))
            {
                parseTextCommand(ref UI.textInputFinal);
            }
            
			MouseState currentMouseState = Mouse.GetState();
            Point mousePos = new Point(currentMouseState.X, currentMouseState.Y);
            short hoverID = ObjectManager.quadtree.getItemFromCursorPos(mousePos);

            if (hoverID != -1)
                UI.getTooltip(ObjectManager.Characters[hoverID]);
            else
                UI.getTooltip(null);

			if (currentMouseState.LeftButton == ButtonState.Pressed)
			{
                short clickedID = hoverID;
				    if (clickedID >= 0 && clickedID != 999)
				    {
				    	UI.addMessage( "NPC: " + ObjectManager.Entities[clickedID].localSpace.ToString());
				    	UI.addMessage("Mouse: " + mousePos.ToString());
				    }
				    else if (clickedID == 999)
                    {
					    UI.addMessage( "Hero: " + player.Hero.localSpace.ToString());
					    UI.addMessage("Mouse: " + mousePos.ToString());
				    }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                UI.addMessage("DEBUG");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.F2))
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
				if (postProcess.enableMonochromatic( 4.0f, 6.0f, 1.0f))
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
			else if (Keyboard.GetState().IsKeyDown(Keys.F10))
			{
				drawDebugText = !drawDebugText;
			}
			else if (Keyboard.GetState().IsKeyDown(Keys.F11))
			{
				drawUI = !drawUI;
			}
			

            //Update Objects
            ObjectManager.updateAll( (short)gameTime.ElapsedGameTime.Milliseconds ); //Will check collisions, so velocities will be final afterwards!
            player.UpdateCharacter();
            ObjectManager.tileManager.checkForUpdate(ref player.tilePosXIncreased,ref player.tilePosYIncreased);

            //Get floating text from each entity
            foreach ( Character_Components.Character character in ObjectManager.Entities.Values )
            {
                if (character.stats.newMessages == true)
                    //Get messages
                    UI.addFloatingTextRange(character.stats.takeMessages(), character.localSpace);
            }

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

            base.Update(gameTime);

            //Can get accurate player movement by: last pos- new pos
            //Update weather
            weather.updateWeather((float) gameTime.ElapsedGameTime.Milliseconds, player.Hero.vel);
			postProcess.update(gameTime.ElapsedGameTime.Milliseconds / 1000f);

			//Update Camera
			if (camera.isFollowing)
			{
				camera.Update(ObjectManager.Entities[camera.entityID]);
			}
			else if (camera.isPanning)
			{
				camera.UpdatePan(gameTime.ElapsedGameTime.Milliseconds / 1000f);
			}
        }

        protected override void Draw(GameTime gameTime)
        {
            Vector2 temp = new Vector2(0, 0);

            renderer.setRenderTargets(GraphicsDevice);  //Initialise render targets
            ObjectManager.drawZMap(ref spriteBatch, ref renderer.gBufferEffect, ref player.tileOffsetX, ref player.tileOffsetY);
			ObjectManager.drawParticles(ref spriteBatch);
			weather.drawWeather(ref spriteBatch);
            GraphicsDevice.SetRenderTarget(null);

            //Draw the game image with lighting to render target
			GraphicsDevice.SetRenderTarget(renderTarget);
			GraphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			spriteBatch.Draw(renderer.targetColour, Vector2.Zero, Color.White);
			//Draw lighting effects on top
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
			if (drawUI) UI.DrawUI(gameTime, ref spriteBatch);
			GraphicsDevice.SetRenderTarget(null);

			gameTex = (Texture2D)renderTarget;

			//draw final post-process effects
			postProcess.drawEffectsOverHUD(ref spriteBatch, ref gameTex, GraphicsDevice);

			//Draw final image to screen
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			spriteBatch.Draw(postProcess.getImage(), Vector2.Zero, Color.White);
			spriteBatch.End();

			if (drawRenderTargetsDebug)
				renderer.drawTargetsToScreen(spriteBatch);

			if (drawDebugText)
			{
				spriteBatch.Begin();
				#region debugText
				//Debug stuff
				spriteBatch.DrawString(UI.font, "Hero pos X: " + player.Hero.localSpace.X + " Hero pos Y: " + player.Hero.localSpace.Y, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "NPC pos X: " + ObjectManager.Entities[998].localSpace.X + " NPC pos Y: " + ObjectManager.Entities[998].localSpace.Y, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "Hero vel X: " + player.Hero.vel.X + " Hero vel Y: " + player.Hero.vel.Y, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "NPC vel X: " + ObjectManager.Entities[998].vel.X + " NPC vel Y: " + ObjectManager.Entities[998].vel.Y, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "Hero Circle: " + player.Hero.circleOrigin, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "NPC Circle: " + ObjectManager.Entities[998].circleOrigin, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "Hero Offset.X: " + player.Hero.Offset.X, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "Hero tile X: " + player.Hero.tilePos.X, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "Hero tile Y: " + player.Hero.tilePos.Y, temp, Color.White);
				temp.Y += 15;
				spriteBatch.DrawString(UI.font, "NPC tile X: " + ObjectManager.Entities[998].tilePos.X, temp, Color.White);
				temp.Y += 15;

				if (postProcess.isMonochromaticEnabled())
				{
					spriteBatch.DrawString(UI.font, "Monochromatic elapsed time: " + postProcess.monoElapsedTime, temp, Color.White);
					temp.Y += 15;
					spriteBatch.DrawString(UI.font, "Monochromatic end time: " + postProcess.monoDuration, temp, Color.White);
					temp.Y += 15;
				}
				#endregion
				spriteBatch.End();
		}

			Window.Title = "Game : FPS: " + currentFPS + (limitFramerate ? " (limited)" : " (not limited)");

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			UI.drawCursor(ref spriteBatch);
			spriteBatch.End();

            base.Draw(gameTime);
            
        }

        #region Command
        public void parseTextCommand(ref string command)
        {
            if (command == "help")
            {
                UI.addMessage("Commands are: ", Message.msgType.System);
                UI.addMessage("1. 'get object ID' to get information about an object.", Message.msgType.System);
                UI.addMessage("     Object is an item, spell, npc etc.", Message.msgType.System);
                UI.addMessage("2. 'set object ID value quantity' value represents a variable", Message.msgType.System);
                UI.addMessage("3. 'give characterID itemID quantity' ", Message.msgType.System);
                UI.addMessage("4. 'query characterID' reports the characters inventory", Message.msgType.System);
                UI.addMessage("5. 'apply effect characterID effectID' applies an effect to a character", Message.msgType.System);
            }

            string[] commandSubstrings = command.Split(' ');
            short counter = 0;

            if (commandSubstrings[0] == "get")
            { //Get object data command
                if (commandSubstrings[1] == "item")
                {
                    try
                    {
                        short itemID = Convert.ToInt16(commandSubstrings[2]);
                        List<String> itemInfo = inventoryManager.getItem(itemID).getInfo();
                        while (counter < itemInfo.Count)
                        {
                            UI.addMessage(itemInfo[counter]);
                            counter++;
                        }
                    }
                    catch (Exception e)
                    {
                        UI.addMessage("Command failure! " + e.Message);
                        command = "";
                        return;
                    }
                }
            }
            else if (commandSubstrings[0] == "give")
            {
                try
                {
                    short characterID = Convert.ToInt16(commandSubstrings[1]);
                    short itemID = Convert.ToInt16(commandSubstrings[2]);
                    short itemQuantity = Convert.ToInt16(commandSubstrings[3]);

                    inventoryManager.giveItem(characterID, itemID, itemQuantity);
                    player.giveItemToPlayer(inventoryManager.getItem(itemID));

                    UI.addMessage("Gave " + characterID + " "
                                    + itemQuantity + " " + itemID + " (" + inventoryManager.getItemName(itemID) + ")!");
                }
                catch (Exception e)
                {
                    UI.addMessage("Command failure! " + e.Message);
                    command = "";
                    return;
                }
            }
            else if (commandSubstrings[0] == "query")
            {
                try
                {
                    short characterID = Convert.ToInt16(commandSubstrings[1]);
                    UI.addMessage(commandSubstrings[1] + "'s inventory:");

                    short emptySpaces = 0;
                    for (short i = 0; i < inventoryManager.inventories[characterID].contents.Capacity; i++)
                    {
                        //Iterate through each item in the backpack. If it is not empty, report to the user  
                        Item currentItem = inventoryManager.getItem(inventoryManager.inventories[characterID].contents[i]);

                        if (currentItem.ID == 0)
                        {
                            emptySpaces++;
                            continue;
                        }
                        else
                        {
                            UI.addMessage("Slot " + i + "- Name: " + currentItem.name + " Quantity: " + inventoryManager.getItemQuantity(currentItem.ID, characterID));
                        }
                    }
                    UI.addMessage(emptySpaces + " empty spaces ");
                }
                catch (Exception e)
                {
                    UI.addMessage("Command failure! " + e.Message);
                    command = "";
                    return;
                }
            }
            else if (commandSubstrings[0] == "remove")
            {
                try
                {
                    short characterID = Convert.ToInt16(commandSubstrings[1]);
                    short itemID = Convert.ToInt16(commandSubstrings[2]);
                    short itemQuantity = Convert.ToInt16(commandSubstrings[3]);

                    inventoryManager.removeItem(itemID, itemQuantity, characterID);

                    UI.addMessage("Took "
                                    + itemQuantity + " of item " + itemID + " (" + inventoryManager.getItemName(itemID) + ") from " + characterID + "!");
                }
                catch (Exception e)
                {
                    UI.addMessage("Command failure! " + e.Message);
                    command = "";
                    return;
                }
            }
            else if (commandSubstrings[0] == "apply" && commandSubstrings[1] == "effect")
            {
                try
                {
                    short characterID = Convert.ToInt16(commandSubstrings[2]);
                    short effectID = Convert.ToInt16(commandSubstrings[3]);

                    ObjectManager.Characters[characterID].stats.applyEffect(effectManager.getEffect(effectID));

                    UI.addMessage("Applied "
                        + effectManager.getEffect(effectID).name + " (" + effectID + ") to " + characterID + "!");
                }
                catch (Exception e)
                {
                    UI.addMessage("Command failure! " + e.Message);
                    command = "";
                    return;
                }
            }
            else
            {
                UI.addMessage("Command failure! command '" + commandSubstrings[0] + "' not found.");
            }

            command = "";

        }

    } 
        #endregion
}
