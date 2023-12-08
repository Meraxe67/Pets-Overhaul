using PetsOverhaul.Config;
using PetsOverhaul.ModSupport;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.ThoriumMod
{
    public sealed class Clownfish : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {

        }
    }
    public sealed class FishEgg : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if (ModManager.ThoriumMod == null)
            {
                return false;
            }

            if (ModManager.ThoriumMod.InternalNameToModdedItemId == null)
            {
                return false;
            }

            if (!ModManager.ThoriumMod.InternalNameToModdedItemId.ContainsKey("FishEgg"))
            {
                return false;
            }

            return entity.type == ModManager.ThoriumMod.InternalNameToModdedItemId["FishEgg"];
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Clownfish clownfish = Main.LocalPlayer.GetModPlayer<Clownfish>();
            tooltips.Add(new(Mod, "Tooltip0", "Pet Overhaul effects coming soon!"/*Language.GetTextValue("Mods.PetsOverhaul.FishEggTooltips.FishEgg")*/

            ));
        }
    }
}
