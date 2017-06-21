using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame;
using MonoGame.Framework;
using Microsoft.Xna.Framework;

using GameName3.Entities;
using DataLoader;

namespace GameName3.Manager {
	//Will be a manager of tiles, a specific form of animated sprites
	class TileManager : ManagerBase<Tile> {
		Manager.Libraries.IDDictLibrary<GameName3.Entities.Tile> tileLibrary;

		public TileManager() {
			//Blah blah load in proxy objects
			
			//Blah blah use Loading.TileLoader
		}

		public void loadItems(IEnumerable<TileData> toLoad) {
			tileLibrary.addItems<TileData>(toLoad, new Func<TileData, Tile>(TileLoader.toObject));
		}

		public override void loadItems(IEnumerable<IProxyObject> toLoad) {
			//Wrong class
			throw new NotSupportedException();
		}

		static class TileLoader {
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

public class TileManager {
	//Cells.contents is a reference type, so when contents are moved from one cell to another,
	//the old cell still contains a reference to contents of both these cells. When the contents of the invalid
	//cell are removed, the other, valid contents are removed aswell!!!
	//FIX: Dictionary that contains map of all cell data. each cell contains the CellID, which points to this for drawing.
	//	This means cell data is not immediately thrown away as soon as the cell is unloaded!!!!

	public int maxLoadedCells = 256; // Max Cells = Max Memory (MB) * 8
	public Dictionary<Point, tile[,]> cellData;
	public TileCell[] cells = new TileCell[9];	/* [0] [1] [2]
													 * [3] [4] [5]
													 * [6] [7] [8]
													 */
	float tileScale;
	readonly float tileGameSize = Declaration.tileGameSize;
	Point cameraFocus;
	public Point originOffset;
	Vector2 focusOffset;
	Vector2 halfScreen;
	public Rectangle screen;
	public bool needsLoad;
	public Point topLeftCoordinate;
	public Point bottomRightCoordinate;

	Texture2D tileTex;
	public SpriteListSimple sprites;

	//Content loading (unused at the moment)
	public ContentManager content;
	System.IO.StreamReader tileReader;
	bool isStreaming;
	int currentCellStreaming;
	Queue<int> cellsToStream;

	public TileManager(ContentManager _content, Rectangle screen) {
		content = _content;
		loadContent();
		this.tileScale = Declaration.Scale;
		cellData = new Dictionary<Point, tile[,]>();
		needsLoad = false;

		for (int i = 0; i < 9; i++) {
			cells[i] = new TileCell(new Point((i % 3 - 1), (i / 3 - 1)));
			cells[i].contentsValid = false;
			needsLoad = true;
		}	//Correct points should be loaded in from characters position

		onScreenChange(screen);
	}

	public void loadContent() {
		tileTex = content.Load<Texture2D>("tileset");
		sprites = new SpriteListSimple();
		Dictionary<int, DataLoader.SimpleSpriteListLoader> _sprites =
			content.Load<Dictionary<int, DataLoader.SimpleSpriteListLoader>>("Object Databases\\tiles");
		foreach (int key in _sprites.Keys) {
			sprites.setFrames(ref _sprites[key].spriteRec, _sprites[key].spriteRec.Width,
				_sprites[key].spriteRec.Height, (byte)(_sprites[key].numberOfFrames), key);
		}
	}

	public int getCellID(Point cellPoint) {
		return cellPoint.X - cells[0].CellCoordinates.X +
			3 * (cellPoint.Y - cells[0].CellCoordinates.Y);
	}

	//IF GC is slowing things down, change previous tile instead of creating a new one?
	public void setContents(Point Cell, int tileID) {
		tile[,] cellContents = new tile[Functions.tilesPerSector, Functions.tilesPerSector];

		for (int row = 0; row < Functions.tilesPerSector; row++) {
			for (int column = 0; column < Functions.tilesPerSector; column++) {
				cellContents[column, row] = new tile(tileID, 0);
			}
		}

		cells[getCellID(Cell)].contentsValid = true;
		cells[getCellID(Cell)].contentsChanged = false;
		if (cellData.ContainsKey(Cell)) {
			cellData[Cell] = cellContents;
		} else {
			cellData.Add(Cell, cellContents);
		}
	}

