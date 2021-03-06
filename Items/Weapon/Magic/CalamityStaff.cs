
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class CalamityStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimstone Blaze");
			Tooltip.SetDefault("'Fire and Brimstone, the heralds of Calamity'\nShoots a brimstone laser that explodes into brimstone flames on htting foes\nHit enemies can combust, with additional hits dealing more damage over time");

		}



		public override void SetDefaults()
		{
			item.damage = 53;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 26;
			item.mana = 8;
			item.useAnimation = 26;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 6;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item33;
			item.autoReuse = true;
			item.shootSpeed = 14;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<Projectiles.BrimBlaze>();
		}

		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<ViashinoStaff>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<InfernalAppendage>(), 3);
			modRecipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 5);
			modRecipe.AddIngredient(ItemID.SoulofSight, 5);
			modRecipe.AddIngredient(ItemID.SoulofNight, 5);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
