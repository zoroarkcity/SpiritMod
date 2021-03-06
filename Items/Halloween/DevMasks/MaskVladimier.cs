using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.DevMasks
{
	[AutoloadEquip(EquipType.Head)]
	public class MaskVladimier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vladimier's Mask");
			Tooltip.SetDefault("Vanity item \n'Great for impersonating devs!'");

		}


		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 3000;
			item.rare = ItemRarityID.Cyan;
		}
	}
}
