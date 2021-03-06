using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class CrimsonRegen : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Bloody Regen");
			Description.SetDefault("Increases life regeneration");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 2;
		}
	}
}
