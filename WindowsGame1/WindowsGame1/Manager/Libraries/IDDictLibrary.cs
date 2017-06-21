using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataLoader;

namespace WindowsGame1.Manager.Libraries {
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

		public override void addItems(IEnumerable<IProxyObject> proxies, Func<IProxyObject, T> conversionMethod) {
			addItems(createEntityCollection(proxies, conversionMethod));
		}

		//Shorthand method for taking a list of proxy objects, converting them, and assigning them to a dictionary with their ID as the key.
		List<KeyValuePair<int, T>> createEntityCollection(IEnumerable<IProxyObject> proxies,
			Func<IProxyObject, T> conversionMethod) {
				
			List<T> entities = new List<T>();
			foreach (IProxyObject temp in proxies) {
				entities.Add(conversionMethod(temp));
			}

			List<KeyValuePair<int, T>> result = new List<KeyValuePair<int, T>>();
			foreach (T entity in entities) {
				KeyValuePair<int, T> kvp = new KeyValuePair<int, T>(entity.getID(), entity);
			}

			return result;
		}
	}
}
