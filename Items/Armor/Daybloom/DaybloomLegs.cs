
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Daybloom
{
	[AutoloadEquip(EquipType.Legs)]
	public class DaybloomLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Daybloom Leggings");
			Tooltip.SetDefault("4% increased magic critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.White;
			item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
