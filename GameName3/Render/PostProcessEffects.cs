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

namespace GameName3
{
	class PostProcessEffects
	{
		/*
		 * Effects - 
		 *	Radial fade fades from screen colour into blackness and vice versa (for cutscenes)
		 *	Radial colour gives the edges of the screen a certain hue
		 *	Monochromatic converts a texture into black and white 
		 *	Lighten/Darken brightens the whole image by a certain factor
		 *	Blur - blurs the whole screen by a certain magnitude
		 *	Radial blur - blurs the outside of a screen by a certain magnitude / radius
		 *	
		 * All effects given time variables so it fades in / out 
		 */
		Rectangle windowSize;
		public List<String> Messages;
		const int effectCount = 2;

		Texture2D imageOut;
		RenderTarget2D[] renderTarget = new RenderTarget2D[effectCount];

		//Called once in loadContent 
		public PostProcessEffects(ContentManager content)
		{
			Messages = new List<string>();
			loadContent(content);
		}

		public void loadContent(ContentManager content)
		{
			radialFade = content.Load<Effect>("Graphical Effects/Effect Files/RadialFog");
			blurEffect = content.Load<Effect>("Graphical Effects/Effect Files/RadialBlur");
			monochromaticEffect = content.Load<Effect>("Graphical Effects/Effect Files/monochromaticEffect");
		}

		public void onScreenChange(Rectangle newWindow, GraphicsDevice device)
		{
			windowSize = newWindow;
			onScreenChangeRadialFade(newWindow);

			for (int i = 0; i < effectCount; i++)
			{
				renderTarget[i] = new RenderTarget2D(device, newWindow.Width, newWindow.Height);
			}
		}

		public void update(float time)
		{
			if (radialFadeEnabled)
				updateRadialFade(time);
			if (isBlurEnabled)
				updateBlur(time);
			if (isMonochromatic)
				updateMonochromatic(time);
		}

		/// <summary>
		/// Draws the effects which affect the whole screen (Aswell as the HUD)
		/// </summary>
		/// <param name="screen">The backbuffer which contains the final image (without debug text)</param>
		public void drawEffectsOverHUD(ref SpriteBatch sprite, ref Texture2D screen, GraphicsDevice device )
		{
			int rTargetCount = 0;
			if (radialFadeEnabled)
			{
				device.SetRenderTarget(renderTarget[rTargetCount]);
				device.Clear(Color.Transparent);

				drawRadialFade(ref sprite, ref screen);

				device.SetRenderTarget(null);
				rTargetCount++;
			}

			if (rTargetCount == 0)
			{
				device.SetRenderTarget(renderTarget[0]);
				device.Clear(Color.Transparent);

				sprite.Begin();
				sprite.Draw(screen, Vector2.Zero, Color.White);
				sprite.End();
				device.SetRenderTarget(null);
				imageOut = renderTarget[0];
				return;
			}
			else
			{
				imageOut = renderTarget[rTargetCount - 1];
			}
		}

		/// <summary>
		/// Draws the effects which only affect the game (not the HUD)
		/// </summary>
		/// <param name="screen">Texture containing the game image, without other elements</param>
		public void drawEffectsUnderHUD(ref SpriteBatch sprite, ref Texture2D screen, GraphicsDevice device)
		{
			int rTargetCount = 0;

			if (isBlurEnabled)
			{
				device.SetRenderTarget(renderTarget[rTargetCount]);
				device.Clear(Color.Transparent);

				drawBlur(sprite, ref screen);

				device.SetRenderTarget(null);
				screen = renderTarget[rTargetCount];
				rTargetCount++;

			}

			if (isMonochromatic)
			{
				device.SetRenderTarget(renderTarget[rTargetCount]);
				device.Clear(Color.Transparent);

				drawMonochromatic(sprite, ref screen);

				device.SetRenderTarget(null);
				screen = renderTarget[rTargetCount];
				rTargetCount++;
			}

			/*
			 * if (effectEnabled)
			 *  take old texture
			 *	set new rendertarget
			 *	set effect parametes
			 *	draw
			 *	reset render target
			 */

			//If no effects were used, draw normal sprite to imageout
			if (rTargetCount == 0)
			{
				device.SetRenderTarget(renderTarget[0]);
				device.Clear(Color.Transparent);

				sprite.Begin();
				sprite.Draw(screen, Vector2.Zero, Color.White);
				sprite.End();
				device.SetRenderTarget(null);
				imageOut = renderTarget[0];
				return;
			}
			else
			{
				//imageOut = renderTarget[rTargetCount - 1];
				imageOut = screen;
			}
		}

		public Texture2D getImage()
		{
			return imageOut;
		}

		//					*	*	*	*	*	Radial Fade		*	*	*	*	*
		public Effect radialFade;
		public float fadeTimeLeft;
		public float changeRadialPerMS;
		public float radialEdge;
		public float blackScreenTime;
		public float fadeInTime;
		public bool radialFadeEnabled;
		public bool isFaded;
		public bool isFadingOut; //Else fading in

