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

namespace WindowsGame1.DrawingEffects
{
    class DeferredRenderer
    {
        //Contains textures used to create final image and apply lighting
        public RenderTarget2D targetColour;
        public RenderTarget2D targetDepth;
        public RenderTarget2D targetLight;

        public Effect gBufferEffect;
		public Effect lightmapEffect;
		private EffectParameter depthTex;
		private EffectParameter lightTex;
		private EffectParameter viewVec;
		public EffectTechnique pointLightTech;

		public Effect lightmapCombineEffect;
		public EffectParameter colourTexture;
		public EffectParameter viewVecCombine;

        public DeferredRenderer(GraphicsDevice device, ContentManager content)
        {
			loadContent(content);
            renewRenderTargets(device);
            Functions.WriteDebugLine("Created Deferred Renderer");
        }

		public void loadContent(ContentManager content)
		{
			gBufferEffect = content.Load<Effect>("Graphical Effects\\Effect Files\\gBufferWrite");
			lightmapEffect = content.Load<Effect>("Graphical Effects\\Effect Files\\lightEffect");
			lightmapCombineEffect = content.Load<Effect>("Graphical Effects\\Effect Files\\lightmapCombine");

			//Assign effect parameters
			depthTex = lightmapEffect.Parameters["depthTex"];
			lightTex = lightmapEffect.Parameters["lightTex"];
			viewVec = lightmapEffect.Parameters["viewport"];
			viewVecCombine = lightmapCombineEffect.Parameters["viewport"];
			colourTexture = lightmapCombineEffect.Parameters["colourTex"];
			pointLightTech = lightmapEffect.Techniques["PointLightPass"];

			lightTex.SetValue(content.Load<Texture2D>("Graphical Effects\\circle"));
		}

        /// <summary>
        /// used to set the render targets. Used on initialisation and screen change
        /// </summary>
        public void renewRenderTargets(GraphicsDevice device)
        {
            int width = device.PresentationParameters.BackBufferWidth;
            int height = device.PresentationParameters.BackBufferHeight;

            targetColour = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
			targetDepth = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
			targetLight = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
        }

        public void setRenderTargets(GraphicsDevice device)
        {
            device.SetRenderTargets(targetColour, targetDepth, targetLight);
        }

        public void resolveRendertargets(GraphicsDevice device)
        {
            device.SetRenderTarget(null);
        }

		public void prepareAndSetLightTarget(GraphicsDevice device, Vector2 view)
		{
			device.SetRenderTarget(targetLight);
			//Map starts with no lighting data
			device.Clear(Color.Black);

			//Set effect parameters
			depthTex.SetValue(targetDepth);
			viewVec.SetValue(view);
		}

        public void drawTargetsToScreen(SpriteBatch spriteBatch)
        {
            Rectangle temp = Rectangle.Empty;
            temp.Height = 200;
            temp.Width = (int)(spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth / 3f);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            //spriteBatch.Draw(targetColour, temp, null, Color.White, 0f, Vector2.Zero, spriteBatch.GraphicsDevice.DisplayMode.Width / 3f, SpriteEffects.None, 0f);
            spriteBatch.Draw(targetColour, temp, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            temp.X += (int)(spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth / 3f);
            spriteBatch.Draw(targetDepth, temp, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            temp.X += (int)(spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth / 3f);
            spriteBatch.Draw(targetLight, temp, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.End();

        }

        public void saveRenderToJPEG(GraphicsDevice device)
        {
			System.IO.Stream stream = System.IO.File.OpenWrite("0" + ".jpg");
			targetColour.SaveAsJpeg(stream, 600, 400);
			stream.Dispose();
			stream = System.IO.File.OpenWrite("1" + ".jpg");
			targetLight.SaveAsJpeg(stream, 600, 400);
			stream.Dispose();
			stream = System.IO.File.OpenWrite("2" + ".jpg");
			targetDepth.SaveAsJpeg(stream, 600, 400);
			stream.Dispose();

			int texSize = 1024;
			int texSizeOver2 = texSize / 2;
			double texSizeSqrd = texSize * texSize;
			Texture2D tex = new Texture2D(device, texSize, texSize);
			Color[] texData = new Color[texSize * texSize];
			for (int x = 0; x < texSize; x++)
			{
				for (int y = 0; y < texSize; y++)
				{
					double distanceSqrd = (x - texSizeOver2) * (x - texSizeOver2) + (y - texSizeOver2) * (y - texSizeOver2);
					float colVal;
					if (distanceSqrd < texSizeSqrd)
						colVal = (float)(32 / (distanceSqrd));
					else colVal = 0;

					texData[x * texSize + y] = new Color(colVal * 255, 0, 0, 255);
				}
			}

			tex.SetData<Color>(texData);
			stream = System.IO.File.OpenWrite("col" + ".jpg");
			tex.SaveAsPng(stream, texSize, texSize);
			stream.Dispose();
        }

    }
}
