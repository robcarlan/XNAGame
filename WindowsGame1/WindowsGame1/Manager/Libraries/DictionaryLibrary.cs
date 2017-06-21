using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Manager.Libraries {
	class DictionaryLibrary<K, T> : LibraryBase<T> {
		protected Dictionary<K, T> items = new Dictionary<K, T>();

		public void addItems(IEnumerable<KeyValuePair<K,T>> _items) {
			foreach (KeyValuePair<K, T> kvp in _items) {
				try {
					items.Add(kvp.Key, kvp.Value);
				} catch (Exception ex) {
					System.Console.WriteLine("Uhoh : " + ex.Message + "\n");
				}
			}
		}

		void onAdd(T value) {

		}

		public int getLength() { return items.Count; }
	}
}
