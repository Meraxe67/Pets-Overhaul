using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    public class BonemerangProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CombatWrench);
        }
    }
}