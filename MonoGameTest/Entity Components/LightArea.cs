using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
	public class LightArea
	{
		private GraphicsDevice graphicsDevice;

		public RenderTarget2D RenderTarget { get; private set; }
		public Vector2 LightPosition { get; set; }
		public Vector2 LightAreaSize { get; set; }
		public short lightID;
		public LightArea(GraphicsDevice graphicsDevice, float lightRadius, short _lightID)
		{
			int ceilPow2 = (int)(Math.Log(lightRadius, 2));
			int baseSize = 2 << ceilPow2;
			LightAreaSize = new Vector2(baseSize);
			RenderTarget = new RenderTarget2D(graphicsDevice, baseSize, baseSize);
			this.graphicsDevice = graphicsDevice;
			lightID = _lightID;
		}

		public Vector2 ToRelativePosition(Vector2 worldPosition)
		{
			return worldPosition - (LightPosition - LightAreaSize * 0.5f);
		}

		public void BeginDrawingShadowCasters()
		{
			graphicsDevice.SetRenderTarget(RenderTarget);
			graphicsDevice.Clear(Color.Transparent);
		}

		public void EndDrawingShadowCasters()
		{
			graphicsDevice.SetRenderTarget(null);
		}
	}
}
