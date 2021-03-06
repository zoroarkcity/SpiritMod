using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class LihzahrdKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Dagger");
			Tooltip.SetDefault("Lights enemies on fire and flies straight");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 22;
			item.height = 22;
			item.autoReuse = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.channel = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Thrown.LihzahrdKnife>();
			item.useAnimation = 11;
			item.consumable = true;
			item.maxStack = 999;
			item.useTime = 11;
			item.shootSpeed = 15.0f;
			item.damage = 64;
			item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Lime;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SunShard>(), 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 99);
			recipe.AddRecipe();
		}
	}
}
