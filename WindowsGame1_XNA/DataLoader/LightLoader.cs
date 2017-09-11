using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;

namespace DataLoader
{
	public class LightLoader
	{
		public float radius;
		public string lightName;
		public Color lightColor;
		public float intensity;
		public short ID;
		public short baseID;

		//Controls ebbing etc.
		public float intensityRange;
		public float periodMS;
		public bool flickers;
		public bool oscillates;
		public float nextFlickerIn;
	}
}
