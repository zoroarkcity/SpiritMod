
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GoreArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class IchorHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Helm");
			Tooltip.SetDefault("8% increased melee speed\nReduces damage taken by 7%");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 70, 0);
			item.rare = ItemRarityID.LightRed;
			item.defense = 15;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<IchorPlate>() && legs.type == ModContent.ItemType<IchorLegs>();

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.setBonus = $"Double tap {tapDir} to spawn six homing ichor clumps that sap enemy life";
			player.GetSpiritPlayer().ichorSet2 = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeSpeed += 0.08f;
			player.endurance += 0.07f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FleshClump>(), 11);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
