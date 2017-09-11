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
using WindowsGame1.Utility;

namespace WindowsGame1
{
    public class UI
    {
        /* Big Stuff */

		//Drawing Textures / Fonts
		public SpriteFont font;
		public SpriteFont cinematicFont;
        public Texture2D UItex;
		public Texture2D itemTex;
        public Texture2D effectIconTexture;
        Texture2D windowTexture;
        public SpriteListSimple UIsprites;
        public KeyValuePair<int, Rectangle>[] effectSprites;
        SpriteListSimple windowSprites;

		//Draw Variables
        SpriteBatch spriteBatch; //A temporary spritebatch to avoid copying in calls

        //Input
        string textInput = "";
        public string textInputFinal;
        public MouseState mouse;
		public MouseState prevMouseState;
        public KeyboardState keyboard;
        public KeyboardState prevKeyboardState;

        //The physical stack of current states
        public Stack<state> gameState;
        //All possible game states
        public enum state{
            MainMenu, Paused, Typing, Game, Map, SpellBook, 
            Crafting, CharacterStatus, Inventory, Talents };

        UI_Components.Windows.WindowBase activeWindow;
        Player Hero;
        List<FloatingText> floatingTextList;
        Rectangle screenDimensions;
        Rectangle maxScreenSize;

        //Booleans!
        public bool
             tooltipActive = false;
        bool spacePressed, mClicked1, enterKeyPressed, isTyping, windowActive,
             pauseKeyPressed, 
             mapKeyPressed, inventoryKeyPressed, charStatusKeyPressed, craftingKeyPressed, spellbookKeyPressed = new bool();

		//Cinematic mode
		public const float DIALOGUE_TIME_PER_TEXT = 0.020f;
		public bool isCinematicMode = false;				//No 'normal' input allowed
		public bool isAwaitingDialogueInput = false;		//Waiting for input 
		public bool isAwaitingDialogueSelection = false;	//Input required is for a selection of choices
		public bool isCinematicModeEnding = false;
		public bool isCinematicModeBeginning = false;
					//Only inputs are for navigation and enter
		public float cinematicTransitionProgress = 0.0f;
		public float dialogueNextTextIn = DIALOGUE_TIME_PER_TEXT;
		public float cinematicTextScale = 1.0f;
		public float dialogueTextTime = 0.0f;
		public int dialogue_textCount;
		public int dialogueSelectedOption = -1;				//The currently highlighted choice				STARTS AT 0
		public int dialogueHighlightedOption = -1;			//The actual option selected, used by scripts
		public int dialogueStartOption = -1;				//Indicates the first option that is drawn,
		public int dialogueDrawBlackBoxHeight = 100;
		public int dialogueMaxTextWidth = 800;
		public int dialogueMaxLines = 7;
		public const int DIALOGUE_MAX_OPTIONS_SHOWN = 4;
		public string currentDialogueString;
		public bool isCinematicModeChoice = false;			//Indicates if the current panel is for a choice
		public List<string> dialogueOptions = new List<string>();
		public Character_Components.Character dialogueSpeaker;
		public UI_Components.Windows.WindowRegion dialogueTextRegion;	//This is where the text is drawn
		public Keys cinematicNextKey = Keys.Space;
		public Keys cinematicUpKey = Keys.Up;
		public Keys cinematicDownKey = Keys.Down;

        //For mouse buttons
        ButtonState  prevmClicked1 = new ButtonState();
        public bool fpsShow;
        private bool textIndicator;
        public bool queueFull;
        public bool
                inInventory, inMainMenu, paused, crafting, inOptions, inMapMenu;

        // Cursor Variables
        const float cursorScale = 1.5f;
        private byte cursorFramePos;
        public enum cursorType { normal, target, talk, attack, shop };
        cursorType currentCursor;
		int cursorID;

        //General
        static public int globalUpdateTime = 0;
        public TooltipBase tooltip;
        Random rand;
		public ScriptEngine scriptEngine;
		public Game1 game;

        //Constants
        int iconWidth;
        const byte itemIconSize = 16;
        const byte itemIconGap = 4;
        const int fontLineSpacing = 25;
        const int fontCharacterSpacing = 0;
        const short typingBoxMaxLettersPerLine = 50;
        const byte numberRighthandIcons = 5;
        const float textScale = 0.8f;
        const byte maxCapcity = 24; //Capacity before forceFlush is called
        public readonly Color textColour    = Color.White;
        public readonly Color npcTextColour = Color.SteelBlue;
        public readonly Color systemColour  = Color.Red;
        public readonly Color QuestColour   = Color.Purple; 
        public const string validLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!'£$%^&*()-=_+,./;'#[]<>?:@~{}";
        //Colours for item quality
        //Key Constants
        private const Keys craftingKey = Keys.I;
        private const Keys inventoryKey = Keys.P;
        private const Keys characterStatusKey = Keys.O;
        private const Keys spellbookKey = Keys.U;
        private const Keys mapKey = Keys.Y;
        private const Keys typingKey = Keys.Enter;
        private const Keys pauseKey = Keys.Escape;

		/* Inventory */
		//Inventory Constants
		const short iconGap = 4; //Gap in pixels between icons 
		const short maxIconsPerRow = 5;
		const short maxIconRows = 5;
		const short itemStartDistFromRight = (short)(itemDimensions * itemScale + iconGap) * maxIconsPerRow + 20;
		const short itemStartDistFromBottom = (short)(itemDimensions * itemScale + iconGap) * maxIconRows + 50;
		const short itemDimensions = 16;
		const float itemScale = 2f;

		//Inventory variables
		Point itemStartPoint;
        Rectangle inventoryRectangle;
		bool dragging;
		short indexActiveItem;
		public Dictionary<short, Point> itemSprites = new Dictionary<short,Point>();

        //Message Log
        public Vector2 MessageLogOrigin = new Vector2(); //Changed on windowResize
        const short messageLife = 10000;
        const short MessageLogSpacing = 20;
        public List<Message> messageLog = new List<Message>();
        public short disposeThem;
        
        //Drawing Spaces
		public Rectangle
			bottomRightIconBoundingBox, bottomRightIconCompleteSpace;
			//castbarBox, inventoryBox, pauseBox; //Changed on windowResize
       // public Vector2
           // characterStatusOrigin, targetStatusOrigin, typingBoxOrigin;
        int currentClickedIcon = 0;

        //Nameplate drawing
        Vector2 namePlateOrigin = new Vector2(20, 20);
        Vector2 healthBarOffset = new Vector2(20, 30);
        Vector2 manaBarOffset = new Vector2(20, 70);
        const short healthBarMaxWidth  = 100;

        Vector2 effectIconOffset = new Vector2(0, 120);
        const short effectIconGap = 24;
        const short maxEffectIconsPerRow = 8;
        const float effectIconScale = 1f;
        const float effectIconTextScale = 0.8f;
		const float nameplateTextScale = 0.8f;
        const short effectIconDimensions = 32;
        const short effectIconDurationTextXDisplacement = 6;
        const short effectIconDurationTextYDisplacement = 40;
        Vector2 healthBarScale = new Vector2(2f, 2f);

