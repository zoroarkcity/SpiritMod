using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class MagalaBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entbehrung");
			Tooltip.SetDefault("Converts wooden arrows into spiny Magala Arrows\n'You actually use a bow?`");
		}



		public override void SetDefaults()
		{
			item.damage = 43;
			item.noMelee = true;
			item.ranged = true;
			item.width = 48;
			item.height = 32;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 4;
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 12f;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<MagalaArrow>();
			}
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MagalaScale>(), 12);
			recipe.AddIngredient(ItemID.DD2PhoenixBow);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}