using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Entities {
	//Each tile should have a maximum of two tile textures associated with it, so tiles can be easily combined.

	/// <summary>
	/// We have a custom class for tile because it is faster and efficient
	/// </summary>
	public class Tile : Entity {
		public int fgID;
		public byte fgRotation;
		public int bgID;
		public byte bgRotation;
		public int fgFramePosX;
		public int fgFramePosMax;
		public int bgFramePosX;
		public int bgFramePosMax;

		const int noTileID = 0;

		public void setValues(int ID, int frameMax) {
			fgID = ID;
			fgFramePosMax = frameMax;
			fgFramePosX = 0;
			setBackgroundNoID();
		}

		public void advanceFrames() {
			if (fgFramePosX >= fgFramePosMax)
				fgFramePosX = 0;
			else fgFramePosX++;

			if (bgFramePosX >= bgFramePosMax)
				bgFramePosX = 0;
			else bgFramePosX++;
		}

		private void setBackgroundNoID() {
			bgID = noTileID;
			bgFramePosMax = 0;
			bgFramePosX = 0;
		}

		public Tile(int ID, int frameMax) {
			fgID = ID;
			fgFramePosMax = frameMax;
			fgFramePosX = 0;
			fgRotation = 0;
			setBackgroundNoID();
		}

		public Tile(int ID, int frameMax, byte rotation) {
			fgID = ID;
			fgFramePosMax = frameMax;
			fgFramePosX = 0;
			fgRotation = rotation;
			setBackgroundNoID();
		}

		public Tile(int fgID, int fgFrameMax, byte fgRot,
			int bgID, int bgFrameMax, byte bgRot) {
			this.fgID = fgID;
			this.fgFramePosMax = fgFrameMax;
			this.fgFramePosX = 0;
			this.fgRotation = fgRot;

			this.bgID = bgID;
			this.bgFramePosMax = bgFrameMax;
			this.bgFramePosX = 0;
			this.bgRotation = bgRot;
		}
	}
}
