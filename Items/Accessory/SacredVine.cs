using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class SacredVine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacred Vine");
			Tooltip.SetDefault("Empowers Oak Heart: Oak Heart now inflicts 'Pollinating Poison'\nIncreases throwing critical strike chance by 4%\nThrowing attacks may briefly cause rapid regeneration");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 2;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().sacredVine = true;
			player.thrownCrit += 4;
		}
	}
}
