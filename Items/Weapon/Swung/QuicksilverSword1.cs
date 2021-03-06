using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class QuicksilverSword1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Blade");
			Tooltip.SetDefault("Melee hits on foes cause them to explode into a pool of Quicksilver, releasing multiple homing Quicksilver Droplets");
		}

		public override void SetDefaults()
		{
			item.damage = 92;
			item.useTime = 16;
			item.useAnimation = 16;
			item.melee = true;
			item.width = 74;
			item.height = 84;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 8;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
			item.useTurn = true;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (!target.chaseable || target.lifeMax <= 5 || target.dontTakeDamage || target.friendly || target.immortal)
				return;

			Vector2 position = target.Center;
			for (int h = 0; h < 2; h++) {
				Vector2 vel = new Vector2(0, -1);
				float rand = Main.rand.NextFloat() * 6.283f;
				vel = vel.RotatedBy(rand);
				vel *= 8f;

				target.friendly = true;
				NPC home = Projectiles.ProjectileExtras.FindRandomNPC(target.Center, 1600f, false);
				target.friendly = false;
				bool homing = home != null;
				Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, ModContent.ProjectileType<QuicksilverBolt>(), 45, 1, player.whoAmI, homing ? home.whoAmI : 0, homing ? 30 : 0);

			}
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 14);
			//Projectile.NewProjectile(position.X, position.Y, 0f, 0f, ModContent.ProjectileType<Wrath>(), damage, knockback, player.whoAmI, 0f, 0f);

			//Projectiles.ProjectileExtras.Explode()
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(2) == 0) {
				int dust2 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SilverCoin);
				int dust3 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SilverCoin);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust3].noGravity = true;
				Main.dust[dust2].velocity *= 0f;
				Main.dust[dust3].velocity *= 0f;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Material.Material>(), 17);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}