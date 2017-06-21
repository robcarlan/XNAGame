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


//Todo::
/*
 * rain delay
 * rain sprite
 * rain colours
 * rain layers
 * 3d esque
 * Shader : render rain to a texture, and sue a shader to give a custom colour for different environments
 * different splash for different types
 */
namespace WindowsGame1.DrawingEffects
{
    public class Rain
    {
        public enum rainStyle { veryHeavy, Heavy, Medium, Light, veryLight };

        public class rainPoints
        {
            public Vector2 position;
            public Vector2 velocity;
            public float angle;
            public float transparency;

            //Represents the y limit for the rain point. Gives a 3d effect to the rain
            public int finalY;
            public int finalX;
            public Vector2 scale;

            public rainPoints(Random rand, float angleDegrees, int minVel, int maxVel, int minX, int maxX, int _finalY, int _finalX,
                Vector2 minScale, Vector2 maxScale)
            {
                float tempVel = rand.Next(minVel, maxVel);

                velocity.Y = Math.Abs((float)Math.Cos(angleDegrees) * tempVel);
                if (angleDegrees > 180)
                {
                    velocity.X = -Math.Abs((float)Math.Sin(angleDegrees) * tempVel);
                }
                else
                {
                    velocity.X = Math.Abs((float)Math.Sin(angleDegrees) * tempVel);
                }

                //Calculate origin
                position.X = rand.Next(minX, maxX);
                position.Y = Functions.rainYSpawn;
                
                //Convert to radians TODO: add some random ( none if 180, not > 180 if < 180 etc )
              //  angleDegrees += rand.Next(10) - 5f;
                angle = MathHelper.ToRadians(angleDegrees);
                transparency = 0.5f + rand.Next(500)/1000f;

                scale = Functions.getRandom(minScale, maxScale, rand);

                finalY = _finalY;
                finalX = _finalX;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="msPassed"></param>
            /// <returns>False if the rain should be deleted</returns>
            public bool update(float msPassed)
            {
                if (position.Y >= finalY || position.X >= finalX)
                    return false;

                position.X += velocity.X * msPassed;
                position.Y += velocity.Y * msPassed;

                return true;
            }

        };
        public class rainSplashes
        {
            public Vector2 position;
            public int phase;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="position">Start position of a piece of rain - end value is calculated</param>
            /// <param name="angle">In radians</param>
            public rainSplashes(Vector2 rainPosition, int rainHeight, int rainWidth, float angle, Vector2 rainScale)
            {
                
                //get the position
                position.Y = (float)(rainPosition.Y + rainHeight * rainScale.Y * Math.Sin((double)angle));
                position.X = (float)(rainPosition.X + (position.Y - rainPosition.Y) * Math.Tan((double)(180 - angle)));
                phase = 0;
                position.X -= Functions.rainSplashMidX * Functions.rainSplashScale;
                position.Y -= Functions.rainSplashMidY * Functions.rainSplashScale;
            }
        };

        //Total elapsed time is tracked by weather class

        public float nextRainIn;
        public float rainDelayMax;
        public float rainDelayMin;
        public float transparency;
        readonly int rainHeight;
        readonly int rainWidth;
        int raindropsPerSplash; //Used as a random numebr to determine roughly when a splash occurs
        int splashLayer;
        int maxVel;
        int minVel;
        int minX, maxX;
        float angle; //180 = vertical, 
        public bool raining;
        public bool rainStarting, rainStopping;
        public bool quickAppearance;
        //public float rainAppearanceRate; unused as of far
        public float rainAppearanceCounter;
        public int currentMaxRain;
        public int maxRain;
        public Vector2 screen;
        public Vector2 minScale;
        public Vector2 maxScale;

        // Rain Splash properties
        float splashPhaseTimer;
        const float timePerPhase = 0.2f;
        const int maxPhases = 4;

        //Defines the colour of each rain layer based on area
		//public readonly List<Color> rainColoursSwamp;
		//public readonly List<Color> rainColoursNormal; Unused as of far - would require shaders to implement

        // Scale max rain to screen resolution: maxRain * (screensCurrentWidth / maximumScreenWidth (i.e. 2560) )
        #region consts

