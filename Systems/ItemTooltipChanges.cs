using PetsOverhaul.Config;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class ItemTooltipChanges : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ModContent.GetInstance<PetRegistry>().TerrariaPetItemIds.ContainsValue(entity.type) || entity.type == ItemID.JojaCola;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.JojaCola)
            {
                tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.JojaCola")));
            }
            else if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.Config.TooltipShiftToggleInGame")));
            }
        }
    }
}