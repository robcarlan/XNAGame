using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Collections.Generic;
using System;

using DataLoader;
using GameName3.Game;
using GameName3.Manager.Loading;

namespace GameName3.Entities {
	public interface IRenderable {
		public void Draw(SpriteBatch sprite);
		public bool isVisible();
	}

	//Should all entities be loadable? I say yes
	public abstract class Entity : IUpdateable, IRenderable, IDisposable {

		int ID;
		public Game1 game;

		/// <summary>
		/// Position in world space
		/// </summary>
		public Vector2 position;

		public int getID() { return ID;  }

		public Entity() { }

		#region Disposable
		void IDisposable.Dispose() {
			throw new NotImplementedException();
		}
		#endregion

		#region Drawable
		public void IRenderable.Draw(SpriteBatch sprite) { }
		public void IRenderable.isVisible() { }
		#endregion

		#region Updateable
		bool IUpdateable.Enabled {
			get { throw new NotImplementedException(); }
		}

		event EventHandler<EventArgs> IUpdateable.EnabledChanged {
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}

		void IUpdateable.Update(GameTime gameTime) {
			throw new NotImplementedException();
		}

		int IUpdateable.UpdateOrder {
			get { throw new NotImplementedException(); }
		}

		event EventHandler<EventArgs> IUpdateable.UpdateOrderChanged {
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}
		#endregion
	}

}
