using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WindowsGame1.Entities;
using DataLoader;

namespace WindowsGame1.Manager {
	//Will be a manager of tiles, a specific form of animated sprites
	class TileManager : ManagerBase<Entities.Entity> {
		Manager.Libraries.IDDictLibrary<WindowsGame1.Entities.Tile> tileLibrary;

		//This style is completely different to normal tileManager - are we going to reuse any of that juice? The tile class was far more efficient.
		//maybe we should copy and paste it all in and fit the interface
		//we can still use tile library yay

		public TileManager() {
			//Blah blah load in proxy objects
			
			//Blah blah use Loading.TileLoader
		}

		public override void loadItems(IEnumerable<TileData> toLoad) {
			tileLibrary.addItems(toLoad, TileLoader.toObject);
		}

		static class TileLoader : Loading.Loader<Entities.Entity, TileData> {
			public static Tile toObject(TileData data) {
				//I think we need access to the sprite list
				Tile temp = new Tile(
					data.fgTileID, 0);
				//Create the new object
				return temp;
			}

			public static TileData toProxy(Tile data) {
				TileData temp = new TileData();
				//Create the proxy
				return temp;
			}
		}
	}
}
