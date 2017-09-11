using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WindowsGame1.Utility;

namespace WindowsGame1.Character_Components
{
	class Pushback
	{
		Vector2 direction;
		float duration;
		float magnitude;
		Character source;
		Character target;

		public Pushback(Character _source, Character _target, float _duration, float _magnitude)
		{
			direction = target.circleOrigin - source.circleOrigin;
			direction.Normalize();
			source = _source;
			target = _target;
			duration = _duration;
			magnitude = _magnitude;
		}

		public void Update(float time)
		{
			if (duration <= time)
			{
				//Move by time

				//set 0, remove
			}
			else
			{
				//Move by time 

				//Update time
			}
		}
	}
}
