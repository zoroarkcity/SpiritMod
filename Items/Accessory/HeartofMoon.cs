using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class HeartofMoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart of the Moon");
			Tooltip.SetDefault("11% increased damage\nGetting hurt spawns six Moon Globules around the player\nAttacks have a chance to grant the player the 'Will of the Celestials,' increasing life regeneration and reducing damage taken");

		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Red;
			item.value = Item.buyPrice(1, 0, 0, 0);

			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.magicDamage += 0.11f;
			player.minionDamage += 0.11f;
			player.rangedDamage += 0.11f;
			player.meleeDamage += 0.11f;
			player.GetSpiritPlayer().moonHeart = true;
		}
	}
}