        public void updateElementPositions(Rectangle newBounds)
        {
            screenDimensions = newBounds;
            this.MessageLogOrigin.X = 20;
            this.MessageLogOrigin.Y = newBounds.Height - 40;

            //Update the bottom right icon area//
                iconWidth = (int)(((float)screenDimensions.Width / (float)maxScreenSize.Width ) * 48);
                bottomRightIconBoundingBox.Width = bottomRightIconBoundingBox.Height = iconWidth;
                bottomRightIconBoundingBox.Y = screenDimensions.Height - iconWidth;
                bottomRightIconBoundingBox.X = screenDimensions.Width - (numberRighthandIcons * iconWidth);
                bottomRightIconCompleteSpace.X = bottomRightIconBoundingBox.X;
                bottomRightIconCompleteSpace.Y = bottomRightIconBoundingBox.Y;
                bottomRightIconCompleteSpace.Width = iconWidth * numberRighthandIcons;
                bottomRightIconCompleteSpace.Height = iconWidth;

			//Update Inventory spaces
				itemStartPoint = new Point(screenDimensions.Width - itemStartDistFromRight, screenDimensions.Height - itemStartDistFromBottom);
                inventoryRectangle = new Rectangle(itemStartPoint.X, itemStartPoint.Y, (itemDimensions + itemIconGap) * maxIconsPerRow, (itemDimensions + itemIconGap) * maxIconRows);

                if (windowActive)
                {
                    activeWindow.updateScreenPos(screenDimensions);
                }
				dialogueTextRegion = new UI_Components.Windows.WindowRegion(16, newBounds.Height - 300, newBounds.Width - 32,
					180, AnimNameUIWin.generic, 16, 16, 2.0f);
				dialogueMaxTextWidth = (int)(newBounds.Width - 64 - UIsprites.list[33][0].Width * 2.0f - 16);
        }

        public UI(Rectangle clientRec, Rectangle maxScreen, SpriteFont font, Game1 game)
        {
            this.font = font;
            font.LineSpacing = fontLineSpacing;
            font.Spacing = fontCharacterSpacing;
			loadContent(game.Content);
            maxScreenSize = maxScreen;
            updateElementPositions(clientRec);
			rand = Functions.seedRand();
            Hero = game.player;
			scriptEngine = game.script;
			this.game = game;

            gameState = new Stack<state>();
            gameState.Push(state.Game);

            floatingTextList = new List<FloatingText>();
        }

		public void loadContent(ContentManager content)
		{
			UItex = content.Load<Texture2D>("Sprite Content\\UITex");
			itemTex = content.Load<Texture2D>("Sprite Content\\itemSprites");
			effectIconTexture = content.Load<Texture2D>("Sprite Content\\SpellIcons");
			windowTexture = content.Load<Texture2D>("Sprite Content\\WindowSprites");
			cinematicFont = content.Load<SpriteFont>("CinematicFont");

			Dictionary<int, DataLoader.SimpleSpriteListLoader> spriteFrames = 
				content.Load<Dictionary<int, DataLoader.SimpleSpriteListLoader>>("Object Databases\\UISpriteParser");
			DataLoader.KeyValueXML<int, Rectangle>[] effectSpriteFrames =
				content.Load<DataLoader.KeyValueXML<int, Rectangle>[]>("Object Databases\\Effects\\EffectSpriteParser");
			Dictionary<int, DataLoader.SimpleSpriteListLoader> _windowSprites =
				content.Load<Dictionary<int, DataLoader.SimpleSpriteListLoader>>("Object Databases\\UIWindowSpritesParser");

			//Parse the sprites into the spriteList
            UIsprites = new SpriteListSimple();
            foreach (int key in spriteFrames.Keys)
            {
                UIsprites.setFrames(ref spriteFrames[key].spriteRec, spriteFrames[key].spriteRec.Width, spriteFrames[key].spriteRec.Height, spriteFrames[key].numberOfFrames, key);
            }

			effectSprites = new KeyValuePair<int, Rectangle>[effectSpriteFrames.Length];
			foreach (DataLoader.KeyValueXML<int, Rectangle> temp in effectSpriteFrames)
			{
				effectSprites[temp.Key - 1] = new KeyValuePair<int, Rectangle>(temp.Key, temp.Value);
			}

            windowSprites = new SpriteListSimple();
			foreach (int key in _windowSprites.Keys)
            {
                windowSprites.setFrames(ref _windowSprites[key].spriteRec, _windowSprites[key].spriteRec.Width,
                    _windowSprites[key].spriteRec.Height, _windowSprites[key].numberOfFrames, key);
            }

		}

		public void getItemSpritePositions ( Dictionary<short, Point> _itemSprites)
		{
			itemSprites = new Dictionary<short, Point>(_itemSprites);
			//loads the dictionary, where the item IDs can be used to access it's sprite

		}

		public bool wasKeyPressed(Keys key)
		{
			return (prevKeyboardState.IsKeyUp(key) && keyboard.IsKeyDown(key));
		}

        public Color getMsgTypeColor( Message.msgType type)
        {
            switch ((int)type)
            {
                case 0:
                    return textColour;
                case 1:
                    return QuestColour;
                case 2:
                    return systemColour;
                case 3: 
                    return npcTextColour;
                default:
                    return textColour;
            }
        }

        public void forceFlush()
        {
            for (int i = messageLog.Count - maxCapcity; i >= 0; i--)
                messageLog.ElementAt(i).fadeValue -= 1;
        }

        #region addMessage

