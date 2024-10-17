using PetsOverhaul.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Projectiles
{
    public sealed class ProjectileSourceChecks : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool isPlanteraProjectile = false;
        public bool petProj = false;
        public bool isFromSentry = false;
        public int sourceNpcId = 0;
        public Item itemProjIsFrom = null;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            isPlanteraProjectile = false;
            petProj = false;
            isFromSentry = false;
            if (source is EntitySource_ItemUse item && (item.Item.type == ItemID.VenusMagnum || item.Item.type == ItemID.NettleBurst || item.Item.type == ItemID.LeafBlower || item.Item.type == ItemID.FlowerPow || item.Item.type == ItemID.WaspGun || item.Item.type == ItemID.Seedler || item.Item.type == ItemID.GrenadeLauncher))
            {
                isPlanteraProjectile = true;
            }
            else if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && (proj.type == ProjectileID.Pygmy || proj.type == ProjectileID.Pygmy2 || proj.type == ProjectileID.Pygmy3 || proj.type == ProjectileID.Pygmy4 || proj.type == ProjectileID.FlowerPow || proj.type == ProjectileID.SeedlerNut))
            {
                isPlanteraProjectile = true;
            }
            if (source is EntitySource_Pet { ContextType: EntitySourcePetIDs.PetProjectile })
            {
                petProj = true;
            }
            if (source is EntitySource_Parent parent2 && parent2.Entity is Projectile proj2 && proj2.sentry)
            {
                isFromSentry = true;
            }
            if (source is EntitySource_Parent parent3 && parent3.Entity is NPC npc)
            {
                sourceNpcId = npc.whoAmI;
            }
            if (source is EntitySource_ItemUse item2)
            {
                itemProjIsFrom = item2.Item;
            }
            else if (source is EntitySource_Parent parent4 && parent4.Entity is Projectile proj3 && proj3.TryGetGlobalProjectile(out ProjectileSourceChecks sourceChecks) && sourceChecks.itemProjIsFrom is not null)
            {
                itemProjIsFrom = sourceChecks.itemProjIsFrom;
            }
        }
    }
}
