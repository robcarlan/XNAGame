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

namespace WindowsGame1.UI_Components.Windows
{
	public class WindowMap : WindowBase 
	{
        //Regions
        protected WindowRegion mapRegion;

        //Text 
        Vector2 mapTextOrigin;

        //The position on the texture
        bool draggingMap;

        Vector2 mapStartPosition;
        Rectangle mapDimensions;
        Rectangle mapWindowLocation;
        WindowItemView quests;
        const float mapScale = 1f;
        const float mapTextScale = 2.5f;
        const int MAP_WINDOW_WIDTH = 600;
        const int MAP_WINDOW_HEIGHT = 400;
        const int MAP_WINDOW_X = 120;
        const int MAP_WINDOW_Y = 60;
        const int WINDOW_HEIGHT = 600;
        const int WINDOW_WIDTH = 800;

        public WindowMap(Texture2D _Texture, Vector2 _origin, Vector2 _size, SpriteListSimple _windowSprites,
            Point _mousePos, Rectangle _screen, SpriteFont _font ) 
            : base(_Texture, _origin, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), _windowSprites, _mousePos, _screen, _font, "World Map" )
        {
            draggingMap = true;

            //Get the map Data
            mapDimensions.X = windowSprites.list[AnimNameUIWin.mapSprite][0].X;
            mapDimensions.Y = windowSprites.list[AnimNameUIWin.mapSprite][0].Y;

            //Set the maps position in the window
            mapWindowLocation.X = (int)(origin.X + (size.X - MAP_WINDOW_WIDTH) / 2 );
            mapWindowLocation.Y = (int)(origin.X + MAP_WINDOW_Y);
            mapDimensions.Width = (int)MathHelper.Clamp(MAP_WINDOW_WIDTH / mapScale, 0f, size.X / mapScale );
            mapDimensions.Height = (int)MathHelper.Clamp(MAP_WINDOW_HEIGHT  / mapScale, 0f, size.Y / mapScale );
            mapWindowLocation.Height = (int)MathHelper.Clamp(MAP_WINDOW_HEIGHT, 0f, size.Y);
            mapWindowLocation.Width = (int)MathHelper.Clamp(MAP_WINDOW_WIDTH, 0f, size.X);

            //Set regions
            mapRegion = new WindowRegion(mapWindowLocation.X, mapWindowLocation.Y, MAP_WINDOW_WIDTH, MAP_WINDOW_HEIGHT, AnimNameUIWin.map,
				 _windowSprites.list[AnimNameUIWin.map + AnimNameUIWin.topLeft][0].Width,
				 _windowSprites.list[AnimNameUIWin.map + AnimNameUIWin.topLeft][0].Height, 2f); 

            //Set text Position
            mapTextOrigin.X = mapWindowLocation.X + (MAP_WINDOW_WIDTH - _windowSprites.list[AnimNameUIWin.mapText][0].Width * mapTextScale) / 2;
			mapTextOrigin.Y = mapWindowLocation.Y - (_windowSprites.list[AnimNameUIWin.mapText][0].Height * mapTextScale);
        }


        protected override void onUpdate(Point mousePos, ref ButtonState mPrimary, ref bool windowActive)
        {
            
            if (mPrimary == ButtonState.Pressed)
            {
                if (prev_mPrimary == ButtonState.Pressed)
                {
                    //Mouse has been held
                    if (mapWindowLocation.Contains(currentMousePos) || draggingMap == true)
                    {
                        //Get movement difference, move object
                        Point movementDifference = new Point(mousePos.X - currentMousePos.X, mousePos.Y - currentMousePos.Y);

                        if (movementDifference.X != 0)
                            moveMap(movementDifference);

                        draggingMap = true;

                        return;
                    }
                }
            }
            else
            {
                //Not dragging
                draggingMap = false;
            }

            base.onUpdate(mousePos, ref mPrimary, ref windowActive);
        }

        /// <summary>
        /// Moves the map by a magnitude opposite to the mouse's new location. Only works when clicking the map.
        /// </summary>
        public void moveMap( Point mouseChange )
        {
            //Clamp the new values
            mapDimensions.X = (int)MathHelper.Clamp(mapDimensions.X - mouseChange.X, windowSprites.list[AnimNameUIWin.mapSprite][0].X,
				windowSprites.list[AnimNameUIWin.mapSprite][0].Width + windowSprites.list[AnimNameUIWin.mapSprite][0].X - mapWindowLocation.Width);
			mapDimensions.Y = (int)MathHelper.Clamp(mapDimensions.Y - mouseChange.Y, windowSprites.list[AnimNameUIWin.mapSprite][0].Y,
				windowSprites.list[AnimNameUIWin.mapSprite][0].Height + windowSprites.list[AnimNameUIWin.mapSprite][0].Y - mapWindowLocation.Height);
        }

        public override bool containsMouse(ref Vector2 mousePos)
        {
            return (base.containsMouse(ref mousePos) || draggingMap);
        }

        public override void draw(ref SpriteBatch sprite)
        {
            base.draw(ref sprite);

            Vector2 temp = new Vector2( 0f, 0f );

            //Draw the map
            temp.X = mapWindowLocation.X;
            temp.Y = mapWindowLocation.Y;
            sprite.Draw(windowTex, temp, mapDimensions, Color.White, 0f, Vector2.Zero, mapScale, SpriteEffects.None, 0f);

            //Draw regions
            drawRegion(ref sprite, ref mapRegion);

            //Draw Text
            sprite.Draw(windowTex, mapTextOrigin, windowSprites.list[AnimNameUIWin.mapText][0], Color.White, 0f, Vector2.Zero, mapTextScale, SpriteEffects.None, 0f);
        }

        protected override Point onMove( Point movementDifference )
        {
            Point totalDifference = base.onMove(movementDifference);

            mapWindowLocation.X += totalDifference.X;
            mapWindowLocation.Y += totalDifference.Y;
            mapTextOrigin.X += totalDifference.X;
            mapTextOrigin.Y += totalDifference.Y;

            mapRegion.updatePosition(totalDifference);

            return totalDifference;
        }
	}

	//Skyrim style map. A map in the middle, which can be dragged about. Shows objectives / player position / POI
	//Quests on the side, with explanation
}
