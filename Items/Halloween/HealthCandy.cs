using SpiritMod.Buffs.Candy;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween
{
	public class HealthCandy : CandyBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Health Candy");
			Tooltip.SetDefault("Increases Health");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Green;
			item.maxStack = 30;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = ModContent.BuffType<HealthBuffC>();
			item.buffTime = 14400;

			item.UseSound = SoundID.Item2;
		}
	}
}
