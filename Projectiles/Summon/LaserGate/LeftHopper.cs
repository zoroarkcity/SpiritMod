using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Dusts;

namespace SpiritMod.Projectiles.Summon.LaserGate
{
	public class LeftHopper : ModProjectile
	{
		Vector2 direction9 = Vector2.Zero;
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Left Gate");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 600;
			projectile.tileCollide = false;
			projectile.alpha = 0;
		}
		public override bool PreAI()
		{
			projectile.timeLeft = 50;
			int rightValue = (int)projectile.ai[1];
			if (rightValue < (double)Main.projectile.Length && rightValue != 0) {
				Projectile other = Main.projectile[rightValue];
				if (other.active) {
					//rotating
					direction9 = other.Center - projectile.Center;
					int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
					direction9.Normalize();
					other.ai[1] = projectile.whoAmI;
					//shoot to other guy
					timer++;
					if (timer > 4 && distance < 300) {
						timer = 0;
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 15, (float)direction9.Y * 15, ModContent.ProjectileType<GateLaser>(), 14, 0, Main.myPlayer);
						Main.projectile[proj].timeLeft = (int)(distance / 15) - 1;
						DustHelper.DrawElectricity(projectile.Center, other.Center, 226, 0.3f);
					}
				}
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
				Color color1 = Lighting.GetColor((int)(projectile.position.X + projectile.width * 0.5) / 16, (int)((projectile.position.Y + projectile.height * 0.5) / 16.0));
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				int r1 = color1.R;
				drawOrigin.Y += 34f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;
				Vector2 position1 = projectile.Bottom - Main.screenPosition;
				Texture2D texture2D2 = Main.glowMaskTexture[239];
				float num11 = (float)(Main.GlobalTime % 1.0 / 1.0);
				float num12 = num11;
				if (num12 > 0.5)
					num12 = 1f - num11;
				if (num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)((num11 + 0.5) % 1.0);
				float num14 = num13;
				if (num14 > 0.5)
					num14 = 1f - num13;
				if (num14 < 0.0)
					num14 = 0.0f;
				Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(3f, -6f);
				Color color3 = new Color(84, 207, 255) * 1.6f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, projectile.rotation, drawOrigin, projectile.scale * 0.33f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, projectile.rotation, drawOrigin, projectile.scale * 0.33f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, projectile.rotation, drawOrigin, projectile.scale * 0.33f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				Texture2D texture2D3 = Main.extraTexture[89];
				Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
				drawOrigin = r3.Size() / 2f;
			}
	}
}