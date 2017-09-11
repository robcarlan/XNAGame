using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameName3.Entities.Components;

namespace GameName3.Entities {
	public class GraphicalEntity : Entity, IRenderable {
		Components.Render.RenderComponent render;
		//TODO :: render registers to update? So on updates we can update frame positions sounds good
		Components.Update.UpdateComponent update;

		public void IRenderable.Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite) {
			
		}

		public bool IRenderable.isVisible() {
			throw new NotImplementedException();
		}
	}
}
