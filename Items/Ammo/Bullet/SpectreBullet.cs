using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Bullet
{
	public class SpectreBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Bullet");
			Tooltip.SetDefault("A spectral bolt that homes on to enemies and occasionally saps their life");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.value = 1000;
			item.rare = ItemRarityID.Cyan;
			item.maxStack = 999;
			item.damage = 10;
			item.knockBack = 1.5f;
			item.ammo = AmmoID.Bullet;
			item.ranged = true;
			item.consumable = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Bullet.SpectreBullet>();
			item.shootSpeed = 9f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpectreBar, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}