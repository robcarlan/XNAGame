using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1.Entities.Components.Render {
	/// <summary>
	/// A LinearAnimationComponent represents rendering one animation with a given amount of sprites. 
	/// </summary>
	class LinearAnimationComponent : RenderComponent {
		public bool cycleBackwards { get; set; }

	}
}