        //Very Heavy
        public const int RAIN_VERY_HEAVY_MAX_RAIN = 1250;
        public const float RAIN_VERY_HEAVY_DELAY_MIN = 0.00001f;
        public const float RAIN_VERY_HEAVY_DELAY_MAX = 0.000005f;
        public const float RAIN_VERY_HEAVY_TRANSPARENCY = 0.8f;
        public readonly Vector2 RAIN_VERY_HEAVY_MIN_SCALE = new Vector2(0.4f, 1.2f);
        public readonly Vector2 RAIN_VERY_HEAVY_MAX_SCALE = new Vector2(0.45f, 1.25f);
        public const int RAIN_VERY_HEAVY_VELOCITY_MIN = 2200;
        public const int RAIN_VERY_HEAVY_VELOCITY_MAX = 2500;
        public const int RAIN_VERY_HEAVY_RPS = 10;
        //Heavy
        public const int RAIN_HEAVY_MAX_RAIN = 1000;
        public const float RAIN_HEAVY_DELAY_MIN = 0.0001f;
        public const float RAIN_HEAVY_DELAY_MAX = 0.00005f;
        public const float RAIN_HEAVY_TRANSPARENCY = 0.75f;
        public readonly Vector2 RAIN_HEAVY_MIN_SCALE = new Vector2(0.3f, 1.05f);
        public readonly Vector2 RAIN_HEAVY_MAX_SCALE = new Vector2(0.35f, 1.1f);
        public const int RAIN_HEAVY_VELOCITY_MIN = 1900;
        public const int RAIN_HEAVY_VELOCITY_MAX = 2200;
        public const int RAIN_HEAVY_RPS = 12;
        //Medium
        public const int RAIN_MEDIUM_MAX_RAIN = 600;
        public const float RAIN_MEDIUM_DELAY_MIN = 0.0002f;
        public const float RAIN_MEDIUM_DELAY_MAX = 0.00001f;
        public const float RAIN_MEDIUM_TRANSPARENCY = 0.65f;
        public readonly Vector2 RAIN_MEDIUM_MIN_SCALE = new Vector2(0.275f, 1f);
        public readonly Vector2 RAIN_MEDIUM_MAX_SCALE = new Vector2(0.3f, 1f);
        public const int RAIN_MEDIUM_VELOCITY_MIN = 1700;
        public const int RAIN_MEDIUM_VELOCITY_MAX = 2000;
        public const int RAIN_MEDIUM_RPS = 13;
        //Light
        public const int RAIN_LIGHT_MAX_RAIN = 250;
        public const float RAIN_LIGHT_DELAY_MIN = 0.002f;
        public const float RAIN_LIGHT_DELAY_MAX = 0.0001f;
        public const float RAIN_LIGHT_TRANSPARENCY = 0.6f;
        public readonly Vector2 RAIN_LIGHT_MIN_SCALE = new Vector2(0.25f, 0.9f);
        public readonly Vector2 RAIN_LIGHT_MAX_SCALE = new Vector2(0.275f, 1f);
        public const int RAIN_LIGHT_VELOCITY_MIN = 1500;
        public const int RAIN_LIGHT_VELOCITY_MAX = 1800;
        public const int RAIN_LIGHT_RPS = 14;
        //Very Light
        public const int RAIN_VERY_LIGHT_MAX_RAIN = 100;
        public const float RAIN_VERY_LIGHT_DELAY_MIN = 0.02f;
        public const float RAIN_VERY_LIGHT_DELAY_MAX = 0.001f;
        public const float RAIN_VERY_LIGHT_TRANSPARENCY = 0.5f;
        public readonly Vector2 RAIN_VERY_LIGHT_MIN_SCALE = new Vector2(0.225f, 0.85f);
        public readonly Vector2 RAIN_VERY_LIGHT_MAX_SCALE = new Vector2(0.25f, 0.95f);
        public const int RAIN_VERY_LIGHT_VELOCITY_MIN = 1400;
        public const int RAIN_VERY_LIGHT_VELOCITY_MAX = 1650;
        public const int RAIN_VERY_LIGHT_RPS = 15;


        #endregion

