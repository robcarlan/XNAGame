using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsGame1.Utility;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;


namespace WindowsGame1.Debug
{
	public partial class navmeshForm : Form
	{
		public Game1 game;
		public bool skipEvent = false;
		public bool isAwaitingInput = false;
		public bool isGettingConnected = false;
		public int verticeInput = -1; //Vertice that the input is obtaining

		public int selectedID = -1;
		public int selectedIndex = -1;
		public NavMeshCell selectedCell = null;

		//Contains the altered data. This can be discarded or saved, in which case the changes will take effect in the game.
		public Navmesh data;
		public List<int> freeID;
		public const int maxMeshCount = 1000;

		public navmeshForm(Game1 game)
		{
			game.drawNavmeshDebug = true;
			this.game = game;
			game.drawNavmeshDebug = true;
			InitializeComponent();

			data = game.ObjectManager.navmesh;

			freeID = new List<int>();
			for (int i = 0; i < maxMeshCount; i++)
			{
				freeID.Insert(0, i);
			}

			listPolys();
		}


		public void listPolys()
		{
			//Clear previous items
			lstPolys.Items.Clear();

			foreach (NavMeshCell cell in data.cells.Values)
			{
				string[] items = new string[3];
				items[0] = cell.polyID.ToString();
				items[1] = cell.polyID.ToString();
				items[2] = cell.polyID.ToString();
				lstPolys.Items.Add(new ListViewItem(items));
			}

			lstPolys.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		public void saveData()
		{
			//Alert if there are any incomplete polygons. Only save if this is not the case
			connectPolygons();

			//Calcualte which of each vertice is tl / tr ... and save them in their correct order
			foreach (NavMeshCell poly in data.cells.Values)
			{
				if (poly.getValidVerts() < 4)
				{	
					//Do some error stuff
					System.Windows.Forms.MessageBox.Show("Polygon " + poly.polyID + " is unfinished! Didn't save");
					return;
				}
			}

			Dictionary<Point, Queue<NavMeshCell>> cellCtr = new Dictionary<Point, Queue<NavMeshCell>>();
			//Recreate the list of cell data
			data.cellNavmesh = new Dictionary<Point,List<int>>();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			
			//Create a list of which cells need to be serialised to where!
			foreach (NavMeshCell poly in data.cells.Values)
			{
				if (!cellCtr.ContainsKey(poly.TLpos.cellPos))
				{
					cellCtr.Add(poly.TLpos.cellPos, new Queue<NavMeshCell>());
					data.cellNavmesh.Add(poly.TLpos.cellPos, new List<int>());
				}
				cellCtr[poly.TLpos.cellPos].Enqueue(poly);
				data.cellNavmesh[poly.TLpos.cellPos].Add(poly.polyID);
			}

			foreach (KeyValuePair<Point,Queue<NavMeshCell>> items in cellCtr)
			{
				Queue<NavMeshCell> serQueue = items.Value;
				Point key = items.Key;
				NavMeshCell[] toSerialise = new NavMeshCell[serQueue.Count];
				int itemItr = 0;
				while (serQueue.Count > 0)
				{
					toSerialise[itemItr] = serQueue.Dequeue();
					itemItr++;
				}

				//Serialise this data
				string filePath = Declaration.navmeshDataFolder + key.X + " " + key.Y + ".xml";
				using (XmlWriter writer = XmlWriter.Create(filePath, settings))
				{
					IntermediateSerializer.Serialize(writer, toSerialise, null);
				}
			}

			//Apply to game data
			applyNavmesh();

		}

		public void applyNavmesh()
		{
			//Apply list to game
			//only apply changes to loaded cells

			foreach (KeyValuePair<Point, List<int>> itr in data.cellNavmesh)
			{
				foreach (int cellID in itr.Value)
				{
					data.cells[cellID].calculateLocalSpace(game.camera);
				}
			}

			//Process the cells
		}

		private void resetIDs()
		{
			freeID.Clear();
			for (int i = 0; i < 1000; i++)
			{
				freeID.Insert(0, i);
			}
		}

		private void cmdRefresh_Click(object sender, EventArgs e)
		{
			data = game.ObjectManager.navmesh;
			selectedCell = null;
			selectedID = -1;
			selectedIndex = -1;
			listPolys();
			resetIDs();

			foreach (NavMeshCell poly in data.cells.Values)
			{
				freeID.Remove(poly.polyID);
				game.createPolygonPrimitive(poly);
			}
		}

		//public bool isConnected(NavMeshCell first, NavMeshCell second)
		//{
		//    //Determine whether two cells are connected
		//    float[] firstAngles = new float[4];
		//    float[] secondAngles = new float[4];

		//    //Calculate two arrays of gradients

		//    //Compare angles of gradients

		//    //If angle is within threshold, check to see if they form a pair
			

		//    return false;
		//}

		public void connectPolygons()
		{
			//Go through each polygon, and find any other polygons that are connected to it
			//give both objects a reference to the connected polygon
			//foreach (NavMeshCell poly in data.cells.Values)
			//{
			//    getConnectedPolys(poly);
			//}
		}

		//public void getConnectedPolys(NavMeshCell testPoly)
		//{
		//    //Remove previous connections, as all cells will need to be tested anyway!
		//    testPoly.connectedCells.Clear();
		//    foreach (KeyValuePair<int, NavMeshCell> pair in data.cells)
		//    {
		//        int key = pair.Key;
		//        NavMeshCell cellData = pair.Value;

		//        int xDif = Math.Abs(cellData.TLpos.cellPos.X - testPoly.TLpos.cellPos.X);
		//        int yDif = Math.Abs(cellData.TLpos.cellPos.Y - testPoly.TLpos.cellPos.Y);
		//        //Skip polygon if it is too far away
		//        if (xDif > 2 || yDif > 2) continue;

		//        if (isConnected(testPoly, cellData))
		//        {
		//            testPoly.connectedCells.Add(cellData.polyID);
		//            if (!cellData.connectedCells.Contains(key))
		//                cellData.connectedCells.Add(key);
		//        }
		//    }
		//}

		//On position change, connected polys will need to be retested!!!
		public void onPositionChange(NavMeshCell poly)
		{
			//Get connected polygons for this cell

			//Remove references to this cell
			//foreach (int connectedCellID in poly.connectedCells)
			//{
			//    data.cells[connectedCellID].connectedCells.Remove(connectedCellID);
			//}

			////Re gather the references
			//getConnectedPolys(poly);
		}

		private void lstPolys_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Select current poly
			if (lstPolys.SelectedIndices.Count == 1)
			{
				selectedIndex = lstPolys.SelectedIndices[0];
				selectPoly(selectedIndex);
			}

		}

