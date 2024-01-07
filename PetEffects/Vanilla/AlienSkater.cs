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
    public sealed class AlienSkater : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public float accelerator = 0.15f;
        public float wingTime = 1.4f;
        public float speedMult = 1.3f;
        public float accMult = 1.5f;
        public float speedAccIncr = 0.9f;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.MartianPetItem))
            {
                Player.runAcceleration += 0.25f;
                Player.wingTimeMax = (int)(Player.wingTimeMax * wingTime);
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            AlienSkater alienSkater = Main.LocalPlayer.GetModPlayer<AlienSkater>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MartianPetItem")
                .Replace("<wingMult>", alienSkater.wingTime.ToString())
                .Replace("<acc>", Math.Round(alienSkater.accelerator * 100, 2).ToString())
                .Replace("<speedMult>", alienSkater.speedMult.ToString())
                .Replace("<accMult>", alienSkater.accMult.ToString())
                .Replace("<flatSpdAcc>", Math.Round(alienSkater.speedAccIncr * 100, 2).ToString())
            ));
        }
    }
}