        public void addMessage(string message, string owner) 
        {
            Message msg = new Message(owner, message);
            messageLog.Insert(0, msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(string message)
        {
            Message msg = new Message(null, message);
			messageLog.Insert(0, msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(string message, string owner, Message.msgType type)
        {
            Message msg = new Message(owner, message,type);
			messageLog.Insert(0, msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(string message, Message.msgType type)
        {
            Message msg = new Message(null, message, type);
			messageLog.Insert(0, msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(List<string> messages, Message.msgType type )
        {
            foreach ( string temp in messages )
            {
				messageLog.Insert(0, new Message(null, temp, type));
            }
            if (messageLog.Count > maxCapcity) queueFull = true;
        }

        #endregion 
        #region updates
        public void processInput()
        {
            prevmClicked1 = prevMouseState.LeftButton;

            //Reset previous stuff
            currentClickedIcon = 0;

			if (isCinematicMode)
			{
				//Cinematic mode limits input
				handleCinematicInput();
			}
			else
			{
				//Only happens when player is free to do whatever
				checkMousePos(mouse);

				#region keyboardPresses
				if (!isTyping)
				{
					if (keyboard.IsKeyDown(pauseKey) && pauseKeyPressed == false)
					{
						pauseKeyPressed = true;
						pushGameState(state.Paused);
					}
					else if (keyboard.IsKeyUp(pauseKey)) pauseKeyPressed = false;

					if (keyboard.IsKeyDown(mapKey) && mapKeyPressed == false)
					{
						mapKeyPressed = true;
						pushGameState(state.Map);
					}
					else if (keyboard.IsKeyUp(mapKey)) mapKeyPressed = false;

					if (keyboard.IsKeyDown(spellbookKey) && spellbookKeyPressed == false)
					{
						spellbookKeyPressed = true;
						pushGameState(state.SpellBook);
					}
					else if (keyboard.IsKeyUp(spellbookKey)) spellbookKeyPressed = false;

					if (keyboard.IsKeyDown(craftingKey) && craftingKeyPressed == false)
					{
						craftingKeyPressed = true;
						pushGameState(state.Crafting);
					}
					else if (keyboard.IsKeyUp(craftingKey)) craftingKeyPressed = false;

					if (keyboard.IsKeyDown(characterStatusKey) && charStatusKeyPressed == false)
					{
						charStatusKeyPressed = true;
						pushGameState(state.CharacterStatus);
					}
					else if (keyboard.IsKeyUp(characterStatusKey)) charStatusKeyPressed = false;


					if (keyboard.IsKeyDown(inventoryKey) && inventoryKeyPressed == false)
					{
						inInventory = !inInventory;
						inventoryKeyPressed = true;
						pushGameState(state.Inventory);
					}
					else if (keyboard.IsKeyUp(inventoryKey)) inventoryKeyPressed = false;
				}

				#endregion

				#region mousePresses
				if (mouse.LeftButton == ButtonState.Released && prevmClicked1 == ButtonState.Pressed)
				{
					mClicked1 = true;
				}
				else mClicked1 = false;
				#endregion

				if (keyboard.IsKeyDown(typingKey))
				{
					enterKeyPressed = true;
					isTyping = !isTyping;
				}
				else enterKeyPressed = false;

				if (isTyping) updateTextInput(globalUpdateTime);
			}
        }

        private bool checkMousePos(MouseState mouse)
        {
            Point mousePoint = new Point(mouse.X, mouse.Y);

            if (bottomRightIconCompleteSpace.Contains(mousePoint) )
            { 
				//Point is hovering over icons, 
                int spacing = bottomRightIconBoundingBox.Width;
                //state iconNumber = state.Map;
				int iconNumber = ((mouse.X - bottomRightIconCompleteSpace.X) / spacing);

				if (mClicked1 == true)
					pushGameState((state)(iconNumber+(int)state.Map));
				else if (mouse.LeftButton == ButtonState.Pressed)
					currentClickedIcon = bottomRightIconCompleteSpace.X + spacing * iconNumber;
				return true;

            }
            return false;
        }

        private void updateTextInput(int delta)
        {
            Keys[] pressedKeys = keyboard.GetPressedKeys();

            if (keyboard.IsKeyDown(Keys.Enter) && !prevKeyboardState.IsKeyDown(Keys.Enter))
            {
                textInputFinal = textInput;
                textInput = "";
                return;
            }
            if (keyboard.IsKeyDown(Keys.Escape))
                textInput = "";
            if (keyboard.IsKeyDown(Keys.Back) && !prevKeyboardState.IsKeyDown(Keys.Back) && textInput.Length > 0)
                textInput = textInput.Substring(0, textInput.Length - 1);

            foreach (Keys key in pressedKeys)
            {
                if (prevKeyboardState.IsKeyUp(key))
                {
                    if (keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
                    { //Capital letters
                        if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            //Convert numbers into shift version
                            textInput += (validLetters[validLetters.LastIndexOf((char)(int)key) + 10]);
                            continue;
                        }

                        if (key >= Keys.A && key <= Keys.Z)
                        {
                            //Capital letter
                            textInput += key;
                            continue;
                        }
                    }
                    else
                    {
                        if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            //Numbers 0 - 9
                            textInput += (char)key;
                            continue;
                        }
                        if (key >= Keys.A && key <= Keys.Z)
                        {
                            textInput += validLetters[validLetters.IndexOf((char)(int)key) - 26];
                            continue;
                        }
                        if (key == Keys.Space)
                            textInput += " ";
                    }
                } 
            }
            //Check each ascii value of the letter. If it is correct, add it to input strign
            
        }
        private void updateMessages(int delta) 
        {
            if (messageLog.Count > maxCapcity && queueFull == true)
            {
                forceFlush();
                queueFull = false;
            }

            foreach (Message X in messageLog)
            {
                X.timeSpan += (short)delta;
                if (X.timeSpan >= messageLife || X.fadeValue < 1000)
                    X.fadeValue -= (short)delta;
                if (X.fadeValue <= 0) disposeThem++;
            }

            while (disposeThem > 0)
            {
                messageLog.RemoveAt(messageLog.Count - 1);
                disposeThem--;
            }
        }
        public void updateUI(GameTime delta)
        {
            globalUpdateTime += delta.ElapsedGameTime.Milliseconds;

            updateFloatingText(delta.ElapsedGameTime.Milliseconds);

			if (isCinematicMode)
			{
				updateCinematicMode(delta.ElapsedGameTime.Milliseconds / 1000.0f);
			}

			//Stuff below this line is probably not necessary to update in cinematic mode
            updateMessages(delta.ElapsedGameTime.Milliseconds);

			if (windowActive)
			{
				activeWindow.Update(new Point(mouse.X, mouse.Y), mouse.LeftButton, ref windowActive);

				//test to see iof window is still active. If not, pop the game state
				if (!windowActive) popGameState();
			}

            if (globalUpdateTime >= 150)
            {
                updateGraphics();
                globalUpdateTime -= 150;
            }
        }

        /// <summary>
        /// Adds debug text from other class into the message queue
        /// </summary>
        public void pushDebugText(List<string> debugStrings)
        {
            addMessage(debugStrings, Message.msgType.System);
        }

        /// <summary>
        /// Adds debug text from UI components
        /// </summary>
        public void pushDebugTextSelf()
        {
            if (activeWindow != null)
            {
                addMessage(activeWindow.getDebugText(), Message.msgType.System);
            }
        }

        private void updateGraphics()
        {
            if (UIsprites.list[cursorID].Length - 1 > cursorFramePos)
                cursorFramePos++;
            else cursorFramePos = 0;
        }
        #endregion 
        #region floatingText

        public void updateFloatingText(int gameTimeMS)
        {
            for (int i = 0; i < floatingTextList.Count; i++)
            {
                if (floatingTextList[i].updateText(gameTimeMS) == true)
                    floatingTextList.RemoveAt(0);
            }
        }
        #endregion

        #region draw
        public void DrawUI(GameTime gameTime, ref SpriteBatch sprite)
        {
            float textScale = 0.8f;
            Vector2 pos = new Vector2(MessageLogOrigin.X, MessageLogOrigin.Y);

			//Draw floating text and message logs
            sprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
				SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
			if (!isCinematicMode)
			{
				for (int i = messageLog.Count - 1; i >= 0; i--)
				{
					drawOutlinedText(messageLog.ElementAt(i), 1.0f, ref pos, ref sprite);
					pos.Y -= 20 * textScale;
				}
			}
            for (int i = 0; i < floatingTextList.Count; i++)
            {
                drawOutlinedText(floatingTextList[i].text, 1f, floatingTextList[i].position, 
					ref sprite, floatingTextList[i].textColour );
            }         
            sprite.End();

            //Start a new call for drawing graphics
            sprite.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, 
                SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone );
			if (isCinematicMode)
			{
				drawCinematic(sprite);
			}
			else
			{
				drawGraphics(ref sprite);
				drawNameplate(ref sprite);
				if (windowActive)
					activeWindow.draw(ref sprite);
			}
			if (tooltipActive == true)
				drawTooltipFrame(ref sprite);

			sprite.End();

			sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
				SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);    

            drawCursor(ref sprite);
            sprite.End();

            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);

            //Draw the text
            if (tooltipActive == true)
            {
                tooltip.draw(ref sprite);
            }
			//Vector2 sourcePos = new Vector2(200, 300);
			//try
			//{
			//    sprite.DrawString(font, textInput + "!", new Vector2(200, 300), Color.White);
			//}
			//catch (Exception)
			//{
			//    addMessage("Key not found");
			//    textInput = "";
			//}

            //End draw Call
            sprite.End();

        }

        private void drawNameplate(ref SpriteBatch sprite)
        {
        //Draw the background

        //Draw the icons
			Rectangle healthSourceQuantity = new Rectangle(UIsprites.list[AnimNameUI.healthFill][0].X, UIsprites.list[AnimNameUI.healthFill][0].Y,
            (int)(UIsprites.list[AnimNameUI.healthFill][0].Width * ((float)Hero.Hero.stats.getHP() / (float)Hero.Hero.stats.getMaxHP())), UIsprites.list[AnimNameUI.healthFill][0].Height);

			//Clamp value to stop whole spritesheet being drawn
			healthSourceQuantity.Width =  (int)MathHelper.Clamp(healthSourceQuantity.Width, 0f, UIsprites.list[AnimNameUI.healthFill][0].Width);

            string healthText = Hero.Hero.stats.getHP() + " / " + Hero.Hero.stats.getMaxHP();
            Vector2 textSize = font.MeasureString(healthText) * textScale;
            Vector2 textSourcePos = new Vector2((int)(UIsprites.list[AnimNameUI.healthFill][0].Width * healthBarScale.X / 2) - textSize.X / 2 + healthBarOffset.X, 
                (int)(UIsprites.list[AnimNameUI.healthFill][0].Height * healthBarScale.Y / 2) - textSize.Y / 2 + healthBarOffset.Y);

            Vector2 effectIconLocalSpace = new Vector2(effectIconOffset.X + namePlateOrigin.X , effectIconOffset.Y + namePlateOrigin.Y);
            short xCounter = 0, yCounter = 0;

			//Draw effect icons
            for (xCounter = 0; xCounter < maxEffectIconsPerRow && yCounter * maxEffectIconsPerRow + xCounter < Hero.Hero.stats.effects.Keys.Count; xCounter++)
            {
				int effectID = Hero.Hero.stats.effects.Keys.ElementAt(yCounter * maxEffectIconsPerRow + xCounter);
				try
				{
					sprite.Draw(effectIconTexture, effectIconLocalSpace, effectSprites[Hero.Hero.stats.effects[effectID].effectID].Value, Color.White, 0, Vector2.Zero, effectIconScale, SpriteEffects.None, 0f);
				}
				catch
				{
					sprite.Draw(effectIconTexture, effectIconLocalSpace, effectSprites[IconID.defaultIcon].Value, Color.White, 0, Vector2.Zero, effectIconScale, SpriteEffects.None, 0f);
				}
				drawOutlinedText(Hero.Hero.stats.getTimeConcise(effectID), 1f, 
					new Vector2(effectIconLocalSpace.X + effectIconDurationTextXDisplacement, effectIconLocalSpace.Y + effectIconDurationTextYDisplacement), ref sprite, Color.Yellow, effectIconTextScale);

                effectIconLocalSpace.X += effectIconDimensions + effectIconGap;

                if (xCounter >= maxEffectIconsPerRow)
                {
                    xCounter = 0;
                    effectIconLocalSpace.X = effectIconOffset.X + namePlateOrigin.X;
                    effectIconLocalSpace.Y += effectIconDimensions + effectIconGap;
                }
            }

            //Additions to effect icons: blue text underneath representing current stacks, text representing duration left
            //Time given via functions, minimal text (nearest minute etc)

        //Draw a fog effect over the bars?
		sprite.Draw(UItex, healthBarOffset + namePlateOrigin, UIsprites.list[AnimNameUI.healthBackground][0], Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);
		sprite.Draw(UItex, healthBarOffset + namePlateOrigin, healthSourceQuantity, Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);

            //Update health source quantity for mana bar
        healthSourceQuantity.Width = (int)(UIsprites.list[AnimNameUI.manaFill][0].Width * ((float)Hero.Hero.stats.getMana() / (float)Hero.Hero.stats.getMaxMana()));
		healthSourceQuantity.X = (int)(UIsprites.list[AnimNameUI.manaFill][0].X);
		healthSourceQuantity.Width = (int)MathHelper.Clamp(healthSourceQuantity.Width, 0f, UIsprites.list[AnimNameUI.healthFill][0].Width);

		sprite.Draw(UItex, manaBarOffset + namePlateOrigin, UIsprites.list[AnimNameUI.manaBackground][0], Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);
        sprite.Draw(UItex, manaBarOffset + namePlateOrigin, healthSourceQuantity, Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);

            //Draw the text        
        drawOutlinedText(healthText, 1f, namePlateOrigin + textSourcePos, ref sprite, Color.White, nameplateTextScale);

        healthText = Hero.Hero.stats.getMana() + " / " + Hero.Hero.stats.getMaxMana();
        textSize = font.MeasureString(healthText) * textScale;
		textSourcePos = new Vector2((int)(UIsprites.list[AnimNameUI.manaFill][0].Width * healthBarScale.X / 2) - textSize.X / 2 + manaBarOffset.X,
			(int)(UIsprites.list[AnimNameUI.manaFill][0].Height * healthBarScale.Y / 2) - textSize.Y / 2 + manaBarOffset.Y);

        drawOutlinedText(healthText, 1f, namePlateOrigin + textSourcePos, ref sprite, Color.White, nameplateTextScale);

        }

        public void drawCursor( ref SpriteBatch sprite )
        {
            if (currentCursor == cursorType.target)
                //The cursor should be offset!
                sprite.Draw(UItex, new Vector2(
                    mouse.X - UIsprites.list[cursorID][cursorFramePos].Width * cursorScale * 0.5f,
                    mouse.Y - UIsprites.list[cursorID][cursorFramePos].Height * cursorScale * 0.5f),
                    UIsprites.list[cursorID][cursorFramePos], Color.White, 0f, Vector2.Zero, cursorScale, SpriteEffects.None, 0f);
            else
            {
                try
                {
                    sprite.Draw(UItex, new Vector2(mouse.X, mouse.Y),
                        UIsprites.list[cursorID][cursorFramePos], Color.White, 0f, Vector2.Zero, cursorScale, SpriteEffects.None, 0f);
                }
                catch (IndexOutOfRangeException)
                {
                    cursorFramePos = 0;
                    sprite.Draw(UItex, new Vector2(mouse.X, mouse.Y),
						UIsprites.list[cursorID][cursorFramePos], Color.White, 0f, Vector2.Zero, cursorScale, SpriteEffects.None, 0f);
                }
            }
        }

        private void drawTooltipFrame( ref SpriteBatch sprite)
        {
            //Draw the tooltip
            float tooltipScale = tooltip.getScale();   

                float counter = 0;
                Vector2 tempPos = new Vector2();

                //Draw the frame
                sprite.Draw(UItex, tooltip.sourcePos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.topLeft][0],
                    Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);

                //Draw the left vertical components:
                tempPos.X = tooltip.sourcePos.X;
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.Y - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {              
                    tempPos.Y = counter + tooltip.sourcePos.Y;
					sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.left][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }

                //Draw the bottom left corner
                tempPos.Y += tooltip.tileSize;
				sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.bottomLeft][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                float finalYHeight = tempPos.Y;

                tempPos.Y = tooltip.sourcePos.Y;
                //Draw the top horizontal components:
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.X - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {
                    tempPos.X = counter + tooltip.sourcePos.X;
					sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.top][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }

                //Draw the top right corner
                tempPos.X += tooltip.tileSize;
				sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.topRight][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                float finalXWidth = tempPos.X;

                tempPos.Y = finalYHeight;
                //Draw the bottom horizontal components:
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.X - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {
                    tempPos.X = counter + tooltip.sourcePos.X; ;
					sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.bottom][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }
                
                //Draw the vertical right components
                tempPos.X = finalXWidth;
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.Y - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {
                    tempPos.Y = counter + tooltip.sourcePos.Y;
					sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.right][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }

                tempPos.Y += tooltip.tileSize;
				sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.bottomRight][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);

            /*          Fill in the Tooltip         */
                finalYHeight -= tooltip.sourcePos.Y;
                finalXWidth -= tooltip.sourcePos.X;
                for (float xCounter = tooltip.tileSize;
                     xCounter < finalXWidth;
                     xCounter += tooltip.tileSize)
                {
                    tempPos.X = xCounter + tooltip.sourcePos.X;
                    for (float yCounter = tooltip.tileSize;
                        yCounter < finalYHeight;
                        yCounter += tooltip.tileSize)
                    {
                        tempPos.Y = yCounter + tooltip.sourcePos.Y ;
                        sprite.Draw(UItex, tempPos, UIsprites.list[AnimNameUI.tooltip + AnimNameUIWin.fill][0],
                            Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                    }
                }
           
            
        }

        public void drawOutlinedText(Message msg, float thickness, ref Vector2 pos, ref SpriteBatch spriteBatch)
        {

            float Alpha = (float)(msg.fadeValue / 1000f);
            if (msg.fadeValue >= 1000f)
            {
                pos.X += thickness;         
                spriteBatch.DrawString(font, msg.getFullText(), pos, Color.Black * Alpha, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
                pos.X -= thickness;
                pos.Y += thickness;
                spriteBatch.DrawString(font, msg.getFullText(), pos, Color.Black * Alpha, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
                pos.Y -= thickness;
            }

            spriteBatch.DrawString(font, msg.getFullText(), pos, getMsgTypeColor(msg.type) * Alpha, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
        }

        public void drawOutlinedText(string msg, float thickness, Vector2 pos, ref SpriteBatch spriteBatch, Color colour)
        {

            pos.X += thickness;
            spriteBatch.DrawString(font, msg, pos, Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);
            pos.X -= thickness;
            pos.Y += thickness;
            spriteBatch.DrawString(font, msg, pos, Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);
            pos.Y -= thickness;

            spriteBatch.DrawString(font, msg, pos, colour, 0f, Vector2.Zero, textScale, SpriteEffects.None, 0f);
        }

        public void drawOutlinedText(string msg, float thickness, Vector2 pos, ref SpriteBatch spriteBatch, Color colour, float scale)
        {

            pos.X += thickness;
            spriteBatch.DrawString(font, msg, pos, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            pos.X -= thickness;
            pos.Y += thickness;
            spriteBatch.DrawString(font, msg, pos, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            pos.Y -= thickness;

            spriteBatch.DrawString(font, msg, pos, colour, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }


		public void drawOutlinedText(string msg, float thickness, Vector2 pos, ref SpriteBatch spriteBatch, Color colour,
			SpriteFont _font, float alpha, float scale)
		{

			pos.X += thickness;
			spriteBatch.DrawString(_font, msg, pos, Color.Black * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			pos.X -= thickness;
			pos.Y += thickness;
			spriteBatch.DrawString(_font, msg, pos, Color.Black * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			pos.Y -= thickness;

			spriteBatch.DrawString(_font, msg, pos, colour * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		}


        private void drawGraphics( ref SpriteBatch sprite)
        {
            //Rectangle bottomRightIconBoundingBox = new Rectangle();

            try
            {           
              //Draw the bottom left hand icons
                for (byte iconNo = 0; iconNo < UIsprites.list[AnimNameUI.bottomRightIcons].Length; iconNo++)
                {
					sprite.Draw(UItex, bottomRightIconBoundingBox, UIsprites.list[AnimNameUI.bottomRightIcons][iconNo], Color.White);
                    bottomRightIconBoundingBox.X += iconWidth;
                }
                if (currentClickedIcon != 0)
                    sprite.Draw(UItex, new Vector2(currentClickedIcon, bottomRightIconBoundingBox.Y), UIsprites.list[AnimNameUI.bottomRightIconClickOverlay][0], Color.White);
				bottomRightIconBoundingBox.X -= iconWidth * UIsprites.list[AnimNameUI.bottomRightIcons].Length;

				//Draw the inventory items
				if (inInventory)
				{
                    try
                    {
                        Vector2 drawPos = new Vector2(screenDimensions.Width - itemStartDistFromRight, screenDimensions.Height - itemStartDistFromBottom);
                        Rectangle itemSpriteSource = new Rectangle(0, 0, itemDimensions, itemDimensions);
                        short drawnItems = 0;

                        for (byte itemRowCounter = 0; itemRowCounter < maxIconRows; itemRowCounter++)
                        {
                            drawPos.X = screenDimensions.Width - itemStartDistFromRight + itemIconGap;

                            for (byte itemColumnCounter = 0; itemColumnCounter < maxIconsPerRow && drawnItems < Hero.Hero.inventory.maxSpace; itemColumnCounter++)
                            {
                                //Set the source position to the correct position on the sprite sheet
                                itemSpriteSource.X = itemSprites[Hero.Hero.inventory.contents[drawnItems]].X;
                                itemSpriteSource.Y = itemSprites[Hero.Hero.inventory.contents[drawnItems]].Y;

                                //Draw the sprite
                                sprite.Draw(
                                    itemTex, drawPos, itemSpriteSource,
                                    Color.White, 0, Vector2.Zero, itemScale, SpriteEffects.None, 0f);

                                //Draw the quantity below?

                                //Increment amount of items drawn
                                drawnItems++;
                                //Advance the X position
                                drawPos.X += itemIconGap + itemScale * itemDimensions;
                            }

                            //Advance the Y position
                            drawPos.Y += itemIconGap + itemScale * itemDimensions;
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        addMessage(" Error drawing inventory, key not found! ");
                        popGameState();
                    }
				}
				//End inventory drawing
            }
            catch (KeyNotFoundException)
            {
                //Handle the exception!
                addMessage("UI Draw: Key not found!");
                
            }
        }
        #endregion 
        private void pushGameState(state pushState)
        {
            //If a menu state exists (ie map or crafting) and a menu state is pushed, pop the old menu state
            if (gameState.Peek() == pushState)
            {
                popGameState();
                return;
            }
            else if (gameState.Peek() >= state.Map && pushState >= state.Map)
                popGameState();
            else if (gameState.Peek() == state.Paused)
                return;

          
            gameState.Push(pushState);
                switch (pushState)
                {
                    case state.Map:
                        windowActive = true;
                        activeWindow = new UI_Components.Windows.WindowMap(windowTexture, new Vector2(200, 200),
                            new Vector2(800, 400), windowSprites, new Point(mouse.X, mouse.Y), screenDimensions, font);
                        addMessage("Pushed map state!");
                        break;
                    case state.Inventory:
                        inInventory = true;
                        addMessage("Pushed inventory state!");
                        break;
                    case state.Crafting:
                        addMessage("Pushed crafting state!");
                        break;
                    case state.CharacterStatus:
                        windowActive = true;
                        activeWindow = new UI_Components.Windows.WindowCharacterStatus(windowTexture, windowSprites, new Point(mouse.X, mouse.Y),
                            screenDimensions, Hero, primaryStatsItemViewFormat(Hero.Hero.stats), font);
                        addMessage("Pushed character status state!");
                        break;
                    case state.SpellBook:
                        //windowActive = true;
                        //activeWindow = new UI_Components.Windows.WindowSpellbook(windowTexture, new Vector2(200, 200),
                        //    new Vector2(400, 200), windowSprites, new Point(mouse.X, mouse.Y), screenDimensions,
                        //    toItemViewFormat(Hero.Hero.abilities), font);
                        addMessage("Pushed spellbook state!");
                        break;
                    case state.Paused:
                        addMessage("Pushed paused state!");
                        break;
                    default:
                        addMessage("Pushed a generic state!");
                        break;
                }        
        }

        private void popGameState()
        {
            state popped = gameState.Pop();
            switch (popped)
            {
                case state.Map:
                    windowActive = false;
                    addMessage("Popped map state!");
                    break;
                case state.Inventory:
                    addMessage("Popped inventory state!");
                    inInventory = false;
                    break;
                case state.Crafting:
                    addMessage("Popped crafting state!");
                    break;
                case state.CharacterStatus:
                    windowActive = false;
                    addMessage("Popped character status state!");
                    break;
                case state.SpellBook:
                    windowActive = false;
                    addMessage("Popped spellbook state!");
                    break;
                case state.Paused:
                    addMessage("Popped paused state!");
                    break;
                default:
                    addMessage("Popped a generic state!");
                    break;
            }

        }

        public void getTooltip( Character_Components.Character currentObject)
        {
            tooltipActive = true;
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
			int xCounter, yCounter = 0;
            //Check whether the mouse is hovering over any UI elements. If so, replace the current selected element with that. (Ui elements will overlap any in game sprites)

            //Check Inventory
            if (inInventory)
            {
                Rectangle itemLocation = new Rectangle();

                if (inventoryRectangle.Contains(mouse.X, mouse.Y))
                {
                    //Mouse inside rectangle zone, test with each icon
                    currentObject = null;
                    int xPos = screenDimensions.Width - itemStartDistFromRight;
                    int yPos = screenDimensions.Height - itemStartDistFromBottom;

                    itemLocation.Width = itemLocation.Height = (int)(itemDimensions * itemScale);

                    for ( xCounter = 0; xCounter < maxIconsPerRow; xCounter++)
                    {
                       
                        for ( yCounter = 0; yCounter < maxIconRows; yCounter++)
                        {
                            if (Hero.Hero.inventory.contents.Count <= maxIconsPerRow * yCounter + xCounter)
                                continue;

                            itemLocation.X = xPos;
                            itemLocation.Y = yPos;

                            if (Hero.Hero.inventory.contents[maxIconsPerRow * yCounter + xCounter] != 0)
                            {

                                if (itemLocation.Contains(mouse.X, mouse.Y))
                                {
                                    int itemIndex = maxIconsPerRow * yCounter + xCounter;

                                    Item selectedItem = Hero.heldItems[Hero.Hero.inventory.contents[itemIndex]];
                                    tooltip = new EquipmentToolTip(mousePos, ref font, selectedItem.name, null, selectedItem.description, Hero.Hero.inventory.quantity[itemIndex],
                                        quality.common, armorSlot.none, screenDimensions.Height, screenDimensions.Width);
						
                                    return;
                                }

                            }
                                //Item is not empty
                            //Check item id
                            yPos += (int)((itemDimensions + iconGap) * itemScale);
                        }

                        yPos = screenDimensions.Height - itemStartDistFromBottom;
                        xPos += (int)( (itemDimensions + iconGap) * itemScale);
                    }

                }
            }
            //Check Abilities

			Rectangle effectIconLocalSpace = new Rectangle( (int)(effectIconOffset.X + namePlateOrigin.X), 
				(int)(effectIconOffset.Y + namePlateOrigin.Y), (int)(effectIconDimensions * effectIconScale), 
				(int)(effectIconDimensions * effectIconScale) );

			for ( xCounter = 0; xCounter < maxEffectIconsPerRow && yCounter * maxEffectIconsPerRow + xCounter < Hero.Hero.stats.effects.Keys.Count; xCounter++)
			{
				int effectID = Hero.Hero.stats.effects.Keys.ElementAt(yCounter * maxEffectIconsPerRow + xCounter);

				if (effectIconLocalSpace.Contains(new Point(mouse.X, mouse.Y)))
				{
					tooltip = new EquipmentToolTip(
						mousePos, ref font, Hero.Hero.stats.effects[effectID].name,
						null, Hero.Hero.stats.effects[effectID].description, Hero.Hero.stats.getQuantity(effectID),
						quality.common, armorSlot.none, screenDimensions.Height, screenDimensions.Width);
					return;
				}

				/*	Update Positions  */
				effectIconLocalSpace.X += effectIconDimensions + effectIconGap;
				if (xCounter >= maxEffectIconsPerRow)
				{
					xCounter = 0;
					effectIconLocalSpace.X = (int)(effectIconOffset.X + namePlateOrigin.X);
					effectIconLocalSpace.Y += effectIconDimensions + effectIconGap;
				}
			}

            //Check windows
            if ( windowActive && activeWindow.containsMouse(ref mousePos))
            {
                tooltipActive = false;
                return;
            }

            //Check Menu buttons

            //else tooltip = currentObject
            
            {
                if (currentObject == null || currentObject.ID == Hero.Hero.ID)
                {
                    tooltipActive = false;
                    return;
                }
                else 
                    tooltip = new CharacterTooltip(mousePos, new Point(screenDimensions.Width, screenDimensions.Height), font, currentObject, Hero.Hero);
            }
     
        }

        public void clear()
        {
            //Clears the whole UI
            while (floatingTextList.Count > 0)
            {
                floatingTextList.RemoveAt(0);
            }
        }

        public void addFloatingTextRange( List<floatingTextData> floatingTextData, Rectangle source )
        {
            foreach (floatingTextData data in floatingTextData)
            {
                floatingTextList.Add( new FloatingText( data.text, source, data.critical, data.textType, rand ));
            }

            floatingTextData = new List<floatingTextData>();
        }

        private List<UI_Components.WindowItemTooltip> toItemViewFormat( List<Abilities.AbilityData> _abilities )
        {
            List<UI_Components.WindowItemTooltip> final = new List<UI_Components.WindowItemTooltip>();

            for (int i = 0 ; i < _abilities.Count; i++ )
            {
                final.Add( new UI_Components.WindowItemTooltip(
                    _abilities[i].name, _abilities[i].description, _abilities[i].manaCost, _abilities[i].cooldown, 
                    _abilities[i].castTime, _abilities[i].channeled, _abilities[i].passive, 0 ) );
            }

            return final;
        }

        private List<UI_Components.WindowItemTooltip> primaryStatsItemViewFormat(Character_Components.Stats stats)
        {
            List<UI_Components.WindowItemTooltip> final = new List<UI_Components.WindowItemTooltip>();

            final.Add(new UI_Components.WindowItemTooltip("Level", stats.level, "Your current level"));
            final.Add(new UI_Components.WindowItemTooltip("HP Max", stats.getMaxHP(), "Amount of damage you can take before death" ));
            final.Add(new UI_Components.WindowItemTooltip("Mana Max", stats.manaMax, "Used to cast spells"));
            final.Add(new UI_Components.WindowItemTooltip("Magic Power", stats.magicPower, "Determines the power of magic spells"));
            final.Add(new UI_Components.WindowItemTooltip("Heal Power", stats.healPower, "Determines the power of healing spells"));
            final.Add(new UI_Components.WindowItemTooltip("Defense", stats.defense, "Your resistance to physical damage"));
            final.Add(new UI_Components.WindowItemTooltip("Magic Defense", stats.magicDefense, "Your resistance to magical damage"));

            return final;
        }

	#region "Cinematic"
		public void startCinematicMode()
		{
			cinematicTransitionProgress = 0f;
			isCinematicModeBeginning = true;
			isCinematicMode = true;
		}

		public void endCinematicMode()
		{
			cinematicTransitionProgress = 1.0f;
			isCinematicModeEnding = true;
		}

		public void updateCinematicMode(float time)
		{
			const float timeConstant = 4;
			if (isCinematicModeBeginning)
			{
				isAwaitingDialogueInput = false;
				isAwaitingDialogueSelection = false;
				cinematicTransitionProgress += time * timeConstant;
				if (cinematicTransitionProgress >= 1.0f)	//Transition into dialogue has ended, dialogue can be displayed
				{
					isCinematicModeBeginning = false;
					cinematicTransitionProgress = 1.0f;
				}
			}
			else if (isCinematicModeEnding)
			{
				cinematicTransitionProgress -= time * timeConstant;
				if (cinematicTransitionProgress <= 0.0f)	//Transition out of dialogue has ended, gameplay can resume
				{
					isCinematicMode = false;
					isCinematicModeEnding = false;
					cinematicTransitionProgress = 0.0f;
				}
			}
			else if (isCinematicMode && !isAwaitingDialogueInput)
			{
				if (!isCinematicModeChoice)	//If the current panel is a piece of text
				{
					//Update text scroll
					dialogueNextTextIn -= time;
					while (dialogueNextTextIn < 0 && dialogue_textCount < currentDialogueString.Length)
					{
						dialogue_textCount++;
						dialogueNextTextIn += DIALOGUE_TIME_PER_TEXT;
					}

					if (dialogue_textCount >= (currentDialogueString.Length - 1))
					{
						//Ready for input to next choice
						isAwaitingDialogueInput = true;		//Flag that the next 
						isAwaitingDialogueSelection = false;
						dialogueTextTime = 0.0f;
					}
				}
			}
			else
			{
				if (isAwaitingDialogueInput) dialogueTextTime += time;
			}
		}

		public void handleCinematicInput()
		{
			if (wasKeyPressed(cinematicNextKey))
			{
				if (isAwaitingDialogueInput)
				{
					if (isAwaitingDialogueSelection)
					{
						//Current choice has been selected
						dialogueSelectedOption = dialogueHighlightedOption;
						requestNextText();
					}
					else
					{
						//Go to next text
						requestNextText();
					}
				}
				else
				{
					//Button was pressed to complete the current dialogue
					dialogue_textCount = currentDialogueString.Length;
				}
			}

			if (isAwaitingDialogueSelection)
			{
				//Allow movement between options
				if (wasKeyPressed(cinematicDownKey))
				{
					//Clamp choice to max
					dialogueHighlightedOption =
						(dialogueHighlightedOption == dialogueOptions.Count - 1 ) ? 
						dialogueHighlightedOption : dialogueHighlightedOption + 1;

					if (dialogueOptions.Count > DIALOGUE_MAX_OPTIONS_SHOWN )
					{
						//Don't show all options, instead scroll
						if (dialogueHighlightedOption - dialogueStartOption >= DIALOGUE_MAX_OPTIONS_SHOWN)
						{
							dialogueStartOption++;
						}
					}
				}
				else if (wasKeyPressed(cinematicUpKey))
				{
					//Clamp choice to 0
					dialogueHighlightedOption =
						(dialogueHighlightedOption == 0) ?
						0 : dialogueHighlightedOption - 1;

					if (dialogueOptions.Count > DIALOGUE_MAX_OPTIONS_SHOWN)
					{
						//Don't show all options, instead scroll
						if (dialogueHighlightedOption < dialogueStartOption)
						{
							dialogueStartOption--;
						}
					}
				}
			}

			//Space Key - scroll text / next page / select
			//If space pressed and text not completed, text panel -> set dialogue count to string length and flag for input
			//Up / down keys - navigate choices
		}

		//called by scripts to set the next text
		public void setDialogue(string text)
		 {
			isAwaitingDialogueSelection = false;
			isAwaitingDialogueInput = false;
			currentDialogueString = text;
			dialogue_textCount = 0;
			dialogueNextTextIn = DIALOGUE_TIME_PER_TEXT;
			//Set string length
		}

		public void setSpeaker(Character_Components.Character _speaker)
		{
			dialogueSpeaker = _speaker;
		}

		public void setDialogue(List<string> options)
		{
			dialogueOptions = options.ToList<string>();
			dialogueHighlightedOption = 0;
			isAwaitingDialogueInput = true;
			isAwaitingDialogueSelection = true;
			dialogueStartOption = 0;
		}

		//Flags script engine that input was recieved, and that new text is needed
		private void requestNextText()
		{
			isAwaitingDialogueSelection = false;
			isAwaitingDialogueInput = false;
			scriptEngine.continueDialogueScript();
		}

		public int getSelectedOption()
		{
			//Called by scriptEngine to get selected option. On being called, resets the selectedOption
			int selected = dialogueSelectedOption;
			dialogueSelectedOption = -1;
			return selected;
		}

		public void drawCinematic(SpriteBatch sprite)
		{
			const float scale = 2.0f;
			//Draw two black bars
			int boxHeight = (int)(dialogueDrawBlackBoxHeight * cinematicTransitionProgress);
			Rectangle blackBarBox = new Rectangle(0, screenDimensions.Height - boxHeight, 
				screenDimensions.Width, dialogueDrawBlackBoxHeight);
			sprite.Draw(UItex, blackBarBox, UIsprites.list[32][0], Color.White);
			blackBarBox.Y = 0;
			blackBarBox.Height = boxHeight;
			sprite.Draw(UItex, blackBarBox, UIsprites.list[32][0], Color.White);
			//Overlay message box
			dialogueTextRegion.drawRegion(ref sprite, windowSprites, windowTexture, Color.White * cinematicTransitionProgress);

			if (isCinematicModeBeginning || isCinematicModeEnding) return; //All drawing is done, so return
			Vector2 drawPosition = Vector2.Zero;

			//Draw Portrait
			drawPosition.Y = dialogueTextRegion.topLeft.Y + 16;
			drawPosition.X = dialogueTextRegion.topLeft.X + 16;
			sprite.Draw(UItex, drawPosition, UIsprites.list[33][0], Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

			drawPosition.X += UIsprites.list[33][0].Height * 1 + 16;
			//Draw text
			if (!isAwaitingDialogueSelection && dialogue_textCount != 0)
			{	//This text only exists when not selecting something
				//Draw speaker name
				if (dialogueSpeaker == null)
				{
					drawOutlinedText("SPEAKER_NULL:", 2.0f, drawPosition, ref sprite,
						Color.CornflowerBlue, cinematicFont, 1.0f, cinematicTextScale);
				}
				else
				{
					drawOutlinedText(dialogueSpeaker.gameName + ":", 2.0f, drawPosition, ref sprite,
						Color.IndianRed, cinematicFont, 1.0f, cinematicTextScale);
				}
				drawPosition.Y += 24;

				if (dialogue_textCount == currentDialogueString.Length)
				{	//Draw complete string
					drawOutlinedText(currentDialogueString, 2.0f, drawPosition, ref sprite, Color.White,
						cinematicFont, 1.0f, 1.0f);
				}
				else
				{	//Draw part of string
					drawOutlinedText(currentDialogueString.Substring(0, dialogue_textCount), 2.0f, 
						drawPosition, ref sprite, Color.White, cinematicFont, 1.0f, 1.0f);
				}
			}

			if (isAwaitingDialogueInput)
			{
				//Text transparency should be drawn at 0.5f + 0.5sin(t)
				float textTransparency = 0.75f + 0.5f * (float)Math.Sin(3 * dialogueTextTime);
				if (isAwaitingDialogueSelection)
				{
					//Write "Choose an option"
					//drawPosition.Y -= 16;
					drawOutlinedText("Select an option: ", 2.0f, drawPosition, ref sprite, Color.CornflowerBlue,
						cinematicFont, textTransparency, cinematicTextScale);
					//Draw arrows indicating whether there are more options above / below
					if (dialogueStartOption > 0)
					{
						//Draw the up arrow
						drawPosition.Y += 40;
						drawPosition.X -= 32;
						sprite.Draw(UItex, drawPosition, UIsprites.list[34][0], Color.White, 
							MathHelper.Pi * 1.5f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
						drawPosition.Y -= 40;
						drawPosition.X += 32;
					}
					if (dialogueStartOption + DIALOGUE_MAX_OPTIONS_SHOWN < dialogueOptions.Count)
					{
						//Draw the down arrow
						drawPosition.Y += 140;
						drawPosition.X -= 16;
						sprite.Draw(UItex, drawPosition, UIsprites.list[34][0], Color.White, 
							MathHelper.PiOver2,	Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
						drawPosition.X += 16;
						drawPosition.Y -= 140;
					}

					//Indent the options
					drawPosition.X += 20;
					for (int i = dialogueStartOption; i < dialogueStartOption + DIALOGUE_MAX_OPTIONS_SHOWN &&
						i < dialogueOptions.Count; i++)
					{
						drawPosition.Y += 32;
						if (dialogueHighlightedOption == i)
						{
							drawOutlinedText(dialogueOptions[i], 2.0f, drawPosition, ref sprite, Color.White, cinematicFont,
								1.0f, cinematicTextScale);
						}
						else
						{
							drawOutlinedText(dialogueOptions[i], 2.0f, drawPosition, ref sprite, Color.Gray, cinematicFont,
								1.0f, cinematicTextScale);
						}
					}

					//Draw current selection icon
					//Draw down / up arrows if there are multiple options
				}
				else
				{
					//Write "Press [NEXT_KEY] to continue"
					string toDraw = "Press 'Space' to continue";
					drawPosition.Y = screenDimensions.Height - 160;
					drawPosition.X = screenDimensions.Width - cinematicFont.MeasureString(toDraw).X - 32;
					drawOutlinedText(toDraw, 2.0f, drawPosition, ref sprite, Color.White, 
						cinematicFont, textTransparency, 1.0f);
				}

			}

			//Draw selection
		}

	#endregion

    }
}
