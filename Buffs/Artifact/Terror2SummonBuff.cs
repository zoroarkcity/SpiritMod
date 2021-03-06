using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Artifact
{
	public class Terror2SummonBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Terror Fiend");
			Description.SetDefault("It's taken a liking to you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.minionDamage += .05f;
			player.minionKB += 0.03f;

			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[mod.ProjectileType("Terror2Summon")] > 0) {
				modPlayer.terror2Summon = true;
			}

			if (!modPlayer.terror2Summon) {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else {
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
