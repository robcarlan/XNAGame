using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameName3.Game;

namespace GameName3.Entities.Components.Update {
	/// <summary>
	/// Update component that allows for input by the user wehich is recieved as a certain message
	/// </summary>
	class InputComponent : UpdateComponent {
		Dictionary<Keys, List<Action<Game1>>> inputEvents;
		
		public InputComponent(Game1 g) {
			
		}

		public void addInputAction(Keys inputKey, Action<Game1> action) {
			if (inputEvents.ContainsKey(inputKey)) {
				//Add another action to this key
			}
		}

		//IObserver
		//public void OnNext(Input value) {
		//	throw new NotImplementedException();
		//}
	}
}
