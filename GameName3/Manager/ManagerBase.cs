using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameName3.Entities;

namespace GameName3
{
	public abstract class ManagerBase<T> where T : Entity
	{
		LibraryBase<T> library;
		GameName3.Game.Game1 game;

		/// <summary>
		/// Takes a collection of IClassLoader and adds to various libraries
		/// </summary>
		public void Initialise()
		{
		}

		public abstract void loadItems(IEnumerable<DataLoader.IProxyObject> toLoad);

		public void Messages()
		{
			throw new System.NotImplementedException();
		}
	}
}
