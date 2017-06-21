using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DataLoader
{
    public class SpriteData : IProxyObject 
    {
        
    }

	public class SimpleSpriteListLoader : IProxyObject 
    {
        public Rectangle spriteRec;
        public byte numberOfFrames;
    }

	public class SpriteListLoader : IProxyObject 
	{
		public Dictionary<int, SimpleSpriteListLoader> spriteSub;
	}

}
