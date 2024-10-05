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
    public sealed class GlitteryButterfly : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public int wingTime = 45;
        public float currentWingPercIncr = 0.5f;
        public float healthPenalty = 0.15f;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.BedazzledNectar))
            {
                if (Player.equippedWings == null)
                {
                    Player.statLifeMax2 -= (int)(Player.statLifeMax2 * healthPenalty);
                    Player.wings = 5; //butterfly wings
                    Player.wingsLogic = 5; //I don't exactly know what wingsLogic is.
                    Player.wingTimeMax = wingTime;
                    Player.noFallDmg = true;
                }
                else
                {
                    Player.wingTimeMax += (int)(Player.wingTimeMax * currentWingPercIncr) + wingTime;
                }
            }
        }
    }
    public sealed class BedazzledNectar : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BedazzledNectar;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            GlitteryButterfly glitteryButterfly = Main.LocalPlayer.GetModPlayer<GlitteryButterfly>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BedazzledNectar")
                .Replace("<class>", PetTextsColors.ClassText(glitteryButterfly.PetClassPrimary, glitteryButterfly.PetClassSecondary))
                        .Replace("<flight>", Math.Round(glitteryButterfly.wingTime / 60f, 2).ToString())
                        .Replace("<percFlight>", Math.Round(glitteryButterfly.currentWingPercIncr * 100, 2).ToString())
                        .Replace("<healthNerf>", Math.Round(glitteryButterfly.healthPenalty * 100, 2).ToString())
                        ));
        }
    }
}
