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


namespace WindowsGame1.Abilities
{
    public class ProjectileAbility : AbilityBase
	{
        Vector2 target;
        Vector2 source;
        Vector2 pos;

        Vector2 direction;
        float speed;
        float maxAngleIncrease;

        bool homing;    //If homing, the ability chases the target, else goes in one continuous direction
        public List<short> onDeathParticleID = new List<short>();
        public List<short> onHitEffectsID = new List<short>();

        public ProjectileAbility( ref Vector2 target, ref Vector2 source, ProjectileAbility temp )
            : base(temp)
        {
            this.target = target;
            this.source = source;
            direction = Vector2.Normalize(target - source);

            speed = temp.speed;
            homing = temp.homing;
            timed = temp.timed;
            duration = temp.duration;
            onDeathParticleID = temp.onDeathParticleID;
        }

        public override bool update(float msPassed)
        {
            bool result = base.update(msPassed);

            //Update position
            if (homing)
            {
                //Alter direction
            }
            else
            {
                //Carry on in same direction
                pos.X += direction.X * speed;
                pos.Y += direction.Y * speed;
            }

            if (Functions.getLengthSqrd(source, pos) > Declaration.AbilityMaxRange)
                result = true;

            return result;
        }
	}
}
