using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Sapling : ModPlayer
    {
        public float planteraLifesteal = 0.03f;
        public float regularLifesteal = 0.009f;
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seedling) && GlobalPet.LifestealCheck(target))
            {
                if (proj.GetGlobalProjectile<ProjectileSourceChecks>().isPlanteraProjectile)
                {
                    Pet.Lifesteal(damageDone, planteraLifesteal);
                }
                else
                {
                    Pet.Lifesteal(damageDone, regularLifesteal);
                }
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.Seedling) && GlobalPet.LifestealCheck(target))
            {
                if (item.type == ItemID.Seedler || item.type == ItemID.TheAxe)
                {
                    Pet.Lifesteal(damageDone, planteraLifesteal);
                }
                else
                {
                    Pet.Lifesteal(damageDone, regularLifesteal);
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Sapling sapling = Main.LocalPlayer.GetModPlayer<Sapling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.Seedling")
                        .Replace("<lifesteal>", Math.Round(sapling.regularLifesteal * 100, 2).ToString())
                        .Replace("<planteraSteal>", Math.Round(sapling.planteraLifesteal * 100, 2).ToString())
                        ));
        }
    }
}
