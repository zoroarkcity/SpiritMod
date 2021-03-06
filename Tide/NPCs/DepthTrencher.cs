using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class DepthTrencher : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Trencher");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 32;
			npc.lifeMax = 400;
			npc.damage = 40;
			npc.defense = 14;
			npc.knockBackResist = 0f;
			npc.aiStyle = 0;
			aiType = NPCID.BoundGoblin;
			npc.noTileCollide = false;
			npc.friendly = false;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
			=> TideWorld.TheTide && spawnInfo.player.ZoneBeach && NPC.downedMechBossAny ? 3.7f : 0f;

		public override bool PreAI()
		{
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 700) {
				npc.ai[0]++;
				if (npc.ai[0] >= 120) {
					int type = ModContent.ProjectileType<PoisonGlob>();
					int p = Projectile.NewProjectile(npc.position.X, npc.position.Y, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)(npc.damage * .5f), 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
					npc.ai[0] = 0;
				}
			}
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 172, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				if (TideWorld.TheTide) {
					TideWorld.TidePoints += 1;
				}
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DepthShard>(), 1);
			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Acid>());
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.10000000149011612;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int num = (int)npc.frameCounter;
			npc.frame.Y = num * frameHeight;
			npc.spriteDirection = npc.direction;
		}
	}
}
