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
    public sealed class SpiderBrain : ModPlayer
    {
        public int lifePool = 0;
        public float lifePoolMaxPerc = 0.075f;
        public int cdDoAddToPool = 20;
        public float lifestealAmount = 0.05f;

        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BrainOfCthulhuPetItem))
            {
                Pet.timerMax = cdDoAddToPool;
            }
        }
        public override void PreUpdateBuffs()
        {
            if (Pet.PetInUse(ItemID.BrainOfCthulhuPetItem) && Pet.timer <= 0 && lifePool <= Player.statLifeMax2 * lifePoolMaxPerc)
            {
                lifePool++;
                Pet.timer = Pet.timerMax;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BrainOfCthulhuPetItem) && GlobalPet.LifestealCheck(target))
            {
                int decreaseFromPool = Pet.Lifesteal(damageDone, lifestealAmount, doLifesteal: false);
                if (decreaseFromPool >= lifePool)
                {
                    Pet.Lifesteal(lifePool, 1f);
                    lifePool = 0;
                }
                else
                {
                    lifePool -= decreaseFromPool;
                    Pet.Lifesteal(decreaseFromPool, 1f);
                }
            }
        }
    }
    public sealed class BrainOfCthulhuPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BrainOfCthulhuPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            SpiderBrain spiderBrain = Main.LocalPlayer.GetModPlayer<SpiderBrain>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BrainOfCthulhuPetItem")
                        .Replace("<lifesteal>", Math.Round(spiderBrain.lifestealAmount * 100, 2).ToString())
                        .Replace("<maxPool>", Math.Round(spiderBrain.lifePoolMaxPerc * 100, 2).ToString())
                        .Replace("<healthRecovery>", Math.Round(spiderBrain.cdDoAddToPool / 60f, 2).ToString())
                        ));
        }
    }
}
