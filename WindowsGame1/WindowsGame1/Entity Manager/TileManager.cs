using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    class tileManager
    {
        public const int tilesPerSector = 128;
        public const int maxOnscreenTiles = 50;
        public long tileOriginX;
        public long tileOriginY;

        Random rnd = new Random();
        
        public byte arraySize = 45;
        public tile[,] onscreenTiles = new tile[maxOnscreenTiles, maxOnscreenTiles];

        private tile[,] CurrentCell = new tile[Functions.tilesPerSector, Functions.tilesPerSector];
        private tile[,] DiagonalCell = new tile[Functions.tilesPerSector, Functions.tilesPerSector];
        private tile[,] HorizontalCell = new tile[Functions.tilesPerSector, Functions.tilesPerSector];
        private tile[,] VerticalCell = new tile[Functions.tilesPerSector, Functions.tilesPerSector];

        public byte arrayStartX = new byte();
        public byte arrayStartY = new byte();
		public Point Offset;

        public Point currentCell = new Point();

        public Point getCell() { return currentCell; }
        public Point getVCell(bool up)
        {
            return new Point(currentCell.X, up ? currentCell.Y + 1 : currentCell.Y - 1);
        }

        public Point getHCell(bool right)
        {
            return new Point(right ? currentCell.X + 1 : currentCell.X - 1, currentCell.Y );
        }
        public Point getDCell(bool right, bool up)
        {
            if (up)
            {
                if (right)
                    return new Point(currentCell.X + 1, currentCell.Y + 1);
                else
                    return new Point(currentCell.X - 1, currentCell.Y + 1);
            }
            else
            {
                if (right)
                     return new Point(currentCell.X + 1, currentCell.Y - 1);
                else
                    return new Point(currentCell.X - 1, currentCell.Y - 1);
            }
        }

        public void changeArrPos(bool column, bool increase)
        {
            if (column)
            {
                if (increase)
                {
                    if (arrayStartX == 0)
                        arrayStartX = (byte)(arraySize - 1);
                    else arrayStartX--;
                }
                else if (arrayStartX == arraySize - 1)
                    arrayStartX = 0;
                else arrayStartX++;
            }
            else
            {
                if (increase)
                {
                    if (arrayStartY == arraySize - 1)
                        arrayStartY = 0;
                    else arrayStartY++;
                }
                else if (arrayStartY == 0)
                    arrayStartY = (byte)(arraySize - 1);
                else arrayStartY--;
            }
        }
        public int getLastArrPos(bool column)
        {
            if (column)
            {
                if (arrayStartX == 0)
                    return arraySize - 1;
                else return arrayStartX - 1;
            }
            else if (arrayStartY == 0)
                return arraySize - 1;
            else return arrayStartY - 1;
        }
        public void checkForUpdate(ref int X, ref int Y)
        {
            if (X == -1)
            {
                addColumn(false);
                X = 0;
            }
            else if (X == 1)
            {
                addColumn(true);
                X = 0;
            }
            if (Y == -1)
            {
                addRow(false);
                Y = 0;
            }
            else if (Y == 1)
            {
                addRow(true);
                Y = 0;
            }

        }
        public void advanceAll() { }
        private int getNextArrayPos(int arrayPos)
        {
            if (arrayPos == arraySize - 1)
                return 0;
            else return arrayPos + 1;
        }
        private int getPrevArrayPos(int arrayPos)
        {
            if (arrayPos == 0)
                return arraySize - 1;
            else return arrayPos - 1;
        }
        public void addRow(bool upwards)
        {
            int counter = new int();
            if (upwards)
            {
                do
                {
                    if (rnd.NextDouble() > 0.5)
                        onscreenTiles[counter, arrayStartX].value = "greenBrick";
                    else onscreenTiles[counter, arrayStartX].value = "beigeBrick";
                    counter = getNextArrayPos(counter);
                } while (counter != arrayStartX);
                changeArrPos(false, true);
            }
            else
            {   //Insert new row at the arrayStartX, then decrease arrayStartX
                do
                {
                    if (rnd.NextDouble() > 0.5)
                        onscreenTiles[counter, arrayStartX].value = "greenBrick";
                    else onscreenTiles[counter, arrayStartX].value = "beigeBrick";

                    counter = getPrevArrayPos(counter);
                } while (counter != arrayStartX);
                changeArrPos(false, false);
            }
            //else decrement arraystart, add new row at that pos

        }
        public void addColumn(bool forwards)
        {
            int counter = new int();
            if (!forwards)
            {
                do
                {
                    if (rnd.NextDouble() > 0.5)
                        onscreenTiles[arrayStartY, counter].value = "greenBrick";
                    else onscreenTiles[arrayStartY, counter].value = "beigeBrick";
                    counter = getNextArrayPos(counter);
                } while (counter != arrayStartY);
                changeArrPos(true, true);
            }

            else
            {   //Insert new row at the arrayStartX, then decrease arrayStartX
                do
                {
                    if (rnd.NextDouble() > 0.5)
                        onscreenTiles[arrayStartY, counter].value = "greenBrick";
                    else onscreenTiles[arrayStartY, counter].value = "beigeBrick";

                    counter = getPrevArrayPos(counter);
                } while (counter != arrayStartY);
                changeArrPos(true, false);
            }
            //else decrement arraystart, add new row at that pos

        }

        public void screenChange(int heightChange, int widthChange, float scale)
        {
            float counter = new float();
            if (widthChange > 0)
            {
                for (counter = (widthChange / scale / 16 * 0.5f); counter >= 1; counter--)
                {
                    addColumn(false);
                }
            }
            else
            {
                for (counter = (widthChange / scale / 16 * 0.5f); counter <= -1; counter++)
                {
                    addColumn(true);
                }
            }
            Offset.X += (int)(counter * scale * 16);

            if (heightChange > 0)
            {
                for (counter = (heightChange / scale / 16 * 0.5f); counter > 0; counter--)
                {
                    addRow(false);
                }
            }
            else
            {
                for (counter = (heightChange / scale / 16 * 0.5f); counter < 0; counter++)
                {
                    addRow(true);
                }
            }
            Offset.Y += (int)(counter * scale * 16);

        }
    }

	/*
	 * TODO: Make tile values an ID rather than string for speed!!!!
	 */
    public class tile
    {
        public string value;
        public byte framePosX;
        public byte framePosMax;
        public void advance()
        {
            framePosMax = new byte();
            framePosX = new byte();
			value = "N";

            if (framePosX < framePosMax)
                framePosX++;
            else framePosX = 0;
        }
    }

	public class TileManagerNew
	{
		/*		How this class will work:
		 *	Given a focus point by camera class (i.e players coordinates), with an offset from 0...tileLength
		 *	Whenever this is changed, the topLeft tileCell is calculated (with it's corresponding tileCell )
		 *	Drawing is used via iteration from this cell, drawing each visible region in each TileCell
		 *	When the focus moves over to another TileCell, the correct contents of each TileCell are transferred
		 *	and each redundant TileCell is flagged, indicating it needs new contents.
		 *	focus is recalculated on screen size changed
		 *	This class should operate independently of all others, apart from the camera class.
		 */

		TileCell[] cells = new TileCell[9];		/* [0] [1] [2]
												 * [3] [4] [5]
												 * [6] [7] [8]
												 */

		float tileScale = 2.0f;
		Point cameraFocus;
		Vector2 focusOffset;
		Rectangle screen;
		public bool needsLoad;

		public TileManagerNew()
		{
			needsLoad = false;

			for (int rowCtr = 0; rowCtr < Functions.tilesPerSector; rowCtr++)
			{
				for (int colCtr = 0; colCtr < Functions.tilesPerSector; colCtr++)
				{
					cells[0].contents[rowCtr, colCtr].value = "beigeBrick";
					cells[1].contents[rowCtr, colCtr].value = "greenBrick";
					cells[2].contents[rowCtr, colCtr].value = "beigeBrick";
					cells[3].contents[rowCtr, colCtr].value = "greenBrick";
					cells[4].contents[rowCtr, colCtr].value = "beigeBrick";
					cells[5].contents[rowCtr, colCtr].value = "greenBrick";
					cells[6].contents[rowCtr, colCtr].value = "beigeBrick";
					cells[7].contents[rowCtr, colCtr].value = "greenBrick";
					cells[8].contents[rowCtr, colCtr].value = "beigeBrick";
				}
			}
		}

		public void Update(Point cameraFocus, Vector2 focusOffset)
		{
			//Check whether focus is still in the center tile
			//Check the edges 
			bool flagCellChange = false;

			while (cameraFocus.X >= cells[5].TileCoordinates.X)	//Might be while(camerafocus > tilecoords) ???
			{
				//Focus has moved one cell to the right
				shiftTileCellsLeft(); //was: 0,1,2. now: 1,2,X
				flagCellChange = true;
			}
			while (cameraFocus.X < cells[4].TileCoordinates.X)
			{
				//Focus has moved one cell to the left
				shiftTileCellsRight(); //was: 0,1,2. now: X,0,1
				flagCellChange = true;
			}

			while (cameraFocus.Y >= cells[4].TileCoordinates.Y)
			{
				//Focus has moved one cell up
				shiftTileCellsDown(); //was: 0,3,6. now: X,0,3
				flagCellChange = true;
			}
			while (cameraFocus.Y <= cells[7].TileCoordinates.Y)
			{
				//Focus has moved one cell down
				shiftTileCellsUp();	//was 0,3,6. now: 0,3,X
				flagCellChange = true;
			}

			if (flagCellChange)
				getDrawRegions();
			else
			{

			}
		}

		private void getDrawRegions()
		{
			//Calculate the screen space values of the corners of the middle cell
			float halfWidth = screen.Width / 2;
			float halfHeight = screen.Height / 2;
			//Point topLeftCoordinate = 
		}

		public void draw(SpriteBatch sprite)
		{
			//Draw each visible region
		}

		public void onScreenChange(Rectangle newScreen)
		{

		}

		private void shiftTileCellsDown()
		{
			//6,7,8 -> 3,4,5
			//3,4,5 -> 0,1,2
			int columnCtr = 3;

			for (columnCtr = 3; columnCtr < 6; columnCtr++)
			{
				cells[columnCtr + 3] = cells[columnCtr];
			}

			for (columnCtr = 0; columnCtr < 3; columnCtr++)
			{
				cells[columnCtr + 3] = cells[columnCtr];
			}

			cells[0].contentsValid = cells[1].contentsValid = cells[2].contentsValid = false;
			needsLoad = true;
			
		}

		private void shiftTileCellsUp()
		{
			//0,1,2 -> 3,4,5
			//3,4,5 -> 6,7,8
			int columnCtr = 3;

			for (columnCtr = 3; columnCtr < 6; columnCtr++)
			{
				cells[columnCtr - 3] = cells[columnCtr];
			}

			for (columnCtr = 6; columnCtr < 9; columnCtr++)
			{
				cells[columnCtr - 3] = cells[columnCtr];
			}

			cells[6].contentsValid = cells[7].contentsValid = cells[8].contentsValid = false;
			needsLoad = true;
		}

		private void shiftTileCellsLeft()
		{
			//0,3,6 -> 1,4,7 
			//1,4,7 -> 2,5,8
			int rowCtr = 1;

			for (rowCtr = 0; rowCtr < 7; rowCtr += 3)
			{
				cells[rowCtr] = cells[rowCtr + 1];
			}

			for (rowCtr = 1; rowCtr < 9; rowCtr += 3)
			{
				cells[rowCtr] = cells[rowCtr + 1];
			}

			cells[2].contentsValid = cells[5].contentsValid = cells[8].contentsValid = false;
			needsLoad = true;
		}

		private void shiftTileCellsRight()
		{
			//2,5,8 -> 1,4,7 
			//1,4,7 -> 0,3,6
			int rowCtr = 1;

			for (rowCtr = 2; rowCtr < 9; rowCtr += 3)
			{
				cells[rowCtr] = cells[rowCtr - 1];
			}

			for (rowCtr = 1; rowCtr < 9; rowCtr += 3)
			{
				cells[rowCtr] = cells[rowCtr - 1];
			}

			cells[0].contentsValid = cells[3].contentsValid = cells[6].contentsValid = false;
			needsLoad = true;
		}

	}

	public class TileCell
	{
		public Point CellCoordinates;
		public Point TileCoordinates;
		public bool contentsValid;
		public bool isVisible;
		Point visibleRegionBegin;	//Only the cells in the rectangle between visible region 
		Point visibleRegionEnd;		//begin and end are drawn!

		public tile[,] contents = new tile[Functions.tilesPerSector, Functions.tilesPerSector];

		public TileCell(Point Coords)
		{
			contentsValid = true;
			CellCoordinates = Coords;
			TileCoordinates = new Point(Coords.X * Functions.tilesPerSector, Coords.Y * Functions.tilesPerSector );
		}

		public TileCell()
		{

		}

		public void Draw(SpriteBatch sprite)
		{

		}

		public bool needsLoad()
		{
			return contentsValid;
		}

		public void setContents()
		{
			//Set all contents to that given in parameter
			//Undefined cells set to "none"
		}
	}
}
