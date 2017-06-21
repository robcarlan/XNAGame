using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLoader
{
	public class ProjectileLoader : IProxyObject 
	{
		public string projName;
		public int spriteID;
		public bool collidable;
		public bool homing;
		public float velocity;
		public float anglePerMS;
		public float collisionRadius;
		public List<short> characterEffectIDs;	//List of effects which occur on hit to enemy
		public List<short> particleEffects;
		public List<short> onCollideParticleEffects;
		public short lightID;
	}
}
