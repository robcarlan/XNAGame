using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName3.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameName3.Entities.Components {

	interface IComponent : IObservable<IComponent>, IObserver<IComponent>, IDisposable {

	}

	abstract class BaseComponent : IComponent {
		public List<IObserver<IComponent>> observers;

		protected Game1 game;
		protected int entityID;

		public Entity getEntity();

		public abstract void Update(Game1 game);
		public IDisposable Subscribe(IObserver<IComponent> observer) {
			if (!observers.Contains(observer))
				observers.Add(observer);
			//TODO :: Might not be correct
			return this;
		}

		public void OnCompleted() {
			throw new NotImplementedException();
		}

		public void OnError(Exception error) {
			throw new NotImplementedException();
		}

		public void OnNext(IComponent value) {
			throw new NotImplementedException();
		}

		public void Dispose() {
			throw new NotImplementedException();
		}
	}
}