	public void setContents(Point Cell, string[] mapData) {
		//Each array is one tile of the cell
		tile[,] cellContents = new tile[Functions.tilesPerSector, Functions.tilesPerSector];
		for (int row = 0; row < Functions.tilesPerSector; row++) {
			int rowArrayValue = row * Functions.tilesPerSector;

			for (int column = 0; column < Functions.tilesPerSector; column++) {
				int arrayIndex = rowArrayValue + column;
				string[] tileData = mapData[arrayIndex].Split(',');
				int fgID = Convert.ToInt32(tileData[0]);
				int bgID = Convert.ToInt32(tileData[2]);
				byte fgRot = Convert.ToByte(tileData[1]);
				byte bgRot = Convert.ToByte(tileData[3]);
				int fgLength = (fgID == -1) ? 1 : sprites.list[fgID].Length - 1;
				int bgLength = (bgID == -1) ? 1 : sprites.list[bgID].Length - 1;

				cellContents[column, row] = new tile(fgID, fgLength,
					fgRot, bgID, bgLength, bgRot);
			}
		}

		int cellID = getCellID(Cell);
		if (cellID != -1) {
			cells[cellID].contentsValid = true;
			cells[cellID].contentsChanged = false;
		}

		if (cellData.ContainsKey(Cell)) {
			cellData[Cell] = cellContents;
		} else {
			cellData.Add(Cell, cellContents);
		}
	}

	public void setCells(Point spriteLocation, Vector2 offset) {
		Point centralCell = new Point(
			(int)Math.Floor((double)spriteLocation.X / Functions.tilesPerSector),
			(int)Math.Floor((double)spriteLocation.Y / Functions.tilesPerSector));
		int cellNo;
		for (int x = 0; x < 3; x++) {
			for (int y = 0; y < 3; y++) {
				cellNo = y * 3 + x;
				cells[cellNo].CellCoordinates.X = centralCell.X - 1 + x;
				cells[cellNo].CellCoordinates.Y = centralCell.Y - 1 + y;
				cells[cellNo].contentsValid = false;
				cells[cellNo].TileCoordinates.X = cells[cellNo].CellCoordinates.X * Functions.tilesPerSector;
				cells[cellNo].TileCoordinates.Y = cells[cellNo].CellCoordinates.Y * Functions.tilesPerSector;
			}
		}

		//cameraFocus = spriteLocation;
		focusOffset = offset;
	}

	public void saveTileData(Point cell) {
		string fname = content.RootDirectory + "\\Object Databases\\World\\Tiles\\" +
			cell.X + " " + cell.Y + ".map";
		//Each tile section needs 10 bytes:
		//	fgID(2) comma(1) fgRot(1) comma(1) (bgID)(2) comma(1) bgRot(1) semiColon(1)
		string toWrite = "";
		int stringPos = 0;
		for (int y = 0; y < Functions.tilesPerSector; y++) {
			for (int x = 0; x < Functions.tilesPerSector; x++) {
				toWrite += cellData[cell][x, y].fgID + "," + cellData[cell][x, y].fgRotation + "," +
					cellData[cell][x, y].bgID + "," + cellData[cell][x, y].bgRotation + ";";
			}
		}
		System.IO.File.WriteAllText(fname, toWrite);
	}

