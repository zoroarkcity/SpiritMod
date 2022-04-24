using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using SpiritMod.Utilities;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System.Collections.Generic;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Mechanics.Trails.CustomTrails;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Sets.FlailsMisc.JadeDao
{
	public class JadeDao : ModItem
	{
		public int combo;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Jade Daos");

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 32;
			item.useAnimation = 32;
			item.shootSpeed = 1f;
			item.knockBack = 4f;
			item.UseSound = SoundID.Item116;
			item.shoot = ModContent.ProjectileType<JadeDaoProj>();
			item.value = Item.sellPrice(gold: 10);
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.autoReuse = true;
			item.melee = true;
			item.damage = 70;
			item.rare = ItemRarityID.LightPurple;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			combo++;

			float distanceMult = Main.rand.NextFloat(0.8f, 1.2f);
			float curvatureMult = 0.7f;

			bool slam = combo % 5 == 4;

			Vector2 direction = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
			Projectile proj = Projectile.NewProjectileDirect(position, direction, type, damage, knockBack, player.whoAmI);
			if (proj.modProjectile is JadeDaoProj modProj)
			{
				modProj.SwingTime = (int)(item.useTime * UseTimeMultiplier(player) * (slam ? 1.75f : 1));
				modProj.SwingDistance = player.Distance(Main.MouseWorld) * distanceMult;
				modProj.Curvature = 0.33f * curvatureMult;
				modProj.Flip = combo % 2 == 1;
				modProj.Slam = slam;
				modProj.PreSlam = combo % 5 == 3;
			}
			if (slam)
			{
				Projectile proj2 = Projectile.NewProjectileDirect(position, direction, type, damage, knockBack, player.whoAmI);
				if (proj2.modProjectile is JadeDaoProj modProj2)
				{
					modProj2.SwingTime = (int)(item.useTime * UseTimeMultiplier(player) * (slam ? 1.75f : 1));
					modProj2.SwingDistance = player.Distance(Main.MouseWorld) * distanceMult;
					modProj2.Curvature = 0.33f * curvatureMult;
					modProj2.Flip = combo % 2 == 0;
					modProj2.Slam = slam;
				}
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj2.whoAmI);
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			return false;
		}

		public override float UseTimeMultiplier(Player player) => player.meleeSpeed; //Scale with melee speed buffs, like whips
	}

	public class JadeDaoProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanguine Flayer");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.Size = new Vector2(85, 85);
			projectile.tileCollide = false;
			projectile.ownerHitCheck = true;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
		}

		private Player Owner => Main.player[projectile.owner];

		public int SwingTime;
		public float SwingDistance;
		public float Curvature;

		public ref float Timer => ref projectile.ai[0];
		public ref float AiState => ref projectile.ai[1];


		private Vector2 returnPosOffset; //The position of the projectile when it starts returning to the player from being hooked
		private Vector2 npcHookOffset = Vector2.Zero; //Used to determine the offset from the hooked npc's center
		private float npcHookRotation; //Stores the projectile's rotation when hitting an npc
		private NPC hookNPC; //The npc the projectile is hooked into

		public const float THROW_RANGE = 320; //Peak distance from player when thrown out, in pixels
		public const float HOOK_MAXRANGE = 700; //Maximum distance between owner and hooked enemies before it automatically rips out
		public const int HOOK_HITTIME = 30; //Time between damage ticks while hooked in
		public const int RETURN_TIME = 6; //Time it takes for the projectile to return to the owner after being ripped out

		private int _flashTime;

		public bool Flip = false;
		public bool Slam = false;
		public bool PreSlam = false;

		private List<float> oldRotation = new List<float>();
		private List<Vector2> oldBase = new List<Vector2>();

		public Vector2 CurrentBase = Vector2.Zero;

		private int slamTimer = 0;

		public override void AI()
		{
			if (projectile.timeLeft > 2) //Initialize chain control points on first tick, in case of projectile hooking in on first tick
			{
				_chainMidA = projectile.Center;
				_chainMidB = projectile.Center;

				SpiritMod.primitives.CreateTrail(new JadeDaoBasicTrail(projectile));
			}

			Lighting.AddLight(CurrentBase, new Color(54, 192, 98).ToVector3());
			projectile.timeLeft = 2;

			if (Slam)
				Owner.itemTime = Owner.itemAnimation = 40;
			else if (PreSlam)
				Owner.itemTime = Owner.itemAnimation = 10;

			ThrowOutAI();

			if (!Slam)
				Owner.itemRotation = MathHelper.WrapAngle(Owner.AngleTo(projectile.Center) - (Owner.direction < 0 ? MathHelper.Pi : 0));
			else
				Owner.itemRotation = MathHelper.WrapAngle(Owner.AngleTo(Main.MouseWorld) - (Owner.direction < 0 ? MathHelper.Pi : 0));
			_flashTime = Math.Max(_flashTime - 1, 0);
		}

		private Vector2 GetSwingPosition(float progress)
		{
			//Starts at owner center, goes to peak range, then returns to owner center
			float distance = MathHelper.Clamp(SwingDistance, THROW_RANGE * 0.1f, THROW_RANGE) * MathHelper.Lerp((float)Math.Sin(progress * MathHelper.Pi), 1, 0.04f);
			distance = Math.Max(distance, 100); //Dont be too close to player

			float angleMaxDeviation = MathHelper.Pi / 1.2f;
			float angleOffset = Owner.direction * (Flip ? -1 : 1) * MathHelper.Lerp(-angleMaxDeviation, angleMaxDeviation, progress); //Moves clockwise if player is facing right, counterclockwise if facing left
			return projectile.velocity.RotatedBy(angleOffset) * distance;
		}

		private void ThrowOutAI()
		{
			projectile.rotation = projectile.AngleFrom(Owner.Center);
			Vector2 position = Owner.MountedCenter;
			float progress = ++Timer / SwingTime; //How far the projectile is through its swing
			if (Slam)
			{
				slamTimer++;
				progress = EaseFunction.EaseCubicInOut.Ease(progress);
				if (progress > 0.15f && progress < 0.85f)
				{
					Vector2 vel = Vector2.Zero;
					int timeLeft = Main.rand.Next(40, 100);

					StarParticle particle = new StarParticle(
					CurrentBase,
					Main.rand.NextVector2Circular(1, 1),
					Color.Lerp(new Color(106, 255, 35), new Color(18, 163, 85), Main.rand.NextFloat()),
					Main.rand.NextFloat(0.1f, 0.2f),
					timeLeft);
					particle.TimeActive = (uint)(timeLeft / 2);
					ParticleHandler.SpawnParticle(particle);
				}
			}
			else
				progress = EaseFunction.EaseQuadOut.Ease(progress);

			if (slamTimer == 5)
				Main.PlaySound(SoundID.NPCDeath7, projectile.Center);

			projectile.Center = position + GetSwingPosition(progress);
			projectile.direction = projectile.spriteDirection = -Owner.direction * (Flip ? -1 : 1);

			if (Timer >= SwingTime)
				projectile.Kill();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.timeLeft > 2)
				return false;

			Texture2D projTexture = Main.projectileTexture[projectile.type];
			Texture2D glowTexture = ModContent.GetTexture(Texture + "_Glow");

			//End control point for the chain
			Vector2 projBottom = projectile.Center + new Vector2(0, projTexture.Height / 2).RotatedBy(projectile.rotation + MathHelper.PiOver2) * 0.75f;
			DrawChainCurve(spriteBatch, projBottom, out Vector2[] chainPositions);

			//Adjust rotation to face from the last point in the bezier curve
			float newRotation = (projBottom - chainPositions[chainPositions.Length - 2]).ToRotation() + MathHelper.PiOver2;

			//Draw from bottom center of texture
			Vector2 origin = new Vector2(projTexture.Width / 2, projTexture.Height);
			SpriteEffects flip = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			lightColor = Lighting.GetColor((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f));

			spriteBatch.Draw(projTexture, projBottom - Main.screenPosition, null, lightColor, newRotation, origin, projectile.scale, flip, 0);

			spriteBatch.Draw(glowTexture, projBottom - Main.screenPosition, null, Color.White, newRotation, origin, projectile.scale, flip, 0);


			CurrentBase = projBottom + (newRotation - 1.57f).ToRotationVector2() * (projTexture.Height / 2);

			oldBase.Add(projBottom - Main.screenPosition);

			if (oldBase.Count > 8)
				oldBase.RemoveAt(0);

			if (!Slam)
				return false;

			Texture2D whiteTexture = ModContent.GetTexture(Texture + "_White");
			if (slamTimer < 20 && slamTimer > 5)
			{
				float progress = (slamTimer - 5) / 15f;
				float transparency = (float)Math.Pow(1 - progress, 2);
				float scale = 1 + progress;
				spriteBatch.Draw(whiteTexture, projBottom - Main.screenPosition, null, Color.White * transparency, newRotation, origin, projectile.scale * scale, flip, 0);
			}
			return false;
		}

		//Control points for drawing chain bezier, update slowly when hooked in
		private Vector2 _chainMidA;
		private Vector2 _chainMidB;
		private void DrawChainCurve(SpriteBatch spriteBatch, Vector2 projBottom, out Vector2[] chainPositions)
		{
			Texture2D chainTex = ModContent.GetTexture(Texture + "_Chain");

			float progress = Timer / SwingTime;

			if (Slam)
				progress = EaseFunction.EaseCubicInOut.Ease(progress);
			else
				progress = EaseFunction.EaseQuadOut.Ease(progress);

			float angleMaxDeviation = MathHelper.Pi * 0.85f;
			float angleOffset = Owner.direction * (Flip ? -1 : 1) * MathHelper.Lerp(angleMaxDeviation, -angleMaxDeviation / 4, progress);

			_chainMidA = Owner.MountedCenter + GetSwingPosition(progress).RotatedBy(angleOffset) * Curvature;
			_chainMidB = Owner.MountedCenter + GetSwingPosition(progress).RotatedBy(angleOffset / 2) * Curvature * 2.5f;

			BezierCurve curve = new BezierCurve(new Vector2[] { Owner.MountedCenter, _chainMidA, _chainMidB, projBottom });

			int numPoints = 30; //Should make dynamic based on curve length, but I'm not sure how to smoothly do that while using a bezier curve
			chainPositions = curve.GetPoints(numPoints).ToArray();

			//Draw each chain segment, skipping the very first one, as it draws partially behind the player
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];

				float rotation = (chainPositions[i] - chainPositions[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
				float yScale = Vector2.Distance(chainPositions[i], chainPositions[i - 1]) / chainTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points

				Vector2 scale = new Vector2(1, yScale); // Stretch/Squash chain segment
				Color chainLightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16); //Lighting of the position of the chain segment
				Vector2 origin = new Vector2(chainTex.Width / 2, chainTex.Height); //Draw from center bottom of texture
				spriteBatch.Draw(chainTex, position - Main.screenPosition, null, chainLightColor, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			BezierCurve curve = new BezierCurve(new Vector2[] { Owner.MountedCenter, _chainMidA, _chainMidB, projectile.Center });

			int numPoints = 32;
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();
			float collisionPoint = 0;
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];
				Vector2 previousPosition = chainPositions[i - 1];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), position, previousPosition, 6, ref collisionPoint))
					return true;
			}
			return base.Colliding(projHitbox, targetHitbox);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Slam)
			{
				crit = true;
			}
			if (Collision.CheckAABBvAABBCollision(target.position, target.Size, projectile.position, projectile.Size))
			{
				damage = (int)(damage * 1.5f);
				for (int i = 0; i < 8; i++)
				{
					Vector2 vel = Main.rand.NextFloat(6.28f).ToRotationVector2();
					vel *= Main.rand.NextFloat(2, 5);
					ImpactLine line = new ImpactLine(target.Center - (vel * 10), vel, Color.Green, new Vector2(0.25f, Main.rand.NextFloat(0.75f, 1.75f) * 1.5f), 70);
					line.TimeActive = 30;
					ParticleHandler.SpawnParticle(line);

				}
				float progress = Timer / SwingTime; //How far the projectile is through its swing
				if (Slam)
					progress = EaseFunction.EaseCubicInOut.Ease(progress);

				if (Slam && progress > 0.4f && progress < 0.6f)
				{
					if (Owner.GetModPlayer<MyPlayer>().Shake < 5)
						Owner.GetModPlayer<MyPlayer>().Shake += 5;
					for (int j = 0; j < 14; j++)
					{
						int timeLeft = Main.rand.Next(20, 40);

						StarParticle particle = new StarParticle(
						target.Center,
						Main.rand.NextVector2Circular(10, 7),
						Color.Lerp(new Color(106, 255, 35), new Color(18, 163, 85), Main.rand.NextFloat()),
						Main.rand.NextFloat(0.15f, 0.3f),
						timeLeft);
						ParticleHandler.SpawnParticle(particle);
					}
				}
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SwingTime);
			writer.Write(SwingDistance);
			writer.WriteVector2(returnPosOffset);
			writer.WriteVector2(npcHookOffset);
			writer.Write(npcHookRotation);
			writer.Write(Flip);
			writer.Write(Slam);
			writer.Write(Curvature);

			if (hookNPC == default(NPC)) //Write a -1 instead if the npc isnt set
				writer.Write(-1);
			else
				writer.Write(hookNPC.whoAmI);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SwingTime = reader.ReadInt32();
			SwingDistance = reader.ReadSingle();
			returnPosOffset = reader.ReadVector2();
			npcHookOffset = reader.ReadVector2();
			npcHookRotation = reader.ReadSingle();
			Flip = reader.ReadBoolean();
			Slam = reader.ReadBoolean();
			Curvature = reader.ReadSingle();

			int whoAmI = reader.ReadInt32(); //Read the whoami value sent
			if (whoAmI == -1) //If its a -1, sync that the npc hasn't been set yet
				hookNPC = default;
			else
				hookNPC = Main.npc[whoAmI];
		}
	}
}