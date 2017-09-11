using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WindowsGame1.Entity_Components;

namespace WindowsGame1
{

    public class ObjManager
    {
        //Contains sprite data (rectangles)
        public const float tileLength = (Declaration.Scale * Declaration.tileLength);
        public SpriteList SpriteList = new SpriteList();
		public Dictionary<int, Rectangle> projectileSprites = new Dictionary<int, Rectangle>();
        public Dictionary<string, Texture2D> textureList = new Dictionary<string, Texture2D>();
		public Texture2D shadowTex;
		public Rectangle debugRect = new Rectangle(0, 0, 10, 10);
		public Rectangle debugCircle = new Rectangle(10, 0, 10, 10);
		public Rectangle[] shadowSprites;
		public Vector2[] shadowHalfOffset;
		public float[] shadowRectangleWidths;
        
        //Contains the keys, boolean value to represent taken or not.
        public int gameHeight;
        private const short maxEntities = 1000;
        private const short maxLengthForLocal = 1000;
        protected const int particleUpdateRangeSqrd = 1000 * 1000;
        public Stack<short> freeID;

        public int scrWidth  { get; set; }
        public int scrHeight { get; set; }
        public float scale { get; set; }

        //Object libraries, used as a factory to create new entities
        public Entity_Manager.CharacterTypes characterLibrary;
        public Dictionary<short, Entity_Components.Dynamic.ParticleEmitter> particleLibrary;
        public Dictionary<short, Abilities.AbilityBase> abilityLibrary;
        public Dictionary<short, Doodad> doodadLibrary;
		public Dictionary<short, DataLoader.LightLoader> lightLibrary;
		public Dictionary<short, Entity_Components.Projectile> projectileLibrary;
        Entity_Components.Dynamic.ParticleSystem masterSystem;

        //Individual objects which are in existence
        public Dictionary<short, Character_Components.Character> Characters = new Dictionary<short, Character_Components.Character>();
        public Dictionary<short, Collidable_Sprite> Entities = new Dictionary<short, Collidable_Sprite>();
			//Entities contain Characters and Doodads
        public Dictionary<short, Doodad> Doodads = new Dictionary<short, Doodad>();
        public Dictionary<short, Entity_Components.Dynamic.ParticleEmitter> particles = new Dictionary<short, Entity_Components.Dynamic.ParticleEmitter>();
        public Dictionary<short, Abilities.AbilityBase> abilities = new Dictionary<short, Abilities.AbilityBase>();
        public Entity_Manager.LightManager lightManager;    
                //Todo : Make an abstract manager class with templates
                //  use these for all objects

        //Map data  --> to improve efficiency, could load the nearest x sectors
        public Dictionary<Point, DataLoader.MapData> worldData;
		//Cells store static data, objects like NPC's are loaded differently.
		public Queue<Point> cellsToLoad;
		public List<Point> loadedCells;				//List of all cells with loaded content
		public Dictionary<Point, short> cellData;	//ID of each entity in a specific cell is contained here.
        
        //'optional' groups to make access easier
        public List<short> drawingEntity = new List<short>(); //Indicates an entity is to be drawn.
        public List<short> localEntity = new List<short>();   //Entity is within spcae to be updated every frame.
        public List<short> distantEntity = new List<short>(); //If entity is not local, it will be here
        public List<short> enemy = new List<short>();
        public List<short> npc = new List<short>();
        public List<short> scenery = new List<short>();       //New methods and IDarray for noncharacter entities
        public List<short> particleEntities = new List<short>(maxParticleEntities); //Entities linked to a particle
		public List<short> stoppedParticles = new List<short>();
		public List<short> projectiles = new List<short>();

		public Entity_Manager.EffectManager effectManager;

        #region maxEntities
        //Represents the max amount of entities allowed to be allocated
        protected const short maxLights = 100;
        protected const short maxParticleEntities = 100;
        #endregion 

        //Hero
        public Player hero;
		public Camera gameCamera;

        //Contains Quadtree information
        public Quadtree quadtree;
		public Utility.Navmesh navmesh = new Utility.Navmesh(); //Each entity should have their own reference to this!