		public bool startRadialFadeOut(float Duration, float blackScreenTime)
		{
			if (!radialFadeEnabled)
			{
				Messages.Add(Functions.WriteDebugLine("Started fading out"));
				if (radialFadeEnabled) return false;
				calculateFadeVariables(Duration);
				radialFadeEnabled = true;
				isFadingOut = true;
				isFaded = false;
				this.blackScreenTime = blackScreenTime;
				fadeInTime = Duration;
				return true;
			}
			else return false;
		}

		public bool startFadeIn(float Duration)
		{
			Messages.Add(Functions.WriteDebugLine("Fade is clearing"));
			if (!isFadingOut) return false;
			calculateFadeVariables(Duration);
			isFaded = false;
			isFadingOut = false;
			return true;
		}

		private void calculateFadeVariables(float Duration)
		{
			//calculate farthest point from center - used as radius start
			fadeTimeLeft = Duration;
			double maxLengthFromCenter = Math.Sqrt((0.5 * windowSize.Width) * (0.5 * windowSize.Width)
				+ (0.5 * windowSize.Height) * (0.5 * windowSize.Height));

			radialEdge = (float)maxLengthFromCenter; // - constant
			changeRadialPerMS = (float)(maxLengthFromCenter / (Duration*1000));
		}

		public void stopRadialFade()
		{
			radialFadeEnabled = false;
			isFadingOut = false;
			isFaded = false;
		}

		public bool radialFadeIsEnabled()
		{
			return radialFadeEnabled;
		}


		/// <param name="time">1 second = 1.0f</param>
		public void updateRadialFade(float time)
		{
			if (!isFaded)
			{
				radialEdge += time * 1000 * ( isFadingOut ? -changeRadialPerMS : changeRadialPerMS);
				if (radialEdge < 0) radialEdge = 0;
				fadeTimeLeft -= time;
				if (fadeTimeLeft <= 0)
				{
					if (isFadingOut)
					{
						fadeTimeLeft += blackScreenTime;
						Messages.Add(Functions.WriteDebugLine("Screen completely faded - clears in " + blackScreenTime * 1000 + " milliseconds."));
						isFaded = true;
					}
					else
					{
						//Screen is clearing
						Messages.Add(Functions.WriteDebugLine("Screen now clear, stopping radial fade."));
						stopRadialFade();
					}
				}
			}
			else
			{
				//Screen is completely black
				fadeTimeLeft -= time;
				if (fadeTimeLeft <= 0)
				{
					isFaded = false;
					isFadingOut = false;
					fadeTimeLeft += fadeInTime;
					Messages.Add(Functions.WriteDebugLine("Black screen time exceeded."));
				}
			}


		}

		//Used when loading takes a long time etc (called when falls below a certain threshold)
		public void lengthenBlackScreenDuration(float amount)
		{
			Messages.Add(Functions.WriteDebugLine("Fade clear delayed by " + amount * 1000 + " miliseconds."));
			blackScreenTime += amount;
		}

		public float getTimeLeft()
		{
			return fadeTimeLeft;
		}

		void drawRadialFade(ref SpriteBatch sprite, ref Texture2D image)
		{
			radialFade.Parameters["Length"].SetValue(radialEdge);
			radialFade.Parameters["texLength"].SetValue(windowSize.Width);
			radialFade.Parameters["texHeight"].SetValue(windowSize.Height);
			radialFade.Parameters["isFadingOut"].SetValue(isFadingOut || isFaded);
			radialFade.CurrentTechnique.Passes[0].Apply();

			sprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp,
				DepthStencilState.Default, RasterizerState.CullNone, radialFade);
			sprite.Draw(image, windowSize, Color.White);
			sprite.End();
		}

		private void onScreenChangeRadialFade(Rectangle newWindow)
		{
			if (radialFadeEnabled)
			{
				double maxLengthFromCenter = Math.Sqrt((0.5 * windowSize.Width) * (0.5 * windowSize.Width)
					+ (0.5 * windowSize.Height) * (0.5 * windowSize.Height));

				radialEdge = isFadingOut ? (float)maxLengthFromCenter : 0; // - constant
				changeRadialPerMS = (float)(maxLengthFromCenter / (fadeInTime * 1000));
				radialEdge += (fadeInTime - fadeTimeLeft) * 1000 * (isFadingOut ? -changeRadialPerMS : changeRadialPerMS);
			}
		}

		/*					*	*	*	*	Radial Blur	*	*	*	*				*/
		private Effect blurEffect;
		private float blurSetTime = 1.0f;
		private float blurDuration;
		private float elapsedTimeBlur;
		private float blurMagnitude;
		//private float currentBlurMagnitude;
		private float blurRadiusHard;	//Blur is a uniform magnitude
		private float blurRadiusSoft;	//Use a wave function to determine blur 
		private bool isBlurEnabled;

		public void onWindowSizeChange(Rectangle newWindow)
		{

		}

		public bool beginBlur(float duration, float radiusHard, float radiusSoft, float magnitude)
		{
			if (isBlurEnabled)
				return false;
			else
			{
				blurDuration = duration;
				elapsedTimeBlur = 0;
				blurRadiusSoft = radiusSoft;
				blurRadiusHard = radiusHard;
				blurMagnitude = magnitude;
				isBlurEnabled = true;

				return true;
			}
		}

