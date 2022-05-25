using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Summon.Zones;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs.Zones;

namespace SpiritMod.Items.Sets.ArcaneZoneSubclass
{
	public class HealingCodex : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arcane Codex: Healing Zone");
			Tooltip.SetDefault("Summons a healing zone at the cursor position\nHealing zones increase player life regen while standing inside\nZones count as sentries");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/ArcaneZoneSubclass/HealingCodex_Glow");
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.19f, .031f, .091f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Sets/ArcaneZoneSubclass/HealingCodex_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void SetDefaults()
		{
			item.damage = 0;
			item.summon = true;
			item.mana = 10;
			item.width = 54;
			item.height = 50;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.value = 10000;
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.DD2_EtherianPortalSpawnEnemy;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<HealingZone>();
			item.shootSpeed = 0f;
			item.buffType = ModContent.BuffType<HealthZoneTimer>();
			item.buffTime = Projectile.SentryLifeTime;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            player.UpdateMaxTurrets();
			return false;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<EmptyCodex>(), 1);
            recipe.AddIngredient(ItemID.LifeCrystal, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}