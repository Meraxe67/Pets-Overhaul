using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SuspiciousEye : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public int phaseCd = 9000;
        public int phaseTime = 1800;
        private int eocTimer = 0;
        public float critMult = 0.2f;
        public float dmgMult = 1f;
        public float spdMult = 0.6f;
        public int eocDefenseConsume = 0;
        public int shieldTime = 600;
        private int inCombatTimer = -1;
        public int inCombatTime = 300;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.EyeOfCthulhuPetItem))
            {
                Pet.SetPetAbilityTimer(phaseCd);
                if (inCombatTimer >= -1)
                    inCombatTimer--;
            }

            if (eocTimer >= -1)
            {
                eocTimer--;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.timer <= 0 && Pet.PetInUseWithSwapCd(ItemID.EyeOfCthulhuPetItem) && Keybinds.UsePetAbility.JustPressed)
            {
                if (Player.statLife > Player.statLifeMax2 / 2)
                {
                    int damageTaken = (int)Math.Floor((float)Player.statLife % (Player.statLifeMax2 / 2));
                    if (damageTaken == 0)
                        damageTaken = Player.statLifeMax2 / 2;
                    Player.Hurt(PlayerDeathReason.ByCustomReason("If you're seeing this death message, report it at our discord or steam page."), damageTaken, 0, quiet: true, scalingArmorPenetration: 1f);
                    Pet.petShield.Add((damageTaken / 5, shieldTime));
                }
                else
                    inCombatTimer = inCombatTime;
            }
        }
        public override void UpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EyeOfCthulhuPetItem))
            {
                if (inCombatTimer >= 0 && Player.statLife <= Player.statLifeMax2 / 2 && Pet.timer <= 0)
                {
                    eocTimer = phaseTime;
                    Pet.timer = Pet.timerMax;
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.ForceRoar with { PitchVariance = 0.3f }, Player.position);
                    }

                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.position, Main.rand.NextVector2Circular(2f, 2f), 8, 0.5f);
                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.position, Main.rand.NextVector2Circular(2f, 2f), 8, 0.5f);
                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.position, Main.rand.NextVector2Circular(2f, 2f), 9, 0.5f);
                    Gore.NewGore(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetMisc), Player.position, Main.rand.NextVector2Circular(2f, 2f), 9, 0.5f);

                    PopupText.NewText(new AdvancedPopupRequest() with
                    {
                        Text = "ENRAGED!",
                        DurationInFrames = 150,
                        Velocity = new Vector2(0, -10),
                        Color = Color.DarkRed
                    }, Player.position);
                    Pet.petShield.Add((Player.statDefense * 2, shieldTime));
                    Player.AddBuff(ModContent.BuffType<EocPetEnrage>(), phaseTime);
                }
                if (eocTimer <= phaseTime && eocTimer >= 0)
                {
                    if (Player.statLife > Player.statLifeMax2 / 2)
                    {
                        Player.statLife = Player.statLifeMax2 / 2;
                    }
                    if (eocDefenseConsume <= 0)
                    {
                        eocDefenseConsume = Player.statDefense;
                    }
                    Player.statDefense *= 0;
                    Player.GetDamage<GenericDamageClass>() += eocDefenseConsume * dmgMult / 100;
                    Player.moveSpeed += eocDefenseConsume * spdMult / 100;
                    Player.GetCritChance<GenericDamageClass>() += eocDefenseConsume * critMult;
                    if (Player.dashDelay > 5)
                    {
                        Player.dashDelay = 5;
                        Player.eocDash = 5;
                    }
                }
                else if (eocTimer == 0)
                {
                    PopupText.NewText(new AdvancedPopupRequest() with
                    {
                        Text = "Calmed Down.",
                        DurationInFrames = 150,
                        Velocity = new Vector2(0, -10),
                        Color = Color.OrangeRed
                    }, Player.position);
                }
            }
        }
        public override void UpdateDead()
        {
            eocTimer = 0;
            inCombatTimer = 0;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUse(ItemID.EyeOfCthulhuPetItem))
            {
                inCombatTimer = inCombatTime;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUse(ItemID.EyeOfCthulhuPetItem))
            {
                inCombatTimer = inCombatTime;
            }
        }
    }
    public sealed class EyeOfCthulhuPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EyeOfCthulhuPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            SuspiciousEye suspiciousEye = Main.LocalPlayer.GetModPlayer<SuspiciousEye>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.EyeOfCthulhuPetItem")
                .Replace("<class>", PetTextsColors.ClassText(suspiciousEye.PetClassPrimary, suspiciousEye.PetClassSecondary))
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                .Replace("<shieldDuration>", Math.Round(suspiciousEye.shieldTime / 60f, 2).ToString())
                .Replace("<outOfCombat>", Math.Round(suspiciousEye.inCombatTime / 60f, 2).ToString())
                .Replace("<defToDmg>", Math.Round(suspiciousEye.dmgMult * 100, 2).ToString())
                .Replace("<defToSpd>", Math.Round(suspiciousEye.spdMult * 100, 2).ToString())
                .Replace("<defToCrit>", Math.Round(suspiciousEye.critMult * 100, 2).ToString())
                .Replace("<enrageLength>", Math.Round(suspiciousEye.phaseTime / 60f, 2).ToString())
                .Replace("<enrageCd>", Math.Round(suspiciousEye.phaseCd / 60f, 2).ToString())
                        ));
        }
    }
}
