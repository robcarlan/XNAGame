using System.Collections;
using System.Collections.Generic;

using GameName3.Manager.Loading;

namespace GameName3
{
	public abstract class LibraryBase<T> where T : Entities.Entity {
		CollectionBase items;

		/// <summary>
		/// Observer function called when an item is added to a library
		/// </summary>
		protected virtual void onAdd(T value) {
			System.Console.WriteLine("Added object with ID " + value.getID());
		}

		public abstract void addItems(IEnumerable<T> _items);

		/// <summary>
		/// Returns a list of items ready to be serialised.
		/// </summary>
		/// <returns></returns>
		public abstract ICollection<DataLoader.IProxyObject> toSaveFormat();

		public abstract T getItem(int ID);
		public abstract int getID(T item);

	}
}
