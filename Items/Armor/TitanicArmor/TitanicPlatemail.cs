using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.TitanicArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class TitanicPlatemail : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanic Platemail");
			Tooltip.SetDefault("Increases melee damage by 13% and melee speed by 10%");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 22;
			item.value = 80000;
			item.rare = ItemRarityID.LightPurple;

			item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.13F;
			player.meleeSpeed += 0.1F;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TidalEssence>(), 20);
			recipe.AddTile(ModContent.TileType<EssenceDistorter>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
