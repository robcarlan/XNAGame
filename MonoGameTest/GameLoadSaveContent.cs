using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml.Serialization;
using WindowsGame1.Utility;
using DataLoader;

namespace WindowsGame1
{
	/// <summary>
	/// This partial class contains functions used in loading and saving data during gameplay.
	/// </summary>
	public partial class Game1 : Microsoft.Xna.Framework.Game
	{
		public static T LoadContent<T>(string filepath)
		{
			if (File.Exists(filepath))
			{
				try {
					XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));
					StreamReader fileReader = new StreamReader(filepath);
					return (T)(reader.Deserialize(fileReader));
				} catch (Exception ex) {
					//Problem loading uhoh
					return default(T);
				}
				//using (FileStream stream = new FileStream(filepath, FileMode.Open))
				//{
				//    using (XmlReader reader = XmlReader.Create(stream))
				//    {
				//        T data = IntermediateSerializer.Deserialize<T>(reader, null);
				//        return data;
				//    }
				//}
			}
			else
			{
				Functions.WriteDebugLine("Could not load data at " + filepath + ", file does not exist.");
				return default(T);
			}
		}

		//Loads the actual entities in this cell
		private void loadCellObjects(Point cell)
		{
			string fName = Declaration.contentDataFolder + cell.X + " " + cell.Y + ".xml";
			if (File.Exists(fName))
			{
				MapData temp = LoadContent<MapData>(fName);
				//Load into object manager
				foreach (DataLoader.EnitityObject character in temp.Characters)
				{
					//TODO: Object Manager function to add a character given position and ID
				}

				foreach (DataLoader.DoodadObject doodad in temp.Doodads)
				{
					ObjectManager.addDoodad(doodad.ID, Functions.getPointWSP(cell, doodad.relTilePos),
						doodad.offset);
					//Add zPos to function
				}

				//Add Lights
				ObjectManager.lightManager.cellData[cell] = new List<short>();
				//foreach (DataLoader.LightObject light in temp.BasicLights)
				//{
				//    //Uses base ID from template
				//    ObjectManager.lightManager.addBasicLight(light.ID, cell, light.relTilePos, light.offset, light.zPos);
				//}
				foreach (DataLoader.DirectionalLightLoader dLight in temp.directionalLights)
				{
					ObjectManager.lightManager.addDirectionalLight(
						dLight.lightColor, dLight.intensity, cell, dLight.tilePos, dLight.Offset, dLight.radius,
						dLight.zPos, dLight.direction);
				}
				foreach (DataLoader.PointLightLoader pLight in temp.pointLights)
				{
					ObjectManager.lightManager.addPointLight(
						pLight.lightColor, pLight.intensity, cell, pLight.tilePos, pLight.Offset, pLight.radius,
						pLight.zPos);
				}


				foreach (DataLoader.ParticleObject particle in temp.Particles)
				{
					ObjectManager.addParticleEffect(particle.ID,
						Functions.getPointWSP(cell, particle.relTilePos), particle.offset, particle.zPos);
				}

				//Load into Obj Manager cell data list
				ObjectManager.loadedCells.Add(cell);
				Functions.WriteDebugLine("Loaded cell data at " + cell.ToString() + ".");
				ObjectManager.cellsToLoad.Dequeue();
			}
			else
			{
				Functions.WriteDebugLine("Could not load Cell data at " + cell.ToString() + ", file not found.");
				ObjectManager.cellsToLoad.Dequeue();
			}
		}

		private void loadCellNavmesh(Point cell)
		{
			//Load data
			string loadPath = Declaration.navmeshDataFolder + cell.X + " " + cell.Y + ".xml";

			Utility.NavMeshCell[] temp = LoadContent<Utility.NavMeshCell[]>(loadPath);

			if (temp == null)
			{
				return;
			}

			List<Utility.NavMeshCell> data = temp.ToList<Utility.NavMeshCell>();
			if (drawNavmeshDebug)
				((Debug.navmeshForm)form.navmeshForm).data.addNavmeshCell(data, cell, camera);
			ObjectManager.navmesh.addNavmeshCell(data, cell, camera);
			UI.addMessage(Functions.WriteDebugLine("Loaded " + data.Count + " navmesh cells at cell: " + cell.X + " " + cell.Y), Message.msgType.System);
		}

		public void resumeContentStreaming(float msPassed)
		{
			//Load content based on how much time left until next render ( 1/60 - msPassed or 1/30 - msPassed)
			float msLeft = 1 / 60 - msPassed;
			while (msLeft > 0)
			{
				//Load based on previous iterator
				msLeft--;
			}
		}


		void Game1_Exiting(object sender, EventArgs e)
		{
			//Save all changes to cells!!!
			for (int i = 0; i < 9; i++)
			{
				if (tileManager.cells[i].contentsChanged)
					tileManager.saveTileData(tileManager.cells[i].CellCoordinates);
			}
		}

		//Loads content from flagged tile cells. Unloads unused data 
		//TODO: Stream data over time, rather than loading all at once. Use (timerperframe(maxFPS) - timeelapsed) 
		//to get allowed time for content loading.

		public void UpdateGameData()
		{
			if (tileManager.needsLoad)
			{	
				Point centralTile = tileManager.getFocus();	//As central tile data may not be valid
				centralTile.X /= Functions.tilesPerSector;
				centralTile.Y /= Functions.tilesPerSector;

				for (int i = 0; i < 9; i++)
				{
					//tileManager.cells[i].contentsValid = false;
					if (!tileManager.cells[i].contentsValid)
					{
						//Cell data invalid, so unload!
						cellsToUnload.Add(tileManager.cells[i].CellCoordinates);

						//Object manager must load this content.
						ObjectManager.cellsToLoad.Enqueue(new Point(tileManager.cells[i].CellCoordinates.X,
																	tileManager.cells[i].CellCoordinates.Y));
						//Load from xml file using tile position
						Point thisTile = new Point(tileManager.cells[i].CellCoordinates.X, 
							tileManager.cells[i].CellCoordinates.Y);

						if (tileManager.cellData.ContainsKey(thisTile))
						{
							//No need to load from disk
							Functions.WriteDebugLine("I have already loaded " + thisTile);
						}
						else
						{
							//Load file from disk
							string fName = Declaration.tileDataFolder + thisTile.X + " " + thisTile.Y;
							if (File.Exists(fName + ".map"))
							{
								try
								{
									string[] mapData = System.IO.File.ReadAllText(fName + ".map").Split(';');
									int cellIndex = thisTile.X - tileManager.cells[0].CellCoordinates.X +
										3 * (thisTile.Y - tileManager.cells[0].CellCoordinates.Y);
									tileManager.setContents(tileManager.cells[i].CellCoordinates, mapData);
									//UI.addMessage(Functions.WriteDebugLine("Loaded tile data at " + thisTile.X + " " + thisTile.Y));
								}
								catch (Exception ex)
								{
									//Error loading file, should not happen
									UI.addMessage(Functions.WriteDebugLine("Error loading tiles at " + thisTile + ":" + ex.Message), Message.msgType.System);
								}
							}
							else
							{
								//Tile data does not exist, so fill with a checkerboard pattern. Occurs on unaltered tiles
								Functions.WriteDebugLine("TIle data does not exist at " + thisTile.ToString() + "; creating data.");
								tileManager.setContents(tileManager.cells[i].CellCoordinates, (Math.Abs(thisTile.X) % 2 + 1));
							}
						}

						tileManager.cells[i].contentsValid = true;
						tileManager.cells[i].contentsChanged = false;
					}

				}

				tileManager.needsLoad = false;
				//After this function ends, cell [3 and 4] changes ID from 1 to 2

				//Load objectManager data
				for (int i = 0; i < ObjectManager.cellsToLoad.Count; i++)
				{
					Point thisCell = ObjectManager.cellsToLoad.Peek();
					loadCellObjects(thisCell);

					//Load navmesh data if not already active
					if (!ObjectManager.navmesh.cellNavmesh.ContainsKey(thisCell))
					{
						loadCellNavmesh(thisCell);
					}
				}

				//Unload relevant data
				while (cellsToUnload.Count > 0)
				{
					//Remove unnecessary cells
					cellsToUnload.RemoveAt(0);
				}
			}
		}

		public void buildGameScripts(ScriptEngine script)
		{
			//Compile cs files from script folder. Add methods to dictionary.
			//Builds game scripts from the desired folder
			//"Content\\Scripts\\Test.cs"
			//string[] scripts = Directory.GetFiles(
			//    System.IO.Path.GetDirectoryName(
			//    System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Content\\Scripts");
			string[] scripts = Directory.GetFiles(
				System.IO.Path.GetDirectoryName(
				System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Scripts");
			foreach (string file in scripts)
			{
				Assembly temp = Compile(file);
				Type scriptClass = temp.GetType("Scripts.Script");
				foreach (MethodInfo method in scriptClass.GetMethods())
				{
					script.functions.Add(method.Name, method);
				}
			}
		}

		private Assembly Compile(string fileName)
		{
			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

			CompilerParameters cp = new CompilerParameters();
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = true;
			cp.TreatWarningsAsErrors = false;
			cp.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe");
			cp.ReferencedAssemblies.Add(Environment.GetEnvironmentVariable("XNAGSv4") + @"\References\Windows\x86\Microsoft.Xna.Framework.dll");
			cp.ReferencedAssemblies.Add(Environment.GetEnvironmentVariable("XNAGSv4") + @"\References\Windows\x86\Microsoft.Xna.Framework.Game.dll");
			cp.ReferencedAssemblies.Add(Environment.GetEnvironmentVariable("XNAGSv4") + @"\References\Windows\x86\Microsoft.Xna.Framework.Graphics.dll");
			foreach (AssemblyName assembly in System.Reflection.Assembly.GetCallingAssembly().GetReferencedAssemblies())
			{
				if (assembly.CodeBase == null) continue;
				cp.ReferencedAssemblies.Add(new System.Uri(assembly.CodeBase).AbsolutePath);
			}
			CompilerResults cr = provider.CompileAssemblyFromFile(cp, fileName);
			if (cr.Errors.Count > 0)
				foreach (CompilerError error in cr.Errors)
				{
					Functions.WriteDebugLine("Error compiling script " + fileName + ": " + error.ErrorText);
				}
			return cr.CompiledAssembly;
		}
	}


}