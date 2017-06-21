using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WindowsGame1.Entities.Components;

namespace WindowsGame1.Entities {
	class TileGraphicalEntity {
		Components.Render.RenderComponent render;
		//TODO :: render registers to update? So on updates we can update frame positions sounds good
		Components.Update.UpdateComponent update;
	}
}
