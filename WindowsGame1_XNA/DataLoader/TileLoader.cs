using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLoader
{
//List sequentially stores the tiles in the cell 
//First tile is top left, last is bottom right.
	[Serializable()]
	public struct TileData
	{
		//Rotation: 0 = 0 rot, 1 = 90, ...
		public int bgTileID;
		public byte bgTileRotation;
		public int fgTileID;
		public byte fgTileRotation;
	}
}
