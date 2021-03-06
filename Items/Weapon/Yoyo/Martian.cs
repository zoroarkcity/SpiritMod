using SpiritMod.Projectiles.Yoyo;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Martian : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terrestrial Ultimatum");
			Tooltip.SetDefault("Shoots electrospheres");
		}



		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 124;
			item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Red;
			item.knockBack = 4;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 28;
			item.useTime = 25;
			item.shoot = ModContent.ProjectileType<MartianP>();
		}
	}
}