        public LinkedListNode<rainPoints> rainIterator;
        public LinkedList<rainPoints> rainLayerOne;
        public LinkedList<rainSplashes> rainSplashesList;
        public LinkedListNode<rainSplashes> rainSplashesIterator;
        public Random rand;
        public Texture2D rainTex;
        public Texture2D rainSplashTex;
        public SpriteListSimple rainSplash;

        Color rainColour = new Color();

        public Rain( Texture2D _rainTex, Texture2D _rainSplash, SpriteListSimple rainSplashSprite, Vector2 _Screen ) 
        {
            rainLayerOne = new LinkedList<rainPoints>();
            rainSplashesList = new LinkedList<rainSplashes>();
            rand = new Random();
            rainTex = _rainTex;
            rainSplash = rainSplashSprite;
            rainSplashTex = _rainSplash;
            screen = _Screen;
            rainHeight = rainTex.Bounds.Height;
            rainWidth = rainTex.Bounds.Width;
            raindropsPerSplash = 10;
        }

        public void setRain( float _angle, bool quickAppearance, rainStyle rainQuantity, rainStyle rainAppearance, rainStyle rainSpeed)
        {
            angle = MathHelper.Clamp(_angle, 90f, 270);
            rainColour = Color.White;
            this.quickAppearance = quickAppearance;
            splashPhaseTimer = 0f;

            //Changes the maximum amount and rate of rain
            #region boring
            switch (rainQuantity)
            {
                case rainStyle.veryHeavy:
                    {
                        maxRain = RAIN_VERY_HEAVY_MAX_RAIN;
                        rainDelayMax = RAIN_VERY_HEAVY_DELAY_MAX;
                        rainDelayMin = RAIN_VERY_HEAVY_DELAY_MIN;
                        raindropsPerSplash = RAIN_VERY_HEAVY_RPS;
                        break;
                    }
                case rainStyle.Heavy:
                    {
                        maxRain = RAIN_HEAVY_MAX_RAIN;
                        rainDelayMax = RAIN_HEAVY_DELAY_MAX;
                        rainDelayMin = RAIN_HEAVY_DELAY_MIN;
                        raindropsPerSplash = RAIN_HEAVY_RPS;
                        break;
                    }
                case rainStyle.Medium:
                    {
                        maxRain = RAIN_MEDIUM_MAX_RAIN;
                        rainDelayMax = RAIN_MEDIUM_DELAY_MAX;
                        rainDelayMin = RAIN_MEDIUM_DELAY_MIN;
                        raindropsPerSplash = RAIN_MEDIUM_RPS;
                        break;
                    }
                case rainStyle.Light:
                    {
                        maxRain = RAIN_LIGHT_MAX_RAIN;
                        rainDelayMax = RAIN_LIGHT_DELAY_MAX;
                        rainDelayMin = RAIN_LIGHT_DELAY_MIN;
                        raindropsPerSplash = RAIN_LIGHT_RPS;
                        break;
                    }
                case rainStyle.veryLight:
                    {
                        maxRain = RAIN_VERY_LIGHT_MAX_RAIN;
                        rainDelayMax = RAIN_VERY_LIGHT_DELAY_MAX;
                        rainDelayMin = RAIN_VERY_LIGHT_DELAY_MIN;
                        raindropsPerSplash = RAIN_VERY_LIGHT_RPS;
                        break;
                    }
            }
            #endregion

            //Changes the appearance of the rain: scale, transparency
            #region boring2
            switch (rainAppearance)
            {
                case rainStyle.veryHeavy:
                    {
                        transparency = RAIN_VERY_HEAVY_TRANSPARENCY;
                        minScale = RAIN_VERY_HEAVY_MIN_SCALE;
                        maxScale = RAIN_VERY_HEAVY_MAX_SCALE;
                        break;
                    }
                case rainStyle.Heavy:
                    {
                        transparency = RAIN_HEAVY_TRANSPARENCY;
                        minScale = RAIN_HEAVY_MIN_SCALE;
                        maxScale = RAIN_HEAVY_MAX_SCALE;
                        break;
                    }
                case rainStyle.Medium:
                    {
                        transparency = RAIN_MEDIUM_TRANSPARENCY;
                        minScale = RAIN_MEDIUM_MIN_SCALE;
                        maxScale = RAIN_MEDIUM_MAX_SCALE;
                        break;
                    }
                case rainStyle.Light:
                    {
                        transparency = RAIN_LIGHT_TRANSPARENCY;
                        minScale = RAIN_LIGHT_MIN_SCALE;
                        maxScale = RAIN_LIGHT_MAX_SCALE;
                        break;
                    }
                case rainStyle.veryLight:
                    {
                        transparency = RAIN_VERY_LIGHT_TRANSPARENCY;
                        minScale = RAIN_VERY_LIGHT_MIN_SCALE;
                        maxScale = RAIN_VERY_LIGHT_MAX_SCALE;
                        break;
                    }
            }
            #endregion

            //Sets rain speeds
            #region boring3
            switch (rainSpeed)
            {
                case rainStyle.veryHeavy:
                    {
                        minVel = RAIN_VERY_HEAVY_VELOCITY_MIN;
                        maxVel = RAIN_VERY_HEAVY_VELOCITY_MAX;
                        splashLayer = 0;
                        break;
                    }
                case rainStyle.Heavy:
                    {
                        minVel = RAIN_HEAVY_VELOCITY_MIN;
                        maxVel = RAIN_HEAVY_VELOCITY_MAX;
                        splashLayer = 0;
                        break;
                    }
                case rainStyle.Medium:
                    {
                        minVel = RAIN_MEDIUM_VELOCITY_MIN;
                        maxVel = RAIN_MEDIUM_VELOCITY_MAX;
                        splashLayer = 1;
                        break;
                    }
                case rainStyle.Light:
                    {
                        minVel = RAIN_LIGHT_VELOCITY_MIN;
                        maxVel = RAIN_LIGHT_VELOCITY_MAX;
                        splashLayer = 1;
                        break;
                    }
                case rainStyle.veryLight:
                    {
                        minVel = RAIN_VERY_LIGHT_VELOCITY_MIN;
                        maxVel = RAIN_VERY_LIGHT_VELOCITY_MAX;
                        splashLayer = 2;
                        break;
                    }
            }
            #endregion
            

            //Calculate min and max x spawn position
            calculateValues();
        }

