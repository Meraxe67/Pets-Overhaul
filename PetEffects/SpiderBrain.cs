﻿using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SpiderBrain : PetEffect
    {
        public override int PetItemID => ItemID.BrainOfCthulhuPetItem;
        public int lifePool = 0;
        public float lifePoolMaxPerc = 0.3f;
        public int cdToAddToPool = 180;
        public float lifestealAmount = 0.035f;

        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override int PetAbilityCooldown => cdToAddToPool;
        public override void ExtraPreUpdate()
        {
            if (PetIsEquipped(false))
            {
                if (Pet.inCombatTimer <= 0)
                    cdToAddToPool = 90;
                else
                    cdToAddToPool = 180;
            }
        }
        public override void PreUpdateBuffs()
        {
            if (PetIsEquipped(false) && Pet.timer <= 0 && lifePool <= Player.statLifeMax2 * lifePoolMaxPerc)
            {
                lifePool++;
                Pet.timer = Pet.timerMax;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && GlobalPet.LifestealCheck(target))
            {
                int decreaseFromPool = Pet.PetRecovery(damageDone, lifestealAmount, doHeal: false);
                if (decreaseFromPool >= lifePool)
                {
                    Pet.PetRecovery(lifePool, 1f);
                    lifePool = 0;
                }
                else
                {
                    lifePool -= decreaseFromPool;
                    Pet.PetRecovery(decreaseFromPool, 1f);
                }
            }
        }
    }
    public sealed class BrainOfCthulhuPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => spiderBrain;
        public static SpiderBrain spiderBrain
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SpiderBrain pet))
                    return pet;
                else
                    return ModContent.GetInstance<SpiderBrain>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BrainOfCthulhuPetItem")
                        .Replace("<lifesteal>", Math.Round(spiderBrain.lifestealAmount * 100, 2).ToString())
                        .Replace("<maxPool>", Math.Round(spiderBrain.lifePoolMaxPerc * 100, 2).ToString())
                        .Replace("<healthRecovery>", Math.Round(spiderBrain.cdToAddToPool / 60f, 2).ToString())
                        .Replace("<pool>", spiderBrain.lifePool.ToString());
    }
}
