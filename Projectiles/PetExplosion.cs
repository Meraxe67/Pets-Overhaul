using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    /// <summary>
    /// ai[0] determines size, ai[1] determines penetration value and ai[2] determines armor penetration of Projectile.
    /// </summary>
    public class PetExplosion : ModProjectile
    {
        public override LocalizedText DisplayName => Language.GetText("Mods.PetsOverhaul.Projectiles.PetExplosion");
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.width = (int)Projectile.ai[0];
            Projectile.height = (int)Projectile.ai[0];
            Projectile.OriginalArmorPenetration = (int)Projectile.ai[2];
            if (Projectile.ai[1] > 0)
            Projectile.penetrate = (int)Projectile.ai[1];

            SoundEngine.PlaySound(SoundID.Item14);
            for (int i = 0; i < Projectile.ai[0] * 0.075f; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 20, default, Main.rand.NextFloat(1.7f, 2f));
                dust.velocity *= Main.rand.NextFloat(1f, 1.3f);
            }
            for (int i = 0; i < Projectile.ai[0] * 0.15f; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 50, default, Main.rand.NextFloat(2.5f, 3f));
                dust.noGravity = true;
                dust.velocity *= Main.rand.NextFloat(1.7f, 2f);
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 70, default, Main.rand.NextFloat(1.2f, 1.5f));
                dust.velocity *= Main.rand.NextFloat(1.7f, 2f);
            }
        }
    }
}