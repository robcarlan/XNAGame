using System;
using System.Collections;
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

    class ObjManager
    {
        //Contains sprite data (rectangles)
        public const float tileLength = (Declaration.Scale * Declaration.tileLength);
        public SpriteList SpriteList = new SpriteList();
        public Dictionary<string, Texture2D> textureList = new Dictionary<string, Texture2D>();
        
        //Contains the keys, boolean value to represent taken or not.
        public int gameHeight;
        private const short maxEntities = 1000;
        private const short maxLengthForLocal = 1000;
        protected const int particleUpdateRangeSqrd = 500 * 600;
        public Stack<short> freeID;

        public int scrWidth  { get; set; }
        public int scrHeight { get; set; }
        public float scale { get; set; }

        //Object libraries, used as a factory to create new entities
        public Entity_Manager.CharacterTypes characterLibrary;
        public readonly Dictionary<short, Entity_Components.Dynamic.ParticleEmitter> particleLibrary;
        public readonly Dictionary<short, Abilities.AbilityBase> abilityLibrary;
        public readonly Dictionary<short, Doodad> doodadLibrary;
        Entity_Components.Dynamic.ParticleSystem masterSystem;

        //Individual objects which are in existence
        public Dictionary<short, Character_Components.Character> Characters = new Dictionary<short, Character_Components.Character>();
        public Dictionary<short, Collidable_Sprite> Entities = new Dictionary<short, Collidable_Sprite>();
        public Dictionary<short, Doodad> Doodads = new Dictionary<short, Doodad>();
        public Dictionary<short, Entity_Components.Dynamic.ParticleEmitter> particles = new Dictionary<short, Entity_Components.Dynamic.ParticleEmitter>();
        public Dictionary<short, Abilities.AbilityBase> abilities = new Dictionary<short, Abilities.AbilityBase>();
        public Entity_Manager.LightManager lightManager;    
                //Todo : Make an abstract manager class with templates
                //  use these for all objects

        //Map data  --> to improve efficiency, could load the nearest x sectors
        public Dictionary<Point, DataLoader.MapData> worldData;
        
        //'optional' groups to make access easier
        public List<short> drawingEntity = new List<short>(); //Indicates an entity is to be drawn.
        public List<short> localEntity = new List<short>();   //Entity is within spcae to be updated every frame.
        public List<short> distantEntity = new List<short>(); //If entity is not local, it will be here
        public List<short> enemy = new List<short>();
        public List<short> npc = new List<short>();
        public List<short> scenery = new List<short>();       //New methods and IDarray for noncharacter entities
        public List<short> particleEntities = new List<short>(maxParticleEntities); //Entities linked to a particle

        //Cell Entities (only for objects in entities list, such as doodads, characters)
        public Point currentCell;
        public Point leftCell;
        public Point rightCell;
        public Point topCell;
        public Point bottomCell;
        public Point topLeftCell;
        public Point topRightCell;
        public Point bottomLeftCell;
        public Point bottomRightCell;

        #region maxEntities
        //Represents the max amount of entities allowed to be allocated
        protected const short maxLights = 25;
        protected const short maxParticleEntities = 20;
        #endregion 

        //Hero
        public List<Player> hero = new List<Player>(1);
        public tileManager tileManager = new tileManager();
        //Contains Quadtree information
        public Quadtree quadtree;

        //Management
        public List<string> UIMessages = new List<string>();
        protected Vector2 Center;
        protected int maxLengthPerLocalSquared = maxLengthForLocal * maxLengthForLocal;

		//Settings
		Settings.particleCount particleCount;

        public ObjManager(Rectangle screen, 
            Dictionary<string, Rectangle[]> tiles, 
            Dictionary<string, DataLoader.SpriteListLoader> sprites,
            Dictionary<short, DataLoader.ParticleLoader> particleLib)
        {
            //Set values
            masterSystem = new Entity_Components.Dynamic.ParticleSystem(Vector2.Zero);
            particleLibrary = new Dictionary<short, Entity_Components.Dynamic.ParticleEmitter>();
            characterLibrary = new Entity_Manager.CharacterTypes();
            abilityLibrary = new Dictionary<short, Abilities.AbilityBase>();
            doodadLibrary = new Dictionary<short, Doodad>();
            lightManager = new Entity_Manager.LightManager();

            particleEntities.Capacity = maxParticleEntities;

			freeID = new Stack<short>();
			for (short i = 0; i < maxEntities && i < 999; i++)
			{
				freeID.Push(i);
			}
			for (short i = 1000; i < maxEntities; i++)
			{
				freeID.Push(i);
			}
			//Reserve 999 for main character

            quadtree = new Quadtree(false, screen);
            scrHeight = screen.Height;
            scrWidth = screen.Width;
            scale = Declaration.Scale;
            screenSizeChange(scrWidth, scrHeight);

            foreach (string key in tiles.Keys)
                SpriteList.list.Add(key, tiles[key]);
            UIMessages.Add("Loaded " + tiles.Count + " tiles");

            foreach (string key in sprites.Keys)
                SpriteList.setFrames(ref sprites[key].spriteRec, sprites[key].spriteRec.Width, sprites[key].spriteRec.Height, sprites[key].numberOfFrames, key);
            UIMessages.Add("Loaded " + sprites.Count + " sprites");

            foreach (short ID in particleLib.Keys)
                particleLibrary.Add(ID, Functions.toParticle(particleLib[ID]));
            UIMessages.Add("Loaded " + particleLib.Count + " particle effects");
        //Todo : function to get current cells based on character position
        }

		public void loadSettings(Settings.particleCount particleCount)
		{
			this.particleCount = particleCount;
			//Apply new settings to each particle system
		}

        public void onCharacterLoad()
        {
            currentCell = hero[0].currentCell;
            loadCells();
        }

        public void setParticleTextures()
        {
            foreach (short ID in particleLibrary.Keys)
                particleLibrary[ID].particleImage = textureList[particleLibrary[ID].texture];
        }

        private short generateID()
        {
            //Improvements - randomise to improve access times
			if (freeID.Count == 0)
			{
				UIMessages.Add(Functions.WriteDebugLine("Could not generate ID for object - max entities reached"));
				return -1;
			}
			else
			{
				short ID = freeID.Pop();
				UIMessages.Add(Functions.WriteDebugLine("Generated ID: " + ID + " for object"));
				return ID;
			}
			
        }

        private void removeID(short ID)
        {
			freeID.Push(ID);
        }

        public void drawAll(ref SpriteBatch spriteBatch, ref float tileOffsetX, ref float tileOffsetY)
        {
            Rectangle rec = new Rectangle(0, 0, (int)tileLength, (int)tileLength);
            rec.Offset((int)(0 - tileOffsetX), (int)(0 - tileOffsetY));
            byte y = new byte();
            byte x = new byte();
            y = tileManager.arrayStartY;
            x = tileManager.arrayStartX;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone );

            //I dont even...
            rec.Y = (int)(0 - tileOffsetY - tileLength + tileManager.Offset.Y  );
            do
            {
                rec.X = (int)(0 - tileOffsetX - 2 * tileLength + tileManager.Offset.X); //tileScale buffer from like 2 tileScale back
                do
                {
                    if (rec.X < scrWidth && rec.Y < scrHeight)
                    {
                        spriteBatch.Draw(textureList["tileset"], rec, SpriteList.getFrame(tileManager.onscreenTiles[x, y].value,
                                     tileManager.onscreenTiles[x, y].framePosX), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    rec.X += (int)(tileLength);

                    if (x == tileManager.arraySize - 1) x = 0;
                    else x++;

                } while (x != tileManager.arrayStartX);


                rec.Y += (int)(tileLength);

                if (y == tileManager.arraySize - 1) y = 0;
                else y++;
            } while (y != tileManager.arrayStartY);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);


            hero[0].Hero.zPos = Entities[998].zPos = 300;

            foreach (short drawingEntity_ID in drawingEntity)
            {
                spriteBatch.Draw(textureList["chara01_a"], new Vector2((int)Entities[drawingEntity_ID].localSpace.X, (int)Entities[drawingEntity_ID].localSpace.Y),
                    SpriteList.list[Entities[drawingEntity_ID].name + 
                            Entities[drawingEntity_ID].animID][Entities[drawingEntity_ID].FramePosX],
                    Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, MathHelper.Clamp(1-(float)Entities[drawingEntity_ID].localSpace.Y / scrHeight, 0f, 1f));
            }

            spriteBatch.Draw(textureList["chara01_a"], new Vector2(hero[0].Hero.localSpace.X, hero[0].Hero.localSpace.Y),
                     SpriteList.list[hero[0].Hero.name + hero[0].Hero.animID][hero[0].Hero.FramePosX],
                     Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1-((float)hero[0].Hero.localSpace.Y)/ (scrHeight));

            spriteBatch.End();
        }

        public void drawZMap(ref SpriteBatch spriteBatch, ref Effect gBufferEffect, ref float tileOffsetX, ref float tileOffsetY)
        {
            //Set up gBufferEffect - minimise texture swapping: use a list of all entities using a specific texture
            gBufferEffect.Parameters["depth"].SetValue(1f);     //Tiles all use the same depth
            gBufferEffect.Parameters["colorTex"].SetValue(textureList["tileset"]);
            gBufferEffect.CurrentTechnique.Passes[0].Apply();

            Rectangle rec = new Rectangle(0, 0, (int)tileLength, (int)tileLength);
            rec.Offset((int)(0 - tileOffsetX), (int)(0 - tileOffsetY));
            byte y = new byte();
            byte x = new byte();
            y = tileManager.arrayStartY;
            x = tileManager.arrayStartX;

            //Draw Tiles

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, gBufferEffect);
            //I dont even...
            rec.Y = (int)(0 - tileOffsetY - tileLength + tileManager.Offset.Y);
            do
            {
                rec.X = (int)(0 - tileOffsetX - 2 * tileLength + tileManager.Offset.X); //tileScale buffer from like 2 tileScale back
                do
                {
                    if (rec.X < scrWidth && rec.Y < scrHeight)
                    {
                        spriteBatch.Draw(textureList["tileset"], rec, SpriteList.getFrame(tileManager.onscreenTiles[x, y].value,
                                     tileManager.onscreenTiles[x, y].framePosX), Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                    }
                    rec.X += (int)(tileLength);

                    if (x == tileManager.arraySize - 1) x = 0;
                    else x++;

                } while (x != tileManager.arrayStartX);


                rec.Y += (int)(tileLength);

                if (y == tileManager.arraySize - 1) y = 0;
                else y++;
            } while (y != tileManager.arrayStartY);

            spriteBatch.End();

            //Draw objects
            gBufferEffect.Parameters["colorTex"].SetValue(textureList["chara01_a"]);
            gBufferEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, gBufferEffect);

            Vector2 depthValue;
            Random rand = new Random();
            hero[0].Hero.zPos = 550;
            Entities[998].zPos = 100;       

            foreach (short drawingEntity_ID in drawingEntity)
            {
                depthValue.X = depthValue.Y = Entities[drawingEntity_ID].zPos;
                gBufferEffect.Parameters["depth"].SetValue(depthValue);
				gBufferEffect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(textureList["chara01_a"], new Vector2((int)Entities[drawingEntity_ID].localSpace.X, (int)Entities[drawingEntity_ID].localSpace.Y),
                    SpriteList.list[Entities[drawingEntity_ID].name +
                            Entities[drawingEntity_ID].animID][Entities[drawingEntity_ID].FramePosX],
                    Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, MathHelper.Clamp(1 - (float)Entities[drawingEntity_ID].localSpace.Y / scrHeight, 0f, 1f));
            }

            depthValue.X = depthValue.Y = hero[0].Hero.zPos;
            gBufferEffect.Parameters["depth"].SetValue(depthValue);
			gBufferEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(textureList["chara01_a"], new Vector2(hero[0].Hero.localSpace.X, hero[0].Hero.localSpace.Y),
                     SpriteList.list[hero[0].Hero.name + hero[0].Hero.animID][hero[0].Hero.FramePosX],
                     Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1 - ((float)hero[0].Hero.localSpace.Y) / (scrHeight));

            spriteBatch.End();

        }

        public void drawParticles (ref SpriteBatch sprite )
        {
			sprite.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
			foreach (short key in particles.Keys)
			{
				if (Functions.getLengthSqrd(hero[0].Hero.circleOrigin, particles[key].relPosition) < particleUpdateRangeSqrd)
					particles[key].draw(sprite, 2, particles[key].relPosition);
			}

			sprite.End();
        }

        public void drawLights(ref SpriteBatch sprite)
        {

        }

        public void screenSizeChange(int newWidth, int newHeight)
        {
            int widthChange = (newWidth  - scrWidth) / 2;
            int heightChange = (newHeight - scrHeight) / 2;
            scrWidth = newWidth;
            scrHeight = newHeight;
            foreach (short localEntity_ID in localEntity)
            {
                    Entities[localEntity_ID].localSpace.X += widthChange;
                    Entities[localEntity_ID].localSpace.Y += heightChange;
                    Entities[localEntity_ID].circleOrigin.X += widthChange;
                    Entities[localEntity_ID].circleOrigin.Y += heightChange;
            }

            quadtree.updateScreen(new Rectangle(0, 0, newWidth, newHeight));
            Center.X = scrWidth / 2;
            Center.Y = scrHeight / 2;

            moveParticles( new Vector2 (widthChange, heightChange));
            gameHeight = scrHeight;
        }

        //Assigns an object to groups based on it's values
        protected void assignGroups(short ID)
        {

            if (checkCollision(ref Entities[ID].localSpace))
            {
                drawingEntity.Add(ID);
                UIMessages.Add(ID + " is a drawingEntity!");
            }
        }

        public void addChar( ref Character_Components.Character L )
        {
            //Character_Components.Character L = new Character_Components.Character(ref temp);
			L.localSpace.Width = (int)((float)L.localSpace.Width * scale);
			L.localSpace.Height = (int)((float)L.localSpace.Height * scale);
            //Generate a free ID, and slot it in!
            L.ID = generateID();
            L.FramePosXMax = (byte)(SpriteList.list[L.name + L.animID].Length - 1);

            //Add sprite to certain groups depending on ID
            Characters.Add(L.ID, L);
            Entities.Add(L.ID, L);

            //Optional groups
            if (checkLength(ref L.localSpace))
            {
                localEntity.Add(L.ID);
                quadtree.insertObject(L);
            }
            if (checkCollision(ref L.localSpace))
                drawingEntity.Add(L.ID);
        }

        /// <summary>
        ///Adds a doodad (acquired from a list of all doodads)
        /// </summary>
        /// <param name="temp"></param>
        public void addDoodad( short doodadID, Point tilePos, Vector2 _Offset )
        {
            if (!doodadLibrary.ContainsKey(doodadID))
            {
                UIMessages.Add("Could not create doodad; library does not contain key: " + doodadID);
                return;
            }
            else
            {
                Doodad temp = new Doodad(doodadLibrary[doodadID]);
                temp.ID = generateID();
                temp.tilePos = tilePos;
                temp.Offset = _Offset;

                Doodads.Add(temp.ID, temp);
                Entities.Add(temp.ID, temp);
                UIMessages.Add("Created new doodad (" + doodadID + ") at tile position: X{" + temp.tilePos.X + "}, Y{ + " + temp.tilePos.Y + "}");
                assignGroups(doodadID);
            }
            
        }

        public void addAbility( short abilityID, short ownerID, short targetID )
        {
            //Inserts an ability, contains ID's about parent ID, target ID (if any)
            short currentAbilityID = generateID();

            if (currentAbilityID == -1)
            {
                UIMessages.Add("Could not generate ID for ability " + abilityID +
                    " (" + abilities[abilityID].name + ").");
                return;
            }

            abilities.Add(currentAbilityID, abilityLibrary[abilityID]);
            abilities[currentAbilityID].ID = currentAbilityID;
            abilities[currentAbilityID].ownerID = ownerID;
            abilities[currentAbilityID].targetID = targetID;

            //Add the particle effects
            foreach (short particleID in abilityLibrary[abilityID].castParticleID)
            {
                addParticleEffect(currentAbilityID, particleID);
            }
        }

        protected void addAbilityExtras(Abilities.AbilityBase ability, short abilityID )
        {
            for (short i = 0; i < ability.castParticleID.Count; i++)
            {
                addParticleEffect(abilityID, ability.castParticleID[i]);
            }

            //Apply use effects
        }

        protected void addAbilityExtras(Abilities.ProjectileAbility ability, short abilityID)
        {
            //Call base method
            addAbilityExtras((Abilities.AbilityBase)ability, abilityID);
        }

        public void addParticleEffect(short abilityID, short particleID)
        {
            //Obtain the effect form the list of all effects
            short currentParticleID = generateID();

            if (currentParticleID == -1)
            {
                UIMessages.Add("Could not generate ID for particle effect " + particleID + 
                    " for ability " + abilities[abilityID].ID + " (" + abilities[abilityID].name + ").");
                return;
            }

            addParticleIDs(particleID, currentParticleID);
            abilities[abilityID].activeParticleIDs.Add(currentParticleID);
            particles[currentParticleID].parentEntityID = abilityID;
            //Add id's to the ability and particle
        }

        public void addParticleEffect(short particleID, Point tilePos, Vector2 offset )
        {
            short currentParticleID = generateID();

            if (currentParticleID == -1)
            {
                UIMessages.Add("Could not generate particle effect - too many objects in existence!");
            }

            Entity_Components.Dynamic.ParticleEmitter temp = new Entity_Components.Dynamic.ParticleEmitter(particleLibrary[particleID]);
            temp.relPosition = toLocalSpace(tilePos, offset);
            UIMessages.Add("Created new particle effect at X: " + temp.relPosition.X + ", Y:" + temp.relPosition.Y);
            particles.Add(currentParticleID, temp);
            particles[currentParticleID].ID = currentParticleID;
            particles[currentParticleID].parent = masterSystem;
            particles[currentParticleID].random = new Random((int)temp.relPosition.X);
        }

        private void addParticleIDs(short particleLibID, short currentParticleID)
        {
            particles.Add(currentParticleID, new Entity_Components.Dynamic.ParticleEmitter(particleLibrary[particleLibID]));
			particles[currentParticleID].maxParticles = (int)(Settings.funcs.getParticleCountModifier(particleCount) * particles[currentParticleID].maxParticles); 
            particles[currentParticleID].ID = currentParticleID;
            particles[currentParticleID].parent = masterSystem;
        }

        public void removeAbility(short abilityID)
        {
            abilities.Remove(abilityID);

            for (short i = 0; i < abilities[abilityID].activeParticleIDs.Count; i++)
            {
                particles.Remove(abilities[abilityID].activeParticleIDs[i]);
                UIMessages.Add("Removed Particle effect: " + abilities[abilityID].activeParticleIDs[i]
                    + ", due to destruction of ability " + abilityID);
            }

            //Add the new destruction particle effects
            //for (short i = 0; i < abilities[abilityID]..Count; i++)
            //{
            //    particles.Remove(abilities[abilityID].particleIDs[i]);
            //}

            removeID(abilityID);
        }

        public void removeParticleEffect(short particleID)
        {
            masterSystem.EmitterList.Remove(particles[particleID]);
            particles.Remove(particleID);
            removeID(particleID);
        }

        protected bool checkLength(ref Rectangle test)
        {

            //calculate length between two points, check against maximum length.
            if ((Math.Pow((Center.X - test.X), 2) + Math.Pow((Center.Y - test.Y), 2)) < maxLengthPerLocalSquared)
                return true;
            return false;
        }

        protected bool checkCollision( ref Rectangle test )
        {

            if (test.X > 0 && test.X + test.Width < scrWidth)
            {
                if (test.Bottom > 0 && test.Y < scrHeight)
                    return true;
                else return false;
            } return false;
        }

        protected bool isDrawingEntity()
        {
            return true;
        }

        public LinkedList<Vector3> generatePath(ref Vector3 start, Vector3 target, ref LinkedList<direction>closedList)
        {
            Vector3 currentCell = start;
            while (start != target)
            {

            }
            /* get movement cost of each cell
             * get distance cost of each cell from goal
             * add cheapest cell to closed list
             * 
             * 
             * 
             * 
             * return closed list 
            */
            return null;
        }
        public void advanceFrames()
        {
            foreach (short localEntity_ID in localEntity)
            {
                Entities[localEntity_ID].frameAdvance();
            }
        }

        public void updateAll( float msPassed )
        {

            updateAbilities(msPassed);
            updateParticles(msPassed/1000f);

            //Frame based update to all entities
            foreach (short characterID in Characters.Keys)
            {
                Characters[characterID].stats.updateValues((short)msPassed);
            }
            
            //Update values
            foreach (short localEntity_ID in localEntity )
            {
                //Entities[localEntity_ID].localSpace.X += (int)Entities[localEntity_ID].vel.X;
                //Entities[localEntity_ID].localSpace.Y += (int)Entities[localEntity_ID].vel.Y;
                //Entities[localEntity_ID].circleOrigin.X += (int)Entities[localEntity_ID].vel.X;
                //Entities[localEntity_ID].circleOrigin.Y += (int)Entities[localEntity_ID].vel.Y;
                Entities[localEntity_ID].applyVelocities();
				quadtree.updateItem(Entities[localEntity_ID]);
            }

			//Check collisions
            foreach (short localEntity_ID in localEntity)
            {
                quadtree.testCollisions(Entities[localEntity_ID]);
            }

			//Calculate difference between hero and center of screen
			float xDifference = hero[0].Hero.localSpace.X + (hero[0].Hero.localSpace.Width - scrWidth) / 2;
			float yDifference = hero[0].Hero.localSpace.Y + (hero[0].Hero.localSpace.Height - scrHeight) / 2;

			//Update the positions based on the hero's offset
            for (short IDcounter = 0; IDcounter < localEntity.Count; IDcounter++)
			{
                short IDValue = localEntity[IDcounter];
                Entities[IDValue].offsetPosition((short)(xDifference), (short)(yDifference));
                Entities[IDValue].UpdatePosition();
                quadtree.updateItem(Entities[IDValue]);
			}

            moveParticles(hero[0].Hero.vel*-1);

			//These woould make neato functions
			//Offset the hero's position to get it back to the center of the screen
		//	hero[0].Hero.circleOrigin.X -= xDifference;
		//	hero[0].Hero.circleOrigin.Y -= yDifference;
		//	hero[0].Hero.localSpace.X = (int)(hero[0].Hero.circleOrigin.X - hero[0].Hero.localSpace.Width / 2f);
			//hero[0].Hero.localSpace.Y = (int)(hero[0].Hero.circleOrigin.Y - hero[0].Hero.localSpace.Height / 2f);

			
        }

        private void updateAbilities(float msPassed)
        {
            bool result;

            for (short i = (short)abilities.Count; i > 0; i--)
            {
                result = abilities[i].update(msPassed);

                if (result)
                    removeAbility(abilities.ElementAt(i).Key);
            }
        }


        //Todo: only update if local entity
        private void updateParticles(float msPassed)
        {
            short key = 0;
            for (short i = (short)(particles.Count - 1); i >= 0; i--)
            {
                key = particles.ElementAt(i).Key;

                if (Functions.getLengthSqrd(hero[0].Hero.circleOrigin, particles[key].relPosition) < particleUpdateRangeSqrd)
                    particles[key].update(msPassed);

                if (particles[key].readyToDestroy )
                {
                    //Remove effect
                    removeParticleEffect(key);
                }
            }
        }

        private void moveParticles(Vector2 movement)
        {
            short key = 0;
            for (short i = (short)(particles.Count - 1); i >= 0; i--)
            {
                key = particles.ElementAt(i).Key;
                particles[key].relPosition += movement;
            }
        }

        private void sprAdvance(Sprite temp)
        {
            temp.queueAdvance();
            temp.frameAdvance();
        }

        /// <summary>
        /// Removes an object from all groups
        /// </summary>
        /// <param name="ID"></param>
        public void destroyObject(short ID)
        {

        }

        public void onCellMoveRight()
        {
            bottomLeftCell = bottomCell;
            leftCell = currentCell;
            topLeftCell = topCell;
            currentCell = rightCell;
            topCell = topRightCell;
            bottomCell = bottomRightCell;

            //Load the new cells
        }
        public void onCellMoveLeft()
        {

        }
        public void onCellMoveUp()
        {

        }
        public void onCellMoveDown()
        {

        }
        public void onCellMoveRightUp()
        {

        }
        public void onCellMoveRightDown()
        {

        }
        public void onCellMoveLeftUp()
        {

        }
        public void onCellMoveLeftDown()
        {

        }

        private void loadCells()
        {
            //Get the neighbouring cells, and load them from mapData
            tileManager.currentCell = hero[0].currentCell;
            leftCell = tileManager.getHCell(false);
            rightCell = tileManager.getHCell(true);
            topCell = tileManager.getVCell(true);
            bottomCell = tileManager.getVCell(false);
            topLeftCell = tileManager.getDCell(false, true);
            topRightCell = tileManager.getDCell(true, true);
            bottomLeftCell = tileManager.getDCell(false, false);
            bottomRightCell = tileManager.getDCell(true, false);

            //load map data
            loadMapData(currentCell);
            loadMapData(leftCell);
            loadMapData(rightCell);
            loadMapData(topCell);
            loadMapData(topLeftCell);
            loadMapData(topRightCell);
            loadMapData(bottomCell);
            loadMapData(bottomLeftCell);
            loadMapData(bottomRightCell);
        }

        private void unloadMapData(Point tileSector)
        {
            //Delete all entities from correct sector list
        }

        private void loadMapData(Point tileSector)
        {
            //Add each entity
            //Tilepos = relative pos + tileSector

            for (short i = 0; i < worldData[tileSector].Doodads.Count; i++)
            {
                addDoodad(worldData[tileSector].Doodads[i].ID,
					new Point(
                    tileSector.X * Functions.tilesPerSector + worldData[tileSector].Doodads[i].relTilePos.X,
                    tileSector.Y * Functions.tilesPerSector + worldData[tileSector].Doodads[i].relTilePos.Y),
                    worldData[tileSector].Doodads[i].offset);
            }

			for (short i = 0; i < worldData[tileSector].Particles.Count; i++)
			{
				addParticleEffect(worldData[tileSector].Particles[i].ID,
					new Point(worldData[tileSector].Particles[i].relTilePos.X + tileSector.X * Functions.tilesPerSector,
						worldData[tileSector].Particles[i].relTilePos.Y + tileSector.Y * Functions.tilesPerSector),
					worldData[tileSector].Particles[i].offset);
			}
        }
        public void getMapData(Dictionary<Point, DataLoader.MapData> temp)
        {
            worldData = temp;
        }

        public Vector2 toLocalSpace( Point tilePos, Vector2 offset )
        {
            Vector2 temp;
            temp.X = (tilePos.X - tileManager.tileOriginX) * Declaration.tileGameSize + offset.X + tileManager.Offset.X;
            temp.Y = (tileManager.tileOriginY - tilePos.Y) * Declaration.tileGameSize + offset.Y + tileManager.Offset.Y;
            return temp;
        }

    }

   
}
