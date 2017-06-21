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

namespace WindowsGame1
{
    class UI
    {
        /* Big Stuff */

		//Drawing Textures / Fonts
		public SpriteFont font;
        Texture2D UItex;
		Texture2D itemTex;
        Texture2D effectIconTexture;
        Texture2D windowTexture;
        SpriteList UIsprites;
        SpriteList effectSprites;
        SpriteList windowSprites;

		//Draw Variables
        SpriteBatch spriteBatch; //A temporary spritebatch to avoid copying in calls

        //Input
        string textInput = "";
        public string textInputFinal;
        MouseState mouse;
        KeyboardState keyboard;
        KeyboardState previousState;

        //The physical stack of current states
        public Stack<state> gameState;
        //All possible game states
        public enum state{
            MainMenu, Paused, Typing, Game, Map, SpellBook, 
            Crafting, CharacterStatus, Inventory, Talents };

        UI_Components.Windows.WindowBase activeWindow;
        List<Player> Hero;
        List<FloatingText> floatingTextList;
        Rectangle screenDimensions;
        Rectangle maxScreenSize;

        //Booleans!
        public bool
             tooltipActive = false;
        bool spacePressed, mClicked1, enterKeyPressed, isTyping, windowActive,
             pauseKeyPressed, 
             mapKeyPressed, inventoryKeyPressed, charStatusKeyPressed, craftingKeyPressed, spellbookKeyPressed = new bool();

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
        static readonly string[] cursors = {
                                               "cursorStandard",
                                               "cursorTarget",
                                               "cursorTalk",
                                               "cursorAttack",
                                               "cursorShop"
                                           };

        //General
        static public int globalUpdateTime = 0;
        public TooltipBase tooltip;
        Random rand;

        //Constants
        int iconWidth;
        const byte itemIconSize = 16;
        const byte itemIconGap = 4;
        const int fontLineSpacing = 25;
        const int fontCharacterSpacing = 0;
        const short typingBoxMaxLettersPerLine = 50;
        const byte numberRighthandIcons = 5;
        const float textScale = 0.8f;
        const byte maxCapcity = 16; //Capacity before forceFlush is called
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
        public Queue<Message> messageLog = new Queue<Message>();
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
        }

        public UI(Texture2D _UItex, Texture2D _ItemTextures, Texture2D _EffectIconTexture, Texture2D _windowTexture,
            Rectangle clientRec, Rectangle maxScreen, 
            Dictionary<string, DataLoader.SpriteListLoader> spriteFrames,
            Dictionary<string, DataLoader.SpriteListLoader> effectSpriteFrames, 
            Dictionary<string, DataLoader.SpriteListLoader> _windowSprites,
            SpriteFont font, Player playerHero)
        {
            Hero = new List<Player>(1);
            UItex = _UItex;
			itemTex = _ItemTextures;
            effectIconTexture = _EffectIconTexture;
            windowTexture = _windowTexture;

            this.font = font;
            font.LineSpacing = fontLineSpacing;
            font.Spacing = fontCharacterSpacing;

            maxScreenSize = maxScreen;
            updateElementPositions(clientRec);
			rand = Functions.seedRand();
            Hero.Add(playerHero);

            gameState = new Stack<state>();
            gameState.Push(state.Game);

            //Parse the sprites into the spriteList
            UIsprites = new SpriteList();
            foreach (string key in spriteFrames.Keys)
            {
                UIsprites.setFrames(ref spriteFrames[key].spriteRec, spriteFrames[key].spriteRec.Width, spriteFrames[key].spriteRec.Height, spriteFrames[key].numberOfFrames, key);
            }

            effectSprites = new SpriteList();
            foreach (string key in effectSpriteFrames.Keys)
            {
                effectSprites.setFrames(ref effectSpriteFrames[key].spriteRec, effectSpriteFrames[key].spriteRec.Width, 
                    effectSpriteFrames[key].spriteRec.Height, effectSpriteFrames[key].numberOfFrames, key);
            }

            windowSprites = new SpriteList();
            foreach (string key in _windowSprites.Keys)
            {
                windowSprites.setFrames(ref _windowSprites[key].spriteRec, _windowSprites[key].spriteRec.Width,
                    _windowSprites[key].spriteRec.Height, _windowSprites[key].numberOfFrames, key);
            }

            floatingTextList = new List<FloatingText>();
        }

