﻿
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class SoulNet : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Net");
		}

		public override void SetDefaults()
		{
			projectile.width = 2000;       //projectile width
			projectile.height = 2000;  //projectile height
			projectile.friendly = true;      //make that the projectile will not damage you
			projectile.melee = true;         // 
			projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
			projectile.penetrate = 1;      //how many npc will penetrate
			projectile.timeLeft = 120;   //how many time projectile projectile has before disepire
			projectile.light = 0.75f;    // projectile light
			projectile.extraUpdates = 1;
			projectile.alpha = 255;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 0 : 0), player.position.Y + 0);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)

			Rectangle rect = new Rectangle((int)projectile.Center.X, (int)projectile.position.Y, 2000, 2000);
			for (int index1 = 0; index1 < 200; index1++) {
				if (rect.Contains(Main.npc[index1].Center.ToPoint()))
					Main.npc[index1].AddBuff(mod.BuffType("DeathWreathe1"), 300);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		//public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		//{
		//	target.AddBuff(ModContent.BuffType<Slow>(), 240); 
		//	damage = 0;
		//}
	}
}
