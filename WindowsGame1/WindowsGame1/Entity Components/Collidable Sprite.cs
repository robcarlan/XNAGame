using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace WindowsGame1
{
    public class Collidable_Sprite : Sprite
    {
        public short ID;

        public bool collides;
        public bool useCollisionBox;

        //If not null, this rectangle is used for collision detection instead of it's sprite box.
        public Rectangle collisionBox;
        public Vector2 circleOrigin;
        public Vector2 previousCircle;
        public short collisionCircleRadius;
     
        //Quadtree data
        public short collisionNode = 0;

        public Collidable_Sprite(Point tilePos, float xAcc, float yAcc, string objName, Rectangle _localSpace)
            :base( tilePos, xAcc, yAcc, objName, _localSpace )
        {
            return;
        }

        public Collidable_Sprite()
        {

        }

        public Collidable_Sprite(Collidable_Sprite temp)
            : base(temp)
        {
            collides = temp.collides;
            useCollisionBox = temp.useCollisionBox;
            collisionBox = temp.collisionBox;
            circleOrigin = temp.circleOrigin;
            previousCircle = temp.previousCircle;
            collisionCircleRadius = temp.collisionCircleRadius;
        }

        virtual public string getText()
        {
           return "empty";
        }

		public void updateCollisionPosition(int xIncrease, int yIncrease)
		{

		}

		public void offsetPosition (short xDisplacement, short yDisplacement)
			
		{
			circleOrigin.X -= xDisplacement;
			circleOrigin.Y -= yDisplacement;
			localSpace.X -= xDisplacement;
			localSpace.Y -= yDisplacement;
		}

		public void increaseX(float X)
		{
			localSpace.X += (int)X;
			circleOrigin.X += (int)X;
		}

		public void decreaseX(float X)
		{
			localSpace.X -= (int)X;
			circleOrigin.X -= (int)X;
		}

        public void applyVelocities()
        {
            previousCircle = circleOrigin;
            //localSpace.X += (int)vel.X;
            //localSpace.Y += (int)vel.Y;
            circleOrigin.X += vel.X;
            circleOrigin.Y += vel.Y;
            localSpace.X = (int)(circleOrigin.X - localSpace.Width / 2f);
            localSpace.Y = (int)(circleOrigin.Y - localSpace.Height / 2f);
        }

        public void applyVelocities( short multiplier )
        {
            previousCircle = circleOrigin;
            circleOrigin.X += vel.X * multiplier;
            circleOrigin.Y += vel.Y * multiplier;
            localSpace.X = (int)(circleOrigin.X - localSpace.Width / 2f);
            localSpace.Y = (int)(circleOrigin.Y - localSpace.Height / 2f);
        }

        public void applyVelocity(Vector2 _vel, bool updateCircle)
        {
            if (updateCircle)
                previousCircle = circleOrigin;

            circleOrigin.X += _vel.X;
            circleOrigin.Y += _vel.Y;
            Offset.X += _vel.X;
            Offset.Y += _vel.Y;
            localSpace.X = (int)(circleOrigin.X - localSpace.Width / 2f);
            localSpace.Y = (int)(circleOrigin.Y - localSpace.Height / 2f);
        }

		public void setCircleCollision( Vector2 origin, short Radius )
		{
			useCollisionBox = false;
			circleOrigin = origin;
			collisionCircleRadius = Radius;
		}

		//Collision Detection
		public bool doesCollide(Vector2 queryCircle, short circleRadius)
		{
			if (useCollisionBox)
			{
				//Circle / Box Collision
				Vector2 closestPoint = getClosestPoint(ref localSpace, ref queryCircle);
				// Test Radius
				float totalLengthSquared = (
					(queryCircle.X - closestPoint.X) * (queryCircle.X - closestPoint.X) +
					(queryCircle.Y - closestPoint.Y) * (queryCircle.Y - closestPoint.Y));

				return (circleRadius^2) > totalLengthSquared ? true : false;
					//Collision
			}
			else
			{
				//Circle / Circle Collision
				//Get difference in length
				float totalLengthSquared = (
					(queryCircle.X - circleOrigin.X) * (queryCircle.X - circleOrigin.X) +
					(queryCircle.Y - circleOrigin.Y) * (queryCircle.Y - circleOrigin.Y));
				//Get max tolerated radius, compare with length
				return (Math.Pow((circleRadius + collisionCircleRadius), 2)) > totalLengthSquared ? true : false;
			}
		}

		public bool doesCollide(Rectangle queryBox)
		{
			if (useCollisionBox)
			{	//Rectangle / Rectangle collision
				return queryBox.Intersects(localSpace) ? true : false;
			}
			else
			{
				//Circle / Box Collision
				Vector2 closestPoint = getClosestPoint(ref queryBox, ref circleOrigin);
				// Test Radius
				float totalLengthSquared = (
					(circleOrigin.X - closestPoint.X) * (circleOrigin.X - closestPoint.X)  +
					(circleOrigin.Y - closestPoint.Y) * (circleOrigin.Y - closestPoint.Y));

				return (collisionCircleRadius * collisionCircleRadius) > totalLengthSquared ? true : false;
				//Collision
			}
		}

		//Collision Resolution
		public bool ResolveCircleCollision(ref Vector2 queryCircle, ref Rectangle queryLocalSpace, ref Vector2 spriteVelocity, ref short circleRadius)
		{
			Vector2 difference = new Vector2( Math.Abs(queryCircle.X - circleOrigin.X), Math.Abs(queryCircle.Y - circleOrigin.Y));
			float totalLengthSqrd = difference.LengthSquared();
			float combinedRadius = (circleRadius + collisionCircleRadius);

			if (totalLengthSqrd < (circleRadius + collisionCircleRadius) * (circleRadius + collisionCircleRadius) )  //Total difference should be less than both radii for collision
			{
				//normalise each vector, multiply by necessary
                Vector2 totalTranslation = Vector2.Normalize(difference);
                totalTranslation = Vector2.Multiply(totalTranslation, (circleRadius + collisionCircleRadius - difference.Length()));

                //Set correct orientation
                if ((circleOrigin.X - queryCircle.X) > 0)
                    totalTranslation.X *= -1;
                else totalTranslation.X *= 1;

				//Floor / Ceiling the results
				roundVector(ref totalTranslation);

                if (spriteVelocity.X == 0 && vel.X != 0)
                {
                    localSpace.X -= (int)(totalTranslation.X);
                    circleOrigin.X -= (int)(totalTranslation.X);
                }
				else if (vel.X == 0 && spriteVelocity.X != 0)
				{
                    queryCircle.X -= (int)(totalTranslation.X);
                    queryLocalSpace.X -= (int)(totalTranslation.X);
				}
				else if (vel.X == 0 && spriteVelocity.X == 0)
				{
					// Velocities both 0
				}
				else
				{	//Split the velocities evenly.
					int velocityDifferenceX = (int)Math.Abs(vel.X - spriteVelocity.X);
					localSpace.X -= (int)((velocityDifferenceX / totalTranslation.X) * vel.X);
					queryCircle.X -= (int)((velocityDifferenceX / totalTranslation.X) * spriteVelocity.X);
				}
				//vel.X = spriteVelocity.X = 0;
                vel.X = circleOrigin.X - previousCircle.X;
                
				return true;
			}
			else return false;
		}

		private Vector2 getClosestPoint(ref Rectangle spriteBox, ref Vector2 circleOrigin)
		{
			Vector2 closestPoint;
			//Gets the closest possible point by clamping values
			closestPoint.X = MathHelper.Clamp(circleOrigin.X, spriteBox.X, spriteBox.X + spriteBox.Width);
			closestPoint.Y = MathHelper.Clamp(circleOrigin.Y, spriteBox.Y, spriteBox.Y + spriteBox.Height);
			
			return closestPoint;
		}

        public void ResolveRectangleCollision2(ref Rectangle queryRec, ref Vector2 queryVelocity)
        {
            Vector2 finalVelocity = queryVelocity + vel;
			//Create a rectangle based on where the rectangles overlap
			Rectangle overlap = Rectangle.Intersect(queryRec, localSpace);

			//Angle are oppositte, return
			if ( Vector2.Dot( vel, queryVelocity ) < 0 )
				return;

        }

        public void ResolveRectangleCollision(ref Rectangle queryRectangle, ref Vector2 querySpriteVelocity)
        {
			//Calculate the intersecting rectangle
			Rectangle overlap = Rectangle.Intersect(queryRectangle, localSpace);
            //Get the difference in velocities
            Vector2 VelocityDifference = new Vector2(Math.Abs(vel.X - querySpriteVelocity.X), Math.Abs(vel.Y - querySpriteVelocity.Y));

			//Calculate pixel overlap value
			Vector2 PixelOverlapValue;
			if (VelocityDifference.X == 0)
				PixelOverlapValue.X = 0;
			else PixelOverlapValue.X = VelocityDifference.X / overlap.Width;

			if (VelocityDifference.Y == 0)
				PixelOverlapValue.Y = 0;
			else PixelOverlapValue.Y = VelocityDifference.Y / overlap.Height;

			//Dot product, to get overall angle. If negative - moving away, do nothing.
			if (Vector2.Dot(querySpriteVelocity, vel) < 0)
				return;

			if (vel.X == 0)
			{
				queryRectangle.X -= (int)(querySpriteVelocity.X);
			}
			else if (querySpriteVelocity.X == 0)
			{
				localSpace.X -= (int)(vel.X);
			}
			else
			{
				localSpace.X -= (int)(PixelOverlapValue.X * vel.X);
				queryRectangle.X -= (int)(querySpriteVelocity.X * PixelOverlapValue.X);
			}

			////If moving in direction of collision, set vel to 0
			//if (vel.X != 0)
			//    if ((queryRectangle.X - localSpace.X) / vel.X < 0)
			//        localSpace.X -= (int)(vel.X);
			//queryRectangle.X -= (int)(querySpriteVelocity.X) ;

            //Set new velocities: If moxing towaqrds collision, set velocity to 0
			//if (VelocityDifference.X != 0)
			//    vel.X = querySpriteVelocity.X = 0;

			if (vel.Y == 0)
			{
				queryRectangle.Y -= (int)(querySpriteVelocity.Y);
			}
			else if (querySpriteVelocity.Y == 0)
			{
				localSpace.Y -= (int)(vel.Y);
			}
			else
			{
				localSpace.Y += (int)(PixelOverlapValue.Y * vel.Y);
				queryRectangle.Y += (int)(querySpriteVelocity.Y * PixelOverlapValue.Y);
			}

			////If moving in direction of collision, set vel to 0
			//if (vel.Y != 0)
			//    if ((queryRectangle.Y - localSpace.Y) / vel.Y < 0)
			//        localSpace.Y -= (int)(vel.Y);
			//queryRectangle.Y -= (int)(querySpriteVelocity.Y);

			//if (VelocityDifference.Y != 0)
			//    vel.Y = querySpriteVelocity.Y = 0;
            return;
        }

		public void roundVector(ref Vector2 value)
		{
			if (value.X < 0)
				value.X = (float)Math.Floor(value.X);
			else value.X = (float)Math.Ceiling(value.X);

			if (value.Y < 0)
				value.Y = (float)Math.Floor(value.Y);
			else value.Y = (float)Math.Ceiling(value.Y);
		}
    }
}
