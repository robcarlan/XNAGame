using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Entity_Manager
{
    public class LightManager
    {
		//When handling drawing, there needs to be a way of seeing if a light affects the screen
        public Dictionary<short, PointLight> lights;
		public Dictionary<short, BasicLight> ambLights;
		public List<short> basicLights = new List<short>();	//Values for colour and radius taken from a template object given by ID
        public List<short> pointLights = new List<short>();
        public List<short> ambientLights = new List<short>();
        public List<short> directionalLights = new List<short>();
		public Dictionary<Point, List<short>> cellData;	//Lights in a specific cell

		public List<short> visibleLights;
		public Dictionary<short, LightArea> lightAreas;

        public Stack<short> freeID; 
		public Stack<short> freeAmbID;
        public const int maxLights = 50;
		public const int maxAmbient = 10;

        public List<string> messages; 
		public Texture2D lightTex;
		public Texture2D blankTex;

		public Camera gameCamera;
		public const float maxLightShadowRadius = 1024.0f; //If higher, draw lights using texture

        public LightManager()
        {
            freeID = new Stack<short>();
			freeAmbID = new Stack<short>();
			visibleLights = new List<short>();
			lightAreas = new Dictionary<short, LightArea>();

            //Free store originally contains all
            for (short i = maxLights; i > 0; i--)
            {
                freeID.Push(i);
            }

			for (short i = maxLights; i > 0; i--)
			{
				freeAmbID.Push(i);
			}

            messages = new List<string>();

			pointLights = ambientLights = directionalLights = new List<short>();
			ambLights = new Dictionary<short, BasicLight>();
			lights = new Dictionary<short, PointLight>();
        }

		public void Update(short msPassed, GraphicsDevice gfx)
		{
			//Upate the current radius of dynamic lights
			foreach (BasicLight amb in ambLights.Values)
			{
				if (amb.flickers)
				{
					amb.nextFlickerIn -= msPassed;
					if (amb.nextFlickerIn <= 0)
					{
						amb.isFlickering = true;
						amb.nextFlickerIn = (int)amb.flickerFreq;
					}
					else amb.isFlickering = false;
				}
			}

			foreach (PointLight point in lights.Values)
			{
				if (point.attachedEntity != null)
				{
					//Update position based on object position
					point.tilePos = point.attachedEntity.tilePos;
					point.Offset = point.attachedEntity.Offset;
					point.Offset.X += point.attachedEntity.localSpace.Width / 2;
					point.Offset.Y += point.attachedEntity.localSpace.Height / 2;
					point.zPos = point.attachedEntity.zPos;
				}

				if (point.flickers)
				{
					point.nextFlickerIn -= msPassed;
					if (point.nextFlickerIn <= 0)
					{
						if (point.isFlickering)
						{
							Random rand = new Random();
							point.isFlickering = false;
							point.nextFlickerIn = 
								rand.Next((int)((int)point.flickerFreq * 0.5), (int)((int)point.flickerFreq * 1.5));
						}
						else
						{
							point.isFlickering = true;
							point.nextFlickerIn = 32;	//This is when the flicker will end
						}
					}
					//else point.isFlickering = false;
				}

				if (point.oscillates)
				{
					point.elapsedMS = (msPassed + point.elapsedMS) % point.periodMS;
					float timeFrac = (point.elapsedMS / point.periodMS);
					float newRadius = timeFrac * (point.maxRadius - point.minRadius) + point.minRadius;
					newRadius *= (float)Math.Sin(timeFrac * MathHelper.PiOver2);
					point.radius = newRadius;
				}
			}

			//Get all currently visible lights
			foreach (short i in pointLights)
			{
				//If this is outside the window, check if it is in the list of visible lights
				if (isLightInside(lights[i]))
				{
					if (!visibleLights.Contains(i))
					{
						visibleLights.Add(i);
						float maxRadius = (lights[i].oscillates) ? lights[i].maxRadius : lights[i].radius;
						if (maxRadius < maxLightShadowRadius)
						{
							LightArea newArea = new LightArea(gfx, maxRadius, i);
							lightAreas.Add(i, newArea);
							Functions.WriteDebugLine("Added new visible light.");
						}
					}
				}
				else
				{
					if (visibleLights.Contains(i))
					{
						visibleLights.Remove(i);
						lightAreas.Remove(i);
					}
				}
			}

			//Update light positions
			foreach (LightArea temp in lightAreas.Values)
			{
				temp.LightPosition = Functions.getLocalSpace(gameCamera.origin, gameCamera.originVec,
					lights[temp.lightID].tilePos, lights[temp.lightID].Offset);
			}
		}

		public bool isLightInside(PointLight light)
		{
			Point difference = new Point(gameCamera.focus.X - light.tilePos.X, gameCamera.focus.Y - light.tilePos.Y);
			return ((difference.X * difference.X + difference.Y * difference.Y) < (light.radius * light.radius));
		}

		public void drawPointLights(SpriteBatch spritebatch)
		{
			float scale;

			for (short i = 0; i < visibleLights.Count; i++)
			{
				//Add intensity factor, dropoff
				PointLight drawLight = (PointLight)lights[pointLights[i]];
				scale = drawLight.radius / 512;
				spritebatch.Draw(lightTex, getLightPositionTopLeft(pointLights[i]), null, drawLight.lightColor, 0f,
					Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
		}

		public Vector2 getLightPositionCenter(short lightID)
		{
			Vector2 lightPos;
			//Get the difference in tile count
			//lightPos.X = gameCamera.origin.X - lights[lightID].tilePos.X;
			//lightPos.Y = gameCamera.origin.Y - lights[lightID].tilePos.Y;
			lightPos.X = lights[lightID].tilePos.X - gameCamera.origin.X;
			lightPos.Y = lights[lightID].tilePos.Y - gameCamera.origin.Y;
			//Scale to game size
			lightPos *= Declaration.tileGameSize;
			//Apply the camera and position offset
			lightPos -= gameCamera.originVec;
			lightPos += lights[lightID].Offset;

			return lightPos;
		}

		public Vector2 getLightPositionTopLeft(short lightID)
		{
			//get position from center, and offset by radius over 2
			Vector2 lightPos = getLightPositionCenter(lightID);
			float radiusHalved = (lights[lightID].radius / 2);
			lightPos.X -= radiusHalved;
			lightPos.Y -= radiusHalved;
			return lightPos;
		}

		public void drawAmbientLight(SpriteBatch spritebatch)
		{
			spritebatch.Draw(blankTex, Vector2.Zero, null, Color.Black);
		}

		public Color getAmbientValue()
		{
			return new Color(0, 0, 0, 20);
		}

		public short addLight(BasicLight obj)
        {
            if (freeID.Count == 0)
            {
                messages.Add(Functions.WriteDebugLine("Could not insert ambient light object: Max lights reached."));
                return -1;
            }

            obj.ID = freeAmbID.Pop();
            ambientLights.Add(obj.ID);
            ambLights.Add(obj.ID, obj);

            messages.Add(Functions.WriteDebugLine("Added new ambient light."));

            return obj.ID;
        }

        public short addLight(PointLight obj)
        {
            if (freeID.Count == 0)
            {
                messages.Add(Functions.WriteDebugLine("Could not insert point light object at position " + obj.tilePos + ": Max lights reached."));
                return -1;
            }

            obj.ID = freeID.Pop();
            pointLights.Add(obj.ID);
            lights.Add(obj.ID, obj);

            messages.Add(Functions.WriteDebugLine("Added new point light at position " + obj.tilePos + "."));

            return obj.ID;
        }

		public short addLight(DirectionalLight obj)
        {
            if (freeID.Count == 0)
            {
                messages.Add(Functions.WriteDebugLine("Could not insert directional light object at position " + obj.tilePos + ": Max lights reached."));
                return -1;
            }

            obj.ID = freeID.Pop();
            directionalLights.Add(obj.ID);
            lights.Add(obj.ID, obj);

            messages.Add(Functions.WriteDebugLine("Added new directional light light at position " + obj.tilePos + "."));

            return obj.ID;
        }

		public bool addAmbientLight(Color lightColor, float intensity, Point cell, Point tilePos, Vector2 Offset, float zPos)
		{
			if (freeID.Count == 0)
			{
				messages.Add(Functions.WriteDebugLine("Could not insert ambient light, max lights reached."));
				return false;
			}

			BasicLight obj = new BasicLight();

			obj.ID = freeAmbID.Pop();
			obj.lightColor = lightColor;
			obj.intensity = intensity;
			obj.tilePos = tilePos;
			obj.Offset = Offset;
			obj.zPos = zPos;

			ambientLights.Add(obj.ID);
			ambLights.Add(obj.ID, obj);
			if (cell != null)
				cellData[cell].Add(obj.ID);

			messages.Add(Functions.WriteDebugLine("Added new ambient light at position " + tilePos.ToString() + "."));

			return true;
		}

		public bool addPointLight(Color lightColor, float intensity, Point cell, Point tilePos,
			Vector2 Offset, float radius, float zPos)
		{
			if (freeID.Count == 0)
			{
				messages.Add(Functions.WriteDebugLine("Could not insert point light, max lights reached."));
				return false;
			}

			PointLight obj = new PointLight();

			obj.ID = freeID.Pop();
			obj.lightColor = lightColor;
			obj.intensity = intensity;
			obj.zPos = zPos;
			obj.tilePos = tilePos;
			obj.Offset = Offset;
			obj.radius = radius;

			pointLights.Add(obj.ID);
			lights.Add(obj.ID, obj);

			//if (cell != null)
			//	cellData[cell].Add(obj.ID);

			messages.Add(Functions.WriteDebugLine("Added new point light at position " + tilePos.ToString() + "."));

			return true;
		}

		public short addPointLight(DataLoader.LightLoader baseLight, Point cell, Point tilePos,
			Vector2 Offset, float zPos)
		{
			if (freeID.Count == 0)
			{
				messages.Add(Functions.WriteDebugLine("Could not insert point light, max lights reached."));
				return -1;
			}

			PointLight obj = new PointLight();

			obj.ID = freeID.Pop();
			obj.lightColor = baseLight.lightColor;
			obj.intensity = baseLight.intensity;
			obj.zPos = zPos;
			obj.tilePos = tilePos;
			obj.Offset = Offset;
			obj.radius = baseLight.radius;
			obj.flickers = baseLight.flickers;
			obj.flickerFreq = flickerValues.Medium;
			obj.nextFlickerIn = (int)obj.flickerFreq;

			pointLights.Add(obj.ID);
			lights.Add(obj.ID, obj);

			//if (cell != null)
			//	cellData[cell].Add(obj.ID);

			messages.Add(Functions.WriteDebugLine("Added new point light at position " + tilePos.ToString() + "."));

			return obj.ID;
		}

		public bool addDirectionalLight(Color lightColor, float intensity, Point cell, Point tilePos, 
			Vector2 Offset, float radius, float zPos, Vector3 direction)
		{
			if (freeID.Count == 0)
			{
				messages.Add(Functions.WriteDebugLine("Could not insert directional light, max lights reached."));
				return false;
			}

			DirectionalLight obj = new DirectionalLight();

			obj.ID = freeID.Pop();
			obj.lightColor = lightColor;
			obj.intensity = intensity;
			obj.tilePos = tilePos;
			obj.Offset = Offset;
			obj.zPos = zPos;
			obj.radius = radius;
			obj.Direction = direction;

			directionalLights.Add(obj.ID);
			lights.Add(obj.ID, obj);
			if (cell != null)
				cellData[cell].Add(obj.ID);

			messages.Add(Functions.WriteDebugLine("Added new directional light at position " + tilePos.ToString() + "."));

			return true;
		}

		public void unloadCell(Point cell)
		{
			while (cellData[cell].Count > 0 )
			{
				deleteLight(cellData[cell][0]);
				cellData[cell].Remove(0);
			}
			cellData.Remove(cell);
		}

        public bool deleteLight(short ID)
        {

            if (lights.Remove(ID))
            {
				if (basicLights.Remove(ID))
				{
					messages.Add(Functions.WriteDebugLine("Removed basic light with ID " + ID + "."));
				}
                else if (pointLights.Remove(ID))
                {
                    messages.Add(Functions.WriteDebugLine("Removed point light with ID " + ID + "."));
                }
                else if (directionalLights.Remove(ID))
                {
                    messages.Add(Functions.WriteDebugLine("Removed directional light with ID " + ID + "."));
                }
                else
                {
                    ambientLights.Remove(ID);
                    messages.Add(Functions.WriteDebugLine("Removed ambient light with ID " + ID + "."));
                }

                freeID.Push(ID);
				if (lightAreas.ContainsKey(ID)) lightAreas.Remove(ID);
				if (visibleLights.Contains(ID))
				{
					visibleLights.Remove(ID);
				}
                return true;
            }
            else
            {
                messages.Add(Functions.WriteDebugLine("Could not delete light, ID " + ID + " does not exist."));
                return false;
            }
        }

        public void removeAll()
        {
            //Renew the free store
            freeID.Clear();

            for (short i = 0; i < maxLights; i++)
            {
                freeID.Push(i);
            }

            lights.Clear();
			basicLights.Clear();
            ambientLights.Clear();
            directionalLights.Clear();
            pointLights.Clear();
            messages.Add(Functions.WriteDebugLine("Cleared all lights."));

        }
    }
}
