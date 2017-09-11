using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
	public static class AnimName
	{
		public static int mainChar = 0;
	}

	public static class AnimID
	{
		public static int Static = 0;
		public static int walkNorth = 1;
		public static int walkSouth = 2;
		public static int walkEast = 3;
		public static int walkWest = 4;

	}

	public static class AnimNameUI
	{
		public const int healthFill = 10;
		public const int healthBackground = 11;
		public const int manaFill = 12;
		public const int manaBackground = 13;
		public const int bottomRightIcons = 14;
		public const int bottomRightIconClickOverlay = 15;

		//constants
		public const int cursorStandard = 0;
		public const int cursorTarget = 1;
		public const int cursorTalk = 2;
		public const int cursorAttack = 3;
		public const int cursorShop = 4;

		//tooltip
		public const int tooltip = 20;

	}

	public static class AnimNameUIWin
	{
		//Unique
		public static int mapSprite = 10;
		public static int mapText = 11;
		public static int CloseButton = 12;
		public static int dragRegion = 13;
		public static int nameBoxLeft = 70;
		public static int nameBoxRight = 71;
		public static int nameBoxMid = 72;


		//Window regions
        public const int topLeft = 0;
        public const int topRight = 1;
        public const int bottomLeft = 2;
        public const int bottomRight = 3;
        public const int left = 4;
        public const int right = 5;
        public const int bottom = 6;
        public const int top = 7;
        public const int fill = 8;

		//Scroll
		public const int scrollBarTop = 19;
		public const int scrollBarMid = 20;
		public const int scrollBarBottom = 21;
		public const int scrollClickTop = 22;
		public const int scrollClickBottom = 23;
		public const int scrollBackground = 24;

		//window Style Prefixes
		public const int generic = 30;
		public const int map = 40;
		public const int select = 50;
		public const int menu = 60;

		//WindowBar prefixes
		public static int barLeft = 0;
		public static int barMid = 1;
		public static int barRight = 2;
		public const int drag = 70;

		//Buttons
		public static int blankIcon = 100;
	}

	public static class IconID
	{
		public static int defaultIcon = 0;
		public static int Bleed = 1;
		public static int FortifyDefense = 2;
	}

}
