using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class OvergrowthStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overgrowth Staff");
			Tooltip.SetDefault("Summons an overgrowth spirit to protect you");

		}


		public override void SetDefaults()
		{
			item.damage = 7;
			item.summon = true;
			item.mana = 10;
			item.UseSound = SoundID.Item44;
			item.width = 36;
			item.height = 38;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 1.25f;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Summon.Overgrowth>();
			item.shootSpeed = 10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wood, 50);
			recipe.AddIngredient(ItemID.Acorn, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(ModContent.BuffType<Buffs.Summon.Overgrowth>(), 3600);
			int i = Main.myPlayer;
			float num72 = item.shootSpeed;
			int num73 = item.damage;
			float num74 = item.knockBack;
			num74 = player.GetWeaponKnockback(item, num74);
			player.itemTime = item.useTime;
			Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
			float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
			float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
			if (player.gravDir == -1f) {
				num79 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
			}
			float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
			float num81 = num80;
			if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f)) {
				num78 = (float)player.direction;
				num79 = 0f;
				num80 = num72;
			}
			else {
				num80 = num72 / num80;
			}
			num78 = 0f;
			num79 = 0f;
			vector2.X = (float)Main.mouseX + Main.screenPosition.X;
			vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
			Projectile.NewProjectile(vector2.X, vector2.Y, num78, num79, ModContent.ProjectileType<Projectiles.Summon.Overgrowth>(), num73, num74, i, 0f, 0f);
			return false;
		}
	}
}