using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IceArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class IceArmor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Plate");
			Tooltip.SetDefault("Reduces mana cost by 12% increases maximum mana by 40");
		}


		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = 86000;
			item.rare = ItemRarityID.LightPurple;
			item.defense = 17;
		}
		public override void UpdateEquip(Player player)
		{
			player.manaCost -= 0.12f;
			player.statManaMax2 += 40;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IcyEssence>(), 20);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}