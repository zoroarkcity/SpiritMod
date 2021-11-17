using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.QuasarGauntlet
{
	public class QuasarOrb : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Orb");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.Size = Vector2.One * 32;
			projectile.tileCollide = true;
			projectile.hide = true;
			//projectile.scale = 0.8f;
			projectile.extraUpdates = 1;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
			projectile.scale = 0f;
			projectile.alpha = 255;
		}

		private const float MAXPOWER = 1.5f; //The maximum size and damage multiplier
		private const float STARTPOWER = 1f; //The starting size and damage multiplier
		private const int MAXPOWERHITS = 3; //The amount of times the projectile needs to hit an enemy to reach max power
		private float power = STARTPOWER;

		public ref float AiState => ref projectile.ai[0];
		private ref float Timer => ref projectile.ai[1];

		public const int STATE_SLOWDOWN = 0;
		public const int STATE_RETURN = 1;
		private const int SLOWDOWN_TIME = 360;
		private bool dying;
		private float scaleMod;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => projectile.Distance(targetHitbox.Center.ToVector2()) < (50 * (projectile.scale + 0.4f)); //circular collision
		public override void AI()
		{
			projectile.rotation += 0.05f * (Math.Sign(projectile.velocity.X) > 0 ? 1 : -1);
			switch (AiState)
			{
				case STATE_SLOWDOWN:

					projectile.alpha = Math.Max(projectile.alpha - 25, 0);
					scaleMod = MathHelper.Lerp(scaleMod, 1f, 0.1f);
					projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.03f);
					if (++Timer > SLOWDOWN_TIME)
					{
						AiState = STATE_RETURN;
						projectile.netUpdate = true;
					}
					break;

				case STATE_RETURN:

					Player player = Main.player[projectile.owner];
					projectile.tileCollide = false;

					Vector2 desiredPosition = player.MountedCenter;
					projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Lerp(projectile.Center, desiredPosition, dying ? 0.13f : 0.07f) - projectile.Center, dying ? 0.15f : 0.18f);

					float maxSpeed = 18;
					float fadeDistance = 100;
					int fadeTime = 20;

					//cap max velocity
					if (projectile.velocity.Length() > maxSpeed)
						projectile.velocity = Vector2.Normalize(projectile.velocity) * maxSpeed;

					//fade out when close to player
					if (projectile.Distance(player.MountedCenter) < fadeDistance || dying)
					{
						if (!dying)
						{
							dying = true;
							projectile.netUpdate = true;
						}

						projectile.alpha += 255 / fadeTime;
						scaleMod = MathHelper.Lerp(scaleMod, 0f, 0.1f);
						if (projectile.alpha >= 255)
							projectile.Kill();
					}
					break;
			}

			power = MathHelper.Clamp(power, STARTPOWER, MAXPOWER);

			projectile.scale = MathHelper.Lerp(projectile.scale, power * scaleMod, 0.05f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			float speedboost = -1 - (0.5f / oldVelocity.Length());
			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = oldVelocity.X * speedboost;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = oldVelocity.Y * speedboost;

			Main.PlaySound(SoundID.Item10, projectile.Center);
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => IncreaseDamage(ref damage);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => IncreaseDamage(ref damage);

        private void IncreaseDamage(ref int damage)
        {
            damage = (int)(damage * power);

            if (power < MAXPOWER)
			{
				projectile.netUpdate = true;
				power += (MAXPOWER - STARTPOWER) / MAXPOWERHITS;
			}
        }

		public void AdditiveCall(SpriteBatch sB)
		{
			Color White = new Color(255, 251, 199);
			Color Yellow = new Color(255, 247, 0) * 0.9f;
			Color Red = new Color(255, 5, 30) * 0.8f;


			float rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			Vector2 scale = new Vector2(1f - projectile.velocity.Length() / 60, 1f + projectile.velocity.Length() / 60) * projectile.scale;

			Vector2 drawCenter = projectile.Center - Main.screenPosition;
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			sB.Draw(bloom, drawCenter, null, Color.Magenta * projectile.Opacity, rotation, bloom.Size() / 2, scale * 0.68f, SpriteEffects.None, 0);

			sB.End(); sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, RasterizerState.CullNone, default, Main.GameViewMatrix.ZoomMatrix);
			Texture2D tex = Main.projectileTexture[projectile.type];

			Effect effect = SpiritMod.Instance.GetEffect("Effects/PortalShader");
			effect.Parameters["PortalNoise"].SetValue(mod.GetTexture("Utilities/Noise/SpiralNoise"));
			effect.Parameters["Timer"].SetValue(MathHelper.WrapAngle(Main.GlobalTime / 3));
			effect.Parameters["DistortionStrength"].SetValue(0);
			effect.Parameters["Rotation"].SetValue(projectile.rotation);
			effect.CurrentTechnique.Passes[0].Apply();

			sB.Draw(tex, drawCenter, null, Red * projectile.Opacity, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
			sB.Draw(tex, drawCenter, null, Yellow * projectile.Opacity, rotation, tex.Size() / 2, scale * 0.85f, SpriteEffects.None, 0);
			sB.Draw(tex, drawCenter, null, White * projectile.Opacity, rotation, tex.Size() / 2, scale * 0.7f, SpriteEffects.None, 0);

			sB.End(); sB.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, RasterizerState.CullNone, default, Main.GameViewMatrix.ZoomMatrix);

			float blurLength = 200 * projectile.scale;
			float blurWidth = 12 * projectile.scale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTime * 10) % 1) * 0.1f) + 0.9f;
			Effect blurEffect = mod.GetEffect("Effects/BlurLine");

			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = projectile.Center - Main.screenPosition,
				Height = blurWidth * flickerStrength,
				Length = blurLength * flickerStrength,
				Rotation = 0,
				Color = White * flickerStrength * projectile.Opacity
			};
			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);

			Effect circleAA = mod.GetEffect("Effects/CirclePrimitiveAA");
			CirclePrimitive circle = new CirclePrimitive()
			{
				Color = Color.Black * projectile.Opacity,
				Radius = 28 * projectile.scale,
				Position = projectile.Center - Main.screenPosition,
				MaxRadians = MathHelper.TwoPi,
				Rotation = projectile.velocity.ToRotation()
			};
			PrimitiveRenderer.DrawPrimitiveShape(circle, circleAA);

		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(power);
			writer.Write(dying);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			power = reader.ReadSingle();
			dying = reader.ReadBoolean();
		}
	}
}