using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{

	public class PurpleBullet2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic Bullet");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 20;
			projectile.aiStyle = -1;
			projectile.hide = true;
			projectile.alpha = 255;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 540;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			for (int i = 0; i < 10; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;
				int num = Dust.NewDust(new Vector2(x, y), 2, 2, 173);
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 14);
			Projectile.NewProjectile(projectile.Center, Vector2.Zero,
				ModContent.ProjectileType<Wrath>(), projectile.damage / 2, projectile.knockBack, projectile.owner);

			for (int i = 0; i < 40; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 173, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}

