using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataLoader;

namespace GameName3.Manager.Loading {
	
	//Used by a library
	public interface ILoadable {
		IProxyObject getLoadType();
		/// <summary>
		/// Converts a loadable type into its proper class counterpart
		/// </summary>
		/// <param name="contents"></param>
		Entities.Entity Load(IProxyObject contents);

		/// <summary>
		/// Returns the serialisable form of the entity
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		IProxyObject Save(Entities.Entity obj);
	}
}
