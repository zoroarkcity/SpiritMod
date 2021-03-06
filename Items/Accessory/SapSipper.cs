
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class SapSipper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sap Sipper");
			Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nIncreases life regeneration slightly\nImmunity to Poison\nIncreases maximum life by 10\nMagic attacks occasionally drench enemies in venom\n8% increased critical strike chance\nCritical hits occasionally steal some life");

		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.lifeRegen += 3;
			player.GetSpiritPlayer().Ward1 = true;
			player.statLifeMax2 += 10;
			player.statDefense -= 1;
			player.buffImmune[BuffID.Poisoned] = true;
			player.meleeCrit += 8;
			player.magicCrit += 8;
			player.rangedCrit += 8;
			player.GetSpiritPlayer().ToxicExtract = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PathogenWard>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ToxicExtract>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
