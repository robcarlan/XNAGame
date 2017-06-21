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
using WindowsGame1.Manager.Loading;

namespace WindowsGame1.Entities {
	//Should all entities be loadable? I say yes
	abstract class Entity : IUpdateable, IDrawable, IDisposable {

		int ID;

		public int getID() { return ID;  }

		public Entity() { }

		//Change this to specific components - so it wont necessarily have any as a base entity
		List<Components.IComponent> components;

		#region Disposable
		void IDisposable.Dispose() {
			throw new NotImplementedException();
		}
		#endregion

		#region Drawable
		void IDrawable.Draw(GameTime gameTime) {
			throw new NotImplementedException();
		}

		int IDrawable.DrawOrder {
			get { throw new NotImplementedException(); }
		}

		event EventHandler<EventArgs> IDrawable.DrawOrderChanged {
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}

		bool IDrawable.Visible {
			get { throw new NotImplementedException(); }
		}

		event EventHandler<EventArgs> IDrawable.VisibleChanged {
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}
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
