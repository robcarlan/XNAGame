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
        public RenderTarget2D targetNormal;
        public RenderTarget2D targetDepth;
        public Effect gBufferEffect;

        public DeferredRenderer(GraphicsDevice device, Effect gBufferFX)
        {
            renewRenderTargets(device);
            gBufferEffect = gBufferFX;
            Functions.WriteDebugLine("Created Deferred Renderer");
        }

        /// <summary>
        /// used to set the render targets. Used on initialisation and screen change
        /// </summary>
        public void renewRenderTargets(GraphicsDevice device)
        {
            int width = device.PresentationParameters.BackBufferWidth;
            int height = device.PresentationParameters.BackBufferHeight;

            targetColour = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            targetNormal = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            targetDepth = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public void setRenderTargets(GraphicsDevice device)
        {
            device.SetRenderTargets(targetColour, targetNormal, targetDepth);
        }

        public void resolveRendertargets(GraphicsDevice device)
        {
            device.SetRenderTarget(null);
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
            spriteBatch.Draw(targetNormal, temp, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            temp.X += (int)(spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth / 3f);
            spriteBatch.Draw(targetDepth, temp, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            spriteBatch.End();

        }

        public void saveRenderToJPEG()
        {
            System.IO.Stream stream = System.IO.File.OpenWrite("0" + ".jpg");
            targetColour.SaveAsJpeg(stream, 600, 400);
            stream.Dispose();
            stream = System.IO.File.OpenWrite("1" + ".jpg");
            targetDepth.SaveAsJpeg(stream, 600, 400);
            stream.Dispose();
            stream = System.IO.File.OpenWrite("2" + ".jpg");
            targetNormal.SaveAsJpeg(stream, 600, 400);
            stream.Dispose();
        }

    }
}