		public void selectPoly(int index)
		{

			selectedID = Convert.ToInt16(lstPolys.Items[index].SubItems[0].Text);
			selectedCell = data.cells[selectedID];
		
			//List connected poly's
			lstConnectedPolys.Items.Clear();
			for (int i = 0; i < selectedCell.connectedCells.Count; i++)
			{
				lstConnectedPolys.Items.Add(selectedCell.connectedCells[i]);
			}

			skipEvent = true;

			//List vertice data
			txtCellX1.Text = selectedCell.TLpos.cellPos.X.ToString();
			txtCellY1.Text = selectedCell.TLpos.cellPos.Y.ToString();
			txtTileX1.Text = selectedCell.TLpos.tilePos.X.ToString();
			txtTileY1.Text = selectedCell.TLpos.tilePos.Y.ToString();
			txtOffsetX1.Text = selectedCell.TLpos.Offset.X.ToString();
			txtOffsetY1.Text = selectedCell.TLpos.Offset.Y.ToString();

			txtCellX2.Text = selectedCell.TRpos.cellPos.X.ToString();
			txtCellY2.Text = selectedCell.TRpos.cellPos.Y.ToString();
			txtTileX2.Text = selectedCell.TRpos.tilePos.X.ToString();
			txtTileY2.Text = selectedCell.TRpos.tilePos.Y.ToString();
			txtOffsetX2.Text = selectedCell.TRpos.Offset.X.ToString();
			txtOffsetY2.Text = selectedCell.TRpos.Offset.Y.ToString();

			txtCellX3.Text = selectedCell.BLpos.cellPos.X.ToString();
			txtCellY3.Text = selectedCell.BLpos.cellPos.Y.ToString();
			txtTileX3.Text = selectedCell.BLpos.tilePos.X.ToString();
			txtTileY3.Text = selectedCell.BLpos.tilePos.Y.ToString();
			txtOffsetX3.Text = selectedCell.BLpos.Offset.X.ToString();
			txtOffsetY3.Text = selectedCell.BLpos.Offset.Y.ToString();

			txtCellX4.Text = selectedCell.BRpos.cellPos.X.ToString();
			txtCellY4.Text = selectedCell.BRpos.cellPos.Y.ToString();
			txtTileX4.Text = selectedCell.BRpos.tilePos.X.ToString();
			txtTileY4.Text = selectedCell.BRpos.tilePos.Y.ToString();
			txtOffsetX4.Text = selectedCell.BRpos.Offset.X.ToString();
			txtOffsetY4.Text = selectedCell.BRpos.Offset.Y.ToString();

			skipEvent = false;

			updatePolygonColours();
		}

