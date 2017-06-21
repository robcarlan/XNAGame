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
	public class WindowBase
	{

		protected Vector2 origin;
        protected Vector2 size;
        protected SpriteList windowSprites;
        protected Texture2D windowTex;
        protected SpriteFont font;

        protected Point currentMousePos;
        protected Point prevMousePos;
        protected ButtonState prev_mPrimary;

        protected bool closeButtonClicked;

        public WindowBar topBar; //suff = name
        //Can be replaced by controls
        protected Rectangle dragRegion; //bar.area
        protected Rectangle closeButtonRegion;

        protected const float itemScale = 1f;
        protected const float BGSpriteScale = 2f;
        protected readonly Vector2 titleFontScale;
        protected Vector2 rectScale, nameBoxScale;
        protected Rectangle Screen;

        protected Vector2 nameBoxL, nameBoxM, nameBoxR, closeRegion;
        protected Vector2 nameBoxMscale;
        protected WindowRegion backgroundRegion;
        protected WindowRegion selectedItemRegion;

        public UI_Components.Controls.ControlCollection controls;
        public List<String> debug;

        /// <summary>
        /// Creates a new instance of a window class.
        /// </summary>
        /// <param name="windowTex">A texture containing the graphics for a window, and it's components.</param>
        /// <param name="_origin">The starting point for the window. </param>
        /// <param name="_size">The size of the window.</param>
        /// <param name="_windowSprites">The sprites for the window</param>
        public WindowBase(Texture2D _windowTex, Vector2 _origin, Vector2 _size, SpriteList _windowSprites,
            Point _mousePos, Rectangle _Screen, SpriteFont _font, string WindowName )
        {
            Vector2 tempScale = new Vector2(itemScale, itemScale);
            titleFontScale = new Vector2(1.4f, 1.4f);

            font = _font;
            windowTex = _windowTex;
            origin = _origin;
            size = _size;
            windowSprites = _windowSprites;
            Screen = _Screen;

            //Set region values

            //Set default values
            debug = new List<string>();
            currentMousePos = _mousePos;
            prev_mPrimary = ButtonState.Released;
            closeButtonClicked = false;
            setDrawValues();
            backgroundRegion = new WindowRegion((int)origin.X, (int)origin.Y, (int)size.X, (int)size.Y, "w", 
                _windowSprites.list["wTLCnr"][0].Width, _windowSprites.list["wTLCnr"][0].Height, BGSpriteScale);

            topBar = new WindowBar(origin, new Vector2(size.X - (windowSprites.list["wCloseButton"][0].Width * itemScale),
                (int)(windowSprites.list["wTLDrag"][0].Height * nameBoxScale.Y)), "Name", nameBoxScale);
            topBar.addText(new Text(WindowName, topBar, titleFontScale, Color.White, font, 1f, true));

            controls = new Controls.ControlCollection(_windowTex, font, windowSprites);
            //Add text from windowbar to control collection

            //Close button control
            controls.addControl(
                new WindowButton(new Vector2((int)origin.X, origin.Y),
                    new Vector2(origin.X + size.X - (int)(windowSprites.list["wCloseButton"][0].Width), origin.Y),
                    (int)(windowSprites.list["wCloseButton"][0].Height), (int)(windowSprites.list["wCloseButton"][0].Width),
                    tempScale, "wCloseButton", false), 0);
            controls.addText(topBar.text, 0);
        }

        public WindowBase()
        {
        }

        /// <summary>
        /// Returns all debug text in a list of strings. All collected messages are destroyed after calling this function
        /// </summary>
        /// <returns></returns>
        public List<string> getDebugText()
        {

            //Get debug text from controls
            debug.AddRange(controls.debug);
            controls.debug.Clear();
            List<string> temp = new List<string>(debug);
            debug.Clear();
            return temp;
        }

        private void setDrawValues()
        {
            nameBoxScale.X = 1f;
            nameBoxScale.Y = 1f;

            closeButtonRegion.Width = closeButtonRegion.Height = (int)(windowSprites.list["wCloseButton"][0].Width * itemScale);
            closeButtonRegion.Y = (int)origin.Y;
            closeButtonRegion.X = (int)(origin.X + size.X - (closeButtonRegion.Width));
            closeRegion.X = closeButtonRegion.X;
            closeRegion.Y = closeButtonRegion.Y;

            //Set the scales
            rectScale.X = size.X / windowSprites.list["wBackCtr"][0].Width;
            rectScale.Y = size.Y / windowSprites.list["wBackCtr"][0].Height;

            dragRegion.X = (int)(origin.X);
            dragRegion.Y = (int)(origin.Y);
            dragRegion.Height = (int)(windowSprites.list["wTLDrag"][0].Height * nameBoxScale.Y);
            dragRegion.Width = (int)(size.X - closeButtonRegion.Width);

            prevMousePos.X = (int)origin.X;
            prevMousePos.Y = (int)origin.Y;

            nameBoxL.Y = nameBoxM.Y = nameBoxR.Y = origin.Y;
			nameBoxL.X = origin.X + (int)(windowSprites.list["wTLDrag"][0].Width * 1);
            nameBoxM.X = nameBoxL.X + (int)(windowSprites.list["wLName"][0].Width * nameBoxScale.X);
            nameBoxR.X = closeButtonRegion.X - (int)(windowSprites.list["wRName"][0].Width * nameBoxScale.X );
            nameBoxMscale.X = (nameBoxR.X - nameBoxM.X) / (int)(windowSprites.list["wMName"][0].Width);
            nameBoxMscale.Y = nameBoxScale.Y;
        }

        public virtual bool containsMouse(ref Vector2 mousePos)
        {
            Rectangle window = new Rectangle((int)origin.X, (int)origin.Y, (int)size.X, (int)size.Y);
            if ( window.Contains((int)mousePos.X, (int)mousePos.Y))
                return true;
            else return false;
        }

         /// <summary>
         /// Updates the window, called on a frame by frame basis
         /// </summary>
         /// <param name="mousePos"></param>
         /// <param name="mPrimary"></param>
        protected virtual void onUpdate(Point mousePos, ref ButtonState mPrimary, ref bool windowActive )
        {

            if (mPrimary == ButtonState.Pressed)
            {
                if (prev_mPrimary == ButtonState.Pressed)
                {
                    //Mouse has been held
                    if (topBar.absoluteArea.Contains(currentMousePos))
                    {
                        //Get movement difference, move object
                        Point movementDifference = new Point(mousePos.X - currentMousePos.X, mousePos.Y - currentMousePos.Y);
                        onMove(movementDifference);
                    }
                }
                else
                {
                    //Mouse has just been clicked6
                    if (closeButtonRegion.Contains(mousePos))
                    {
                        closeButtonClicked = true;
                    }
                }
            }
            else
            {   //Mouse has been released
                if (closeButtonRegion.Contains(mousePos))
                {
                    if (closeButtonClicked)
                    {
                        //Delete the window
                        windowActive = false;
                    }
                }
                else
                {
                    closeButtonClicked = false; //Mouse no longer hovering over close button
                }
            }

        }

        public void Update(Point mousePos, ButtonState mPrimary, ref bool windowActive)
        {
            onUpdate(mousePos, ref mPrimary, ref windowActive);

            //Update mouse values
            currentMousePos = mousePos;
            prev_mPrimary = mPrimary;
        }

        //Alter the position of each element
        protected virtual Point onMove( Point movementDifference )
        {
            Point totalDifference = new Point
                ( 
                (int) (MathHelper.Clamp(movementDifference.X + origin.X, 0, Screen.Width - 20)  - origin.X),
                (int) (MathHelper.Clamp(movementDifference.Y + origin.Y, 0, Screen.Height - 20) - origin.Y)
                );

            origin.X += totalDifference.X;
            origin.Y += totalDifference.Y;
            
            closeButtonRegion.X += totalDifference.X;
            closeButtonRegion.Y += totalDifference.Y;
            closeRegion.X += totalDifference.X;
            closeRegion.Y += totalDifference.Y;

            topBar.incrementPositions(totalDifference);
            backgroundRegion.updatePosition(totalDifference);

            controls.onMove(totalDifference);

            return totalDifference;
        }

        /// <summary>
        /// Draws the window.
        /// </summary>
        /// <param name="sprite">The spritebatch object.</param>
		public virtual void draw(ref SpriteBatch sprite)
		{
            //Draw regions / bars
            drawRegion(ref sprite, ref backgroundRegion);
            drawBar(ref sprite, topBar);

            sprite.End();

            controls.drawControls(sprite);
            sprite.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            //sprite.DrawString(font, "fdsghdfhfdfh", Vector2.Zero, Color.AliceBlue);
            //Draw the close button
            sprite.Draw(windowTex, closeRegion, 
                windowSprites.list["wCloseButton"][( closeButtonClicked ? 0 : 1 )], Color.White, 0f, Vector2.Zero, itemScale, SpriteEffects.None, 0f);
		}

        public void updateScreenPos(Rectangle newScreen)
        {
            Screen = newScreen;
        }

        protected void drawRegion(ref SpriteBatch sprite, ref WindowRegion region)
        {
            sprite.Draw(windowTex, region.bottomLeft, windowSprites.list[region.namePrefix + consts.bottomLeft][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.bottomRight, windowSprites.list[region.namePrefix + consts.bottomRight][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.topLeft, windowSprites.list[ region.namePrefix + consts.topLeft ][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.topRight, windowSprites.list[region.namePrefix + consts.topRight ][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.top, windowSprites.list[region.namePrefix + consts.top][0], Color.White, 0f, Vector2.Zero, region.horizontalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.bottom, windowSprites.list[region.namePrefix + consts.bottom][0], Color.White, 0f, Vector2.Zero, region.horizontalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.left, windowSprites.list[region.namePrefix + consts.left][0], Color.White, 0f, Vector2.Zero, region.verticalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.right, windowSprites.list[region.namePrefix + consts.right][0], Color.White, 0f, Vector2.Zero, region.verticalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.fill, windowSprites.list[region.namePrefix + consts.fill][0], Color.White, 0f, Vector2.Zero, region.fillScale, SpriteEffects.None, 0f);
        }

        protected void drawRegion(ref SpriteBatch sprite, WindowRegion region)
        {
            sprite.Draw(windowTex, region.bottomLeft, windowSprites.list[region.namePrefix + consts.bottomLeft][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.bottomRight, windowSprites.list[region.namePrefix + consts.bottomRight][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.topLeft, windowSprites.list[region.namePrefix + consts.topLeft][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.topRight, windowSprites.list[region.namePrefix + consts.topRight][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.top, windowSprites.list[region.namePrefix + consts.top][0], Color.White, 0f, Vector2.Zero, region.horizontalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.bottom, windowSprites.list[region.namePrefix + consts.bottom][0], Color.White, 0f, Vector2.Zero, region.horizontalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.left, windowSprites.list[region.namePrefix + consts.left][0], Color.White, 0f, Vector2.Zero, region.verticalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.right, windowSprites.list[region.namePrefix + consts.right][0], Color.White, 0f, Vector2.Zero, region.verticalScale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.fill, windowSprites.list[region.namePrefix + consts.fill][0], Color.White, 0f, Vector2.Zero, region.fillScale, SpriteEffects.None, 0f);
        }

        protected void drawItemView(ref SpriteBatch sprite, ref WindowItemView region)
        {

            drawRegion(ref sprite, (WindowRegion)region);

            //Draw the scroll bar
            //Draw back
            sprite.Draw(windowTex, region.right, windowSprites.list[consts.scrollPrefix + "Back"][0], Color.White, 0f, Vector2.Zero, region.verticalScale, SpriteEffects.None, 0f);
    
            //The scroll click buttons
            sprite.Draw(windowTex, region.ScrollDown, windowSprites.list[consts.scrollPrefix + "Down"][region.scrlDownPressed ? 1 : 0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.ScrollUp, windowSprites.list[consts.scrollPrefix + "Up"][region.scrlUpPressed ? 1 : 0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
    
            //The scroll bar
            sprite.Draw(windowTex, region.ScrollBarTop, windowSprites.list[consts.scrollPrefix + "BarTop"][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.ScrollBarBottom, windowSprites.list[consts.scrollPrefix + "BarBottom"][0], Color.White, 0f, Vector2.Zero, region.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, region.ScrollBarMid, windowSprites.list[consts.scrollPrefix + "BarMid"][0], Color.White, 0f, Vector2.Zero, region.ScrollBarScale, SpriteEffects.None, 0f);

            //Draw selected item region
            if (region.items != null)
            {
                if (region.selectedItem - region.currentItemNumber > -1 && region.selectedItem < region.currentItemNumber + region.maxVisibleItems)
                    drawRegion(ref sprite, region.shownItems[region.selectedItem - region.currentItemNumber]);

                for (short i = 0; i < region.shownItems.Count; i++)
                {
                    drawItem(ref sprite, region.shownItems[i]);
                }
            }
        }

        protected void drawItem(ref SpriteBatch sprite, WindowItemRegion region )
        {


            //Draw text and icon
            sprite.Draw(windowTex, region.IconPos, windowSprites.list["iconBlank"][0], Color.White, 0f, Vector2.Zero, consts.abilityIconScale, SpriteEffects.None, 0f);

            if (region.Name != string.Empty)
                consts.drawOutlinedText(region.Name, 1f, region.nameDrawPos, ref sprite, consts.NAME_COLOUR, 0.7f, ref font);

            if (region.infoLine != string.Empty)
                consts.drawOutlinedText(region.infoLine, 1f, region.infoLinePos, ref sprite, consts.DATA_COLOUR, 0.5f, ref font);
        }

        protected void drawBar(ref SpriteBatch sprite, WindowBar bar)
        {
            //Draw the drag region
            sprite.Draw(windowTex, topBar.barL, windowSprites.list["wL" + topBar.spriteSuffix][0], Color.White, 0f, Vector2.Zero, topBar.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, topBar.barR, windowSprites.list["wR" + topBar.spriteSuffix][0], Color.White, 0f, Vector2.Zero, topBar.scale, SpriteEffects.None, 0f);
            sprite.Draw(windowTex, topBar.barM, windowSprites.list["wM" + topBar.spriteSuffix][0], Color.White, 0f, Vector2.Zero, topBar.barMScale, SpriteEffects.None, 0f);
        }

        ~WindowBase()
        {

        }
	}
}
