using System.Collections;
using System.Collections.Generic;

using WindowsGame1.Manager.Loading;

namespace WindowsGame1
{
	public abstract class LibraryBase<T> where T : Entities.Entity {
		CollectionBase items;

		/// <summary>
		/// Observer function called when an item is added to a library
		/// </summary>
		void onAdd(T value);

		public void addItems(IEnumerable<T> _items);

		/// <summary>
		/// Returns a list of items ready to be serialised.
		/// </summary>
		/// <returns></returns>
		public CollectionBase toSaveFormat();

		public T getItem(int ID);
		public int getID(T item);

	}
}
