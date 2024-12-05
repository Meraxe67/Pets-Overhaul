using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Sapling : PetEffect
    {
        public override int PetItemID => ItemID.Seedling;
        public float planteraLifesteal = 0.035f;
        public float regularLifesteal = 0.015f;
        public float damagePenalty = 0.7f;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && GlobalPet.LifestealCheck(target))
            {
                modifiers.FinalDamage *= damagePenalty;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && GlobalPet.LifestealCheck(target))
            {
                if (proj.GetGlobalProjectile<ProjectileSourceChecks>().isPlanteraProjectile)
                {
                    Pet.PetRecovery(damageDone, planteraLifesteal);
                }
                else
                {
                    Pet.PetRecovery(damageDone, regularLifesteal);
                }
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && GlobalPet.LifestealCheck(target))
            {
                if (item.type == ItemID.Seedler || item.type == ItemID.TheAxe)
                {
                    Pet.PetRecovery(damageDone, planteraLifesteal);
                }
                else
                {
                    Pet.PetRecovery(damageDone, regularLifesteal);
                }
            }
        }
    }
    public sealed class Seedling : PetTooltip
    {
        public override PetEffect PetsEffect => sapling;
        public static Sapling sapling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Sapling pet))
                    return pet;
                else
                    return ModContent.GetInstance<Sapling>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Seedling")
                .Replace("<dmgPenalty>", sapling.damagePenalty.ToString())
                .Replace("<lifesteal>", Math.Round(sapling.regularLifesteal * 100, 2).ToString())
                .Replace("<planteraSteal>", Math.Round(sapling.planteraLifesteal * 100, 2).ToString());
    }
}
