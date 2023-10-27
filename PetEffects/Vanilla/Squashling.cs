using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public class Squashling : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int squashlingCommonChance = 80;
        public int squashlingRareChance = 8;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.MagicalPumpkinSeed, itemChck))
            {
                if (itemChck.herbBoost == true)
                {
                    for (int i = 0; i < ItemPet.Randomizer(squashlingCommonChance * item.stack); i++)
                    {
                        Player.QuickSpawnItem(Player.GetSource_Misc("HarvestingItem"), item, 1);
                    }
                }

                if (itemChck.rareHerbBoost == true)
                {
                    for (int i = 0; i < ItemPet.Randomizer(squashlingRareChance * item.stack); i++)
                    {
                        Player.QuickSpawnItem(Player.GetSource_Misc("HarvestingItem"), item, 1);
                    }
                }
            }
            return true;
        }
    }
    public sealed class MagicalPumpkinSeed : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MagicalPumpkinSeed;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Squashling squashling = Main.LocalPlayer.GetModPlayer<Squashling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MagicalPumpkinSeed")
                        .Replace("<plant>", squashling.squashlingCommonChance.ToString())
                        .Replace("<rarePlant>", squashling.squashlingRareChance.ToString())
                        ));
        }
    }
}

