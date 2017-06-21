using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameName3.Manager.Libraries {
	class DictionaryLibrary<K, T> : LibraryBase<T> where T : Entities.Entity {
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

		public int getLength() { return items.Count; }

		public override void addItems(IEnumerable<T> _items) {
			//This implies the id's do not matter - so just generate some
			throw new NotImplementedException();
		}

		public override ICollection<DataLoader.IProxyObject> toSaveFormat() {
			throw new NotImplementedException();
		}

		public override T getItem(int ID) {
			throw new NotImplementedException();
		}

		public override int getID(T item) {
			throw new NotImplementedException();
		}
	}
}
