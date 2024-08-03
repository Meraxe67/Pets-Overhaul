using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    public class DynamiteKittyBoom : ModProjectile
    {
        public override LocalizedText DisplayName => Language.GetText("Mods.PetsOverhaul.Projectiles.DynamiteKittyBoom");
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 0;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 1;
            Projectile.netImportant = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item14);
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 20, default, Main.rand.NextFloat(1.7f, 2f));
                dust.velocity *= Main.rand.NextFloat(1f, 1.3f);
            }
            for (int i = 0; i < 30; i++)
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