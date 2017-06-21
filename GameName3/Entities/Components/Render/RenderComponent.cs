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
using GameName3.Render;

namespace GameName3.Entities.Components.Render {
	public abstract class RenderComponent : BaseComponent, IRenderable {
		//Render by getting the local space
		protected Vector2 halfSize;

		public RenderComponent(GraphicalEntity entity, GameRender render);
		public RenderComponent();

		public abstract virtual void IRenderable.Draw(ComponentRenderer render) {
			//Implemented by all subclasses
			throw new NotImplementedException();
		}

		public bool IRenderable.isVisible() {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Called when the renderer recieves a message from the update component - for example when it's position has changed.
		/// </summary>
		/// <param name="updateComponent">The observable instance</param>
		public virtual void onNext(Components.Update.UpdateComponent updateComponent) {
		}

	}
}