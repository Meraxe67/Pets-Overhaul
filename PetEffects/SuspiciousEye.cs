using Microsoft.Xna.Framework;
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
        public int phaseCd = 9000;
        public int phaseTime = 1800;
        private int eocTimer = 0;
        public float critMult = 0.2f;
        public float dmgMult = 1f;
        public float spdMult = 0.6f;
        public int eocDefenseConsume;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.EyeOfCthulhuPetItem))
            {
                Pet.SetPetAbilityTimer(phaseCd);
            }

            if (eocTimer >= -1)
            {
                eocTimer--;
            }
        }
        public override void UpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.EyeOfCthulhuPetItem))
            {
                if (Player.statLife < Player.statLifeMax2 / 2 && Pet.timer <= 0)
                {
                    eocTimer = phaseTime;
                    Pet.timer = Pet.timerMax;
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.ForceRoar with { PitchVariance = 0.3f }, Player.position);
                    }

                    AdvancedPopupRequest popupMessage = new()
                    {
                        Text = "ENRAGED!",
                        DurationInFrames = 150,
                        Velocity = new Vector2(0, -10),
                        Color = Color.DarkRed
                    };
                    PopupText.NewText(popupMessage, Player.position);
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
                    AdvancedPopupRequest popupMessage = new()
                    {
                        Text = "Calmed Down.",
                        DurationInFrames = 150,
                        Velocity = new Vector2(0, -10),
                        Color = Color.OrangeRed
                    };
                    PopupText.NewText(popupMessage, Player.position);
                }
            }
        }
        public override void UpdateDead()
        {
            eocTimer = 0;
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
                .Replace("<class>", PetColors.ClassText(suspiciousEye.PetClassPrimary, suspiciousEye.PetClassSecondary))
                        .Replace("<defToDmg>", Math.Round(suspiciousEye.dmgMult * 100, 2).ToString())
                        .Replace("<defToSpd>", Math.Round(suspiciousEye.spdMult * 100, 2).ToString())
                        .Replace("<defToCrit>", Math.Round(suspiciousEye.critMult * 100, 2).ToString())
                        .Replace("<enrageLength>", Math.Round(suspiciousEye.phaseTime / 60f, 2).ToString())
                        .Replace("<enrageCd>", Math.Round(suspiciousEye.phaseCd / 60f, 2).ToString())
                        ));
        }
    }
}
