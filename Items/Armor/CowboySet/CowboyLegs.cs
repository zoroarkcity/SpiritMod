using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CowboySet
{
	[AutoloadEquip(EquipType.Legs)]
	public class CowboyLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outlaw's Pants");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
    }
}
