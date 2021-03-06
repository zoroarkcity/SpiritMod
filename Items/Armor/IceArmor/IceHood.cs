using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IceArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class IceHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Hood");
			Tooltip.SetDefault("15% increased magic damage\nIncreases maximum mana by 40");
		}

		int timer = 0;
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = Item.buyPrice(gold: 4, silver: 60);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.15f;
			player.statManaMax2 += 40;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<IceArmor>() && legs.type == ModContent.ItemType<IceRobe>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Magic hits occasionally grant you the Blizzard's Wrath";
			player.GetSpiritPlayer().icySet = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IcyEssence>(), 16);
			recipe.AddTile(ModContent.TileType<EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}