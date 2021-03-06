﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Projectiles.Clubs
{
	public class BoneShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Axe Fire");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.damage = 1;
			projectile.penetrate = -1;
			projectile.alpha = 255;
            projectile.timeLeft = 3;
			projectile.tileCollide = true;
            projectile.ignoreWater = true;
		}

		//projectile.ai[0]: how many more pillars. Each one is one less
		//projectile.ai[1]: 0: center, -1: going left, 1: going right
		bool activated = false;
		float startposY = 0;
		public override bool PreAI()
		{
			if (startposY == 0) {
				startposY = projectile.position.Y;
                if (Main.tile[(int)projectile.Center.X / 16, (int)(projectile.Center.Y / 16)].collisionType == 1)
                {
                    projectile.active = false;
                }
			}
			projectile.velocity.X = 0;
			if (!activated) {
				projectile.velocity.Y = 24;
			}
			else {
				projectile.velocity.Y = -3;
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height * 2, DustType<Dusts.FloranClubDust>());
                    Main.dust[dust].scale *= Main.rand.NextFloat(.65f, .9f);
					//Main.dust[dust].velocity = Vector2.Zero;
					//Main.dust[dust].noGravity = true;
				}
				if (projectile.timeLeft == 5 && projectile.ai[0] > 0) {
					if (projectile.ai[1] == -1 || projectile.ai[1] == 0) {
						Projectile.NewProjectile(projectile.Center.X - projectile.width, startposY, 0, 0, ModContent.ProjectileType<BoneShockwave>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0] - 1, -1);
					}
					if (projectile.ai[1] == 1 || projectile.ai[1] == 0) {
						Projectile.NewProjectile(projectile.Center.X + projectile.width, startposY, 0, 0, ModContent.ProjectileType<BoneShockwave>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0] - 1, 1);
					}
				}
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != projectile.velocity.Y && !activated) {
				startposY = projectile.position.Y;
				projectile.velocity.Y = -2;
				activated = true;
				projectile.timeLeft = 10;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = false;
			return true;
		}

	}
}