using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.Generic;

namespace WindowsGame1
{
	public abstract class ManagerBase<T>
	{
		LibraryBase<T> library;

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
