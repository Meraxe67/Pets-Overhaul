using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaul.TownPets;
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
        public int eocTimer = 0;
        public float critMult = 0.18f;
        public float dmgMult = 0.9f;
        public float spdMult = 0.5f;
        public int ragePoints = 0;
        public int shieldTime = 600;
        private int inCombatTimer = -1;
        public int inCombatTime = 300;
        public float shieldMult = 0.5f;
        public float eocShieldMult = 1f;
        public bool eocShieldEquipped = false;
        public int dashFrameReduce = 10;
        public float forcedEnrageShield = 0.1f;
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
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(ItemID.EyeOfCthulhuPetItem))
            {
                if (Player.statLife > Player.statLifeMax2 / 2)
                {
                    int damageTaken = (int)Math.Floor((float)Player.statLife % (Player.statLifeMax2 / 2));
                    if (damageTaken == 0)
                        damageTaken = Player.statLifeMax2 / 2;
                    Player.Hurt(new Player.HurtInfo() with { Damage = damageTaken, Dodgeable = false, Knockback = 0, DamageSource = PlayerDeathReason.ByCustomReason("If you're seeing this death message, report it through our discord or steam page.") });
                    Pet.petShield.Add(((int)(damageTaken * forcedEnrageShield), shieldTime));
                }
                else
                    inCombatTimer = inCombatTime;
            }
        }
        public override void PostUpdateMiscEffects()
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

                    Pet.petShield.Add(((int)((eocShieldEquipped ? eocShieldMult : shieldMult) * (Player.statDefense + Player.endurance * 100)), shieldTime));
                    Player.AddBuff(ModContent.BuffType<EocPetEnrage>(), phaseTime);
                }
                if (eocTimer <= phaseTime && eocTimer >= 0)
                {
                    if (Player.statLife > Player.statLifeMax2 / 2)
                    {
                        Player.statLife = Player.statLifeMax2 / 2;
                    }
                    ragePoints = 0;
                    ragePoints += Player.statDefense;
                    ragePoints += (int)(Player.endurance * 100);
                    Player.statDefense *= 0;
                    Player.endurance *= 0;
                    Player.GetDamage<GenericDamageClass>() += ragePoints * dmgMult / 100;
                    Player.moveSpeed += ragePoints * spdMult / 100;
                    Player.GetCritChance<GenericDamageClass>() += ragePoints * critMult;
                    if (eocShieldEquipped && Player.dashType == DashID.ShieldOfCthulhu && Player.dashDelay > dashFrameReduce)
                    {
                        Player.dashDelay = dashFrameReduce;
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
                    ragePoints = 0;
                }
                eocShieldEquipped = false;
            }
        }
        public override void UpdateDead()
        {
            eocTimer = 0;
            inCombatTimer = 0;
            ragePoints = 0;
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
    public class ShieldOfCthulhuCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.EoCShield;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            player.GetModPlayer<SuspiciousEye>().eocShieldEquipped = true;
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
                .Replace("<forcedEnrageMult>", Math.Round(suspiciousEye.forcedEnrageShield * 100, 2).ToString())
                .Replace("<shieldDuration>", Math.Round(suspiciousEye.shieldTime / 60f, 2).ToString())
                .Replace("<frameReduction>", suspiciousEye.dashFrameReduce.ToString())
                .Replace("<shieldMult>", suspiciousEye.shieldMult.ToString())
                .Replace("<eocShieldMult>", suspiciousEye.eocShieldMult.ToString())
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
