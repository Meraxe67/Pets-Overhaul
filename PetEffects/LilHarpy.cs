using Microsoft.Xna.Framework;
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
    public sealed class LilHarpy : PetEffect
    {
        public int harpyCd = 780;
        public int fuelMax = 150;
        public int harpyFlight = 150;
        private bool cooldownStarted;
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(ItemID.BirdieRattle))
            {
                Pet.SetPetAbilityTimer(harpyCd);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.timer == 0)
            {
                cooldownStarted = false;
                harpyFlight = fuelMax;
            }
            if (Pet.PetInUseWithSwapCd(ItemID.BirdieRattle))
            {
                if (Player.equippedWings == null)
                {
                    if (harpyFlight == fuelMax && Player.wingTime == 0)
                    {
                        Player.wingTime = fuelMax;
                    }
                    if (harpyFlight > 0)
                    {
                        Player.wings = 7;
                        Player.wingsLogic = 1;
                        Player.wingTimeMax = harpyFlight;
                        harpyFlight = (int)Player.wingTime;
                    }
                    Player.noFallDmg = true;
                }
                if (cooldownStarted == false && harpyFlight < fuelMax)
                {
                    Pet.timer = Pet.timerMax;
                    cooldownStarted = true;
                }
            }
        }
    }
    public sealed class BirdieRattle : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BirdieRattle;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            LilHarpy lilHarpy = Main.LocalPlayer.GetModPlayer<LilHarpy>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BirdieRattle")
                .Replace("<class>", PetTextsColors.ClassText(lilHarpy.PetClassPrimary, lilHarpy.PetClassSecondary))
                        .Replace("<flightTime>", Math.Round(lilHarpy.fuelMax / 60f, 2).ToString())
                        .Replace("<cooldown>", Math.Round(lilHarpy.harpyCd / 60f, 2).ToString())
                        ));
        }
    }
}
