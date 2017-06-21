using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DataLoader
{
	public class MapData : IProxyObject 
	{
		//Unique characters to be stored in a global list!!??
		public List<EnitityObject> Characters;
		public List<LightObject> BasicLights;
		public List<DirectionalLightLoader> directionalLights;
		public List<PointLightLoader> pointLights;
		public List<DoodadObject> Doodads;
		public List<ParticleObject> Particles;
	}

	public class EnitityObject : IProxyObject 
	{
		public short ID;
		public Point relTilePos;
		public float zPos;
		public Vector2 offset;
	}

	public class LightObject : IProxyObject 
	{
		public short ID;
		public Point relTilePos;
		public float zPos;
		public Vector2 offset;
	}

	public class DirectionalLightLoader : IProxyObject 
	{
		public Point tilePos;
		public Vector2 Offset;
		public float zPos;
		public short ID;
		public Color lightColor;
		public float intensity;
		public float radius;
		public Vector3 direction;
	}

	public class PointLightLoader : IProxyObject 
	{
		public Point tilePos;
		public Vector2 Offset;
		public float zPos;
		public short ID;
		public Color lightColor;
		public float intensity;
		public float radius;
	}

	public class DoodadObject : IProxyObject 
	{
		public short ID;
		public Point relTilePos;
		public float zPos;
		public Vector2 offset;
	}

	public class ParticleObject : IProxyObject 
	{
		public short ID;
		public Point relTilePos;
		public float zPos;
		public Vector2 offset;
	}

	//Class for dynamic object which stores the objects state?
	public class DynamicObjectData : DoodadObject
	{
		//Enum state
	}

	//can be overloaded for other objects which require more customisation
}