		public void removePoly(int index)
		{
			//Remove connected polys
			for (int i = 0; i < selectedCell.connectedCells.Count; i++)
			{
				int connectedID = selectedCell.connectedCells[i];
				data.cells[connectedID].connectedCells.Remove(selectedID);
			}

			data.cells.Remove(selectedID);
			lstConnectedPolys.Items.Clear();
			lstPolys.Items.RemoveAt(selectedIndex);
			if (game.navmeshVertices.ContainsKey(selectedID)) game.navmeshVertices.Remove(selectedID);
			updatePolygonColours();

			//Delete item, refresh list
			selectedID = -1;
			selectedIndex = -1;
			selectedCell = null;
		}

		public int addNewPoly()
		{
			NavMeshCell newCell = new Utility.NavMeshCell();
			newCell.polyID = freeID[0];
			freeID.RemoveAt(0);
			data.cells.Add(newCell.polyID, newCell);
			String[] items = new string[3];
			items[0] = newCell.polyID.ToString();
			items[1] = newCell.polyID.ToString();
			items[2] = newCell.polyID.ToString();
			ListViewItem newListItem = new ListViewItem(items);
			lstPolys.Items.Add(newListItem);
			return newCell.polyID;
		}

		//Snap to the position (midpoint) of the navmesh cell on the game. 
		private void cmdGotoPoly_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			Point temp = new Point(selectedCell.TLpos.cellPos.X * Functions.tilesPerSector + selectedCell.TLpos.tilePos.X,
				selectedCell.TLpos.cellPos.Y * Functions.tilesPerSector + selectedCell.TLpos.tilePos.Y);
			Vector2 offset = selectedCell.TLpos.Offset;
			game.camera.Pan(temp, offset, 0.0f);
		}

		private void cmdCreateNew_Click(object sender, EventArgs e)
		{
			addNewPoly();
		}