        private void calculateValues()
        {
            if (angle < 180)
            {  //rain will be coming from the left
                minX = (int)-Math.Abs((screen.Y / (Math.Tan(angle - 90))));
                maxX = (int)screen.X;
            }
            else if (angle > 180)
            {
                minX = 0;
                maxX = (int)((screen.X + (screen.Y / (Math.Tan(angle - 180)))));
            }
            else
            {
                minX = 0;
                maxX = (int)screen.X;
            }
        }

        public void enableRain()
        {
            raining = true;
            rainStarting = true;
            rainStopping = false;
            currentMaxRain = 1;
            rainAppearanceCounter = 0f;
        }

        public void disableRain()
        {
            //raining = false; Raining is disabled once maxRain = 0
            rainStopping = true;
            rainStarting = false;
            currentMaxRain = rainLayerOne.Count;
            rainAppearanceCounter = 0f;
        }

        public void updateRain(float msPassed, Vector2 characterMovement)
        {
            nextRainIn -= msPassed;
            splashPhaseTimer += msPassed;

            //Check for starting rain
            if (rainStarting)
            {
                rainAppearanceCounter += msPassed;

                if (quickAppearance)
                {
                    while (rainAppearanceCounter > 0.005 && currentMaxRain < maxRain )
                    {
                        //Maximum amount of rain increases linearly
                        currentMaxRain += 2;
                        rainAppearanceCounter -= 0.05f;
                    }
                }
                else
                {
                    //Rain appears slowly at first, and builds up
                    float nextRain = (1f / (currentMaxRain) + (float)rand.NextDouble() );
                    while (rainAppearanceCounter > nextRain && currentMaxRain < maxRain)
                    {
                        currentMaxRain += 1;
                        rainAppearanceCounter -= nextRain;
                        nextRain = (float)(1f / (currentMaxRain) + (float)rand.NextDouble());
                    }
                }

                //Update maximum amount of rain allowed
                if (currentMaxRain >= maxRain)
                {
                    rainStarting = false;
                }
            }
            else if (rainStopping)
            {
                rainAppearanceCounter += msPassed;

                while (currentMaxRain > 0 && rainAppearanceCounter > (1 / (float)currentMaxRain) )
                {
                    rainAppearanceCounter -= (1 / (float)currentMaxRain);
                    currentMaxRain -= 1;
                    rainDelayMax += 0.00001f;
                    rainDelayMin += 0.00001f;
                }
                if (currentMaxRain == 0)
                {
                    rainStopping = false;
                    raining = false;
                    return;
                }
                //Decrease max amount of rain allowed until 0
            }

            while (nextRainIn <= 0)
            {
                if (rainLayerOne.Count < currentMaxRain)
                {
                    //Generate some rain
                    rainLayerOne.AddLast( new rainPoints(
                        rand, angle, minVel, maxVel, minX, maxX, rand.Next(0, (int)(screen.Y*1.42f)), (int)screen.X, minScale, maxScale ));
                }

                //Create a new spawn time
                nextRainIn += Functions.getRandom(rainDelayMin, rainDelayMax, rand);
            }

            //Update rain positions
            rainIterator = rainLayerOne.First;
            while (rainIterator != null)
            {
                bool isAlive = rainIterator.Value.update(msPassed);
                rainIterator.Value.position -= characterMovement;

                if (!isAlive && (rand.Next(raindropsPerSplash) == 0) && rainIterator.Value.position.Y < (screen.Y + 20))
                {
                    rainSplashesList.AddLast(new rainSplashes(rainIterator.Value.position, rainHeight, rainWidth, angle, rainIterator.Value.scale));
                }

                //So the element is not deleted whilst the iterator is on it
                rainIterator = rainIterator.Next;

                if (!isAlive)
                {
                    if (rainIterator == null)
                    {
                        rainLayerOne.RemoveLast();
                    }
                    else
                    {
                        rainLayerOne.Remove(rainIterator.Previous);
                    }
                }
            }

            //Update rainSplashes
            while (splashPhaseTimer > timePerPhase)
            {
                splashPhaseTimer -= timePerPhase;

                //Advance each element
                rainSplashesIterator = rainSplashesList.First;
                while (rainSplashesIterator != null)
                {
                    //Splash is not alive if it exceeds its maximum amount of phases
                    bool isAlive = ++rainSplashesIterator.Value.phase >= maxPhases ? false : true;

                    rainSplashesIterator = rainSplashesIterator.Next;

                    //removes the previous element
                    if (!isAlive)
                    {
                        if (rainSplashesIterator == null)
                            rainSplashesList.RemoveLast();
                        else
                            rainSplashesList.Remove(rainSplashesIterator.Previous);
                    }

                }
            }


            for (rainSplashesIterator = rainSplashesList.First; rainSplashesIterator != null; rainSplashesIterator = rainSplashesIterator.Next)
            {
                //Remove the velocity of the character, so that the splashes appear to stay in the correct place
                rainSplashesIterator.Value.position -= characterMovement;
            }

        }

