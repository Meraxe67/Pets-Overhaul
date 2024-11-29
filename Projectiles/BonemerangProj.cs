using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    /// <summary>
    /// ai[0] determines size. Sound not included, play your own sound before creating projectile however you want.
    /// </summary>
    public class BonemerangProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.CombatWrench);
        }
    }
}