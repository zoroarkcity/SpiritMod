
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Bullet
{
	public class MarsBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electrified Bullet");
			Tooltip.SetDefault("Zaps foes with powerful energy!");
		}


		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 16;
			item.value = 1000;
			item.rare = ItemRarityID.Red;
			item.value = Item.buyPrice(0, 0, 3, 0);

			item.maxStack = 999;

			item.damage = 15;
			item.knockBack = 1.5f;
			item.ammo = AmmoID.Bullet;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<Projectiles.Bullet.MarsBulletProj>();
			item.shootSpeed = 9f;

		}
	}
}