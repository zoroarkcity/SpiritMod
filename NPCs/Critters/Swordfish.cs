using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class Swordfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swordfish");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 20;
			npc.damage = 40;
			npc.defense = 0;
			npc.dontCountMe = true;
			npc.lifeMax = 165;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.npcSlots = 0;
			aiType = NPCID.CorruptGoldfish;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Swordfish/SwordfishGore"));
			}
		}
		private int Counter;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Counter++;
			if (Main.player[npc.target].wet)
			{
				if (Counter == 100) {
					npc.velocity.Y *= 10.0f;
					npc.velocity.X *= 4.0f;
				}
				if (Counter >= 200) {
					Counter = 0;
				}
			}
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.031f;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Swordfish, 1);
			}
		}
	}
}
