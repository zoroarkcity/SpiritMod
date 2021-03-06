
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Waist)]
	public class AnimationStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Animation Stone");
			Tooltip.SetDefault("10% increased movement speed\n10% reduced jump height");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 49;
			item.value = Item.sellPrice(0, 0, 6, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.1f;
			player.maxRunSpeed += .05f;
			player.jumpSpeedBoost -= 1f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.DemoniteBar, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddIngredient(ItemID.CrimtaneBar, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
