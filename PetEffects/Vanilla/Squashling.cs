using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
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
        public int squashlingCommonChance = 50;
        public int squashlingRareChance = 10;
        public override bool OnPickup(Item item)
        {
            if (Pet.PickupChecks(item, ItemID.MagicalPumpkinSeed, out ItemPet itemChck))
            {
                if (itemChck.herbBoost == true)
                {
                    for (int i = 0; i < ItemPet.Randomizer((Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant) ? squashlingRareChance : squashlingCommonChance) * item.stack; i++)
                    {
                        Player.QuickSpawnItemDirect(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), item, 1);
                    }
                }
            }
            return base.OnPickup(item);
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

