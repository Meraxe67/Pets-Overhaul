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
    public sealed class MiniPrime : ModPlayer
    {
        public int shieldRecovery = 7500; //all 5 shields timer combined
        public float dmgIncrease = 0.07f;
        public int critIncrease = 7;
        public float defIncrease = 1.15f;
        public float shieldMult = 0.06f;
        public int shieldTime = 300;
        private int lastShield = 0;
        private int shieldIndex = 0;
        private int oldShieldCount = 0;

        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.SkeletronPrimePetItem))
            {
                Pet.timerMax = shieldRecovery;
            }
        }
        private void AddDefaultShield()
        {
            shieldIndex = Pet.petShield.Count - 1;
            Pet.petShield[shieldIndex] = ((int)(Player.statLifeMax2 * shieldMult), 2);
        }
        private void AddShieldWithTimer()
        {
            Pet.timer += Pet.timerMax / 5;
            (int shieldAmount, int shieldTimer) shield = Pet.petShield[shieldIndex];
            shield.shieldTimer = shieldTime;
            Pet.petShield[shieldIndex] = shield;
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SkeletronPrimePetItem))
            {
                if (Pet.petShield.Count <= 0)
                {
                    Pet.petShield.Add((0, 0));
                }
                if (Pet.timer <= Pet.timerMax * 0.8f && Player.statLife <= Player.statLifeMax2 * 0.25f)
                {
                    if (oldShieldCount > shieldIndex && Pet.petShield[shieldIndex].shieldAmount < lastShield)
                    {
                        AddShieldWithTimer();
                    }
                    else
                    {
                        AddDefaultShield();
                    }
                }
                else if (Pet.timer <= Pet.timerMax * 0.6f && Player.statLife <= Player.statLifeMax2 * 0.5f)
                {
                    if (oldShieldCount > shieldIndex && Pet.petShield[shieldIndex].shieldAmount < lastShield)
                    {
                        AddShieldWithTimer();
                    }
                    else
                    {
                        AddDefaultShield();
                    }
                }
                else if (Pet.timer <= Pet.timerMax * 0.4f && Player.statLife <= Player.statLifeMax2 * 0.75f)
                {
                    if (oldShieldCount > shieldIndex && Pet.petShield[shieldIndex].shieldAmount < lastShield)
                    {
                        AddShieldWithTimer();
                    }
                    else
                    {
                        AddDefaultShield();
                    }
                }
                else if (Pet.timer <= Pet.timerMax * 0.2f && Player.statLife < Player.statLifeMax2)
                {
                    if (oldShieldCount > shieldIndex && Pet.petShield[shieldIndex].shieldAmount < lastShield)
                    {
                        AddShieldWithTimer();
                    }
                    else
                    {
                        AddDefaultShield();
                    }
                }
                else if (Pet.timer <= 0 && Player.statLife <= Player.statLifeMax2)
                {
                    if (oldShieldCount > shieldIndex && Pet.petShield[shieldIndex].shieldAmount < lastShield)
                    {
                        AddShieldWithTimer();
                    }
                    else
                    {
                        AddDefaultShield();
                    }
                }
                if (oldShieldCount > shieldIndex)
                {
                    lastShield = Pet.petShield[shieldIndex].shieldAmount;
                }

                if (Pet.currentShield > 0)
                {
                    Player.GetDamage<GenericDamageClass>() += dmgIncrease;
                    Player.GetCritChance<GenericDamageClass>() += critIncrease;
                    Player.statDefense *= defIncrease;
                }
                oldShieldCount = Pet.petShield.Count;
            }
        }
    }
    public sealed class SkeletronPrimePetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.SkeletronPrimePetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            MiniPrime miniPrime = Main.LocalPlayer.GetModPlayer<MiniPrime>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.SkeletronPrimePetItem")
                        .Replace("<shieldMaxHealthAmount>", Math.Round(miniPrime.shieldMult * 100, 2).ToString())
                        .Replace("<shieldCooldown>", Math.Round(miniPrime.shieldRecovery / 300f, 2).ToString())
                        .Replace("<dmg>", Math.Round(miniPrime.dmgIncrease * 100, 2).ToString())
                        .Replace("<crit>", miniPrime.critIncrease.ToString())
                        .Replace("<def>", miniPrime.defIncrease.ToString())
                        .Replace("<shieldLifetime>", Math.Round(miniPrime.shieldTime / 60f, 2).ToString())
                        ));
        }
    }
}