        public void draw( ref SpriteBatch sprite)
        {
            rainIterator = rainLayerOne.First;


            while (rainIterator != null)
            {
                if (rainIterator.Value.position.X > -50)
                {
                    sprite.Draw(
                        rainTex, rainIterator.Value.position, null, rainColour * transparency * rainIterator.Value.transparency, rainIterator.Value.angle,
                        Vector2.Zero, rainIterator.Value.scale, SpriteEffects.None, 0f);
                }

                rainIterator = rainIterator.Next;
            }

            //Draw splashses
            rainSplashesIterator = rainSplashesList.First;
            while (rainSplashesIterator != null)
            {
                sprite.Draw(
                    rainSplashTex, rainSplashesIterator.Value.position, rainSplash.list[splashLayer][rainSplashesIterator.Value.phase], Color.White * transparency,
                    0f, Vector2.Zero, Functions.rainSplashScale, SpriteEffects.None, 0f);
                rainSplashesIterator = rainSplashesIterator.Next;
            }
        }

        public void onResize(Vector2 newScreen)
        {
            screen = newScreen;
            calculateValues();

            rainIterator = rainLayerOne.First;
            while (rainIterator != null)
            {
                rainIterator.Value.finalY = (int)screen.Y;
                rainIterator = rainIterator.Next;
            }
        }

        public void clearRain()
        {
            rainLayerOne = new LinkedList<rainPoints>();
        }
    }
}