        //Management
        public List<string> UIMessages = new List<string>();
        protected Vector2 Center;
        protected int maxLengthPerLocalSquared = maxLengthForLocal * maxLengthForLocal;
		public Random rand = new Random();
		public Utility.ScriptEngine scriptEngine;

		//Settings
		Settings.particleCount particleCount;

        public ObjManager(Rectangle screen, ContentManager content, GraphicsDevice gfx, 
			Entity_Manager.EffectManager effectManager, Utility.ScriptEngine _Script)
        {
            //Set values / initialise structures
            masterSystem = new Entity_Components.Dynamic.ParticleSystem(Vector2.Zero, content);
            particleLibrary = new Dictionary<short, Entity_Components.Dynamic.ParticleEmitter>();
            characterLibrary = new Entity_Manager.CharacterTypes();
            abilityLibrary = new Dictionary<short, Abilities.AbilityBase>();
            doodadLibrary = new Dictionary<short, Doodad>();
            lightManager = new Entity_Manager.LightManager();
			cellData = new Dictionary<Point, short>();
			lightLibrary = new Dictionary<short, DataLoader.LightLoader>();
			cellsToLoad = new Queue<Point>();
			loadedCells = new List<Point>();
			this.effectManager = effectManager;
			scriptEngine = _Script;

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

			loadContent(content, gfx);
			//Create SpriteList for tilemanager which follows [tileID][framePos] -> contained in TileManager class, not ObjManager
			//foreach (string key in tiles.Keys)
			//    SpriteList.list.Add(key, tiles[key]);
			//UIMessages.Add("Loaded " + tiles.Count + " tiles");
        }

		public void loadContent(ContentManager content, GraphicsDevice gfx)
		{
			Dictionary<int, Dictionary<int, DataLoader.SimpleSpriteListLoader>> sprites = 
				//content.Load<Dictionary<int, Dictionary<int, DataLoader.SimpleSpriteListLoader>>>("Object Databases\\CharacterSprites");
				Game1.LoadContent<Dictionary<int, Dictionary<int, DataLoader.SimpleSpriteListLoader>>>
					(Declaration.characterSpriteLoaderPath + ".xml");
            Dictionary<short, DataLoader.ParticleLoader> particleLib = Game1.LoadContent<Dictionary<short, DataLoader.ParticleLoader>>
				(Declaration.particleLoaderPath + ".xml");
				//content.Load<Dictionary<short, DataLoader.ParticleLoader>>("Object Databases\\Particles");
			Dictionary<short, DataLoader.LightLoader> lightLib = Game1.LoadContent<Dictionary<short, DataLoader.LightLoader>>
				(Declaration.lightLoaderPath + ".xml");
				//content.Load<Dictionary<short, DataLoader.LightLoader>>("Object Databases\\LightLibrary");

			Dictionary<short, DataLoader.ProjectileLoader> projLib = 
				Game1.LoadContent<Dictionary<short, DataLoader.ProjectileLoader>>(Declaration.projectileLoaderPath + ".xml");

			int animNameCtr = 0;
			int animIDCtr = 0;
			foreach (int _animName in sprites.Keys)
			{
				animNameCtr++;
				foreach (int _animID in sprites[_animName].Keys)
				{
					animIDCtr++;
					SpriteList.setFrames(ref sprites[_animName][_animID].spriteRec, sprites[_animName][_animID].spriteRec.Width,
						sprites[_animName][_animID].spriteRec.Height, sprites[_animName][_animID].numberOfFrames, _animName, _animID);
				}
			}
			UIMessages.Add("Loaded " + animNameCtr + " sprite characters, with " + animIDCtr + " animations.");

			foreach (short ID in particleLib.Keys)
			{
				particleLibrary.Add(ID, Functions.toParticle(particleLib[ID]));
				particleLibrary[ID].sprites = masterSystem.Sprites;
			}
            UIMessages.Add("Loaded " + particleLib.Count + " particle effects");

			lightLibrary = lightLib;
			UIMessages.Add("Loaded" + lightLib.Keys.Count + " lights");

			lightManager.lightTex = content.Load<Texture2D>("Graphical Effects\\circle");
			lightManager.blankTex = new Texture2D(gfx, scrWidth, scrHeight);

			shadowTex = content.Load<Texture2D>("Sprite Content\\Shadows");
			Dictionary<int, Rectangle> shadowList = content.Load<Dictionary<int, Rectangle>>("Object Databases\\ShadowSprites");
			shadowHalfOffset = new Vector2[shadowList.Count];
			shadowRectangleWidths = new float[shadowList.Count];
			shadowSprites = new Rectangle[shadowList.Count];
			textureList.Add("projectiles", content.Load<Texture2D>("Sprite Content\\Projectiles"));
			for (int i = 0; i < shadowList.Count; i++)
			{
				shadowSprites[i] = shadowList[i];
				shadowHalfOffset[i] = new Vector2(shadowSprites[i].Width / 2, shadowSprites[i].Height / 2) * scale;
				shadowRectangleWidths[i] = shadowSprites[i].Width * Declaration.Scale;
			}

			projectileLibrary = new Dictionary<short, Entity_Components.Projectile>();
			foreach (KeyValuePair<short, DataLoader.ProjectileLoader> tempProjectile in projLib)
			{
				projectileLibrary.Add(tempProjectile.Key, Functions.toProjectile(tempProjectile.Value, tempProjectile.Key, this));
			}
			projectileSprites = Game1.LoadContent<Dictionary<int, Rectangle>>(Declaration.projectileSpriteLoaderPath + ".xml");
		}

