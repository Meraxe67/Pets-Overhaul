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
    public sealed class Chester : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int placementRange = 2;
        public int chestOpenDef = 8;
        public int suckingUpRange = 64;
        public override void PostUpdateEquips()
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Chester chester = Main.LocalPlayer.GetModPlayer<Chester>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ChesterPetItem")
                .Replace("<pickupRange>", Math.Round(chester.suckingUpRange / 16f, 2).ToString())
                .Replace("<placementRange>", chester.placementRange.ToString())
                .Replace("<chestDef>", chester.chestOpenDef.ToString())
            ));
        }
    }
}
