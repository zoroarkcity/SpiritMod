using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Summon
{
	public class CarnivorousPlantMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Carnivorous Plant");
			Description.SetDefault("A primal guard with a taste for blood...");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<CarnivorousPlantMinion>()] > 0) {
				modPlayer.carnivorousPlantMinion = true;
			}

			if (!modPlayer.carnivorousPlantMinion) {
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			else {
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}
