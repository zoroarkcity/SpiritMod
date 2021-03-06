﻿using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Boss;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	[AutoloadBossHead]
	public class ReachBoss : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		bool text = false;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vinewrath Bane");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 132;
			npc.height = 222;
			npc.damage = 28;
            npc.boss = true;
			npc.lifeMax = 2500;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.npcSlots = 20;
			npc.defense = 15;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ReachBoss");
			npc.buffImmune[20] = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[70] = true;

			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += .15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		int aitimer = 0;
		private int Counter;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			if (player.GetSpiritPlayer().ZoneReach) {
				npc.defense = 25;
				npc.damage = 45;
			}
			else {
				npc.defense = 14;
				npc.damage = 28;
			}
			Counter++;
			if (npc.life >= (npc.lifeMax / 3)) {
				if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
					moveSpeed--;
				else if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
					moveSpeed++;

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - 30f && moveSpeedY >= -20) //Flies to players Y position
					moveSpeedY--;
				else if (npc.Center.Y <= player.Center.Y - 30f && moveSpeedY <= 20)
					moveSpeedY++;
			}
			else {
				int num621 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 235, 0f, 0f, 100, default(Color), 2f);
				Main.dust[num621].noGravity = true;
				if (npc.Center.X >= player.Center.X && moveSpeed >= -50) // flies to players x position
					moveSpeed--;
				else if (npc.Center.X <= player.Center.X && moveSpeed <= 50)
					moveSpeed++;

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - 50f && moveSpeedY >= -30) //Flies to players Y position
					moveSpeedY--;
				else if (npc.Center.Y <= player.Center.Y - 50f && moveSpeedY <= 30)
					moveSpeedY++;
			}
			aitimer++;
			npc.velocity.Y = moveSpeedY * 0.1f;
			if (aitimer >= 540 && aitimer < 560) {
				npc.SimpleFlyMovement(npc.DirectionTo(player.Center + new Vector2((float)((double)npc.direction * 1000), player.Center.Y + .001f)) * 25.5f, 1.8f);
				npc.direction = npc.spriteDirection = (double)npc.Center.X < (double)player.Center.X ? 1 : -1;
			}
			int npcType = ModContent.NPCType<SunFlower>();
			bool plantAlive = false;
			for (int num569 = 0; num569 < 200; num569++) {
				if ((Main.npc[num569].active && Main.npc[num569].type == (npcType)))
					plantAlive = true;
			}
			if (!plantAlive) {
				if (Counter > 2000) {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(100));
                        int newNPC = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SunFlower>());
                        int newNPC1 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SunFlower>());
                        int newNPC2 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SunFlower>());
                        int newNPC3 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SunFlower>());
                        int newNPC4 = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<SunFlower>());
                        Main.npc[newNPC].velocity = direction * (Main.rand.Next(4, 6));
                        Main.npc[newNPC1].velocity = direction * (Main.rand.Next(-7, 11));
                        Main.npc[newNPC2].velocity = direction * (Main.rand.Next(12, 15));
                        Main.npc[newNPC3].velocity = direction * (Main.rand.Next(3, 7));
                        Main.npc[newNPC4].velocity = direction * (Main.rand.Next(-5, 8));
                        Counter = 0;
                    }
				}
			}

			bool expertMode = Main.expertMode;
			if (Main.rand.Next(195) == 1 && npc.life >= (npc.lifeMax / 3)) {
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 12f;
                    direction.Y *= 12f;

                    int amountOfProjectiles = 1;
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                        int damage = expertMode ? 13 : 17;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BouncingSpore>(), damage, 1, Main.myPlayer, 0, 0);

                    }
                }
			}
			else if (Main.rand.Next(210) == 3 && npc.life <= (npc.lifeMax / 3)) {

				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 12f;
                    direction.Y *= 12f;

                    int amountOfProjectiles = 1;
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                        int damage = expertMode ? 14 : 20;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BouncingSpore>(), damage, 1, Main.myPlayer, 0, 0);

                    }
                }
			}

			if (Main.rand.Next(240) == 0 && npc.life >= (npc.lifeMax / 2)) {

				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 14f;
                    direction.Y *= 14f;

                    int amountOfProjectiles = Main.rand.Next(3, 5);
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                        int damage = expertMode ? 11 : 16;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BossSpike>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
			}
			if (Main.rand.Next(190) == 0 && npc.life >= (npc.lifeMax / 3) && npc.life <= (npc.lifeMax / 2)) {
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 14f;
                    direction.Y *= 14f;

                    int amountOfProjectiles = Main.rand.Next(3, 5);
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-200, 200) * 0.05f;
                        float B = (float)Main.rand.Next(-200, 200) * 0.05f;
                        int damage = expertMode ? 11 : 16;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BossSpike>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }

			}
			else if (Main.rand.Next(28) == 1 && npc.life <= (npc.lifeMax / 3)) {
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 12f;
                    direction.Y *= 12f;

                    int amountOfProjectiles = 1;
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = (float)Main.rand.Next(-100, 100) * 0.01f;
                        float B = (float)Main.rand.Next(-100, 100) * 0.01f;
                        int damage = expertMode ? 12 : 20;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BossRedSpike>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
			}
			else {

			}
			return true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (!player.active || player.dead) {
				npc.TargetClosest(false);
				npc.velocity.Y = -2000;
			}
			npc.spriteDirection = npc.direction;
			npc.ai[1]++;
			if (npc.ai[1] >= 300) {
				npc.ai[0] = 1;
				npc.ai[1] = 60;
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				npc.netUpdate = true;
			}
			// Rage Phase Switch
			if (npc.life <= 9000) {
				npc.ai[0] = 2;
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				npc.netUpdate = true;
			}
			if (npc.ai[0] == 1) // Charging.
			{
				npc.ai[1]++;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					if (npc.ai[1] % 45 == 0) {
						npc.TargetClosest(true);
						float speed = 10 + (2 * (int)(npc.life / 5000));
						Vector2 vector2_1 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float dirX = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector2_1.X;
						float dirY = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector2_1.Y;
						float targetVel = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;

						float speedMultiplier = targetVel + (10f - targetVel);
						if (speedMultiplier < 6.0)
							speedMultiplier = 6f;
						if (speedMultiplier > 16.0)
							speedMultiplier = 16f;
						float speedX = dirX - Main.player[npc.target].velocity.X * speedMultiplier;
						float speedY = dirY - (Main.player[npc.target].velocity.Y * speedMultiplier / 4);
						speedX = speedX * (float)(1 + Main.rand.Next(-10, 11) * 0.01);
						speedY = speedY * (float)(1 + Main.rand.Next(-10, 11) * 0.01);
						float speedLength = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float actualSpeed = speed / speedLength;
						npc.velocity.X = speedX * actualSpeed;
						npc.velocity.Y = speedY * actualSpeed;
						npc.velocity.X = npc.velocity.X + Main.rand.Next(-40, 41) * 0.1f;
						npc.velocity.Y = npc.velocity.Y + Main.rand.Next(-40, 41) * 0.1f;
						npc.netUpdate = true;
					}
				}
				if (npc.ai[1] >= 270) {
					npc.ai[0] = 0;
					npc.ai[1] = 0;
					npc.ai[2] = 0;
					npc.ai[3] = 0;
					npc.velocity *= 0.3F;
					npc.netUpdate = true;
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && npc.life <= 0) {
				if (!text) {
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(0, 200, 80, 100),
					"You cannot stop the wrath of nature!");
					text = true;
				}
				Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height / 2f);
				NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType("ReachBoss1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss"), 1f);

				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ReachBoss1"), 1f);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 2, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
	}
}
