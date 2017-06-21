using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataLoader;

namespace GameName3.Manager.Libraries {
	//Corresponds to a list of entities which are stored by ID
	class IDDictLibrary<T> : DictionaryLibrary<int, T> where T : Entities.Entity {
		//Automatically take the ID and add it as the key
		public override void addItems(IEnumerable<T> _items) {
			foreach (T item in _items) {
				if (items.ContainsKey(item.getID())) {
					System.Console.WriteLine("Error adding item " + item.getID() + ", id already present in dictionary.\n");
				} else {
					items.Add(item.getID(), item);
				}
			}

		}

		/// <summary>
		/// Adds items of proxy type Y to a dictionary of entities X. The keys of X are the ID's provided by Y. Y must be a loader of X
		/// </summary>
		/// <typeparam name="Y">The loader type of X</typeparam>
		/// <param name="proxies"></param>
		/// <param name="conversionMethod"></param>
		public void addItems<Y>(IEnumerable<Y> proxies, Func<Y, T> conversionMethod) where Y : IProxyObject {
			addItems(createEntityCollection<Y>(proxies, conversionMethod));
		}

		//Shorthand method for taking a list of proxy objects, converting them, and assigning them to a dictionary with their ID as the key.
		List<KeyValuePair<int, T>> createEntityCollection<Y>(IEnumerable<Y> proxies,
			Func<Y, T> conversionMethod) where Y : IProxyObject {
				
			List<T> entities = new List<T>();
			foreach (Y temp in proxies) {
				entities.Add(conversionMethod(temp));
			}

			List<KeyValuePair<int, T>> result = new List<KeyValuePair<int, T>>();
			foreach (T entity in entities) {
				KeyValuePair<int, T> kvp = new KeyValuePair<int, T>(entity.getID(), entity);
			}

			return result;
		}

		public override ICollection<DataLoader.IProxyObject> toSaveFormat() {
			throw new NotImplementedException();
		}

		public override T getItem(int ID) {
			return items[ID];
		}

		public override int getID(T item) {
			throw new NotImplementedException();
		}
	}
}
