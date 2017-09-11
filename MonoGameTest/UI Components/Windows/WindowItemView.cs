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
    public class WindowItemView : WindowRegion
    {
        int totalY;
        int currentY;
        float pixelIncreasePerItem;
        public List<WindowItemTooltip> items;
        public List<WindowItemRegion> shownItems;
        public const int itemGap = 5;
        public int selectedItem = -1;
        public int currentItemNumber;
        public int maxItems;
        public int maxVisibleItems;

        public bool dragging;
        public bool scrlUpPressed;
        public bool scrlDownPressed;

        //Change these for a clickable item class
        public Rectangle ScrollBar;
        public Rectangle ScrollRegion;
        public Rectangle ScrollUpRec;
        public Rectangle ScrollDownRec;

        public Vector2 ScrollUp;
        public Vector2 ScrollDown;
        public Vector2 ScrollBarTop;
        public Vector2 ScrollBarMid;
        public Vector2 ScrollBarBottom;

        public Vector2 ScrollBarScale;
        short scrollBarTopDimensions;

        public WindowItemView(int _x, int _y, int _width, int _height, int _namePrefix, int _spriteX, int _spriteY, float _scale, 
            List<WindowItemTooltip> _items, short _scrollBarTopDimensions, int itemWidth, int itemHeight )
            : base(_x, _y, _width, _height, _namePrefix, _spriteX, _spriteY, _scale)
        {
            shownItems = new List<WindowItemRegion>();

            maxItems = _items.Count;
            items = _items;
            dragging = scrlDownPressed = scrlUpPressed = false;
            currentItemNumber = 0;
            currentY = 0;
            totalY = maxItems;

            //Calculate max number of visible items at once
            maxVisibleItems = (int)Math.Floor((double)(_height / (itemHeight + itemGap)));
            if (_items.Count < maxVisibleItems) maxVisibleItems = _items.Count;

            //Set scroll values
            scrollBarTopDimensions = _scrollBarTopDimensions;
            calculateScrollValues();

            /* To do : add multiple items along each row */

            //Set the values 
            for (short ItemNumber = 0; ItemNumber < maxVisibleItems; ItemNumber++)
            {
                shownItems.Add( new WindowItemRegion(
                    totalRegion.X + 8, totalRegion.Y + (ItemNumber * (itemHeight + itemGap)) + 8,
                    itemWidth - 16 - (int)(ScrollBarScale.X * spriteDimensions.X), itemHeight, AnimNameUIWin.select, 16, 16, 2f, items[ItemNumber].Name, items[ItemNumber].description[0], 0)
                    );
                        
            }

            shownItems[0].highlighted = true;
        }

        private void calculateScrollValues()
        {
            ScrollUp = topRight;
            ScrollDown = bottomRight;
            ScrollDownRec.Y = (int)ScrollDown.Y;

            //Get the total height of the scroll bar
            ScrollRegion.X = ScrollBar.X = ScrollDownRec.X = ScrollUpRec.X = (int)ScrollUp.X;
            ScrollRegion.Y = ScrollUpRec.Y = (int)ScrollUp.Y;
            ScrollBar.Y = (int)(ScrollRegion.Y + (scale * spriteDimensions.Y));
            pixelIncreasePerItem = totalRegion.Height / (int)MathHelper.Clamp((maxItems - maxVisibleItems + 1), 1, totalRegion.Height);
            ScrollBar.Height = (int)(totalRegion.Height / (maxItems - maxVisibleItems + 1));
            ScrollRegion.Width = ScrollDownRec.Width = ScrollUpRec.Width = ScrollBar.Width = (int)(spriteDimensions.X * scale);
            ScrollUpRec.Height = ScrollDownRec.Height = (int)(scale * spriteDimensions.Y);
            
            //Split into sections
            ScrollBarTop.Y = ScrollBar.Y;
            ScrollBarBottom.Y = (ScrollBar.Y + ScrollBar.Height - (scale * scrollBarTopDimensions));
            ScrollBarMid.Y = (ScrollBar.Y + (scale * scrollBarTopDimensions));
            ScrollBarBottom.X = ScrollBarTop.X = ScrollBarMid.X = ScrollBar.X;
            ScrollBarScale.X = scale;
            ScrollBarScale.Y = MathHelper.Clamp((ScrollBarBottom.Y - ScrollBarMid.Y) / spriteDimensions.Y, 1, 100);

            ScrollRegion.Height = (int)(ScrollDown.Y + (scale * spriteDimensions.Y) - ScrollUp.Y);
       
        }

        public override void updatePosition ( Point totalDifference )
        {
            base.updatePosition(totalDifference);

            //Update scroll bar
            changeScrollPos(totalDifference);
            ScrollDownRec.X = (int)(ScrollDown.X += totalDifference.X);
            ScrollDownRec.Y = (int)(ScrollDown.Y += totalDifference.Y);
            ScrollUpRec.X = (int)(ScrollUp.X += totalDifference.X);
            ScrollUpRec.Y = (int)(ScrollUp.Y += totalDifference.Y);
            ScrollRegion.X += totalDifference.X;
            ScrollRegion.Y += totalDifference.Y;

            //Update selected items
            for (short i = 0; i < shownItems.Count; i++)
                shownItems[i].updatePosition(totalDifference);
        }

        public void changeScrollbarPosition(short Increase)
        {
            //Clamp to number of items which can be iterated,
            //i.e if scroll bar is already at first index
            currentItemNumber = (int)MathHelper.Clamp(currentItemNumber += Increase, 0, maxItems - maxVisibleItems);
            changeScrollPos(currentItemNumber);
          //  selectedItem -= Increase;

            getItems();
        }

        public void changeScrollPos(Point totalDifference)
        {
            ScrollBar.X += totalDifference.X;
            ScrollBar.Y += totalDifference.Y;
            ScrollBarTop.X += totalDifference.X;
            ScrollBarTop.Y += totalDifference.Y;
            ScrollBarBottom.X += totalDifference.X;
            ScrollBarBottom.Y += totalDifference.Y;
            ScrollBarMid.X += totalDifference.X;
            ScrollBarMid.Y += totalDifference.Y;
        }

        public void changeScrollPos(int itemIndex)
        {
            //Change scroll bar directly to next element
            //Height is proportional to aomunt of items
            int yVal = (int)MathHelper.Clamp((int)(ScrollUp.Y + ScrollUpRec.Height + (itemIndex * pixelIncreasePerItem)),
                ScrollUpRec.Y + ScrollUpRec.Height, ScrollDownRec.Top - ScrollBar.Height);
            ScrollBar.Y = yVal;
            ScrollBarTop.Y = yVal;
            ScrollBarMid.Y = yVal + scrollBarTopDimensions * scale;
            ScrollBarBottom.Y = ScrollBarMid.Y + (spriteDimensions.Y * ScrollBarScale.Y);
        }

        public void getItems()
        {
           // selectedItem = (int)MathHelper.Clamp(selectedItem, 0, items.Count - maxVisibleItems);

            //Set the values 
            for (short ItemNumber = 0; ItemNumber < maxVisibleItems; ItemNumber++)
            {

                shownItems[ItemNumber].Name = items[currentItemNumber + ItemNumber].Name;
                shownItems[ItemNumber].infoLine = items[currentItemNumber + ItemNumber].description[0];
               
            }
        }

        public void getNextItems()
        {

        }
    }
}