	public void Update(Point cameraFocus, Vector2 focusOffset) {
		//Check whether focus is still in the center tile
		//Check the edges 
		if (this.cameraFocus == cameraFocus) {
			//Only offset has changed, so no recalculation necessary
			this.focusOffset = focusOffset;
		} else {
			bool flagCellChange = false;

			while (cameraFocus.X >= cells[5].TileCoordinates.X) {
				//Focus has moved one cell to the right
				shiftTileCellsLeft(); //was: 0,1,2. now: 1,2,X
				flagCellChange = true;
			}
			while (cameraFocus.X < cells[4].TileCoordinates.X) {
				//Focus has moved one cell to the left
				shiftTileCellsRight(); //was: 0,1,2. now: X,0,1
				flagCellChange = true;
			}

			while (cameraFocus.Y < cells[4].TileCoordinates.Y) {
				//Focus has moved one cell up
				//Causes all cells to be invalid
				shiftTileCellsDown(); //was: 0,3,6. now: X,0,3
				flagCellChange = true;
			}
			while (cameraFocus.Y >= cells[7].TileCoordinates.Y) {
				//Focus has moved one cell down
				shiftTileCellsUp();	//was 0,3,6. now: 0,3,X
				flagCellChange = true;
			}

			if (flagCellChange) needsLoad = true;

			this.cameraFocus = cameraFocus;
			this.focusOffset = focusOffset;
			getDrawRegions();
		}
	}

	public void advanceFrames() {
		for (int itr = 0; itr < 9; itr++) {
			advanceTileFrames(cells[itr].CellCoordinates);
		}
	}

	public void advanceTileFrames(Point cell) {
		for (int row = 0; row < Functions.tilesPerSector; row++) {
			for (int column = 0; column < Functions.tilesPerSector; column++) {
				cellData[cell][row, column].advanceFrames();
			}
		}
	}
	#region "getDraw"
	private void getDrawRegions() {
		//First cell should be one to the topleft 
		//Calculate the screen space values of the corners of the middle cell
		topLeftCoordinate.X = cameraFocus.X - (int)Math.Floor(halfScreen.X / tileGameSize) - 1;
		topLeftCoordinate.Y = cameraFocus.Y - (int)Math.Floor(halfScreen.Y / tileGameSize) - 1;
		bottomRightCoordinate.X = cameraFocus.X + (int)Math.Ceiling(halfScreen.X / tileGameSize) + 1;
		bottomRightCoordinate.Y = cameraFocus.Y + (int)Math.Ceiling(halfScreen.Y / tileGameSize) + 1;

		bool drawLeft = false;
		bool drawRight = false;
		bool drawTop = false;
		bool drawBottom = false;

		if (topLeftCoordinate.X < cells[4].TileCoordinates.X) {
			//Left regions should be drawn
			drawLeft = true;
			//Calculate [0],[3],[6]
			cells[2].isVisible = cells[5].isVisible = cells[8].isVisible = false;
		} else if (bottomRightCoordinate.X >= cells[4].TileCoordinates.X + Functions.tilesPerSector) {
			//Right regions should be drawn
			drawRight = true;
			//Calculate [2],[5],[8]
			cells[0].isVisible = cells[3].isVisible = cells[6].isVisible = false;
		}

		if (topLeftCoordinate.Y < cells[4].TileCoordinates.Y) {
			//Top regions should be drawn
			drawTop = true;
			//Calculate cells[1]
			cells[6].isVisible = cells[7].isVisible = cells[8].isVisible = false;
		} else if (bottomRightCoordinate.Y >= cells[4].TileCoordinates.Y + Functions.tilesPerSector) {
			//Bottom regions should be drawn
			drawBottom = true;
			//Calculate cells[7]
			cells[0].isVisible = cells[1].isVisible = cells[2].isVisible = false;
		}

		for (int i = 0; i < 9; i++) {
			cells[i].isVisible = false;
		}

		if (drawRight) {
			if (drawTop) {
				getDrawRegionsTopRight();
			} else if (drawBottom) {
				getDrawRegionsBottomRight();
			} else {
				getDrawRegionsRight();
			}
		} else if (drawLeft) {
			if (drawTop) {
				getDrawRegionsTopLeft();
			} else if (drawBottom) {
				getDrawRegionsBottomLeft();
			} else {
				getDrawRegionsLeft();
			}
		} else if (drawTop) {
			getDrawRegionsTop();
		} else if (drawBottom) {
			getDrawRegionsBottom();
		} else
			getDrawRegionsMiddle();
	}