		private void cmdRemovePoly_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			removePoly(selectedIndex);
		}

		//Select a position from the game client. Return to the dialog once this is chosen
		private void cmdSelectVert1_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			getPositionInput(1);
		}

		private void cmdSelectVert2_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			getPositionInput(2);
		}

		private void cmdSelectVert3_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			getPositionInput(3);
		}

		private void cmdSelectVert4_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			getPositionInput(4);
		}

		private void cmdAddConnected_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			isAwaitingInput = true;
			isGettingConnected = true;
		}

		private void cmdRemoveConnected_Click(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
		}

		public void getPositionInput(int vertice)
		{
			//Focuses on game to obtain input. Debug form is hidden until input is recieved, or 
			//the operation is cancelled
			verticeInput = vertice;
			isAwaitingInput = true;	

			//Update text box whilst obtaining position
		}

		public void connectPolygon(int selectedPolyID)
		{
			//Called by input update. Connects the two polygons.
			if (selectedPolyID == -1)
			{
				isAwaitingInput = false;
				isGettingConnected = false;
				return;
			}

			if (!selectedCell.connectedCells.Contains(selectedPolyID))
			{
				selectedCell.connectedCells.Add(selectedPolyID);
				data.cells[selectedPolyID].connectedCells.Add(selectedID);
			}

			lstConnectedPolys.Items.Add(selectedPolyID);

			updatePolygonColours();
			isAwaitingInput = false;
			isGettingConnected = false;
		}

		public void updatePolygonColours()
		{
			foreach ( VertexPositionColor[] vert in game.navmeshVertices.Values)
			{
				vert[0].Color = vert[1].Color = vert[2].Color = vert[3].Color = Color.Blue;
			}

			foreach (int i in selectedCell.connectedCells)
			{
				if (game.navmeshVertices.ContainsKey(i))
				{
					VertexPositionColor[] vert = game.navmeshVertices[i];
					vert[0].Color = vert[1].Color = vert[2].Color = vert[3].Color = Color.Turquoise;
				}
			}

			if (game.navmeshVertices.ContainsKey(selectedID))
			{
				VertexPositionColor[] selectedVerts = game.navmeshVertices[selectedID];
				selectedVerts[0].Color = selectedVerts[1].Color = selectedVerts[2].Color = selectedVerts[3].Color = Color.Green;
			}
		}

		public void onPositionSet()
		{
			if (game.navmeshVertices.ContainsKey(selectedID))
			{
				game.navmeshVertices.Remove(selectedID);
				game.createPolygonPrimitive(selectedCell);
			}
			else
			{
				game.createPolygonPrimitive(selectedCell);
			}

			isAwaitingInput = false;
			verticeInput = 0;
		}

		public void setVerticePosition(Point _cell, Point _tile, Vector2 _offset)
		{
			//Called by game class when a position is selected.
			//Set vertice data to the data provided
			if (verticeInput == 1)
			{	//TL
				txtCellX1.Text = _cell.X.ToString();
				txtCellY1.Text = _cell.Y.ToString();
				txtTileX1.Text = _tile.X.ToString();
				txtTileY1.Text = _tile.Y.ToString();
				txtOffsetX1.Text = _offset.X.ToString();
				txtOffsetY1.Text = _offset.Y.ToString();
				selectedCell.TLpos.valid = true;
			}
			else if (verticeInput == 2)
			{	//TR
				txtCellX2.Text = _cell.X.ToString();
				txtCellY2.Text = _cell.Y.ToString();
				txtTileX2.Text = _tile.X.ToString();
				txtTileY2.Text = _tile.Y.ToString();
				txtOffsetX2.Text = _offset.X.ToString();
				txtOffsetY2.Text = _offset.Y.ToString();
				selectedCell.TRpos.valid = true;
			}
			else if (verticeInput == 3)
			{	//BL
				txtCellX3.Text = _cell.X.ToString();
				txtCellY3.Text = _cell.Y.ToString();
				txtTileX3.Text = _tile.X.ToString();
				txtTileY3.Text = _tile.Y.ToString();
				txtOffsetX3.Text = _offset.X.ToString();
				txtOffsetY3.Text = _offset.Y.ToString();
				selectedCell.BLpos.valid = true;
			}
			else if (verticeInput == 4)
			{	//BR
				txtCellX4.Text = _cell.X.ToString();
				txtCellY4.Text = _cell.Y.ToString();
				txtTileX4.Text = _tile.X.ToString();
				txtTileY4.Text = _tile.Y.ToString();
				txtOffsetX4.Text = _offset.X.ToString();
				txtOffsetY4.Text = _offset.Y.ToString();
				selectedCell.BRpos.valid = true;
			}
		}

		//Fine tuning of polys
		void isValidTL()
		{
			if (txtCellX1.Text == "" || txtCellY1.Text == ""
				|| txtTileX1.Text == "" || txtTileY1.Text == ""
				||	txtOffsetX1.Text == "" || txtOffsetY1.Text == "")
				selectedCell.TLpos.valid = false;
			else selectedCell.TLpos.valid = true;
		}

		void isValidTR()
		{
			if (txtCellX2.Text == "" || txtCellY2.Text == ""
				|| txtTileX2.Text == "" || txtTileY2.Text == ""
				|| txtOffsetX2.Text == "" || txtOffsetY2.Text == "")
				selectedCell.TRpos.valid = false;
			else selectedCell.TRpos.valid = true;
		}

		void isValidBL()
		{
			if (txtCellX3.Text == "" || txtCellY3.Text == ""
				|| txtTileX3.Text == "" || txtTileY3.Text == ""
				|| txtOffsetX3.Text == "" || txtOffsetY3.Text == "")
				selectedCell.BLpos.valid = false;
			else selectedCell.BLpos.valid = true;
		}

		void isValidBR()
		{
			if (txtCellX4.Text == "" || txtCellY4.Text == ""
				|| txtTileX4.Text == "" || txtTileY4.Text == ""
				|| txtOffsetX4.Text == "" || txtOffsetY4.Text == "")
				selectedCell.BRpos.valid = false;
			else selectedCell.BRpos.valid = true;
		}

		//Cells
		private void txtCellX1_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TLpos.cellPos.X = Convert.ToInt32(txtCellX1.Text);
			isValidTL();
		}

		private void txtCellX2_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TRpos.cellPos.X = Convert.ToInt32(txtCellX2.Text);
			isValidTR();
		}

		private void txtCellX3_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BLpos.cellPos.X = Convert.ToInt32(txtCellX3.Text);
			isValidBL();
		}

		private void txtCellX4_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BRpos.cellPos.X = Convert.ToInt32(txtCellX4.Text);
			isValidBR();
		}

		private void txtCellY1_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TLpos.cellPos.Y = Convert.ToInt32(txtCellY1.Text);
			isValidTL();
		}

		private void txtCellY2_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TRpos.cellPos.Y = Convert.ToInt32(txtCellY2.Text);
			isValidTR();
		}

		private void txtCellY3_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BLpos.cellPos.Y = Convert.ToInt32(txtCellY3.Text);
			isValidBL();
		}

		private void txtCellY4_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BRpos.cellPos.Y = Convert.ToInt32(txtCellY4.Text);
			isValidBR();
		}

		//Tiles
		private void txtTileX1_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TLpos.tilePos.X = Convert.ToInt32(txtTileX1.Text);
			isValidTL();
		}

		private void txtTileX2_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TRpos.tilePos.X = Convert.ToInt32(txtTileX2.Text);
			isValidTR();
		}

		private void txtTileX3_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BLpos.tilePos.X = Convert.ToInt32(txtTileX3.Text);
			isValidBL();
		}

		private void txtTileX4_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BRpos.tilePos.X = Convert.ToInt32(txtTileX4.Text);
			isValidBR();
		}

		private void txtTileY1_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TLpos.tilePos.Y = Convert.ToInt32(txtTileY1.Text);
			isValidTL();
		}

		private void txtTileY2_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TRpos.tilePos.Y = Convert.ToInt32(txtTileY2.Text);
			isValidTR();
		}

		private void txtTileY3_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BLpos.tilePos.Y = Convert.ToInt32(txtTileY3.Text);
			isValidBL();
		}

		private void txtTileY4_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BRpos.tilePos.Y = Convert.ToInt32(txtTileY4.Text);
			isValidBR();
		}

		//Offset
		private void txtOffsetX1_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TLpos.Offset.X = Convert.ToInt32(txtOffsetX1.Text);
			isValidTL();
		}

		private void txtOffsetX2_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TRpos.Offset.X = Convert.ToInt32(txtOffsetX2.Text);
			isValidTR();
		}

		private void txtOffsetX3_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BLpos.Offset.X = Convert.ToInt32(txtOffsetX3.Text);
			isValidBL();
		}

		private void txtOffsetX4_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BRpos.Offset.X = Convert.ToInt32(txtOffsetX4.Text);
			isValidBR();
		}

		private void txtOffsetY1_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TLpos.Offset.Y = Convert.ToInt32(txtOffsetY1.Text);
			isValidTL();
		}

		private void txtOffsetY2_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.TRpos.Offset.Y = Convert.ToInt32(txtOffsetY2.Text);
			isValidTR();
		}

		private void txtOffsetY3_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BLpos.Offset.Y = Convert.ToInt32(txtOffsetY3.Text);
			isValidBL();
		}

		private void txtOffsetY4_TextChanged(object sender, EventArgs e)
		{
			if (selectedIndex == -1 || skipEvent) return;
			selectedCell.BRpos.Offset.Y = Convert.ToInt32(txtOffsetY4.Text);
			isValidBR();
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			saveData();
		}
	}
}
