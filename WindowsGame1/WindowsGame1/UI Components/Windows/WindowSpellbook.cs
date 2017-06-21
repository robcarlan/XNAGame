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
	class WindowSpellbook : WindowBase 
	{
        public const int WINDOW_WIDTH = 600;
        public const int WINDOW_HEIGHT = 800;
        public const int ABILITY_REGION_X = 50;
        public const int ABILITY_REGION_Y = 50;
        public const int ABILTIY_REGION_WIDTH = 500;
        public const int ABILITY_REGION_HEIGHT = 700;
        WindowItemView abilityRegion;
        int selectedAbility = 0;

        public WindowSpellbook(Texture2D _Texture, Vector2 _origin, Vector2 _size, SpriteList _windowSprites,
            Point _mousePos, Rectangle _screen, List<WindowItemTooltip> _items, SpriteFont _font )
            : base(_Texture, _origin, new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT), _windowSprites, _mousePos, _screen, _font, "Spellbook" )
        {
            abilityRegion = new WindowItemView((int)(origin.X + ABILITY_REGION_X), (int)(origin.Y + ABILITY_REGION_Y),
                ABILTIY_REGION_WIDTH, ABILITY_REGION_HEIGHT, "reg", 16, 16, 2f, _items, 4, ABILTIY_REGION_WIDTH, 60 );
        }

        public override void draw(ref SpriteBatch sprite)
        {
            base.draw(ref sprite);

            //Draw regions
            drawItemView(ref sprite, ref abilityRegion);
        }

        protected override Point onMove(Point movementDifference)
        {
            Point totalDifference = base.onMove(movementDifference);

            abilityRegion.updatePosition(totalDifference);

            return totalDifference;
        }

        protected override void onUpdate(Point mousePos, ref ButtonState mPrimary, ref bool windowActive)
        {

            if (abilityRegion.ScrollRegion.Contains(mousePos))
            {

                if ( mPrimary == ButtonState.Pressed )
                {
                    if (abilityRegion.ScrollBar.Contains(mousePos))
                    {
                        abilityRegion.dragging = true;
                       
                    }
                    else abilityRegion.dragging = false;

                    if (abilityRegion.ScrollDownRec.Contains(mousePos))
                    {
                        abilityRegion.scrlDownPressed = true;
                    }

                    if (abilityRegion.ScrollUpRec.Contains(mousePos))
                    {
                        abilityRegion.scrlUpPressed = true;
                    }
                }

            }
            //Scroll event may have occured
            if (mPrimary == ButtonState.Released)
            {
                if (prev_mPrimary == ButtonState.Pressed)
                {
                    //Mouse has just been released

                    if (abilityRegion.ScrollDownRec.Contains(mousePos))
                    {
                        abilityRegion.changeScrollbarPosition(1);
                    }

                    if (abilityRegion.ScrollUpRec.Contains(mousePos))
                    {
                        abilityRegion.changeScrollbarPosition(-1);
                    }

                }

                abilityRegion.scrlDownPressed = false;
                abilityRegion.scrlUpPressed = false;
            }

            if (abilityRegion.totalRegion.Contains(mousePos))
            {
                //Mouse inside ability region

                for (short i = 0; i < abilityRegion.shownItems.Count; i++)
                {
                    if (abilityRegion.shownItems[i].totalRegion.Contains(mousePos))
                    {
                        if (mPrimary == ButtonState.Released && prev_mPrimary == ButtonState.Pressed)
                        {
                            if (abilityRegion.shownItems[i].clicked == true)
                            {
                                abilityRegion.shownItems[i].highlighted = true;
                                abilityRegion.selectedItem = i + abilityRegion.currentItemNumber;
                            }
                        }
                        else if (mPrimary == ButtonState.Pressed)
                            abilityRegion.shownItems[i].clicked = true;

                        if (abilityRegion.shownItems[i].iconSpace.Contains(mousePos))
                        {
                            //Create Tooltip

                        }
                    }
                }

            }

            base.onUpdate(mousePos, ref mPrimary, ref windowActive);
        }

	}

	/* Gives a list to the user of all spells. The user can drag the icons on to their own cast bar to set them as a spell,
	 * or cast them from the book directly. The book has multiple tabs, such as general spells, or combat spells
	 * 
	 */

}
