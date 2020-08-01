using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CenturionSet
{
	[AutoloadEquip(EquipType.Head)]
	public class CenturionHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Centurion's Helmet");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
    }
}