		public void loadSettings(Settings.particleCount particleCount)
		{
			this.particleCount = particleCount;
			//Apply new settings to each particle system
		}

        public void onCharacterLoad()
        {
            
        }

        public void setParticleTextures()
        {
            foreach (short ID in particleLibrary.Keys)
                particleLibrary[ID].particleImage = textureList[particleLibrary[ID].texture];
        }

        private short generateID()
        {
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

        public void drawAll(SpriteBatch spriteBatch, float tileOffsetX, float tileOffsetY, Effect fx, Vector2 view)
        {
            // spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, 
			//	DepthStencilState.Default, RasterizerState.CullNone, fx);
			fx.Parameters["viewport"].SetValue(view);
			fx.Techniques[0].Passes[0].Apply();
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp,
				DepthStencilState.Default, null, fx);

			Vector2 spriteLocalPos = Vector2.Zero;
			Color spriteColour = Color.White;
			Vector2 source = Vector2.Zero;

			//More efficient draw loop than foreach
			//Change to loop through chracters!!!
			for (short itr = 0; itr < drawingEntity.Count; itr++)
			{
				short entID = drawingEntity[itr];
				if (projectiles.Contains(entID)) continue;
				Rectangle sprite  = SpriteList.list[Entities[entID].animName]
					[Entities[entID].animID][Entities[entID].FramePosX];

				spriteLocalPos.X = Entities[entID].localSpace.X;// -sprite.Width * scale / 2;
				spriteLocalPos.Y = Entities[entID].localSpace.Y;// -Entities[entID].zPos - sprite.Height * scale / 2;
				float depthValue = MathHelper.Clamp(1 - (float)Entities[entID].localSpace.Y / scrHeight, 0f, 1f);
				float finalDepth = depthValue + (float)(Entities[entID].zPos / scrHeight);
				int colorVal = (int)(finalDepth * 255);
				//Each channel can store seperate data.
				spriteColour = new Color(colorVal, colorVal, colorVal, colorVal);
				source.X = sprite.Width / 2;
				source.Y = sprite.Height / 2;

				spriteBatch.Draw(textureList["chara01_a"], spriteLocalPos, sprite, spriteColour, 0, source, scale, 
					SpriteEffects.None, depthValue);
			}

			drawProjectiles(spriteBatch);

            spriteBatch.End();
        }

