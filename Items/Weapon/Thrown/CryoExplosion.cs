using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using SpiritMod.Projectiles.Thrown;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
    public class CryoExplosion : ModProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cryolite Bomb");
             Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults() {
            projectile.width = 80;
            projectile.height = 80;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.hide = true;
            projectile.penetrate = 5;
            projectile.timeLeft = 150;
            projectile.alpha = 0;
         //   aiType = ProjectileID.ThrowingKnife;
            projectile.tileCollide = false;
        }
       float counter = 3f;
        public override void Kill(int timeLeft) {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
            for (int i = 0; i < 40; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 68, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Dust expr_62_cp_0 = Main.dust[num];
                expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                Dust expr_92_cp_0 = Main.dust[num];
                expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                if (Main.dust[num].position != projectile.Center)
                {
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
                }
            }
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool PreAI()
        {
            if (counter > 0)
            {
             counter-=0.5f;
            }
            projectile.frame = (int)counter;
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(4) == 0)
                target.AddBuff(ModContent.BuffType<CryoCrush>(), 240);
        }
    }
}