using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.UI_Components.Windows
{
	class WindowCharacterStatus : WindowBase
	{
        const int WINDOW_HEIGHT = 800;
        const int WINDOW_WIDTH = 1000;
        const int WINDOW_X = 200;
        const int WINDOW_Y = 100;

        const int charViewRegionX = 20;
        const int charViewRegionY = 40;
        const int charViewRegionWidth = 600;
        const int charViewRegionHeight = 350;

        const int primaryStatsRegionX = charViewRegionWidth + charViewRegionX + 15;
        const int primaryStatsRegionY = charViewRegionY;
        const int primaryStatsRegionWidth = 350;
        const int primaryStatsRegionHeight = 350;

        const int secondaryStatsRegionX = charViewRegionX;
        const int secondaryStatsRegionY = 400;
        const int secondaryStatsRegionWidth = 500;
        const int secondaryStatsRegionHeight = 350;

        public WindowRegion charViewRegion;
        public WindowItemView primaryStats;
        public WindowItemView skills;
        public WindowItemView secondaryStats;

        public WindowCharacterStatus(Texture2D _Texture, SpriteListSimple _windowSprites,
            Point _mousePos, Rectangle _screen, Player _player, List<WindowItemTooltip> _items, SpriteFont _font)
            : base(_Texture, new Vector2(WINDOW_X, WINDOW_Y), new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), _windowSprites, _mousePos, _screen, _font, "Character Status")
        {
            charViewRegion = new WindowRegion((int)(origin.X + charViewRegionX), (int)(origin.Y + charViewRegionY), charViewRegionWidth, charViewRegionHeight,
                AnimNameUIWin.menu, 16, 16, 2f);

            primaryStats = new WindowItemView((int)(origin.X + primaryStatsRegionX), (int)(origin.Y + primaryStatsRegionY), primaryStatsRegionWidth, primaryStatsRegionHeight,
                AnimNameUIWin.menu, 16, 16, 2f, _items, 4, 250, 50);

            secondaryStats = new WindowItemView((int)(origin.X + secondaryStatsRegionX), (int)(origin.Y + secondaryStatsRegionY), secondaryStatsRegionWidth, secondaryStatsRegionHeight,
				AnimNameUIWin.menu, 16, 16, 2f, _items, 4, 250, 50);

        }

        public override void draw(ref Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            base.draw(ref sprite);

            drawItemView(ref sprite, ref secondaryStats);
            drawItemView(ref sprite, ref primaryStats);
        //    drawItemView(ref sprite, ref skills);
            drawRegion(ref sprite, ref charViewRegion);
        }

        protected override Point onMove(Point movementDifference)
        {
            Point totalDifference =  base.onMove(movementDifference);
            charViewRegion.updatePosition(totalDifference);
            primaryStats.updatePosition(totalDifference);
            secondaryStats.updatePosition(totalDifference);

            return totalDifference;
        }
	}

	//Shows the characters stats and equipment. Equipment can be removed from here
}
