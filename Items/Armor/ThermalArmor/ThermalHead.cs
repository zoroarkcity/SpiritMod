using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ThermalArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ThermalHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Faceplate");
			Tooltip.SetDefault("Increases melee damage by 12% and melee speed by 13%");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.defense = 21;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ThermalBody>() && legs.type == ModContent.ItemType<ThermalLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Melee attacks may release a cluster of rockets that explode upon hitting foes";
			player.GetSpiritPlayer().thermalSet = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += .12f;
			player.meleeSpeed += .13f;

		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ThermiteBar>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}