		public void endBlur()
		{
			isBlurEnabled = false;
			Messages.Add(Functions.WriteDebugLine("Blur ended."));
		}

		public void updateBlur(float gameTime)
		{
			elapsedTimeBlur += gameTime;
			if (elapsedTimeBlur > blurDuration)
			{
				Messages.Add(Functions.WriteDebugLine("Blur duration complete"));
				endBlur();
				return;
			}
		}

		public void extendBlur(float time)
		{
			blurDuration += time;
		}

		private float getBlurMagnitude()
		{
			if (elapsedTimeBlur < blurSetTime)
				return elapsedTimeBlur * blurSetTime;
			else if ((blurDuration - elapsedTimeBlur) < blurSetTime)
				return (blurDuration - elapsedTimeBlur) * blurSetTime;
			else return 1.0f;
		}

		private void drawBlur(SpriteBatch sprite, ref Texture2D tex)
		{

			blurEffect.Parameters["hardLengthSqrd"].SetValue(blurRadiusHard*blurRadiusHard);
			blurEffect.Parameters["softLengthSqrd"].SetValue(blurRadiusSoft*blurRadiusSoft);
			blurEffect.Parameters["texLength"].SetValue(windowSize.Width);
			blurEffect.Parameters["texHeight"].SetValue(windowSize.Height);
			blurEffect.Parameters["magnitudeBase"].SetValue(blurMagnitude);
			blurEffect.Parameters["magnitudeMultiplier"].SetValue(getBlurMagnitude());
			//blurEffect.Parameters["magnitudeMultiplier"].SetValue(1.0f);
			blurEffect.Parameters["renderedTexture"].SetValue(tex);
			blurEffect.Parameters["fPerPixX"].SetValue(1 / windowSize.Width);
			blurEffect.Parameters["fPerPixY"].SetValue(1 / windowSize.Height);
			blurEffect.CurrentTechnique.Passes[0].Apply();

			sprite.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None,
				RasterizerState.CullNone, blurEffect);
			sprite.Draw(tex, Vector2.Zero, Color.White);
			sprite.End();
		}

		/*				*	*	*	*	Radial Color	*	*	*	*				*/

		/*				*	*	*	*	Monochromatic	*	*	*	*				*/
		Effect monochromaticEffect;
		bool isMonochromatic;
		public float monoDuration;
		public float monoElapsedTime;
		public float monoSetDuration;
		public float monoSaturation;

		public bool enableMonochromatic(float monoDuration, float monoMagnitude, float monoSetTime)
		{
			if (isMonochromatic)
				return false;
			else
			{
				isMonochromatic = true;
				this.monoDuration = monoDuration;
				this.monoSetDuration = (monoDuration / 2 > monoSetDuration ? monoSetTime : monoDuration / 2);
				this.monoSaturation = monoMagnitude;
				monoElapsedTime = 0;
				return true;
			}

		}

		public void updateMonochromatic(float gameTime)
		{
			monoElapsedTime += gameTime;
			if (monoElapsedTime > monoDuration)
			{	
				Messages.Add(Functions.WriteDebugLine("Monochromatic effect ended"));
				isMonochromatic = false;
			}
		}

		public void stopMonochromatic(float fadeTime)
		{
			monoSetDuration = fadeTime;
			if (monoElapsedTime > monoDuration - fadeTime) 
			{
				float ratioComplete = (monoDuration - monoElapsedTime) / fadeTime;
				monoElapsedTime = monoDuration - fadeTime * (1 - ratioComplete);

			}
			else if (monoElapsedTime < fadeTime)
			{
				float ratioComplete = monoElapsedTime / fadeTime;
				monoElapsedTime = monoDuration - fadeTime * (1 - ratioComplete);
			}
			else 
				monoElapsedTime = monoDuration - fadeTime;
		}

		private void drawMonochromatic(SpriteBatch sprite, ref Texture2D image)
		{
			monochromaticEffect.Parameters["renderedTexture"].SetValue(image);
			monochromaticEffect.Parameters["magnitudeBase"].SetValue(monoSaturation);
			monochromaticEffect.Parameters["magnitudeMultiplier"].SetValue(getSaturationMult());
			monochromaticEffect.Parameters["rand"].SetValue(monoElapsedTime);

			sprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp,
				DepthStencilState.Default, RasterizerState.CullNone, monochromaticEffect);
			sprite.Draw(image, windowSize, Color.White);
			sprite.End();
		}

		private float getSaturationMult()
		{
			if (monoElapsedTime > monoDuration - monoSetDuration)
			{
				return monoDuration - monoSetDuration * (1 - ((monoDuration - monoElapsedTime) / monoSetDuration));
			}
			else if (monoElapsedTime < monoSetDuration)
			{
				return monoDuration - monoSetDuration * (1 - ((monoElapsedTime) / monoSetDuration));
			}
			else
				return 1.0f;
		}

		public bool isMonochromaticEnabled()
		{
			return isMonochromatic;
		}

	}
}
