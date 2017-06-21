using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Entity_Manager
{
    class LightManager
    {
        public Dictionary<short, AmbientLight> lights;
        public List<short> pointLights;
        public List<short> ambientLights;
        public List<short> directionalLights;

        public Stack<short> freeID;
        public const int maxLights = 50;

        public List<string> messages;

        public LightManager()
        {
            freeID = new Stack<short>();

            //Free store originally contains all
            for (short i = 0; i < maxLights; i++)
            {
                freeID.Push(i);
            }

            messages = new List<string>();

			pointLights = ambientLights = directionalLights = new List<short>();
        }

        public bool addLight(ref AmbientLight obj)
        {
            if (freeID.Count == 0)
            {
                messages.Add(Functions.WriteDebugLine("Could not insert ambient light object: Max lights reached."));
                return false;
            }

            obj.ID = freeID.Pop();
            ambientLights.Add(obj.ID);
            lights.Add(obj.ID, obj);

            messages.Add(Functions.WriteDebugLine("Added new ambient light."));

            return true;
        }

        public bool addLight(ref PointLight obj)
        {
            if (freeID.Count == 0)
            {
                messages.Add(Functions.WriteDebugLine("Could not insert point light object at position " + obj.pos + ": Max lights reached."));
                return false;
            }

            obj.ID = freeID.Pop();
            pointLights.Add(obj.ID);
            lights.Add(obj.ID, obj);

            messages.Add(Functions.WriteDebugLine("Added new point light at position " + obj.pos + "."));

            return true;
        }

        public bool addLight(ref DirectionalLight obj)
        {
            if (freeID.Count == 0)
            {
                messages.Add(Functions.WriteDebugLine("Could not insert directional light object at position " + obj.pos + ": Max lights reached."));
                return false;
            }

            obj.ID = freeID.Pop();
            directionalLights.Add(obj.ID);
            lights.Add(obj.ID, obj);

            messages.Add(Functions.WriteDebugLine("Added new directional light light at position " + obj.pos + "."));

            return true;
        }

        public bool deleteLight(short ID)
        {
            if (lights.Remove(ID))
            {
                if (pointLights.Remove(ID))
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
            ambientLights.Clear();
            directionalLights.Clear();
            pointLights.Clear();
            messages.Add(Functions.WriteDebugLine("Cleared all lights."));

        }
    }
}
