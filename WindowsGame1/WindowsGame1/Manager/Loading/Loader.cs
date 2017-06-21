using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WindowsGame1.Entities;
using DataLoader;

namespace WindowsGame1.Manager.Loading {
	abstract class Loader<T, U> where U : DataLoader.IProxyObject {
		public Loader();
		public abstract T toObject(U proxy);
		public abstract U toProxy(T data);
	}
}
