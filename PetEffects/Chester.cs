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
    public sealed class Chester : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int placementRange = 2;
        public int chestOpenDef = 10;
        public int suckingUpRange = 80;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(ItemID.ChesterPetItem))
            {
                Player.blockRange += placementRange;
                if (Player.chest != -1)
                {
                    Player.statDefense += chestOpenDef;
                }
            }
        }
    }
    public sealed class ChesterItemGrab : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (player.GetModPlayer<GlobalPet>().PetInUse(ItemID.ChesterPetItem))
            {
                grabRange += player.GetModPlayer<Chester>().suckingUpRange;
            }
        }
    }
    public sealed class ChesterPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ChesterPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            Chester chester = Main.LocalPlayer.GetModPlayer<Chester>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ChesterPetItem")
                .Replace("<class>", PetTextsColors.ClassText(chester.PetClassPrimary, chester.PetClassSecondary))
                .Replace("<pickupRange>", Math.Round(chester.suckingUpRange / 16f, 2).ToString())
                .Replace("<placementRange>", chester.placementRange.ToString())
                .Replace("<chestDef>", chester.chestOpenDef.ToString())
            ));
        }
    }
}
