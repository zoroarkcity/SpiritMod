
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GoreArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class IchorPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Platemail");
			Tooltip.SetDefault("7% increased melee damage\n6% increased melee speed");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 70, 0);
			item.rare = ItemRarityID.LightRed;

			item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.07f;

			player.meleeSpeed += 0.06f; ;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FleshClump>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
