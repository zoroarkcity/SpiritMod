using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
	public class PumpBubbleGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Blaster");
			Tooltip.SetDefault("Hold for a longer blast\nConsumes 15 mana each second");

		}

		public override void SetDefaults()
		{
			item.channel = true;
			item.damage = 30;
			item.magic = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 42, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<BubblePumpProj>();
			item.shootSpeed = 6f;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
				Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
				Vector2 offset = mouse - player.Center;
				offset.Normalize();
				offset *= 30;
				position -= offset;
				return true;
		}
	}
}