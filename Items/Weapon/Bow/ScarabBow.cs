using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class ScarabBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adorned Bow");
			Tooltip.SetDefault("Converts wooden arrows into 'Topaz Shafts'\nTopaz Bolts move fast and illuminate hit foes");
		}



		public override void SetDefaults()
		{
			item.damage = 13;
			item.noMelee = true;
			item.ranged = true;
			item.width = 26;
			item.height = 62;
			item.useTime = 37;
			item.useAnimation = 37;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<ScarabArrow>();
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 7.2f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<ScarabArrow>();
			}
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}