using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using DataLoader;

namespace WindowsGame1
{
	/// <summary>
	/// This partial class contains functions and data to control the debugging mode used for the game.
	/// Debugging can be used to add content to the game, and save these changes.
	/// </summary>
	public partial class Game1 : Microsoft.Xna.Framework.Game
	{
		//Debug

		//Global tools:
		//Listbox control to select an item from a list of items, (i.e. spawn points, doors, triggers)
		//Textbox to input new data

		//DebugMode can be used to view / edit properties of entities
		//Intended use is for creating the game world whilst playing!
		//Debug mode changed by shift - function keys
		public enum debugMode
		{
			normal, //No debug mode enbaled, play the game normally
			tile,	//Can view tile data, edit tile id, rotation, for background and foreground tiles
			character,	//Can view stats, inventory, ai(i.e. goals and paths) for all characters, can edit position
			obj,	//Can view state, sprite, edit position
			item,	//Can view / change item properties
		}
		debugMode currentDebugMode = debugMode.normal;

		//Debug Graphics
		SpriteListSimple debugSprites;
		Texture2D debugTexture;
		Rectangle debugPanel;
		Color panelColour = new Color(128, 128, 128, 128);
		Texture2D rectTex;
		Vector2 prevButton;
		Vector2 nextButton;
		int nextPrevButtonLength = 24;
		public bool awaitingMouseClick = false;

		//Tile Debug
		Point highlightedTileCell;
		Point highlightedTile;
		bool writeToFirstLayer = true;
		int currentTilePage = 1;
		int selectedRow;
		int selectedColumn;
		int selectedIcon;
		int selectedIconID;
		int iconsThisPage;

		//TileDebug Graphics
		Vector2 bgSwitchButton;
		Vector2 tileIconBegin;	//Indicates where the tile previews are drawn
		Vector2 textLayerWriteLoc;
		int tileOffset = 8;
		int tileIconsPerRow;
		int tileIconsPerColumn;
		int tileIconsPerPage;

		//Object Debug
		int selectedEntity;
		//Object Debug Graphics

		//Character Debug

		//Character Debug Graphics
		public Dictionary<int, VertexPositionColor[]> navmeshVertices = new Dictionary<int,VertexPositionColor[]>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mousePosition"></param>
		/// <returns>Item1 = Cell, Item2 = tile</returns>
		public Tuple<Point, Point> getTileAtMousePos(Point mousePosition)
		{
			mousePosition.X -= (int)(camera.originVec.X);//- tileManager.originOffset.X);
			mousePosition.Y -= (int)(camera.originVec.Y - tileManager.originOffset.Y);
			Point cellPos = Point.Zero;
			Point tilePos = Point.Zero;

			//Do stuff
			tilePos.X = (int)(camera.origin.X + mousePosition.X / Declaration.tileGameSize);
			tilePos.Y = (int)(camera.origin.Y + mousePosition.Y / Declaration.tileGameSize);
			//tilePos.X += (int)Math.Floor((double)(mousePosition.X % Declaration.tileGameSize - camera.focusVec.X));
			//tilePos.Y += (int)Math.Floor((double)(mousePosition.Y % Declaration.tileGameSize - camera.focusVec.Y));
			cellPos.X = tilePos.X / Functions.tilesPerSector;
			cellPos.Y = tilePos.Y / Functions.tilesPerSector;
			tilePos.X = tilePos.X % Functions.tilesPerSector;
			tilePos.Y = tilePos.Y % Functions.tilesPerSector;

			Tuple<Point, Point> position = new Tuple<Point, Point>(cellPos, tilePos);
			return position;
		}

		public Tuple<Point, Point, Point> getSelectedPosition(Point mousePosition)
		{
			mousePosition.X += (int)(camera.originVec.X);// + tileManager.originOffset.X);
			mousePosition.Y += (int)(camera.originVec.Y);// + tileManager.originOffset.Y);
			Point cellPos = Point.Zero;
			Point tilePos = Point.Zero;
			Point Offset = Point.Zero;

			//Do stuff
			tilePos.X = (int)(camera.origin.X + mousePosition.X / Declaration.tileGameSize);
			tilePos.Y = (int)(camera.origin.Y + mousePosition.Y / Declaration.tileGameSize);
			cellPos.X = tilePos.X / Functions.tilesPerSector;
			cellPos.Y = tilePos.Y / Functions.tilesPerSector;
			tilePos.X = tilePos.X % Functions.tilesPerSector;
			tilePos.Y = tilePos.Y % Functions.tilesPerSector;
			Offset.X = mousePosition.X % Declaration.tileGameSize;
			Offset.Y = mousePosition.Y % Declaration.tileGameSize;

			Tuple<Point, Point, Point> position = new Tuple<Point, Point, Point>(cellPos, tilePos, Offset);
			return position;
		}

