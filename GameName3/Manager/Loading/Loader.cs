using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameName3.Entities;
using DataLoader;

namespace GameName3.Manager.Loading {
	//We might not need this
	abstract class Loader<T, U> where U : DataLoader.IProxyObject {
		public abstract T toObject(U proxy);
		public abstract U toProxy(T data);
	}
}
