using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Prim;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System.IO;
using System.Linq;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechSword
{
	public class GranitechSaber_Hologram : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Technobrand");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}


		public int SwingTime; //Total time for weapon to be used
		public Vector2 InitialVelocity = Vector2.Zero; //Starting velocity, used for determining swing arc direction
		public Vector2 BasePosition = Vector2.Zero;
		public float SwingRadians;

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(88, 92);
			projectile.friendly = true;
			projectile.melee = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
			projectile.netUpdate = true;
			projectile.ownerHitCheck = true;
			projectile.hide = true;
		}

		private ref float SwingDirection => ref projectile.ai[0];
		private ref float Timer => ref projectile.ai[1];

		private int _hitTimer = 0;
		private const int MAX_HITTIMER = 10;

		public override void AI()
		{
			projectile.timeLeft = 2;

			_hitTimer = Math.Max(_hitTimer - 1, 0);
			float progress = Timer / SwingTime;
			progress = EaseFunction.EaseCircularInOut.Ease(progress);
			projectile.velocity = InitialVelocity.RotatedBy(MathHelper.Lerp(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection, progress));

			//Get current distance from base spinning point, then use that to calculate new position
			float distance = Vector2.Distance(projectile.Center, BasePosition);
			projectile.Center = BasePosition + projectile.velocity * distance;

			projectile.alpha = (int)MathHelper.Lerp(0, 200, 1 - (float)Math.Sin((Timer / SwingTime) * MathHelper.Pi));

			projectile.direction = projectile.spriteDirection = (projectile.Center.X < BasePosition.X) ? -1 : 1;

			projectile.rotation = projectile.velocity.ToRotation() - (projectile.spriteDirection < 0 ? MathHelper.Pi : 0);
			projectile.rotation += MathHelper.PiOver4 * projectile.direction;
			if (SwingDirection == projectile.direction)
			{
				projectile.rotation += MathHelper.PiOver2 * projectile.direction;
				projectile.direction = projectile.spriteDirection *= -1;
			}

			/*if (!Main.dedServ && projectile.oldPos[0] != Vector2.Zero)
			{
				int numParticles = Main.rand.Next(0, 2); //0-1
				for (int i = 0; i < numParticles; i++)
				{
					Vector2 position = BasePosition + projectile.velocity * (distance + Main.rand.NextFloat(-projectile.Size.Length() / 2, projectile.Size.Length()/2));
					Vector2 velocity = Vector2.Normalize(projectile.oldPos[0] - projectile.position) * Main.rand.NextFloat(1.25f);
					ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, (Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247)) * projectile.Opacity, Main.rand.NextFloat(), 20));
				}
			}*/

			++Timer;
			if (Timer > SwingTime)
				projectile.Kill();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 halfLine = projectile.velocity * projectile.Size.Length() / 2;
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffect(projectile.Center, projectile.velocity);

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffect(projectile.Center, projectile.velocity);

		private void HitEffect(Vector2 position, Vector2 direction)
		{
			if (Main.dedServ)
				return;

			if(_hitTimer == 0)
			{
				_hitTimer = MAX_HITTIMER;
				ParticleHandler.SpawnParticle(new GranitechSaber_Hit(position, Main.rand.NextFloat(0.9f, 1.1f), direction.ToRotation()));
			}

			int numParticles = Main.rand.Next(5, 8);
			for (int i = 0; i < numParticles; i++)
			{
				Vector2 velocity = direction.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(3, 20);
				ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247), Main.rand.NextFloat(2f, 2.5f), 25));
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SwingTime);
			writer.WriteVector2(InitialVelocity);
			writer.WriteVector2(BasePosition);
			writer.Write(SwingRadians);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SwingTime = reader.ReadInt32();
			InitialVelocity = reader.ReadVector2();
			BasePosition = reader.ReadVector2();
			SwingRadians = reader.Read();
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (projectile.timeLeft > 2) //bandaid fix for flickering
				return;

			float opacity = projectile.Opacity;
			Effect effect = mod.GetEffect("Effects/GSaber");
			effect.Parameters["baseTexture"].SetValue(mod.GetTexture("Textures/GeometricTexture_2"));
			effect.Parameters["baseColor"].SetValue(new Color(25, 132, 247).ToVector4());
			effect.Parameters["overlayTexture"].SetValue(mod.GetTexture("Textures/GeometricTexture_1"));
			effect.Parameters["overlayColor"].SetValue(new Color(99, 255, 229).ToVector4());

			effect.Parameters["xMod"].SetValue(2); //scale with the total length of the strip
			effect.Parameters["yMod"].SetValue(2.5f);

			float slashProgress = EaseFunction.EaseCircularInOut.Ease(Timer / SwingTime);
			effect.Parameters["timer"].SetValue(Main.GlobalTime * 2);
			effect.Parameters["progress"].SetValue(slashProgress);

			Vector2 pos = BasePosition - Main.screenPosition;
			float distance = Vector2.Distance(projectile.Center, BasePosition);

			for (int i = -1; i <= 1; i++)
			{
				Color aberrationColor = Color.White;
				switch (i)
				{
					case -1:
						aberrationColor = new Color(255, 0, 0, 0);
						break;
					case 0:
						aberrationColor = new Color(0, 255, 0, 0);
						break;
					case 1:
						aberrationColor = new Color(0, 0, 255, 0);
						break;
				}

				float offset = i * 2;
				PrimitiveSlashArc slash = new PrimitiveSlashArc
				{
					BasePosition = pos,
					StartDistance = offset + distance - projectile.Size.Length()/2 * SwingDirection,
					EndDistance = offset + distance + projectile.Size.Length() / 2 * SwingDirection,
					AngleRange = new Vector2(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection),
					DirectionUnit = InitialVelocity,
					Color = aberrationColor * opacity * 0.33f,
					SlashProgress = slashProgress
				};

				PrimitiveRenderer.DrawPrimitiveShape(slash, effect);
			}

			projectile.QuickDraw(spriteBatch, drawColor: Color.White);
		}
	}
}