		public void drawProjectiles(SpriteBatch sprite)
		{
			Vector2 local;
			Vector2 source;
			foreach (short projID in projectiles)
			{
				Projectile thisProj = (Projectile)(Entities[projID]);
				float depthValue = MathHelper.Clamp(1 - (float)thisProj.localSpace.Y / scrHeight, 0f, 1f);
				float finalDepth = depthValue + (float)(thisProj.zPos / scrHeight);

				//Draw shadow
				local.X = thisProj.localSpace.X - shadowHalfOffset[0].X;
				local.Y = thisProj.localSpace.Y - shadowHalfOffset[0].Y;
				sprite.Draw(shadowTex, local, getShadowSprite(0), Color.Black * 0.5f, 0f, Vector2.Zero, scale,
						SpriteEffects.None, depthValue);
				//Draw sprite
				if (thisProj.animID == -1) continue;
				else
				{
					//get depth
					Rectangle drawRec = projectileSprites[thisProj.animID];

					local.X = thisProj.localSpace.X;
					local.Y = thisProj.localSpace.Y;
					source.X = drawRec.Width / 2;
					source.Y = drawRec.Height / 2;

					sprite.Draw(textureList["projectiles"], local, drawRec, Color.White, -thisProj.angle, 
						source, scale, SpriteEffects.None, depthValue);
				}
			}
		}

        public void drawParticles (SpriteBatch sprite )
        {
			sprite.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
			foreach (short key in particles.Keys)
			{
				if (Functions.getLengthSqrd(hero.Hero.circleOrigin, particles[key].relPosition) < particleUpdateRangeSqrd)
					particles[key].draw(sprite, 2, particles[key].relPosition);
			}

			sprite.End();
        }

		public void drawShadows(SpriteBatch sprite)
		{
			sprite.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp,
				DepthStencilState.None, RasterizerState.CullNone);
			
			//Currently, only particles have shadows drawn
			foreach (short particleID in particles.Keys)
			{
				Entity_Components.Dynamic.ParticleEmitter temp = particles[particleID];
				Vector2 drawPos = temp.relPosition - shadowHalfOffset[2];
				sprite.Draw(shadowTex, drawPos, getShadowSprite(2), Color.Black * 0.5f, 0f, Vector2.Zero, scale,
						SpriteEffects.None, 0f);
			}

			sprite.End();
		}

		public Vector2 getShadowPosition(Collidable_Sprite spr)
		{
			Vector2 drawPos = Vector2.Zero;
			drawPos.X = spr.localSpace.X + getSpriteKey(spr).Width / 2;//- shadowHalfOffset[spr.shadowID].X;
			drawPos.Y = spr.localSpace.Y + getSpriteKey(spr).Height * scale - shadowHalfOffset[spr.shadowID].Y;
			return drawPos;
		}

		public Rectangle getShadowSprite(int ShadowID)
		{
			return shadowSprites[ShadowID];
		}

		public void drawCollisionBoxes(SpriteBatch spritebatch, bool characters, bool doodads, bool projectiles )
		{
			Vector2 origin = Vector2.Zero;
			Vector2 vecScale = Vector2.Zero;
			float fScale = 1.0f;
			//If square
			//Draw scaled square
			//else draw scaled circle
			for (short itr = 0; itr < drawingEntity.Count; itr++)
			{
				short entID = drawingEntity[itr];

				if (Entities[entID].useCollisionBox)
				{
					//Draw rectangle
					origin.X = Entities[entID].collisionBox.Left;
					origin.Y = Entities[entID].collisionBox.Right;
					vecScale.X = Entities[entID].collisionBox.Width / Declaration.drawRecSize;
					vecScale.Y = Entities[entID].collisionBox.Height / Declaration.drawRecSize;

					spritebatch.Draw(textureList[Declaration.DEBUG_TEX], origin, debugRect, Color.Green, 0f, 
						Vector2.Zero, vecScale, SpriteEffects.None, 0f);

				}
				else
				{
					//Draw circle
					origin.X = Entities[entID].circleOrigin.X - Entities[entID].collisionCircleRadius;
					origin.Y = Entities[entID].circleOrigin.Y - Entities[entID].collisionCircleRadius;
					fScale = (2 * Entities[entID].collisionCircleRadius) / Declaration.drawCircleDiameter;
					
					spritebatch.Draw(textureList[Declaration.DEBUG_TEX], origin, debugCircle, Color.Green, 0f, 
						Vector2.Zero, fScale, SpriteEffects.None, 0f);
				}
			}
		}

