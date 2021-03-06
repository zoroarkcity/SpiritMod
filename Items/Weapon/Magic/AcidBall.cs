using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
	public class AcidBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Globs");
			Tooltip.SetDefault("Throws out multiple balls of acid that inflict 'Acid Burn'\nAcid Burn increases in potency as you continue hitting foes");

		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.useTurn = false;
			item.autoReuse = true;
			item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.damage = 44;
			item.mana = 7;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 17;
			item.useAnimation = 17;
			item.magic = true;
			item.channel = true;
			item.UseSound = SoundID.Item66;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<AcidGlobs>();
			item.shootSpeed = 12f;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<Acid>(), 8);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
