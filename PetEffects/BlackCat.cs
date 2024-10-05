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
    public sealed class BlackCat : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public float luckFlat = 0.09f;
        public float luckMoonLowest = -0.03f;
        public float luckMoonLow = -0.01f;
        public float luckMoonMid = 0.01f;
        public float luckMoonHigh = 0.03f;
        public float luckMoonHighest = 0.05f;
        public int moonlightCd = 1020;
        public int moonlightLowest = -10;
        public int moonlightHighest = 20;
        public float currentMoonLuck = 0;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.UnluckyYarn))
            {
                Pet.SetPetAbilityTimer(moonlightCd);
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Pet.PetInUseWithSwapCd(ItemID.UnluckyYarn))
            {
                currentMoonLuck = 0;
                if (Main.dayTime == false)
                {
                    switch (Main.moonPhase)
                    {
                        case 0:
                            currentMoonLuck = luckMoonLowest;
                            break;
                        case 1:
                            currentMoonLuck = luckMoonLow;
                            break;
                        case 2:
                            currentMoonLuck = luckMoonMid;
                            break;
                        case 3:
                            currentMoonLuck = luckMoonHigh;
                            break;
                        case 4:
                            currentMoonLuck = luckMoonHighest;
                            break;
                        case 5:
                            currentMoonLuck = luckMoonHigh;
                            break;
                        case 6:
                            currentMoonLuck = luckMoonMid;
                            break;
                        case 7:
                            currentMoonLuck = luckMoonLow;
                            break;
                        default:
                            currentMoonLuck = 0;
                            break;
                    }
                }
                luck += currentMoonLuck + luckFlat;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Main.dayTime == false && Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(ItemID.UnluckyYarn))
            {
                SoundEngine.PlaySound(SoundID.Item29 with { PitchRange = (-1f, -0.8f) }, Player.Center);
                int moonlightRoll = Main.rand.Next(moonlightLowest, moonlightHighest + 1);
                moonlightRoll = GlobalPet.Randomizer((int)(moonlightRoll * (Player.luck + 1) * 100));
                if (moonlightRoll == 0)
                {
                    moonlightRoll = Main.rand.NextBool() ? -1 : 1;
                }
                if (moonlightRoll < 0)
                {
                    moonlightRoll *= -1;
                    PlayerDeathReason reason = new();
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            reason = PlayerDeathReason.ByCustomReason(Player.name + " walked under a ladder.");
                            break;
                        case 1:
                            reason = PlayerDeathReason.ByCustomReason(Player.name + " now knows that: 'The Moon does not make its own light ”moonlight” is actually reflected sunlight.'");
                            break;
                        case 2:
                            reason = PlayerDeathReason.ByCustomReason(Player.name + "'s superstitious beliefs turned out to be real.");
                            break;
                        case 3:
                            reason = PlayerDeathReason.ByCustomReason(Player.name + " was unlucky.");
                            break;
                        case 4:
                            reason = PlayerDeathReason.ByCustomReason(Player.name + "'s Black Cat did bring bad luck afterall.");
                            break;
                        default:
                            break;
                    }
                    Player.Hurt(reason, moonlightRoll, 0, dodgeable: false, knockback: 0, scalingArmorPenetration: 1f);
                }
                else
                {
                    Pet.PetRecovery(moonlightRoll, 1f, isLifesteal: false);
                }
                Pet.timer = Pet.timerMax;
            }
        }
    }

    public sealed class UnluckyYarn : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.UnluckyYarn;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            BlackCat blackCat = Main.LocalPlayer.GetModPlayer<BlackCat>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.UnluckyYarn")
                .Replace("<class>", PetTextsColors.ClassText(blackCat.PetClassPrimary, blackCat.PetClassSecondary))
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                .Replace("<moonlightMin>", blackCat.moonlightLowest.ToString())
                .Replace("<moonlightMax>", blackCat.moonlightHighest.ToString())
                .Replace("<moonlightCd>", Math.Round(blackCat.moonlightCd / 60f, 2).ToString())
                .Replace("<flatLuck>", blackCat.luckFlat.ToString())
                .Replace("<minimumMoon>", blackCat.luckMoonLowest.ToString())
                .Replace("<maximumMoon>", blackCat.luckMoonHighest.ToString())
                .Replace("<moonLuck>", Math.Round(blackCat.currentMoonLuck, 2).ToString())
                .Replace("<playerLuck>", Math.Round(blackCat.Player.luck, 2).ToString())
            ));
        }
    }
}
