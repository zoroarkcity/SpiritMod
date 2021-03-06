﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class MoonLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Lightning");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.damage = 1;
			projectile.penetrate = 8;
			projectile.alpha = 200;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
		}

	}
}