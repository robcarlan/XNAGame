using System;
using Microsoft.Xna;
using WindowsGame1;
using WindowsGame1.Character_Components;
using WindowsGame1.Entity_Components;

namespace Scripts
{
	public static partial class Script
	{
		public static int getPlayerGold(Player player)
		{
			return 4; //player.gold
		}

		public static void someFunction(Character bob, Character target, ObjManager obj)
		{
			obj.addProjectile(2, bob, target, (float)(0));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 1 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 2 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 3 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 4 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 5 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 6 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 7 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 8 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 9 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 10 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 11 / 6));
		}

		public static void onCollision_p_sword(Projectile proj, Collidable_Sprite target, ObjManager obj)
		{
			float xDifference = target.circleOrigin.X - proj.parent.circleOrigin.X;
			float yDifference = target.circleOrigin.Y - proj.parent.circleOrigin.Y;
			obj.addProjectile(2, (Character)target, proj.parent, (float)Math.Atan2(yDifference, xDifference));
		}
	}
}