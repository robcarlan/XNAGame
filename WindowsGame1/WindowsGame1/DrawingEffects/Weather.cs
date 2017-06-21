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

namespace WindowsGame1
{
    class Weather
    {

        //General
        public Dictionary<String, Effect> effectList = new Dictionary<String, Effect>();
        public Rectangle screenDimensions;
        public Random random = new Random();

        //Rain
        public DrawingEffects.Rain rain;
        public bool raining;
        public float rainDuration;
        public float startRainIn;

        //Sun
        public Texture sunPos { get; set; }
        public float time;
        public float msPassedCounter;
        public int secondsPassed;
		public int minutesPassed;
        public int hour;
		public int daysPassed;

        //Fog
        private float fogSharpness = 0.2f;
        private float fogSpeed = 1;
        public float currentFogTransparency = 0f;
        public float fogTransparency = 0.2f;
        public bool isFogActive = false;
        private Texture2D fogTexture;
		private Texture2D tempTex;
        private Color fogColour = Color.MediumPurple;
        protected const float fogFadeRate = 0.002f;
        protected const short fogResolution = 16;

        public Weather( Rectangle _screenDimensions, Texture sunPosAsset, Texture2D _rainTexture, Texture2D _rainSplashTex, Dictionary<string, DataLoader.SpriteListLoader> rainSplash, Effect sunEffect, Effect fogEffect)
        {
            Vector2 screen = new Vector2(_screenDimensions.Width, _screenDimensions.Height);
            SpriteList tempSprites;
            tempSprites = new SpriteList();

            foreach (string key in rainSplash.Keys)
            {
                tempSprites.setFrames(ref rainSplash[key].spriteRec, rainSplash[key].spriteRec.Width, rainSplash[key].spriteRec.Height, rainSplash[key].numberOfFrames, key);
            }

            rain = new DrawingEffects.Rain(_rainTexture, _rainSplashTex, tempSprites, screen); 

            screenDimensions = _screenDimensions;
            loadWeather(sunPosAsset, sunEffect, fogEffect);
            raining = false;

        }

        public void loadWeather( Texture sunPosAsset, Effect sunEffect, Effect fogEffect )
        {
            sunPos = sunPosAsset;
            effectList.Add("sunEffect", sunEffect);
            effectList.Add("fogEffect", fogEffect);
        }

        public void setFog( GraphicsDevice graphics )
        {
            fogTexture = generateNoise(graphics);
            isFogActive = true;
            currentFogTransparency = 0f;
            fogSpeed = 1 / 16f;
        }

        public void disableFog()
        {
            isFogActive = false;
        }

        public bool setRain()
        {
            if (raining) return false;
            rain.setRain(160, true, DrawingEffects.Rain.rainStyle.Medium,
                DrawingEffects.Rain.rainStyle.Medium, DrawingEffects.Rain.rainStyle.Medium);
            rain.enableRain();
            raining = true;
            rainDuration = 30;
            return true;
        }

        public bool disableRain()
        {
            if (rain.rainStopping) return false;
            rain.disableRain();
            startRainIn = 30;
            return true;
        }

        public Texture2D generateNoise( GraphicsDevice graphics)
        {
            Random rand = new Random();
            Color[] noiseContent = new Color[fogResolution * fogResolution];

            for (short yCounter = 0; yCounter < fogResolution; yCounter++)
            {
                for (short xCounter = 0; xCounter < fogResolution; xCounter++)
                {
                    noiseContent[ xCounter + (yCounter * fogResolution) ] = new Color( new Vector4( rand.Next(255)/255f ) );
                }
            }

            Texture2D noiseMap = new Texture2D(graphics, fogResolution, fogResolution, false, SurfaceFormat.Color);

            noiseMap.SetData<Color>(noiseContent);

            return noiseMap;
        }

        //Updates
        public void updateTime(float msPassed)
        {
            msPassedCounter += msPassed;
            updateRainTime(msPassed);

            while (msPassedCounter >= 1f)
            {
                msPassedCounter -= 1f;
                secondsPassed++;
            }

            while (secondsPassed > 60)
            {
                minutesPassed++;
                secondsPassed -= 60;
            }

			while (minutesPassed > 60)
			{
				hour++;
				minutesPassed -= 60;
			}

			while (hour > 24)
			{
				hour -= 24;
				daysPassed++;
			}
        }

        public void updateWeather( float msPassed, Vector2 characterMovement )
        {
            if (isFogActive)
                updateFog();
            else if (currentFogTransparency != -1f)
            {
                currentFogTransparency = MathHelper.Clamp(currentFogTransparency -= fogFadeRate, currentFogTransparency, currentFogTransparency);
            }

            if (raining)
            {
                rain.updateRain(msPassed/1000, characterMovement);

                if (!rain.raining)
                    raining = false;
            }
        }

        public void updateRainTime(float msPassed)
        {

            if (raining)
            {
                rainDuration -= msPassed;
                if (rainDuration <= 0)
                    disableRain();
            }
            else
            {
                startRainIn -= msPassed;
                if (startRainIn <= 0)
                    setRain();
            }
        }

        private void updateFog()
        {
            if (currentFogTransparency != fogTransparency)
            {
                if (currentFogTransparency < fogTransparency)
                    currentFogTransparency += fogFadeRate;
                else currentFogTransparency -= fogFadeRate;
            }

            return;
        }

        //Draw Functions

        public void drawWeather(ref SpriteBatch spriteBatch )
        {
            // -- Remove these two lines, as lighting will be done completely by the deferred renderer
            //drawGlobalLighting(ref spriteBatch, ref renderedTexture);

            //renderedTexture = (Texture2D)graphics;


            if (isFogActive || currentFogTransparency != -1f)
                drawFog(ref spriteBatch);

            if (raining)
                drawRain(ref spriteBatch);

        }

        private void drawRain(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            rain.draw( ref spriteBatch);
            spriteBatch.End();
        }

        private void drawFog(ref SpriteBatch spriteBatch )
        {
            
           effectList["fogEffect"].Parameters["perlinTex"].SetValue(fogTexture);
           effectList["fogEffect"].Parameters["fogSharpness"].SetValue(fogSharpness);
           effectList["fogEffect"].Parameters["fogTransparency"].SetValue(currentFogTransparency);
           effectList["fogEffect"].Parameters["fogSpeed"].SetValue(fogSpeed);
           effectList["fogEffect"].Parameters["time"].SetValue(time);
 
           spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, effectList["fogEffect"]);
           effectList["fogEffect"].CurrentTechnique.Passes[0].Apply();
           spriteBatch.Draw(tempTex, screenDimensions, fogColour);
           spriteBatch.End();
        }

        private void drawGlobalLighting(ref SpriteBatch spriteBatch, ref Texture2D renderedTexture)
        {
            
            //Apply the sun effect
            effectList["sunEffect"].Parameters["time"].SetValue(time);
            effectList["sunEffect"].Parameters["renderedTexture"].SetValue(renderedTexture);
            effectList["sunEffect"].Parameters["dayTexture"].SetValue(sunPos);

            spriteBatch.Begin(SpriteSortMode.Texture, null, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, effectList["sunEffect"]);
            effectList["sunEffect"].CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(renderedTexture, screenDimensions, Color.White);
            spriteBatch.End();
        }

        public void onResize(Vector2 newScreen, GraphicsDevice device)
        {
            rain.onResize(newScreen);
            screenDimensions.Width = (int)newScreen.X;
            screenDimensions.Height = (int)newScreen.Y;
			tempTex = new Texture2D(device, (int)newScreen.X, (int)newScreen.Y);
        }
    }
}