		private Vector2 getPanAmount()
		{
			Vector2 final = new Vector2(mousePos.X - screen.Width / 2, mousePos.Y - screen.Height / 2);
			if (final.X < 100 && final.X > -100)
				final.X = 0;
			if (final.Y < 100 && final.Y > -100)
				final.Y = 0;
			if (final.X < -100) final.X += 100;
			if (final.X > 100) final.X -= 100;
			if (final.Y < -100) final.X += 100;
			if (final.Y > 100) final.Y -= 100;
			final.X /= 20;
			final.Y /= 20;
			final.X += camera.focusVec.X + camera.focus.X* Declaration.tileGameSize;
			final.Y += camera.focusVec.Y + camera.focus.Y* Declaration.tileGameSize;
			return final;
		}

		public void onMouseDebugClick()
		{
			//Get clicked mouse location
		}

		//Input Handling
		private void handleInputDebug()
		{
			if (keyboardState.IsKeyDown(Keys.LeftShift))
			{
				if (keyboardState.IsKeyDown(Keys.D1))
				{
					currentDebugMode = debugMode.normal;
				}
				if (keyboardState.IsKeyDown(Keys.D2))
				{
					currentDebugMode = debugMode.tile;
					onTileDebugActivate();
				}
				if (keyboardState.IsKeyDown(Keys.D3))
				{
					currentDebugMode = debugMode.obj;
					onObjDebugActivate();
				}
				if (keyboardState.IsKeyDown(Keys.D4))
				{
					currentDebugMode = debugMode.character;
					onCharDebugActivate();
				}
				if (keyboardState.IsKeyDown(Keys.D5))
				{
					currentDebugMode = debugMode.item;
					onItemDebugActivate();
				}
			}

			switch (currentDebugMode)
			{
				case debugMode.normal:
					break;
				case debugMode.obj:
					handleObjectDebugInput();
					break;
				case debugMode.tile:
					handleTileDebugInput();
					break;
				case debugMode.character:
					handleTileDebugInput();
					break;
				case debugMode.item:
					handleItemDebugInput();
					break;
				default:
					break;
			}

			
		}

