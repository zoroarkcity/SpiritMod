using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WindArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class WindArmorLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wind God's Greaves");
			Tooltip.SetDefault("Increases summon damage by 13%\nIncreases maximum amount of minions by 1");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.defense = 14;
		}
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.13f;
			player.maxMinions += 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<WorshipCrystal>(), 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}