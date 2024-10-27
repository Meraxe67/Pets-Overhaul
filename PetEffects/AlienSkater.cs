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
    public sealed class AlienSkater : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public float accelerator = 0.50f;
        public float wingTime = 0.25f;
        public float speedMult = 1.2f;
        public float accMult = 1.35f;
        public float speedAccIncr = 0.4f;
        public override void PostUpdateRunSpeeds()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MartianPetItem))
            {
                Player.runAcceleration *= accelerator + 1f;
                Player.wingTimeMax = (int)(Player.wingTimeMax * (1f + wingTime));
            }
        }
    }
    public sealed class AlienSkaterWing : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            if (player.TryGetModPlayer(out AlienSkater alienSkater) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ItemID.MartianPetItem))
            {
                speed *= alienSkater.speedMult;
                acceleration *= alienSkater.accMult;
                speed += alienSkater.speedAccIncr;
                acceleration += alienSkater.speedAccIncr;
            }
        }
    }
    public sealed class MartianPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MartianPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            AlienSkater alienSkater = Main.LocalPlayer.GetModPlayer<AlienSkater>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MartianPetItem")
                .Replace("<class>", PetTextsColors.ClassText(alienSkater.PetClassPrimary, alienSkater.PetClassSecondary))
                .Replace("<wingMult>", Math.Round(alienSkater.wingTime * 100, 2).ToString())
                .Replace("<acc>", Math.Round(alienSkater.accelerator * 100, 2).ToString())
                .Replace("<speedMult>", alienSkater.speedMult.ToString())
                .Replace("<accMult>", alienSkater.accMult.ToString())
                .Replace("<flatSpdAcc>", Math.Round(alienSkater.speedAccIncr * 100, 2).ToString())
            ));
        }
    }
}