		public void getItemSpritePositions ( Dictionary<short, Point> _itemSprites)
		{
			itemSprites = new Dictionary<short, Point>(_itemSprites);
			//loads the dictionary, where the item IDs can be used to access it's sprite

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
            messageLog.Enqueue(msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(string message)
        {
            Message msg = new Message(null, message);
            messageLog.Enqueue(msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(string message, string owner, Message.msgType type)
        {
            Message msg = new Message(owner, message,type);
            messageLog.Enqueue(msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(string message, Message.msgType type)
        {
            Message msg = new Message(null, message, type);
            messageLog.Enqueue(msg);
            if (messageLog.Count > maxCapcity) queueFull = true;
        }
        public void addMessage(List<string> messages, Message.msgType type )
        {
            foreach ( string temp in messages )
            {
                messageLog.Enqueue(new Message(null, temp, type));
            }
            if (messageLog.Count > maxCapcity) queueFull = true;
        }

        #endregion 
        #region updates
        public void processInput()
        {
            prevmClicked1 = mouse.LeftButton;
            previousState = keyboard;

            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            //Reset previous stuff
            currentClickedIcon = 0;
            //
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

            #region movement
            if (keyboard.IsKeyDown(Keys.Up) && keyboard.IsKeyUp(Keys.Down))
            {
                Hero[0].Hero.setDirection('n');
                Hero[0].Hero.queueAdvance();
            }
            else if (keyboard.IsKeyDown(Keys.Down) && keyboard.IsKeyUp(Keys.Up))
            {
                Hero[0].Hero.setDirection('s');
                Hero[0].Hero.queueAdvance();
            }
            else Hero[0].Hero.vel.Y = 0;


            if (keyboard.IsKeyDown(Keys.Left) && keyboard.IsKeyUp(Keys.Right))
            {
                Hero[0].Hero.setDirection('w');
                Hero[0].Hero.queueAdvance();
            }
            else if (keyboard.IsKeyDown(Keys.Right) && keyboard.IsKeyUp(Keys.Left))
            {
                Hero[0].Hero.setDirection('e');
                Hero[0].Hero.queueAdvance();
            }
            else Hero[0].Hero.vel.X = 0;

            #endregion

            #region mousePresses
            if (mouse.LeftButton == ButtonState.Released && prevmClicked1 == ButtonState.Pressed)
            {
                mClicked1 = true;
            }
            else mClicked1 = false;
            #endregion

            if (keyboard.IsKeyDown(Keys.LeftControl))
            {
                addMessage("DEBUG", Message.msgType.System);
                //ALLOW DEBUG
            }
            
            if (keyboard.IsKeyDown(typingKey))
            {
                enterKeyPressed = true;
                isTyping = !isTyping;
            }
            else enterKeyPressed = false;

            if (isTyping) updateTextInput(globalUpdateTime);

            if (keyboard.IsKeyDown(Keys.F1) && fpsShow == true)
                fpsShow = true;
            else fpsShow = false;
            if (keyboard.IsKeyDown(Keys.Space) && spacePressed == false)
            {

            }
            if (keyboard.IsKeyUp(Keys.Space)) spacePressed = false;
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
				//for (int i = bottomRightIconBoundingBox.X; i < bottomRightIconCompleteSpace.X + bottomRightIconCompleteSpace.Width; i += iconWidth)
				//{
				//    if (mousePoint.X >= i && mousePoint.X < i + iconWidth)
				//    {
				//        if (mClicked1 == true)
				//            pushGameState(iconNumber);
				//        else if (mouse.LeftButton == ButtonState.Pressed)
				//            currentClickedIcon = i;
				//        return true;
				//    }
				//    iconNumber++;
				//}
            }
            return false;
        }

        private void updateTextInput(int delta)
        {
            Keys[] pressedKeys = keyboard.GetPressedKeys();

            if (keyboard.IsKeyDown(Keys.Enter) && !previousState.IsKeyDown(Keys.Enter))
            {
                textInputFinal = textInput;
                textInput = "";
                return;
            }
            if (keyboard.IsKeyDown(Keys.Escape))
                textInput = "";
            if (keyboard.IsKeyDown(Keys.Back) && !previousState.IsKeyDown(Keys.Back) && textInput.Length > 0)
                textInput = textInput.Substring(0, textInput.Length - 1);

            foreach (Keys key in pressedKeys)
            {
                if (!previousState.IsKeyDown(key))
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
                messageLog.Dequeue();
                disposeThem--;
            }
        }
        public void updateUI(GameTime delta)
        {
            globalUpdateTime += delta.ElapsedGameTime.Milliseconds;

            updateFloatingText(delta.ElapsedGameTime.Milliseconds);
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
            if (UIsprites.list[cursors[(int)currentCursor]].Length - 1 > cursorFramePos)
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
            Message[] tempArr = new Message[messageLog.Count];
            messageLog.CopyTo(tempArr, 0);

            //Begin Draw call
            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

            for (int i = tempArr.Length - 1; i >= 0; i--)
            {
                drawOutlinedText(tempArr[i], 1f, ref pos, ref sprite );
                pos.Y -= 20 * textScale;
            }


            for (int i = 0; i < floatingTextList.Count; i++)
            {
                drawOutlinedText(floatingTextList[i].text, 1f, floatingTextList[i].position, ref sprite, floatingTextList[i].textColour );
            }
            
            sprite.End();

            //Start a new call for drawing graphics
            sprite.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, 
                SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone );    
            drawGraphics(ref sprite);
			drawNameplate(ref sprite);
            if (windowActive)
                activeWindow.draw(ref sprite);
            if (tooltipActive == true)
                drawTooltipFrame(ref sprite);
			sprite.End();

			sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
				SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);    

            //drawCursor(ref sprite);
            sprite.End();

            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);

            //Draw the text
            if (tooltipActive == true)
            {
                tooltip.draw(ref sprite);
            }
            Vector2 sourcePos = new Vector2(200, 300);
            try
            {
                sprite.DrawString(font, textInput + "!", new Vector2(200, 300), Color.White);
            }
            catch (Exception)
            {
                addMessage("Key not found");
                textInput = "";
            }

            //End draw Call
            sprite.End();

        }

        private void drawNameplate(ref SpriteBatch sprite)
        {
        //Draw the background

        //Draw the icons
            Rectangle healthSourceQuantity = new Rectangle(UIsprites.list["healthFill"][0].X, UIsprites.list["healthFill"][0].Y,
            (int)(UIsprites.list["healthFill"][0].Width * ((float)Hero[0].Hero.stats.getHP() / (float)Hero[0].Hero.stats.getMaxHP())), UIsprites.list["healthFill"][0].Height);

			//Clamp value to stop whole spritesheet being drawn
			healthSourceQuantity.Width =  (int)MathHelper.Clamp(healthSourceQuantity.Width, 0f, UIsprites.list["healthFill"][0].Width);

            string healthText = Hero[0].Hero.stats.getHP() + " / " + Hero[0].Hero.stats.getMaxHP();
            Vector2 textSize = font.MeasureString(healthText) * textScale;
            Vector2 textSourcePos = new Vector2((int)(UIsprites.list["healthFill"][0].Width * healthBarScale.X / 2) - textSize.X / 2 + healthBarOffset.X, 
                (int)(UIsprites.list["healthFill"][0].Height * healthBarScale.Y / 2) - textSize.Y / 2 + healthBarOffset.Y);

            Vector2 effectIconLocalSpace = new Vector2(effectIconOffset.X + namePlateOrigin.X , effectIconOffset.Y + namePlateOrigin.Y);
            short xCounter = 0, yCounter = 0;

			//Draw effect icons
            for (xCounter = 0; xCounter < maxEffectIconsPerRow && yCounter * maxEffectIconsPerRow + xCounter < Hero[0].Hero.stats.effects.Keys.Count; xCounter++)
            {
				int effectID = Hero[0].Hero.stats.effects.Keys.ElementAt(yCounter * maxEffectIconsPerRow + xCounter);
				try
				{
					sprite.Draw(effectIconTexture, effectIconLocalSpace, effectSprites.list[Hero[0].Hero.stats.effects[effectID].name][0], Color.White, 0, Vector2.Zero, effectIconScale, SpriteEffects.None, 0f);
				}
				catch
				{
					sprite.Draw(effectIconTexture, effectIconLocalSpace, effectSprites.list["Default"][0], Color.White, 0, Vector2.Zero, effectIconScale, SpriteEffects.None, 0f);
				}
				drawOutlinedText(Hero[0].Hero.stats.getTimeConcise(effectID), 1f, 
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
		sprite.Draw(UItex, healthBarOffset + namePlateOrigin, UIsprites.list["healthBackground"][0], Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);
		sprite.Draw(UItex, healthBarOffset + namePlateOrigin, healthSourceQuantity, Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);

            //Update health source quantity for mana bar
        healthSourceQuantity.Width = (int)(UIsprites.list["manaFill"][0].Width * ((float)Hero[0].Hero.stats.getMana() / (float)Hero[0].Hero.stats.getMaxMana()));
		healthSourceQuantity.X = (int)(UIsprites.list["manaFill"][0].X);
		healthSourceQuantity.Y = (int)(UIsprites.list["manaFill"][0].Y);
		healthSourceQuantity.Width = (int)MathHelper.Clamp(healthSourceQuantity.Width, 0f, UIsprites.list["healthFill"][0].Width);

        sprite.Draw(UItex, manaBarOffset + namePlateOrigin, UIsprites.list["manaBackground"][0], Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);
        sprite.Draw(UItex, manaBarOffset + namePlateOrigin, healthSourceQuantity, Color.White, 0, Vector2.Zero, healthBarScale, SpriteEffects.None, 0f);

            //Draw the text        
        drawOutlinedText(healthText, 1f, namePlateOrigin + textSourcePos, ref sprite, Color.White, nameplateTextScale);

        healthText = Hero[0].Hero.stats.getMana() + " / " + Hero[0].Hero.stats.getMaxMana();
        textSize = font.MeasureString(healthText) * textScale;
        textSourcePos = new Vector2((int)(UIsprites.list["manaFill"][0].Width * healthBarScale.X / 2) - textSize.X / 2 + manaBarOffset.X,
            (int)(UIsprites.list["manaFill"][0].Height * healthBarScale.Y / 2) - textSize.Y / 2 + manaBarOffset.Y);

        drawOutlinedText(healthText, 1f, namePlateOrigin + textSourcePos, ref sprite, Color.White, nameplateTextScale);

        }

        public void drawCursor( ref SpriteBatch sprite )
        {
            if (currentCursor == cursorType.target)
                //The cursor should be offset!
                sprite.Draw(UItex, new Vector2(
                    mouse.X - UIsprites.list[cursors[(int)currentCursor]][cursorFramePos].Width * cursorScale * 0.5f,
                    mouse.Y - UIsprites.list[cursors[(int)currentCursor]][cursorFramePos].Height * cursorScale * 0.5f),
                    UIsprites.list[cursors[(int)cursorType.target]][cursorFramePos], Color.White, 0f, Vector2.Zero, cursorScale, SpriteEffects.None, 0f);
            else
            {
                try
                {
                    sprite.Draw(UItex, new Vector2(mouse.X, mouse.Y),
                        UIsprites.list[cursors[(int)currentCursor]][cursorFramePos], Color.White, 0f, Vector2.Zero, cursorScale, SpriteEffects.None, 0f);
                }
                catch (IndexOutOfRangeException)
                {
                    cursorFramePos = 0;
                    sprite.Draw(UItex, new Vector2(mouse.X, mouse.Y),
                        UIsprites.list[cursors[(int)currentCursor]][cursorFramePos], Color.White, 0f, Vector2.Zero, cursorScale, SpriteEffects.None, 0f);
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
                sprite.Draw(UItex, tooltip.sourcePos, UIsprites.list["TooltipFrameDiagonalTL"][0],
                    Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);

                //Draw the left vertical components:
                tempPos.X = tooltip.sourcePos.X;
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.Y - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {              
                    tempPos.Y = counter + tooltip.sourcePos.Y;
                    sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameVerticalL"][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }

                //Draw the bottom left corner
                tempPos.Y += tooltip.tileSize;
                sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameDiagonalBL"][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                float finalYHeight = tempPos.Y;

                tempPos.Y = tooltip.sourcePos.Y;
                //Draw the top horizontal components:
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.X - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {
                    tempPos.X = counter + tooltip.sourcePos.X;
                    sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameHorizontalT"][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }

                //Draw the top right corner
                tempPos.X += tooltip.tileSize;
                sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameDiagonalTR"][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                float finalXWidth = tempPos.X;

                tempPos.Y = finalYHeight;
                //Draw the bottom horizontal components:
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.X - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {
                    tempPos.X = counter + tooltip.sourcePos.X; ;
                    sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameHorizontalD"][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }
                
                //Draw the vertical right components
                tempPos.X = finalXWidth;
                for (counter = tooltip.tileSize;
                    counter < tooltip.Dimensions.Y - tooltip.tileSize;
                    counter += tooltip.tileSize)
                {
                    tempPos.Y = counter + tooltip.sourcePos.Y;
                    sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameVerticalR"][0],
                        Color.White, 0, Vector2.Zero, tooltipScale, SpriteEffects.None, 0f);
                }

                tempPos.Y += tooltip.tileSize;
                sprite.Draw(UItex, tempPos, UIsprites.list["TooltipFrameDiagonalBR"][0],
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
                        sprite.Draw(UItex, tempPos, UIsprites.list["TooltipRectangleFill"][0],
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


        private void drawGraphics( ref SpriteBatch sprite)
        {
            //Rectangle bottomRightIconBoundingBox = new Rectangle();

            try
            {           
              //Draw the bottom left hand icons
                for (byte iconNo = 0; iconNo < UIsprites.list["bottomRightIcon"].Length; iconNo++)
                {
                    sprite.Draw(UItex, bottomRightIconBoundingBox, UIsprites.list["bottomRightIcon"][iconNo], Color.White);
                    bottomRightIconBoundingBox.X += iconWidth;
                }
                if (currentClickedIcon != 0)
                    sprite.Draw(UItex, new Vector2(currentClickedIcon, bottomRightIconBoundingBox.Y), UIsprites.list["bottomRightIconClickOverlay"][0], Color.White);
                bottomRightIconBoundingBox.X -= iconWidth * UIsprites.list["bottomRightIcon"].Length;

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

                            for (byte itemColumnCounter = 0; itemColumnCounter < maxIconsPerRow && drawnItems < Hero[0].Hero.inventory.maxSpace; itemColumnCounter++)
                            {
                                //Set the source position to the correct position on the sprite sheet
                                itemSpriteSource.X = itemSprites[Hero[0].Hero.inventory.contents[drawnItems]].X;
                                itemSpriteSource.Y = itemSprites[Hero[0].Hero.inventory.contents[drawnItems]].Y;

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
                            screenDimensions, Hero[0], primaryStatsItemViewFormat(Hero[0].Hero.stats), font);
                        addMessage("Pushed character status state!");
                        break;
                    case state.SpellBook:
                        //windowActive = true;
                        //activeWindow = new UI_Components.Windows.WindowSpellbook(windowTexture, new Vector2(200, 200),
                        //    new Vector2(400, 200), windowSprites, new Point(mouse.X, mouse.Y), screenDimensions,
                        //    toItemViewFormat(Hero[0].Hero.abilities), font);
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
                            if (Hero[0].Hero.inventory.contents.Count <= maxIconsPerRow * yCounter + xCounter)
                                continue;

                            itemLocation.X = xPos;
                            itemLocation.Y = yPos;

                            if (Hero[0].Hero.inventory.contents[maxIconsPerRow * yCounter + xCounter] != 0)
                            {

                                if (itemLocation.Contains(mouse.X, mouse.Y))
                                {
                                    int itemIndex = maxIconsPerRow * yCounter + xCounter;

                                    Item selectedItem = Hero[0].heldItems[Hero[0].Hero.inventory.contents[itemIndex]];
                                    tooltip = new EquipmentToolTip(mousePos, ref font, selectedItem.name, null, selectedItem.description, Hero[0].Hero.inventory.quantity[itemIndex],
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

			for ( xCounter = 0; xCounter < maxEffectIconsPerRow && yCounter * maxEffectIconsPerRow + xCounter < Hero[0].Hero.stats.effects.Keys.Count; xCounter++)
			{
				int effectID = Hero[0].Hero.stats.effects.Keys.ElementAt(yCounter * maxEffectIconsPerRow + xCounter);

				if (effectIconLocalSpace.Contains(new Point(mouse.X, mouse.Y)))
				{
					tooltip = new EquipmentToolTip(
						mousePos, ref font, Hero[0].Hero.stats.effects[effectID].name,
						null, Hero[0].Hero.stats.effects[effectID].description, Hero[0].Hero.stats.getQuantity(effectID),
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
                if (currentObject == null || currentObject.ID == Hero[0].Hero.ID)
                {
                    tooltipActive = false;
                    return;
                }
                else 
                    tooltip = new CharacterTooltip(mousePos, new Point(screenDimensions.Width, screenDimensions.Height), font, currentObject, Hero[0].Hero);
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

        private List<UI_Components.WindowItemTooltip> toItemViewFormat( List<Abilities.AbilityContainer> _abilities )
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

    }
}
