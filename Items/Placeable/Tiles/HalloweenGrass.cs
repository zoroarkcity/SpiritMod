using Terraria.ID;
using Terraria.ModLoader;
using HalloweenGrassTile = SpiritMod.Tiles.Block.HalloweenGrass;
namespace SpiritMod.Items.Placeable.Tiles
{
	public class HalloweenGrass : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spooky Grass");
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;
			item.value = 500;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<HalloweenGrassTile>();
		}
	}
}