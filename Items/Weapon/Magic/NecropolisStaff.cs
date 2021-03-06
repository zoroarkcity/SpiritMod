using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class NecropolisStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necropolis Staff");
			Tooltip.SetDefault("Shoots a slow moving trident");
		}


		public override void SetDefaults()
		{
			item.damage = 36;
			item.magic = true;
			item.mana = 13;
			item.width = 40;
			item.height = 40;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 6;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<NecropolisTrident>();
			item.shootSpeed = 12f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PutridPiece>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