		public void drawLights(SpriteBatch spriteBatch, Effect lightmapEffect, EffectTechnique point)
        {
			//Draw point lights
			lightmapEffect.CurrentTechnique = point;
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp,
				DepthStencilState.Default, RasterizerState.CullNone, lightmapEffect);
			lightManager.drawPointLights(spriteBatch);
			spriteBatch.End();
        }

		public Rectangle getSpriteKey(Sprite temp)
		{
			return SpriteList.list[temp.animName]
					[temp.animID][temp.FramePosX];
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

			navmesh.onResize(widthChange, heightChange);
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
            L.FramePosXMax = (byte)(SpriteList.list[L.animName][L.animID].Length - 1);

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

        public short addParticleEffect(short abilityID, short particleID)
        {
            //Obtain the effect form the list of all effects
            short currentParticleID = generateID();

            if (currentParticleID == -1)
            {
                UIMessages.Add("Could not generate ID for particle effect " + particleID + 
                    " for ability " + abilities[abilityID].ID + " (" + abilities[abilityID].name + ").");
                return -1;
            }

            addParticleIDs(particleID, currentParticleID);
            abilities[abilityID].castParticleID.Add(currentParticleID);
            particles[currentParticleID].parentEntityID = abilityID;
            //Add id's to the ability and particle

			return currentParticleID;
        }

        public short addParticleEffect(short particleID, Point tilePos, Vector2 offset, float zPos )
        {
            short currentParticleID = generateID();

            if (currentParticleID == -1)
            {
                UIMessages.Add("Could not generate particle effect - too many objects in existence!");
            }

            Entity_Components.Dynamic.ParticleEmitter temp = new Entity_Components.Dynamic.ParticleEmitter(particleLibrary[particleID]);
            temp.relPosition = gameCamera.toLocalSpace(tilePos, offset);
			temp.zPos = zPos;
            UIMessages.Add("Created new particle effect at X: " + temp.relPosition.X + ", Y:" + temp.relPosition.Y);
            particles.Add(currentParticleID, temp);
            particles[currentParticleID].ID = currentParticleID;
            particles[currentParticleID].parent = masterSystem;
            particles[currentParticleID].random = new Random((int)temp.relPosition.X);
			particleEntities.Add(currentParticleID);
			return currentParticleID;
        }

		public short addParticleEffect(short particleID, Point tilePos, Vector2 offset, float zPos, float duration)
		{
			short parID = addParticleEffect(particleID, tilePos, offset, zPos);
			if (parID == -1) return -1;
			else
			{
				Entity_Components.Dynamic.ParticleEmitter particle = particles[parID];
				particle.timed = true;
				particle.duration = duration;
				return parID;
			}
		}

        private void addParticleIDs(short particleLibID, short currentParticleID)
		{
            particles.Add(currentParticleID, new Entity_Components.Dynamic.ParticleEmitter(particleLibrary[particleLibID]));
			particles[currentParticleID].maxParticles = (int)(Settings.funcs.getParticleCountModifier(particleCount) * particles[currentParticleID].maxParticles); 
            particles[currentParticleID].ID = currentParticleID;
            particles[currentParticleID].parent = masterSystem;
			particleEntities.Add(currentParticleID);
        }

		public short addProjectile(short projLibID, Character_Components.Character owner, Character_Components.Character target,
			float startDirection)
		{
			short ID = generateID();
			if (ID == -1) 
			{
                UIMessages.Add("Could not generate projectile - too many objects in existence!");
				return -1;
            }
			else
			{
				//Create particle effects
				List<short> newParticles = new List<short>();
				foreach (short partID in projectileLibrary[projLibID].particles)
				{
					//short thisID = generateID();
					//addParticleIDs(partID, thisID);
					newParticles.Add(addParticleEffect(partID, owner.tilePos, 
						owner.Offset, projectileLibrary[projLibID].zPos));
				}

				Entity_Components.Projectile newProj = new Entity_Components.Projectile(
					 projectileLibrary[projLibID], ID, owner, target, startDirection, newParticles);
				projectiles.Add(ID);
				Entities.Add(ID, newProj);
				if (newProj.lightID != -1)
				{
					newProj.lightID = attachLight(ID, newProj.lightID);
				}
				else newProj.lightID = -1;
				newProj.onCreateEntity(ID);
				assignGroups(ID);
				return projLibID;
			}
		}

		public void onProjectileCollide(Projectile proj, short collideID)
		{
		    //Deal with effects, create all particles needed
		    //Stop attached particles
			Collidable_Sprite collider = Entities[collideID];
		    List<short> newParticles = new List<short>();
			foreach (short idToAdd in proj.onCollideParticleEffects)
			{
				//short newID = generateID();
				//addParticleIDs(idToAdd, newID);
				addParticleEffect(idToAdd, proj.tilePos, proj.Offset, proj.zPos, 0);
			}

			//Check that the collision is against a character that can be harmed
			//Apply effects
			if (typeof(Character_Components.Character) == Entities[collideID].GetType())
			{
				Character_Components.Character thisChar = (Character_Components.Character)Entities[collideID];
				foreach (short effectID in proj.onCollideParticleEffects)
				{
					short result = 
						addParticleEffect(effectID, Point.Zero, new Vector2(proj.localSpace.X, proj.circleOrigin.Y), proj.zPos);
					particles[result].duration = 0.0f;
				}

				foreach (CharacterEffect effect in proj.effects)
				{
					thisChar.stats.applyEffect(effect);
				}
			}

			//TODO :: Call script if necessary
			string scriptFunction = Declaration.onCollide + Declaration.projectilePrefix + proj.name;
			if (scriptEngine.functions.ContainsKey(scriptFunction))
			{
				//Build parameters - projectile, target, objectmanager
				object[] paramaters = new object[] { proj, collider, this };
				//Calls script
				scriptEngine.functions[scriptFunction].Invoke(null, paramaters);
			}

			removeProjectile(proj.ID);
		}

		public void removeProjectile(short projID)
		{
			Entity_Components.Projectile projectile = (Entity_Components.Projectile)(Entities[projID]);
			foreach (short particleID in projectile.particles)
			{
				stopParticleEffect(particleID);
			}

			if (projectile.lightID != -1)
			{
				lightManager.deleteLight(projectile.lightID);
			}

			removeID(projID);
			projectiles.Remove(projID);
			if (localEntity.Contains(projID)) localEntity.Remove(projID);
			if (drawingEntity.Contains(projID)) drawingEntity.Remove(projID);
			Entities.Remove(projID);
		}

		public short attachParticleEffect(short entityID, short particleLibID)
		{
			//Create new particle effect, attach to entity
			short ID;
			ID = addParticleEffect(particleLibID, Point.Zero, Vector2.Zero, 0f);

			if (ID != -1)
			{
				Entities[entityID].particles.Add(ID);
				particles[ID].attachedEntity = entityID;
				particles[ID].attachedEntityObj = Entities[entityID];
				particles[ID].attachTo(Entities[entityID]);
			}

			return ID;
		}

		public short attachLight(short entityID, short particleLightID)
		{
			//Create new light, attach to entity
			short ID;
			ID = lightManager.addPointLight(lightLibrary[particleLightID],
				Point.Zero, Entities[entityID].tilePos, Entities[entityID].Offset, Entities[entityID].zPos);

			if (ID != -1)
			{
				Entities[entityID].attachedLights.Add(ID);
				lightManager.lights[ID].attachedEntity = Entities[entityID];
			}

			return ID;
		}

		public void removeAttachedParticleEffects(short entityID)
		{
			//Delete lights, then remove them from list
			Collidable_Sprite ent = Entities[entityID];
			while (ent.particles.Count > 0)
			{
				stopParticleEffect(ent.particles[0]);
				ent.particles.RemoveAt(0);
			}
		}

		public void removeAttachedLights(short entityID)
		{
			//Delete lights, then remove them from list.
			Collidable_Sprite ent = Entities[entityID];
			while (ent.attachedLights.Count > 0)
			{
				lightManager.deleteLight(entityID);
				ent.particles.RemoveAt(0);
			}
		}

        public void removeAbility(short abilityID)
        {
			//This should be changed to remove a projectile!
            abilities.Remove(abilityID);

			////Update this

			//for (short i = 0; i < abilities[abilityID].activeParticleIDs.Count; i++)
			//{
			//    particles.Remove(abilities[abilityID].activeParticleIDs[i]);
			//    UIMessages.Add("Removed Particle effect: " + abilities[abilityID].activeParticleIDs[i]
			//        + ", due to destruction of ability " + abilityID);
			//}

			////Add the new destruction particle effects
			//for (short i = 0; i < abilities[abilityID]..Count; i++)
			//{
			//    particles.Remove(abilities[abilityID].particleIDs[i]);
			//}

            removeID(abilityID);
        }

		//Instantly removes a particle effect
        public void removeParticleEffect(short particleID)
        {
            masterSystem.EmitterList.Remove(particles[particleID]);
            particles.Remove(particleID);
			particleEntities.Remove(particleID);
            removeID(particleID);
        }

		/// <summary>
		/// Stops the particle effect from spawning new particles, then removes the emitter once the particle count reaches 0.
		/// </summary>
		public void stopParticleEffect(short particleID)
		{
			//Particle will be removed when particle count reaches 0
			particles[particleID].timed = true;
			particles[particleID].duration = 0;
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

        public void advanceFrames()
        {
            foreach (short localEntity_ID in localEntity)
            {
				Sprite entity = Entities[localEntity_ID];
                entity.frameAdvance();
				entity.localSpace.Width = getSpriteKey(entity).Width;
				entity.localSpace.Height = getSpriteKey(entity).Height;
            }
        }

		/// <summary>
		/// Calculates the local space of each entity.
		/// </summary>
		/// <param name="cameraOriginVec">The vector offset of the camera origin from a tile.</param>
		/// <param name="cameraOrigin">The X and Y tile of the camera origin (top left of the screen)</param>
		/// <param name="translationThisUpdate">The camera vector difference between this frame and the last frame.</param>
		public void onCameraTranslation(Vector2 cameraOriginVec, Point cameraOrigin, Vector2 translationThisUpdate, short cameraFocusEntityID)
		{
			//Could add a variable to hold (float)translation - (int)translation, to store remainder for next update
			//Measure displacement from oldPosition to newPosition
			//Move each sprite by that amount
			foreach (short entityID in Entities.Keys)
			{
				//If main entity not updated - position chanages on camera resize
				//Don't update the entity which has focus on camera.
				//if (cameraFocusEntityID != entityID)
				//{
					Vector2 local = Functions.getLocalSpace(cameraOrigin, cameraOriginVec,
						Entities[entityID].tilePos, Entities[entityID].Offset);
					Entities[entityID].localSpace.X = (int)local.X;
					Entities[entityID].localSpace.Y = (int)local.Y;
				//}
			}

			foreach (short entityID in particles.Keys)
			{
				particles[entityID].relPosition -= translationThisUpdate;
			}

			foreach (short charID in Characters.Keys)
			{
				Characters[charID].brain.onTranslatation(translationThisUpdate);
			}
		}
		//Checking for lcoal entities could be done here
		public void onCameraTranslation(Vector2 cameraOriginVec, Point cameraOrigin, Vector2 translationThisUpdate)
		{
			//Could add a variable to hold (float)translation - (int)translation, to store remainder for next update
			//Measure displacement from oldPosition to newPosition
			//Move each sprite by that amount
			foreach (short entityID in Entities.Keys)
			{
				Vector2 local = Functions.getLocalSpace(cameraOrigin, cameraOriginVec,
					Entities[entityID].tilePos, Entities[entityID].Offset);
				Entities[entityID].localSpace.X = (int)local.X;
				Entities[entityID].localSpace.Y = (int)local.Y;
			}

			foreach (short charID in Characters.Keys)
			{
				Characters[charID].brain.onTranslatation(translationThisUpdate);
			}

			foreach (short entityID in particles.Keys)
			{
				particles[entityID].relPosition -= translationThisUpdate;
			}
			
		}

        public void updateAll( float msPassed, GraphicsDevice gfx, Point direction)
        {

            updateAbilities(msPassed);
            updateParticles(msPassed/1000f, direction);
			lightManager.Update((short)msPassed, gfx);
            //Frame based update to all entities
            foreach (short characterID in Characters.Keys)
            {
				//Updates stats
                Characters[characterID].stats.updateValues((short)msPassed);
            }
            
            //Update positions of sprite
            foreach (short localEntity_ID in localEntity )
            {
				Entities[localEntity_ID].setLastCircle();
                Entities[localEntity_ID].UpdatePosition();
				quadtree.updateItem(Entities[localEntity_ID]);
            }

			//Check collisions and resolve
            foreach (short localEntity_ID in localEntity)
            {
                quadtree.testCollisions(Entities[localEntity_ID]);
            }

			updateProjectiles(msPassed);
        }

		private void updateProjectiles(float msPassed)
		{
			//Loop through all the shit
			short result;
			List<short> toRemove = new List<short>();
			List<Tuple<short, short>> toSolve = new List<Tuple<short, short>>();
			foreach (short projID in projectiles)
			{
				Projectile proj = (Entity_Components.Projectile)(Entities[projID]);
				result = proj.Update(msPassed, this);

				foreach (short particleID in proj.particles)
				{
					particles[particleID].relPosition = proj.circleOrigin;
					//TODO :: Change direction
					//particles[particleID].setDirection(proj.vel);
				}

				if (result != -2)
				{
					if (result == -1)
					{
						toRemove.Add(projID);
						//removeProjectile(projID);
					}
					else
					{
						toSolve.Add(new Tuple<short, short>(projID, result));
						//onProjectileCollide((Projectile)Entities[projID], result);
					}
				}
				else continue;
			}

			while (toSolve.Count > 0)
			{
				Tuple<short, short> pair = toSolve[0];
				onProjectileCollide((Projectile)Entities[pair.Item1], pair.Item2);
				toSolve.RemoveAt(0);
			}

			while (toRemove.Count > 0)
			{
				removeProjectile(toRemove[0]);
				toRemove.RemoveAt(0);
			}
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
        private void updateParticles(float msPassed, Point direction)
        {
            short key = 0;
			for (short i = (short)(particles.Count - 1); i >= 0; i--)
			{
				key = particles.ElementAt(i).Key;

				//Vector2 facing = new Vector2(direction.X - particles[key].relPosition.X,
				//    direction.Y - particles[key].relPosition.Y);
				//particles[key].setDirection(facing);

				if (Functions.getLengthSqrd(hero.Hero.circleOrigin, particles[key].relPosition) < particleUpdateRangeSqrd)
					particles[key].update(msPassed);

				if (particles[key].readyToDestroy)
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

        public void unloadMapData(Point tileSector)
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
					worldData[tileSector].Particles[i].offset, worldData[tileSector].Particles[i].zPos);
			}
        }
        public void getMapData(Dictionary<Point, DataLoader.MapData> temp)
        {
			worldData = temp;
        }

		//public Vector2 toLocalSpace( Point tilePos, Vector2 offset )
		//{
		//    Vector2 temp;
		//    temp.X = (tilePos.X - tileManager.tileOriginX) * Declaration.tileGameSize + offset.X + tileManager.Offset.X;
		//    temp.Y = (tileManager.tileOriginY - tilePos.Y) * Declaration.tileGameSize + offset.Y + tileManager.Offset.Y;
		//    return temp;
		//}

    }

   
}
