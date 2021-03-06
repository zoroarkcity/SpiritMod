using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
	public class LavaSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Spear");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 56;
			item.height = 56;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.noMelee = true;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 7f;
			item.knockBack = 6f;
			item.damage = 65;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<LavaSpearProj>();
		}
	}
}
