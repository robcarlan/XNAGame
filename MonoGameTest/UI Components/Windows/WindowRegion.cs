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
    public class WindowRegion
    {
        public Rectangle totalRegion;
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
        public Vector2 fill;
        public Vector2 left;
        public Vector2 right;
        public Vector2 bottom;
        public Vector2 top;
        public Vector2 horizontalScale;
        public Vector2 verticalScale;
        public Vector2 fillScale;
        public Vector2 spriteDimensions;
        public float scale;
        public int namePrefix;

        /// <summary>
        /// Creates a window region
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="_namePrefix"></param>
        /// <param name="spriteX"></param>
        /// <param name="spriteY"></param>
        /// <param name="_scale"></param>
        public WindowRegion( int _x, int _y, int _width, int _height, int _namePrefix, int spriteX, int spriteY, float _scale )
        {
            totalRegion.X = _x;
            totalRegion.Y = _y;
            totalRegion.Width = _width;
            totalRegion.Height = _height;
            namePrefix = _namePrefix;
            scale = _scale;
            spriteDimensions.X = spriteX;
            spriteDimensions.Y = spriteY;

            calculateValues();
        }

        public void calculateValues()
        {
            horizontalScale.Y = scale;
            verticalScale.X = scale;

            topLeft.Y = topRight.Y = top.Y = totalRegion.Y;
            topLeft.X = left.X = bottomLeft.X = totalRegion.X;
            bottomRight.X = right.X = topRight.X = totalRegion.X + totalRegion.Width - (int)(spriteDimensions.X * scale);
            top.X = bottom.X = totalRegion.X + (int)(spriteDimensions.X * scale);
            horizontalScale.X = ((topRight.X - top.X) / (spriteDimensions.X));
            left.Y = right.Y = topLeft.Y + (int)(spriteDimensions.Y * scale);
            bottomLeft.Y = bottomRight.Y = totalRegion.Y + totalRegion.Height - (int)(spriteDimensions.Y * scale);
            verticalScale.Y = ((bottomLeft.Y - left.Y) / (spriteDimensions.Y));
            bottom.Y = bottomLeft.Y;

            fill.X = top.X;
            fill.Y = left.Y;
            fillScale.X = (right.X - fill.X) / (spriteDimensions.X);
            fillScale.Y = (bottom.Y - fill.Y) / (spriteDimensions.Y);
        }

        public virtual void updatePosition(Point totalDifference)
        {

            left.X = topLeft.X = bottomLeft.X += totalDifference.X;
            right.X = topRight.X = bottomRight.X += totalDifference.X;
            top.X = fill.X = bottom.X += totalDifference.X;
            topLeft.Y = top.Y = topRight.Y += totalDifference.Y;
            left.Y = right.Y = fill.Y += totalDifference.Y;
            bottomLeft.Y = bottom.Y = bottomRight.Y += totalDifference.Y;

            totalRegion.X += totalDifference.X;
            totalRegion.Y += totalDifference.Y;
        }

		public void drawRegion(ref SpriteBatch sprite, SpriteListSimple windowSprites, Texture2D texture, Color Col)
		{
			sprite.Draw(texture, bottomLeft, windowSprites.list[namePrefix + AnimNameUIWin.bottomLeft][0], Col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sprite.Draw(texture, bottomRight, windowSprites.list[namePrefix + AnimNameUIWin.bottomRight][0], Col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sprite.Draw(texture, topLeft, windowSprites.list[namePrefix + AnimNameUIWin.topLeft][0], Col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sprite.Draw(texture, topRight, windowSprites.list[namePrefix + AnimNameUIWin.topRight][0], Col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			sprite.Draw(texture, top, windowSprites.list[namePrefix + AnimNameUIWin.top][0], Col, 0f, Vector2.Zero, horizontalScale, SpriteEffects.None, 0f);
			sprite.Draw(texture, bottom, windowSprites.list[namePrefix + AnimNameUIWin.bottom][0], Col, 0f, Vector2.Zero, horizontalScale, SpriteEffects.None, 0f);
			sprite.Draw(texture, left, windowSprites.list[namePrefix + AnimNameUIWin.left][0], Col, 0f, Vector2.Zero, verticalScale, SpriteEffects.None, 0f);
			sprite.Draw(texture, right, windowSprites.list[namePrefix + AnimNameUIWin.right][0], Col, 0f, Vector2.Zero, verticalScale, SpriteEffects.None, 0f);
			sprite.Draw(texture, fill, windowSprites.list[namePrefix + AnimNameUIWin.fill][0], Col, 0f, Vector2.Zero, fillScale, SpriteEffects.None, 0f);
		}

    }

    public class WindowItemRegion : WindowRegion 
    {
        public bool highlighted; //Draws region if highlighted, else daws the icon and name / description
        public Vector2 IconPos;
        public bool clicked;
        public string Name;
        public string infoLine;
        public Vector2 infoLinePos;
        public Vector2 nameDrawPos;
        public Rectangle iconSpace;

        public override void updatePosition(Point totalDifference)
        {
            base.updatePosition(totalDifference);

            iconSpace.X = (int)(IconPos.X += totalDifference.X);
            iconSpace.Y = (int)(IconPos.Y += totalDifference.Y);

            nameDrawPos.X += totalDifference.X;
            nameDrawPos.Y += totalDifference.Y;
            infoLinePos.X += totalDifference.X;
            infoLinePos.Y += totalDifference.Y;
        }

        public WindowItemRegion(int _x, int _y, int _width, int _height, int _namePrefix, int spriteX, int spriteY, float _scale,
            string mainText, string subText, int iconID )
            : base(_x, _y, _width, _height, _namePrefix, spriteX, spriteY, _scale )
        {
            //Set Icon Pos
            iconSpace.X = (int)(IconPos.X = (_x + 20));
            iconSpace.Width = iconSpace.Height = (int)(consts.iconDimensions * consts.abilityIconScale);
            iconSpace.Y = (int)(IconPos.Y =  _y + (_height - iconSpace.Height) / 2);

            //Set text pos
            Name = mainText;
            infoLine = subText;
            nameDrawPos.X = infoLinePos.X = iconSpace.Right + 10;
            nameDrawPos.Y = _y + (_height / 2) - 20;
            infoLinePos.Y = nameDrawPos.Y + 25;

            highlighted = false;
        }
    }
}
