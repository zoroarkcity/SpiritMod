using Terraria.ID;
using Terraria.ModLoader;
using SpiritChestTile = SpiritMod.Tiles.Furniture.SpiritChest;
namespace SpiritMod.Items.Placeable.Furniture
{
	public class SpiritChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Chest");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 500;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<SpiritChestTile>();
		}
	}
}