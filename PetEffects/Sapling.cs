using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Sapling : PetEffect
    {
        public float planteraLifesteal = 0.03f;
        public float regularLifesteal = 0.008f;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seedling) && GlobalPet.LifestealCheck(target))
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
            if (Pet.PetInUseWithSwapCd(ItemID.Seedling) && GlobalPet.LifestealCheck(target))
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
    public sealed class Seedling : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Seedling;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Sapling sapling = Main.LocalPlayer.GetModPlayer<Sapling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Seedling")
                .Replace("<class>", PetColors.ClassText(sapling.PetClassPrimary, sapling.PetClassSecondary))
                        .Replace("<lifesteal>", Math.Round(sapling.regularLifesteal * 100, 2).ToString())
                        .Replace("<planteraSteal>", Math.Round(sapling.planteraLifesteal * 100, 2).ToString())
                        ));
        }
    }
}