	//Cell [4] will always be drawn, so these coordinates are correct for every combination of draw areas
	private Tuple<Point, Point> getDrawRegionsMiddle() {
		//Calculate [4]
		Point begin;
		Point end;
		begin.X = Math.Max(topLeftCoordinate.X, cells[4].TileCoordinates.X);
		begin.Y = Math.Max(topLeftCoordinate.Y, cells[4].TileCoordinates.Y);
		end.X = Math.Min(bottomRightCoordinate.X, cells[4].TileCoordinates.X + Functions.tilesPerSector - 1);
		end.Y = Math.Min(bottomRightCoordinate.Y, cells[4].TileCoordinates.Y + Functions.tilesPerSector - 1);

		cells[4].setDrawRegion(begin, end);
		return new Tuple<Point, Point>(begin, end);
	}

	private void getDrawRegionsTopRight() {
		//Calculate [4],[5],[1],[2]
		Tuple<Point, Point> mid = getDrawRegionsMiddle();
		Point start;
		Point end;
		//1
		start.X = topLeftCoordinate.X;
		start.Y = topLeftCoordinate.Y;
		end.X = mid.Item2.X;
		end.Y = Functions.tilesPerSector - 1;
		cells[1].setDrawRegion(start, end);

		//2
		start.X = 0;
		end.X = bottomRightCoordinate.X;
		cells[2].setDrawRegion(start, end);

		//5
		start.Y = 0;
		end.Y = bottomRightCoordinate.Y;
		cells[5].setDrawRegion(start, end);
	}

	private void getDrawRegionsRight() {
		//Calculate [4] and [5]
		getDrawRegionsMiddle();
		Point start;
		start.X = 0;
		start.Y = topLeftCoordinate.Y;
		cells[5].setDrawRegion(start, bottomRightCoordinate);
	}

	private void getDrawRegionsBottomRight() {
		//Calculate [4],[5],[7],[8]
		Tuple<Point, Point> mid = getDrawRegionsMiddle();
		Point start;
		Point end;
		//7
		start.X = topLeftCoordinate.X;
		start.Y = 0;
		end.X = mid.Item2.X;
		end.Y = bottomRightCoordinate.Y;
		cells[7].setDrawRegion(start, end);
		//8
		start.X = 0;
		start.Y = 0;
		cells[8].setDrawRegion(start, bottomRightCoordinate);
		//5
		start.Y = topLeftCoordinate.Y;
		end.X = bottomRightCoordinate.X;
		end.Y = Functions.tilesPerSector - 1;
		cells[5].setDrawRegion(start, end);
	}

	private void getDrawRegionsBottom() {
		//Calculate [4],[7]
		getDrawRegionsMiddle();
		Point start;
		//7
		start.X = topLeftCoordinate.X;
		start.Y = 0;
		cells[7].setDrawRegion(start, bottomRightCoordinate);
	}

	private void getDrawRegionsBottomLeft() {
		//Calculate [3],[4],[6],[7]
		Tuple<Point, Point> mid = getDrawRegionsMiddle();
		Point start;
		Point end;
		//7
		start.X = 0;
		start.Y = 0;
		cells[7].setDrawRegion(start, bottomRightCoordinate);
		//6
		start.X = topLeftCoordinate.X;
		end.X = Functions.tilesPerSector - 1;
		end.Y = bottomRightCoordinate.Y;
		cells[6].setDrawRegion(start, end);
		//3
		start.Y = topLeftCoordinate.Y;
		end.Y = Functions.tilesPerSector - 1;
		cells[3].setDrawRegion(start, end);
	}

	private void getDrawRegionsLeft() {
		//Calculate [3],[4]
		getDrawRegionsMiddle();
		Point end;
		end.X = Functions.tilesPerSector - 1;
		end.Y = bottomRightCoordinate.Y;
		cells[3].setDrawRegion(topLeftCoordinate, end);
	}

