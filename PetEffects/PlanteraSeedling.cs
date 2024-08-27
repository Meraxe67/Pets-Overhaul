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
    public sealed class PlanteraSeedling : PetEffect
    {
        public float secondMultiplier = 0.1f;

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.PlanteraPetItem) && modifiers.DamageType.CountsAsClass<RangedDamageClass>())
            {
                float multiplyDamage = 2 / ((float)target.life / target.lifeMax + 1);
                multiplyDamage += (2 / ((float)target.life / target.lifeMax + 1) - 1) * secondMultiplier;
                modifiers.FinalDamage *= multiplyDamage;
            }
        }
    }
    public sealed class PlanteraPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.PlanteraPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PlanteraSeedling planteraSeedling = Main.LocalPlayer.GetModPlayer<PlanteraSeedling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.PlanteraPetItem")
                .Replace("<class>", PetTextsColors.ClassText(planteraSeedling.PetClassPrimary, planteraSeedling.PetClassSecondary))
                        .Replace("<maxAmount>", Math.Round(planteraSeedling.secondMultiplier * 100 + 100, 2).ToString())
                        ));
        }
    }
}
