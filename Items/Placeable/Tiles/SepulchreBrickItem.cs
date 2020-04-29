using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
    public class SepulchreBrickItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sepulchre Brick");
        }


        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 14;

            item.maxStack = 999;

            item.useStyle = 1;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = mod.TileType("SepulchreBrick");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(TileID.GrayBrick, 5);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}