	private void getDrawRegionsTopLeft() {
		//Calculate [0],[1],[3],[4]
		Tuple<Point, Point> mid = getDrawRegionsMiddle();
		Point start;
		Point end;
		//0
		end.X = Functions.tilesPerSector - 1;
		end.Y = Functions.tilesPerSector - 1;
		cells[0].setDrawRegion(topLeftCoordinate, end);
		//1
		start.X = 0;
		start.Y = topLeftCoordinate.Y;
		end.X = bottomRightCoordinate.X;
		end.Y = Functions.tilesPerSector - 1;
		cells[1].setDrawRegion(start, end);
		//3
		start.X = topLeftCoordinate.X;
		start.Y = 0;
		end.X = Functions.tilesPerSector - 1;
		end.Y = bottomRightCoordinate.Y;
		cells[3].setDrawRegion(start, end);
	}

	private void getDrawRegionsTop() {
		//Calculate [1],[4]
		getDrawRegionsMiddle();
		//4
		Point end;
		end.X = bottomRightCoordinate.X;
		end.Y = Functions.tilesPerSector - 1;
		cells[1].setDrawRegion(topLeftCoordinate, end);
	}
	#endregion
	public void draw(ref SpriteBatch sprite, Effect fx, Vector2 view) {
		//Draw each visible region
		fx.Parameters["viewport"].SetValue(view);
		sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
			RasterizerState.CullNone, fx);
		Vector2 drawPos = Vector2.Zero;
		Point temp;
		Color spriteColor;
		focusOffset.X = (int)focusOffset.X;		//Using float values cause artifacts around each tile sprite on pan
		originOffset.X = (int)originOffset.X;
		focusOffset.Y = (int)focusOffset.Y;
		originOffset.Y = (int)originOffset.Y;

