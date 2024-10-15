using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyTruffle : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Mobility;
        public float increaseFloat = 0.04f;
        public int increaseInt = 4;
        public float moveSpd = 0.2f;
        public int shroomPotionCd = 60;
        public int buffIncrease = 30;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.StrangeGlowingMushroom))
            {
                Pet.SetPetAbilityTimer(shroomPotionCd);
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
            {
                luck += increaseFloat;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
            {
                Player.buffImmune[BuffID.Confused] = false;
                Player.AddBuff(BuffID.Confused, 1);
                Player.GetAttackSpeed<GenericDamageClass>() += increaseFloat;
                Player.GetDamage<GenericDamageClass>() += increaseFloat;
                Player.GetCritChance<GenericDamageClass>() += increaseInt;
                Player.statManaMax2 += increaseInt;
                Player.statLifeMax2 += increaseInt;
                Player.statDefense += increaseInt;
                Player.GetArmorPenetration<GenericDamageClass>() += increaseInt;
                Player.GetKnockback<GenericDamageClass>() += increaseFloat;
                Player.manaCost -= increaseFloat;
                Player.manaRegenBonus += increaseInt;
                Player.jumpSpeedBoost += increaseFloat;
                Player.moveSpeed += moveSpd;
                Player.wingTimeMax += increaseInt;
                Player.nightVision = true;
                Player.endurance += increaseFloat;
                Player.fishingSkill += increaseInt;
                Pet.abilityHaste += increaseFloat;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
            {
                modifiers.CritDamage += increaseFloat;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom) && Pet.timer <= 0)
            {
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Main.debuff[Player.buffType[i]] == false && Main.buffNoSave[Player.buffType[i]] == false)
                    {
                        Player.buffTime[i] += buffIncrease;
                    }
                }
                Pet.timer = Pet.timerMax;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.StrangeGlowingMushroom))
            {
                modifiers.Knockback *= 1f - increaseFloat;
            }
        }
    }
    public sealed class StrangeGlowingMushroom : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.StrangeGlowingMushroom;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyTruffle babyTruffle = Main.LocalPlayer.GetModPlayer<BabyTruffle>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.StrangeGlowingMushroom")
                .Replace("<class>", PetTextsColors.ClassText(babyTruffle.PetClassPrimary, babyTruffle.PetClassSecondary))
                .Replace("<buffRecover>", Math.Round(babyTruffle.buffIncrease / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(babyTruffle.shroomPotionCd / 60f, 2).ToString())
                .Replace("<intIncr>", babyTruffle.increaseInt.ToString())
                .Replace("<moveSpd>", Math.Round(babyTruffle.moveSpd * 100, 2).ToString())
            ));
        }
    }
}
