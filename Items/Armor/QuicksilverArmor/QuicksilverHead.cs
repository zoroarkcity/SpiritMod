using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.QuicksilverArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class QuicksilverHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Mask");
			Tooltip.SetDefault("10% increased ranged damage\n6% increased critical strike chance\nIncreases your max number of minions");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Item.sellPrice(gold: 3);
			item.rare = ItemRarityID.Yellow;
			item.defense = 19;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<QuicksilverBody>() && legs.type == ModContent.ItemType<QuicksilverLegs>();

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"12% increased damage\nDouble tap {tapDir} to cause your cursor to release multiple damaging quicksilver droplets\nIf these droplets hit foes, they will regenerate some of the player's life\n30 second cooldown";
			player.allDamage += 0.12f;
			player.GetSpiritPlayer().quickSilverSet = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedCrit += 6;
			player.meleeCrit += 6;
			player.magicCrit += 6;
			player.rangedDamage += 0.1f;
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Material.Material>(), 11);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}