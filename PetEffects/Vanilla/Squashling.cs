using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameInput;
using PetsOverhaul.Config;

using PetsOverhaul.Config;
using Terraria.GameInput;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public class Squashling : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public int squashlingCommonChance = 80;
        public int squashlingRareChance = 8;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.MagicalPumpkinSeed, itemChck))
            {
                if (itemChck.herbBoost == true)
                    Player.QuickSpawnItem(Player.GetSource_Misc("HarvestingItem"), item, ItemPet.Randomizer(squashlingCommonChance * item.stack));
                if (itemChck.rareHerbBoost == true)
                    Player.QuickSpawnItem(Player.GetSource_Misc("HarvestingItem"), item, ItemPet.Randomizer(squashlingRareChance * item.stack));
            }
            return true;
        }
    }
    sealed public class MagicalPumpkinSeed : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.MagicalPumpkinSeed;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            Squashling squashling = Main.LocalPlayer.GetModPlayer<Squashling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MagicalPumpkinSeed")
                        .Replace("<plant>", squashling.squashlingCommonChance.ToString())
                        .Replace("<rarePlant>", squashling.squashlingRareChance.ToString())
                        ));
        }
    }
}

