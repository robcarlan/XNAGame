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

namespace WindowsGame1.UI_Components
{
    /// <summary>
    /// Used to represent a static sprite on an interface, for example, icons for skills
    /// Base class for: Checkbox, Clickable Icon
    /// </summary>
    public class WindowIcon
    {
        public Rectangle absoluteArea;
        public Vector2 absolutePosition;
        public Vector2 relativePosition;
        public Vector2 scale;
        public int xDimensions, yDimensions;
        public int spriteID;
        public bool isVisible = true;

        public Text text;
        public enums.textLocation textLoc;
        public bool hasText;


        //Specific to the form of the UI, used to access via control collection
        public int controlID;

        public WindowIcon(Vector2 windowBaseBegin, Vector2 IconArea, int height, int width, Vector2 scale, int spriteID)
        {
            xDimensions = width;
            yDimensions = height;
            this.scale = scale;

            absoluteArea.Width = (int)(width * scale.X);
            absoluteArea.Height = (int)(height * scale.Y);

            relativePosition.X = IconArea.X;
            relativePosition.Y = IconArea.Y;
            setPositions(windowBaseBegin);
            
            this.spriteID = spriteID;
        }

        /// <summary>
        /// Called by Control Collection
        /// </summary>
        /// <param name="ID"></param>
        public void setID(int ID)
        {
            controlID = ID;
        }

        /// <summary>
        /// Sets the absolute position of the icon based on the window position
        /// </summary>
        public void setPositions(Vector2 windowBaseBegin)
        {
            absoluteArea.X = (int)(windowBaseBegin.X + relativePosition.X);
            absoluteArea.Y = (int)(windowBaseBegin.Y + relativePosition.Y);
            absolutePosition.X = absoluteArea.X;
            absolutePosition.Y = absoluteArea.Y;
        }

        public void incrementPosition(Point increase)
        {
            if (hasText)
                text.increasePosition(increase);

            absoluteArea.X += increase.X;
            absoluteArea.Y += increase.Y;
            absolutePosition.X += increase.X;
            absolutePosition.Y += increase.Y;
            relativePosition.X += increase.X;
            relativePosition.Y += increase.Y;
        }

        /// <summary>
        /// Returns the drawable state of the device
        /// </summary>
        /// <returns></returns>
        public virtual int getCurrentState()
        {
            return consts.BUTTON_DEFAULT;
        }
    }

    /// <summary>
    /// Represents an icon which can be clicked to allow interaction with the user
    /// </summary>
    public class WindowButton : WindowIcon
    {
        protected bool clickHeld;
        protected bool isHovering;
        protected readonly bool hoverSprite;

        public WindowButton(Vector2 windowBaseBegin, Vector2 IconArea, int height, int width, Vector2 scale, int spriteID, bool hoverSprite)
            : base(windowBaseBegin, IconArea, height, width, scale, spriteID)
        {
            clickHeld = isHovering = false;
            this.hoverSprite = hoverSprite;
        }

        /// <summary>
        /// Returns true when button is released
        /// </summary>
        /// <param name="mousePos">co-ordinate of mouse position</param>
        /// <param name="isHeld">Indicates whetehr the omuse is currrently being pressed</param>
        /// <returns></returns>
        public bool checkMouseInput(Point mousePos, bool isHeld)
        {
            if (absoluteArea.Contains(mousePos))
            {
                if (clickHeld && !isHeld)
                {
                    clickHeld = false;
                    return true; //Button has been clicked
                }

                if (isHeld)
                    clickHeld = true; // Mouse is held down inside the icon
                else
                    isHovering = true;
            }
            else
            {   
                isHovering = false;
                if (!isHeld)
                    clickHeld = false; //Button has been released outside the region
            }
            return false;
        }

        public override int getCurrentState()
        {
            if (clickHeld)          return consts.BUTTON_HELD;
            else if (isHovering)    return consts.BUTTON_HOVER;
            else                    return consts.BUTTON_DEFAULT;
        }
    }

    public class WindowTextButton : WindowButton 
    {
        public WindowTextButton(Vector2 windowBaseBegin, Vector2 IconArea, int height, int width, Vector2 scale, int spriteID, bool hoverSprite)
            : base(windowBaseBegin, IconArea, height, width, scale, spriteID, hoverSprite)
        {
        }
    }

    /// <summary>
    /// Box with two different states. Inherits from WindowButton
    /// </summary>
    public class WindowCheckbox : WindowButton
    {
        bool isChecked;

        public WindowCheckbox(Vector2 windowBaseBegin, Vector2 IconArea, int height, int width, Vector2 scale, int spriteID, bool hoverSprite, bool defaultChecked)
            : base(windowBaseBegin, IconArea, height, width, scale, spriteID, hoverSprite)
        {
            isChecked = defaultChecked;
        }

        public void Toggle()
        {
            isChecked = !isChecked;
        }
        public void Check()
        {
            isChecked = true;
        }
        public void Uncheck()
        {
            isChecked = false;
        }

        public override int getCurrentState()
        {
            if (isChecked) return consts.BUTTON_CHECKED;
            else if (clickHeld) return consts.BUTTON_HELD;
            else return consts.BUTTON_UNCHECKED;
        }
    }
    
}
