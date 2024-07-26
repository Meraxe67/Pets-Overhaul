using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public class Squashling : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Harvesting;
        public int squashlingCommonChance = 50;
        public int squashlingRareChance = 10;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Squashling squash = player.GetModPlayer<Squashling>();
            if (PickerPet.PickupChecks(item, ItemID.MagicalPumpkinSeed, out ItemPet itemChck))
            {
                if (itemChck.herbBoost == true)
                {
                    for (int i = 0; i < ItemPet.Randomizer(Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant ? squash.squashlingRareChance : squash.squashlingCommonChance) * item.stack; i++)
                    {
                        player.QuickSpawnItemDirect(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), item.type, 1);
                    }
                }
            }
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Squashling squashling = Main.LocalPlayer.GetModPlayer<Squashling>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MagicalPumpkinSeed")
                .Replace("<class>", PetColors.ClassText(squashling.PetClassPrimary, squashling.PetClassSecondary))
                        .Replace("<plant>", squashling.squashlingCommonChance.ToString())
                        .Replace("<rarePlant>", squashling.squashlingRareChance.ToString())
                        ));
        }
    }
}