		private void handleTileDebugInput()
		{
			if (mousePos.X < debugPanel.X)
			{
				Tuple<Point, Point> highlighted = getTileAtMousePos(mousePos);
				highlightedTile = highlighted.Item2;
				highlightedTileCell = highlighted.Item1;
			}

			if ((mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed))
			{
				//Mouse has been clicked down
				if (prevMouseState.LeftButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Released)
				{	//Mouse has been clicked and released.
					if (mouseState.X > debugPanel.X)
					{
						//Debug panel has been selected
						if (mouseState.Y > tileIconBegin.Y && mouseState.Y < screen.Height - 100)
						{
							//A tile Icon has been selected
							selectedColumn = (int)((mouseState.X - tileIconBegin.X ) / (Declaration.tileLength + tileOffset));
							selectedRow = (int)((mouseState.Y - tileIconBegin.Y) / (Declaration.tileLength + tileOffset));
							selectedIcon = selectedRow * tileIconsPerRow + selectedColumn;
							selectedIconID = selectedIcon + (currentTilePage) * tileIconsPerPage;
							if (!tileManager.sprites.list.ContainsKey(selectedIconID))
								selectedIconID = -1;
						}

						//Check for next / prev button click
						if (mouseState.Y < prevButton.Y && mouseState.Y < (prevButton.Y + 8))
						{
							//Icon along the row of next / prev has been clicked
							if (mouseState.X > nextButton.X && mouseState.X < (nextButton.X + nextPrevButtonLength))
							{
								if (((currentTilePage + 1) * tileIconsPerPage) < tileManager.sprites.list.Count)
								{
									//New icons can be displayed
									currentTilePage++;
									iconsThisPage = (tileManager.sprites.list.Count - (currentTilePage * tileIconsPerPage)) % 100;
								}
							}
							else if (mouseState.X > prevButton.X && mouseState.X < (prevButton.X + nextPrevButtonLength))
							{
								if (currentTilePage > 1)
								{
									//New icons can be displayed
									currentTilePage--;
									iconsThisPage = (tileManager.sprites.list.Count - (currentTilePage * tileIconsPerPage)) % 100;
								}
							}
						}

						if (false)
						{
							//Check for bg / fg button press
						}
					
					}
				}
				else
				{	
					//Mouse is being held, only edit the cell if an icon is selected
					if (mousePos.X < debugPanel.X && selectedIconID != -1)
					{
						//Edit the selected tile
						for (int i = 0; i < 9; i++)
						{
							if (tileManager.cells[i].CellCoordinates == highlightedTileCell)
							{
								tileManager.cells[i].contentsChanged = true; //Mark this file to be saved!
								Point editedCell = tileManager.cells[i].CellCoordinates;
								if (mouseState.LeftButton == ButtonState.Pressed)
								{	//Primary key has been pressed
									if (writeToFirstLayer)
									{
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].fgID = selectedIconID;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].fgFramePosX = 0;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].fgFramePosMax =
											tileManager.sprites.list[selectedIconID].Length - 1;
									}
									else
									{
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].bgID = selectedIconID;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].bgFramePosX = 0;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].bgFramePosMax =
											tileManager.sprites.list[selectedIconID].Length - 1;
									}
								}
								else
								{	//Secondary key has been pressd
									if (writeToFirstLayer)
									{
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].fgID = -1;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].fgFramePosX = 0;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].fgFramePosMax = 0;
									}
									else
									{
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].bgID = -1;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].bgFramePosX = 0;
										tileManager.cellData[editedCell][highlightedTile.X, highlightedTile.Y].bgFramePosMax = 0;
									}
								}

							}
						}
					}
				}
			}

			if (keyboardState.IsKeyDown(Keys.LeftShift) && prevKeyState.IsKeyUp(Keys.LeftShift))
			{
				//Shift was pressed, alternate between writign to first and second layer
				writeToFirstLayer = !writeToFirstLayer;
			}
		}

		private void handleCharacterDebugInput()
		{

		}

		private void handleItemDebugInput()
		{

		}

		private void handleObjectDebugInput()
		{

		}


		private void drawDebug()
		{
			Vector2 temp = Vector2.Zero;
			temp.Y += 40;

			drawCharDebugFeatures();

			if (drawNavmeshDebug)
				drawNavmesh();
			if (drawDebugText)
			{
				spriteBatch.Begin();
				#region debugText
				//Debug stuff
				spriteBatch.DrawString(UI.font, "DEBUG MODE: " + currentDebugMode.ToString(), temp, Color.Red);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Light Count : " + ObjectManager.lightManager.lightAreas.Count, temp, Color.Red);
				temp.Y += 20;

				Tuple<Point, Point, Point> selected = getSelectedPosition(mousePos);
				spriteBatch.DrawString(UI.font, "Selected Tile - Cell: " + selected.Item1 +
					" point: " + selected.Item2 + " vec: " + selected.Item3, temp, Color.Red);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Selected Position: " + mousePos, temp, Color.Red);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Vertice Position: " + ObjectManager.navmesh.cells[998].TLVert, temp, Color.Red);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Camera Origin: " + camera.origin, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Camera Origin Vec: " + camera.originVec, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Camera Focus: " + camera.focus, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Camera Focus Vec: " + camera.focusVec, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Camera Translation this update: " + camera.translationThisUpdate, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Hero tile : " + player.Hero.tilePos, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Hero Offset: " + player.Hero.Offset, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Hero Vel: " + player.Hero.vel, temp, Color.White);
				temp.Y += 20;
				spriteBatch.DrawString(UI.font, "Hero Local: " + player.Hero.localSpace, temp, Color.White);
				temp.Y += 20;
				//Draw tile debug text
				//foreach (short entityID in ObjectManager.drawingEntity)
				//{
				//    spriteBatch.DrawString(UI.font, "Entity " + entityID + ": " + ObjectManager.Entities[entityID].localSpace,
				//        temp, Color.White);
				//    temp.Y += 20;
				//}
				#endregion
				spriteBatch.End();
			}

			switch (currentDebugMode)
			{
				case debugMode.normal:
					break;
				case debugMode.tile:
					drawTileDebugFeatures();
					break;
				case debugMode.obj:
					drawObjDebugFeatures();
					break;
				case debugMode.item:
					drawItemDebugFeatures();
					break;
				case debugMode.character:
					drawCharDebugFeatures();
					break;
			}
		}

		private void drawNavmesh()
		{
            if (navmeshVertices.Count == 0 || ObjectManager.navmesh.segments.Count == 0) return;

			navmeshEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
			navmeshEffect.Parameters["xViewProjection"].SetValue(transformed);
			navmeshEffect.Parameters["meshColour"].SetValue(Color.Blue.ToVector4());
			navmeshEffect.Parameters["lineColour"].SetValue(Color.White.ToVector4());

			foreach (EffectPass pass in navmeshEffect.CurrentTechnique.Passes)
			{
				if (pass.Name == "outlinePass") continue;
				pass.Apply();
				//Everything needs to be drawn here
				foreach(KeyValuePair<int, VertexPositionColor[]> tempPair in navmeshVertices)
				{
					VertexPositionColor[] temp = tempPair.Value;
					int polyID = tempPair.Key;
					//Get amount fo valid vertices
					int validVerts = ((Debug.navmeshForm)form.navmeshForm).data.cells[polyID].getValidVerts();

					if (validVerts == 4)
					{
						GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, temp,
							0, 2);
					}
					else
					{
						//Not all vertices are set - 
						//3 verts draw one triangle

						//2 verts draw a line
						if (validVerts == 2)
						{
							VertexPositionColor[] lineVerts = new VertexPositionColor[2];

							int vertCtr = 0;
							for (int i = 0; i < 4; i++)
							{
								if (((Debug.navmeshForm)form.navmeshForm).data.cells[polyID].getVert(i).valid)
								{
									lineVerts[i] = temp[i];
									vertCtr++;
								}
							}

							GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
								lineVerts, 0, 1);
						}
						else if (validVerts == 3)
						{
							VertexPositionColor[] triVerts = new VertexPositionColor[3];

							int vertCtr = 0;
							for (int i = 0; i < 4; i++)
							{
								if (((Debug.navmeshForm)form.navmeshForm).data.cells[polyID].getVert(i).valid)
								{
									triVerts[i] = temp[i];
									triVerts[i].Color = Color.Red;
									vertCtr++;
								}
							}

							GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList,
								triVerts, 0, 1);
						}

					}
				}
			}


			//Draw segments
			navmeshEffect.Parameters["lineColour"].SetValue(Color.Red.ToVector4());
			navmeshEffect.Techniques["Lines"].Passes["outlinePass"].Apply();
			int segCount = ObjectManager.navmesh.segments.Count;

			VertexPositionColor[] segVerts = new VertexPositionColor[segCount * 2];
			int segItr = 0;
			foreach (Utility.Segment seg in ObjectManager.navmesh.segments.Values)
			{
				segVerts[segItr] = new VertexPositionColor(Functions.toVector3(seg.p1Local), Color.Black);
				segVerts[segItr + 1] = new VertexPositionColor(Functions.toVector3(seg.p2Local), Color.Black);
				segItr += 2;
			}

			GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
				segVerts, 0, segCount);

			if (true)
			{
				navmeshEffect.Parameters["lineColour"].SetValue(Color.Red.ToVector4());
				navmeshEffect.Techniques["Lines"].Passes["outlinePass"].Apply();
				foreach (Character_Components.Character temp in ObjectManager.Characters.Values)
				{
					if (temp.brain.drawPath)
						temp.brain.Draw(GraphicsDevice, navmeshEffect);
				}
			}
		}

		//Draw Handling
		private void drawTileDebugFeatures()
		{
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
				RasterizerState.CullNone);
			//Draw Panel
			spriteBatch.Draw(rectTex, debugPanel, panelColour);
			Vector2 tileDrawLocation;

			//Draw next / prev buttons
			spriteBatch.Draw(UI.UItex, nextButton,
				UI.UIsprites.list[29][0], Color.White);
			spriteBatch.Draw(UI.UItex, prevButton,
				UI.UIsprites.list[29][0], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
			//Draw tileIcons
			int iconItr = 0;
			for (iconItr = 0; (iconItr < tileManager.sprites.list.Count) && iconItr < tileIconsPerPage; iconItr++)
			{
				int rowNo = iconItr / tileIconsPerRow;
				int columnNo = iconItr % tileIconsPerRow;
				tileDrawLocation = new Vector2(tileIconBegin.X + columnNo * (Declaration.tileLength + tileOffset),
					tileIconBegin.Y + rowNo * (Declaration.tileLength + tileOffset));
				spriteBatch.Draw(ObjectManager.textureList["tileset"], tileDrawLocation, 
					tileManager.sprites.list[iconItr][0], Color.White);
			}
			//Highlight the current icon with red.
			tileDrawLocation.X = tileIconBegin.X + selectedColumn * (tileOffset + Declaration.tileLength) - 2;
			tileDrawLocation.Y = tileIconBegin.Y + selectedRow * (tileOffset + Declaration.tileLength) - 2;
			spriteBatch.Draw(debugTexture, tileDrawLocation,
				debugSprites.list[3][0], Color.White);
			
			//Draw the box highlighting the selected in game tile.
			if (mousePos.X < debugPanel.X && selectedIconID != -1)
			{
				//tileDrawLocation = tileManager.toLocalSpace(highlightedTile, highlightedTileCell);
				//tileDrawLocation -= camera.focusVec;
				//tileDrawLocation.X -= tileManager.originOffset.X;
				//tileDrawLocation.Y -= tileManager.originOffset.Y;
				Point lol = new Point(highlightedTile.X + highlightedTileCell.X * Functions.tilesPerSector,
					highlightedTile.Y + highlightedTileCell.Y * Functions.tilesPerSector);
				tileDrawLocation = camera.toLocalSpace(lol, Vector2.Zero);
				spriteBatch.Draw(ObjectManager.textureList["tileset"], tileDrawLocation,
					tileManager.sprites.list[selectedIcon][0], Color.Red, 0f, Vector2.Zero, Declaration.Scale, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(font, (writeToFirstLayer ? "Foreground" : "Background"), textLayerWriteLoc, Color.Red);
			spriteBatch.End();
		}
		private void drawObjDebugFeatures()
		{
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
				RasterizerState.CullNone);

			//Draw collision shapes of all objects
			Rectangle collisionBoxPos;
			Vector2 entLocalSpace;
			foreach (short collidable_ID in ObjectManager.drawingEntity)
			{
				Collidable_Sprite entity = ObjectManager.Entities[collidable_ID];

				if (entity.useCollisionBox)
				{
					collisionBoxPos.X = entity.localSpace.Center.X - entity.collisionBox.Width / 2;
					collisionBoxPos.Y = entity.localSpace.Center.Y - entity.collisionBox.Height / 2;
					collisionBoxPos.Width = entity.collisionBox.Width;
					collisionBoxPos.Height = entity.collisionBox.Height;
					spriteBatch.Draw(debugTexture, collisionBoxPos, debugSprites.list[2][0], Color.Black);
				}
				else
				{
					collisionBoxPos.X = entity.localSpace.Center.X - (int)entity.collisionCircleRadius;
					collisionBoxPos.Y = entity.localSpace.Center.Y - (int)entity.collisionCircleRadius;
					collisionBoxPos.Width = (int)entity.collisionCircleRadius * 2;
					collisionBoxPos.Height = (int)entity.collisionCircleRadius * 2;
					spriteBatch.Draw(debugTexture, collisionBoxPos, debugSprites.list[1][0], Color.Black);
				}
			}

			spriteBatch.End();
		}
		private void drawNavMesh1()
		{
			//Draws polygons representing navmesh
			for (int i = 0; i < ObjectManager.navmesh.cells.Count; i++)
			{
				//3 or more verts = Draw a primitive
				//2 verts = draw a line
				Utility.NavMeshCell temp = ObjectManager.navmesh.cells[i];
				int vertCount = 0;
				if (temp.BRpos.valid) vertCount++;
				if (temp.BLpos.valid) vertCount++;
				if (temp.TRpos.valid) vertCount++;
				if (temp.TLpos.valid) vertCount++;
			}
		}
		private void drawItemDebugFeatures()
		{

		}
		private void drawCharDebugFeatures()
		{
			foreach(Character_Components.Character temp in ObjectManager.Characters.Values)
			{
				if (temp.brain.isWalking)
				{
				//	temp.brain.Draw(GraphicsDevice);
				}
			}
		}

		//New Debug Select
		private void onTileDebugActivate()
		{
			onScreenResizeTileDebug();
			tileIconsPerRow = (int)Math.Floor((screen.Width - tileIconBegin.X) / (tileOffset + Declaration.tileLength));
			tileIconsPerColumn = (int)Math.Floor((screen.Height - 100 - tileIconBegin.Y) / (tileOffset + Declaration.tileLength));
			tileIconsPerPage = tileIconsPerColumn * tileIconsPerRow;
			currentTilePage = 0;
			iconsThisPage = tileManager.sprites.list.Count % 100;
		}
	
		private void onObjDebugActivate()
		{

		}
		private void onItemDebugActivate()
		{

		}
		private void onCharDebugActivate()
		{

		}

		private void onScreenResizeDebug()
		{
			switch (currentDebugMode)
			{
				case debugMode.normal:
					break;
				case debugMode.obj:
					onScreenResizeTileDebug();
					break;
				case debugMode.tile:
					onScreenResizeTileDebug();
					break;
				case debugMode.character:
					onScreenResizeTileDebug();
					break;
				case debugMode.item:
					onScreenResizeTileDebug();
					break;
				default:
					break;
			}
		}

		private void onScreenResizeTileDebug()
		{
			debugPanel.X = screen.Width - 400;
			debugPanel.Y = 0;
			debugPanel.Width = 400;
			debugPanel.Height = screen.Height;
			tileIconBegin.X = debugPanel.X + tileOffset;
			tileIconBegin.Y = debugPanel.Y + 200;
			textLayerWriteLoc.X = debugPanel.X + 20;
			textLayerWriteLoc.Y = debugPanel.Y + 20;
			nextButton.Y = 16;
			prevButton.Y = 16;
			nextButton.X = screen.Width - 50;
			prevButton.X = nextButton.X + nextPrevButtonLength + 8;
		}
		//Commands to add:
		//Load cell
		//Save Cell
		//save char
		//Load char
		//Clear lights
		//Clear entities

		public void initialiseDebug()
		{
			//loads sprites etc
			debugTexture = Content.Load<Texture2D>("Sprite Content\\DebugTex");
			debugSprites = new SpriteListSimple();
			Dictionary<int, SimpleSpriteListLoader> temp = this.Content.Load<Dictionary<int, SimpleSpriteListLoader>>
				("Object Databases\\DebugSpriteParser");
			foreach(KeyValuePair<int, SimpleSpriteListLoader> val in temp)
			{
				debugSprites.setFrames(ref val.Value.spriteRec, val.Value.spriteRec.Width, val.Value.spriteRec.Height,
					1, val.Key);
			}
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
				UI.addMessage("6. 'list boolListDir directory' lists all files in a directory", Message.msgType.System);
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
                    int itemQuantity = Convert.ToInt16(commandSubstrings[3]);

					//inventoryManager.giveItem(characterID, itemID, itemQuantity);
					//player.giveItemToPlayer(inventoryManager.getItem(itemID));

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
			else if (commandSubstrings[0] == "list")
			{
				try
				{
					List<string> temp = new List<string>();
					if (commandSubstrings[1] == "1")
					{
						temp.AddRange(Directory.GetFiles(Content.RootDirectory + commandSubstrings[3]));
						temp.AddRange(Directory.GetDirectories(Content.RootDirectory + commandSubstrings[3]));
					}
					else
						temp.AddRange(Directory.GetFiles(Content.RootDirectory + commandSubstrings[3]));
					foreach (String str in temp)
					{
						UI.addMessage(str);
					}
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
        #endregion

		#region "NavMesh"
		public void createPolygonPrimitive(Utility.NavMeshCell poly)
		{
			if (navmeshVertices.ContainsKey(poly.polyID))
				return;
			else
			{
				VertexPositionColor[] tempVerts = new VertexPositionColor[4];
				if (poly.TLpos.valid) tempVerts[0] = new VertexPositionColor(
					Functions.toVector3(camera.toLocalSpace(poly.TLpos.cellPos,
					poly.TLpos.tilePos, poly.TLpos.Offset)), Color.Blue);
				if (poly.TRpos.valid) tempVerts[1] = new VertexPositionColor(
					Functions.toVector3(camera.toLocalSpace(poly.TRpos.cellPos,
					poly.TRpos.tilePos, poly.TRpos.Offset)), Color.Blue);
				if (poly.BLpos.valid) tempVerts[2] = new VertexPositionColor(
					Functions.toVector3(camera.toLocalSpace(poly.BLpos.cellPos,
					poly.BLpos.tilePos, poly.BLpos.Offset)), Color.Blue);
				if (poly.BRpos.valid) tempVerts[3] = new VertexPositionColor(
					Functions.toVector3(camera.toLocalSpace(poly.BRpos.cellPos,
					poly.BRpos.tilePos, poly.BRpos.Offset)), Color.Blue);
				navmeshVertices.Add(poly.polyID, tempVerts);
			}
		}

		public void translateVerts(Vector2 translation)
		{
			foreach (VertexPositionColor[] temp in navmeshVertices.Values)
			{
				for (int i = 0; i < 4; i++)
				{
					if (temp[i] != null) temp[i].Position += Functions.toVector3(translation);
				}
			}
		}

		#endregion
	}
}