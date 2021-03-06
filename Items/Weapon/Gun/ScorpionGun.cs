using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class ScorpionGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Poacher");
			Tooltip.SetDefault("Converts regular bullets into venomous bullets");
		}
		public override void SetDefaults()
		{
			item.damage = 32;
			item.ranged = true;
			item.width = 54;
			item.height = 18;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 7;
			item.useTurn = false;
			item.value = Terraria.Item.buyPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.crit = 10;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ProjectileID.VenomBullet;
			item.shootSpeed = 13f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.Bullet) {
				type = ProjectileID.VenomBullet;
			}
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}

	}
}