		for (int itr = 0; itr < 9; itr++) {
			if (cells[itr].isVisible) {
				for (int row = cells[itr].visibleRegionBegin.X; row <= cells[itr].visibleRegionEnd.X; row++) {
					temp.X = row;
					for (int col = cells[itr].visibleRegionBegin.Y; col <= cells[itr].visibleRegionEnd.Y; col++) {
						temp.Y = col;
						//drawPos.X = (row - topLeftCoordinate.X) * tileGameSize - focusOffset.X;
						//drawPos.Y = (col - topLeftCoordinate.Y) * tileGameSize - focusOffset.Y;
						drawPos = toLocalSpace(temp, cells[itr].CellCoordinates);
						drawPos.X -= focusOffset.X;
						drawPos.Y -= focusOffset.Y;
						drawPos.X -= originOffset.X;
						drawPos.Y -= originOffset.Y;

						float depthValue = MathHelper.Clamp(1 - (float)drawPos.Y / view.Y, 0f, 1f);
						int colorVal = (int)(depthValue * 255);
						//Each channel can store seperate data.
						spriteColor = new Color(colorVal, colorVal, colorVal, colorVal);
						tile thisTile = cellData[cells[itr].CellCoordinates][row, col];

						if (thisTile.bgID != -1) {	//Draw a background tile if there is one
							sprite.Draw(tileTex, drawPos,
								sprites.list[thisTile.bgID][thisTile.bgFramePosX],
								spriteColor, thisTile.fgRotation * 90.0f, Vector2.Zero, tileScale, SpriteEffects.None, 1.0f);
						}
						if (thisTile.fgID != -1) {	//Draw a foreground tile if there is one
							sprite.Draw(tileTex, drawPos,
								sprites.list[thisTile.fgID][thisTile.fgFramePosX],
								spriteColor, thisTile.fgRotation * 90.0f, Vector2.Zero, tileScale, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}

		sprite.End();
	}

	public Vector2 toLocalSpace(Point localCoord, Point cellCoord) {
		Vector2 wspCoord = new Vector2(cellCoord.X * Functions.tilesPerSector + localCoord.X,
			cellCoord.Y * Functions.tilesPerSector + localCoord.Y);
		wspCoord.X -= topLeftCoordinate.X + 1;
		wspCoord.Y -= topLeftCoordinate.Y + 1;
		wspCoord.X *= 40;
		wspCoord.Y *= 40;
		return wspCoord;
	}

	public void onScreenChange(Rectangle newScreen) {
		screen = newScreen;
		halfScreen.X = screen.Width / 2;
		halfScreen.Y = screen.Height / 2;
		originOffset.X = -(int)((halfScreen.X) % tileGameSize);
		originOffset.Y = -(int)((halfScreen.Y) % tileGameSize);

		getDrawRegions();
	}

	private void shiftTileCellsDown() {
		//Load top row - 012 needs to be initialised
		//Save Bottom row - 678 needs to be saved
		//6,7,8 -> 3,4,5
		//3,4,5 -> 0,1,2
		int columnCtr = 3;

		if (cells[6].contentsChanged == true)
			saveTileData(cells[6].CellCoordinates);
		if (cells[7].contentsChanged == true)
			saveTileData(cells[7].CellCoordinates);
		if (cells[8].contentsChanged == true)
			saveTileData(cells[8].CellCoordinates);

		for (columnCtr = 3; columnCtr < 6; columnCtr++) {
			//cells[columnCtr + 3] = cells[columnCtr];
			cells[columnCtr + 3].contentsChanged = cells[columnCtr].contentsChanged;
			cells[columnCtr + 3].CellCoordinates = cells[columnCtr].CellCoordinates;
			cells[columnCtr + 3].TileCoordinates = cells[columnCtr].TileCoordinates;
			cells[columnCtr + 3].contentsValid = cells[columnCtr].contentsValid;
		}

		for (columnCtr = 0; columnCtr < 3; columnCtr++) {
			//cells[columnCtr + 3] = cells[columnCtr];
			cells[columnCtr + 3].contentsChanged = cells[columnCtr].contentsChanged;
			cells[columnCtr + 3].CellCoordinates = cells[columnCtr].CellCoordinates;
			cells[columnCtr + 3].TileCoordinates = cells[columnCtr].TileCoordinates;
			cells[columnCtr + 3].contentsValid = cells[columnCtr].contentsValid;
		}

		cells[0].contentsChanged = cells[1].contentsChanged = cells[2].contentsChanged = false;
		cells[0].contentsValid = cells[1].contentsValid = cells[2].contentsValid = false;
		cells[2].CellCoordinates.Y--;
		cells[0].CellCoordinates.Y = cells[1].CellCoordinates.Y = cells[2].CellCoordinates.Y;
		cells[0].TileCoordinates.Y = cells[0].CellCoordinates.Y * Functions.tilesPerSector;
		cells[1].TileCoordinates.Y = cells[1].CellCoordinates.Y * Functions.tilesPerSector;
		cells[2].TileCoordinates.Y = cells[2].CellCoordinates.Y * Functions.tilesPerSector;
		needsLoad = true;

	}

	private void shiftTileCellsUp() {
		//Load bottom row - 678 needs to be initialised
		//Save top row - 012 needs to be saved
		//0,1,2 -> 3,4,5
		//3,4,5 -> 6,7,8
		int columnCtr = 3;

		if (cells[0].contentsChanged == true)
			saveTileData(cells[0].CellCoordinates);
		if (cells[1].contentsChanged == true)
			saveTileData(cells[1].CellCoordinates);
		if (cells[2].contentsChanged == true)
			saveTileData(cells[2].CellCoordinates);

		for (columnCtr = 3; columnCtr < 6; columnCtr++) {
			//cells[columnCtr - 3] = cells[columnCtr];
			cells[columnCtr - 3].contentsChanged = cells[columnCtr].contentsChanged;
			cells[columnCtr - 3].CellCoordinates = cells[columnCtr].CellCoordinates;
			cells[columnCtr - 3].TileCoordinates = cells[columnCtr].TileCoordinates;
			cells[columnCtr - 3].contentsValid = cells[columnCtr].contentsValid;
		}

		for (columnCtr = 6; columnCtr < 9; columnCtr++) {
			//cells[columnCtr - 3] = cells[columnCtr];
			cells[columnCtr - 3].contentsChanged = cells[columnCtr].contentsChanged;
			cells[columnCtr - 3].CellCoordinates = cells[columnCtr].CellCoordinates;
			cells[columnCtr - 3].TileCoordinates = cells[columnCtr].TileCoordinates;
			cells[columnCtr - 3].contentsValid = cells[columnCtr].contentsValid;
		}

		cells[6].contentsChanged = cells[7].contentsChanged = cells[8].contentsChanged = false;
		cells[6].contentsValid = cells[7].contentsValid = cells[8].contentsValid = false;
		cells[8].CellCoordinates.Y++;
		cells[6].CellCoordinates.Y = cells[7].CellCoordinates.Y = cells[8].CellCoordinates.Y;
		cells[6].TileCoordinates.Y = cells[6].CellCoordinates.Y * Functions.tilesPerSector;
		cells[7].TileCoordinates.Y = cells[7].CellCoordinates.Y * Functions.tilesPerSector;
		cells[8].TileCoordinates.Y = cells[8].CellCoordinates.Y * Functions.tilesPerSector;
		needsLoad = true;
	}

	private void shiftTileCellsLeft() {
		//036 needs to be saved
		//0,3,6 -> 1,4,7 
		//1,4,7 -> 2,5,8
		int rowCtr = 1;
		if (cells[0].contentsChanged == true)
			saveTileData(cells[0].CellCoordinates);
		if (cells[3].contentsChanged == true)
			saveTileData(cells[3].CellCoordinates);
		if (cells[6].contentsChanged == true)
			saveTileData(cells[6].CellCoordinates);

		//First Column
		for (rowCtr = 0; rowCtr < 7; rowCtr += 3) {
			cells[rowCtr].contentsChanged = cells[rowCtr + 1].contentsChanged;
			cells[rowCtr].CellCoordinates = cells[rowCtr + 1].CellCoordinates;
			cells[rowCtr].TileCoordinates = cells[rowCtr + 1].TileCoordinates;
			cells[rowCtr].contentsValid = cells[rowCtr + 1].contentsValid;
		}

		//Second Column
		for (rowCtr = 1; rowCtr < 9; rowCtr += 3) {
			cells[rowCtr].contentsChanged = cells[rowCtr + 1].contentsChanged;
			cells[rowCtr].CellCoordinates = cells[rowCtr + 1].CellCoordinates;
			cells[rowCtr].TileCoordinates = cells[rowCtr + 1].TileCoordinates;
			cells[rowCtr].contentsValid = cells[rowCtr + 1].contentsValid;
		}

		cells[2].contentsChanged = cells[5].contentsChanged = cells[8].contentsChanged = false;
		cells[2].contentsValid = cells[5].contentsValid = cells[8].contentsValid = false;
		cells[8].CellCoordinates.X++;
		cells[2].CellCoordinates.X = cells[5].CellCoordinates.X = cells[8].CellCoordinates.X;
		cells[2].TileCoordinates.X = cells[2].CellCoordinates.X * Functions.tilesPerSector;
		cells[5].TileCoordinates.X = cells[5].CellCoordinates.X * Functions.tilesPerSector;
		cells[8].TileCoordinates.X = cells[8].CellCoordinates.X * Functions.tilesPerSector;
		needsLoad = true;
	}

	private void shiftTileCellsRight() {
		//2,5,8 needs to be saved
		//2,5,8 -> 1,4,7 
		//1,4,7 -> 0,3,6
		int rowCtr = 1;
		if (cells[2].contentsChanged == true)
			saveTileData(cells[2].CellCoordinates);
		if (cells[5].contentsChanged == true)
			saveTileData(cells[5].CellCoordinates);
		if (cells[8].contentsChanged == true)
			saveTileData(cells[8].CellCoordinates);

		for (rowCtr = 2; rowCtr < 9; rowCtr += 3) {
			cells[rowCtr].CellCoordinates = cells[rowCtr - 1].CellCoordinates;
			cells[rowCtr].contentsChanged = cells[rowCtr - 1].contentsChanged;
			cells[rowCtr].TileCoordinates = cells[rowCtr - 1].TileCoordinates;
			cells[rowCtr].contentsValid = cells[rowCtr - 1].contentsValid;
		}

		for (rowCtr = 1; rowCtr < 9; rowCtr += 3) {
			cells[rowCtr].CellCoordinates = cells[rowCtr - 1].CellCoordinates;
			cells[rowCtr].contentsChanged = cells[rowCtr - 1].contentsChanged;
			cells[rowCtr].TileCoordinates = cells[rowCtr - 1].TileCoordinates;
			cells[rowCtr].contentsValid = cells[rowCtr - 1].contentsValid;
		}

		cells[0].contentsValid = cells[3].contentsValid = cells[6].contentsValid = false;
		cells[0].contentsChanged = cells[3].contentsChanged = cells[6].contentsChanged = false;
		cells[6].CellCoordinates.X--;
		cells[0].CellCoordinates.X = cells[3].CellCoordinates.X = cells[6].CellCoordinates.X;
		cells[0].TileCoordinates.X = cells[0].CellCoordinates.X * Functions.tilesPerSector;
		cells[3].TileCoordinates.X = cells[3].CellCoordinates.X * Functions.tilesPerSector;
		cells[6].TileCoordinates.X = cells[6].CellCoordinates.X * Functions.tilesPerSector;
		needsLoad = true;
	}

	public Point getFocus() { return cameraFocus; }

}

public class TileCell {
	public Point CellCoordinates;
	public Point TileCoordinates;
	public bool contentsValid;
	public bool isVisible;
	public Point visibleRegionBegin;	//Only the cells in the rectangle between visible region 
	public Point visibleRegionEnd;		//begin and end are drawn!
	public bool contentsChanged = false;

	//public tile[,] contents = new tile[Functions.tilesPerSector, Functions.tilesPerSector];

	public TileCell(Point Coords) {
		contentsValid = true;
		CellCoordinates = Coords;
		TileCoordinates = new Point(Coords.X * Functions.tilesPerSector, Coords.Y * Functions.tilesPerSector);
		contentsChanged = false;
	}

	public TileCell() {

	}

	public bool needsLoad() {
		return contentsValid;
	}

	public void setDrawRegion(Point start, Point end) {
		//visibleRegionBegin = start;
		//visibleRegionEnd = end;

		//Convert to relative index
		//visibleRegionBegin = new Point(Math.Abs(TileCoordinates.X) - Math.Abs(start.X),
		//    Math.Abs(TileCoordinates.Y) - Math.Abs(start.Y));
		//visibleRegionEnd = new Point(Math.Abs(TileCoordinates.X) - Math.Abs(end.X),
		//    Math.Abs(TileCoordinates.Y) - Math.Abs(end.Y));
		visibleRegionBegin.X = start.X < 0 ? Math.Abs(TileCoordinates.X) - Math.Abs(start.X) : start.X % Functions.tilesPerSector;
		visibleRegionBegin.Y = start.Y < 0 ? Math.Abs(TileCoordinates.Y) - Math.Abs(start.Y) : start.Y % Functions.tilesPerSector;
		visibleRegionEnd.X = end.X < 0 ? Math.Abs(TileCoordinates.X) - Math.Abs(end.X) : end.X % Functions.tilesPerSector;
		visibleRegionEnd.Y = end.Y < 0 ? Math.Abs(TileCoordinates.Y) - Math.Abs(end.Y) : end.Y % Functions.tilesPerSector;
		isVisible = true;
	}
}