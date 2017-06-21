using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;

namespace DataLoader
{
	class CollidableLoader : IProxyObject 
	{
		bool useCollisionBox;

		public Rectangle collisionBox;
		public Point circleOrigin;
		public short collisionCircleRadius;
	}
}
