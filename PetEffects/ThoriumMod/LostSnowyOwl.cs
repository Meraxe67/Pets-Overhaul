using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using PetsOverhaul.Config;
using Terraria.GameInput;
using PetsOverhaul.ModSupport;
using System;

namespace PetsOverhaul.PetEffects.ThoriumMod
{
    sealed public class LostSnowyOwl : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            
        }
    }
    sealed public class ForgottenLetter : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if(ModManager.ThoriumMod == null) return false;
            if(ModManager.ThoriumMod.InternalNameToModdedItemId == null) return false;
            if(!ModManager.ThoriumMod.InternalNameToModdedItemId.ContainsKey("ForgottenLetter")) return false;

            return entity.type == ModManager.ThoriumMod.InternalNameToModdedItemId["ForgottenLetter"];
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            LostSnowyOwl lostSnowyOwl = Main.LocalPlayer.GetModPlayer<LostSnowyOwl>();
            tooltips.Add(new(Mod, "Tooltip0", "Pet Overhaul effects coming soon!"/*Language.GetTextValue("Mods.PetsOverhaul.ForgottenLetterTooltips.ForgottenLetter")*/
                
            ));
        }
    }
}
