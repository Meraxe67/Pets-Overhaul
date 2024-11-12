using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public class Squashling : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Harvesting;
        public int squashlingCommonChance = 50;
        public int squashlingRareChance = 20;
        public int pumpkinArmorBonusHp = 5;
        public int pumpkinArmorBonusHarvestingFortune = 10;
        public int WornPumpkinAmount => 0 + (Player.armor[0].type == ItemID.PumpkinHelmet ? 1 : 0) + (Player.armor[1].type == ItemID.PumpkinBreastplate ? 1 : 0) + (Player.armor[2].type == ItemID.PumpkinLeggings ? 1 : 0);
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(ItemID.MagicalPumpkinSeed))
            {
                Player.statLifeMax2 += WornPumpkinAmount * pumpkinArmorBonusHp;
                Pet.harvestingFortune += WornPumpkinAmount * pumpkinArmorBonusHarvestingFortune;
            }
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Squashling squash = player.GetModPlayer<Squashling>();
            if (PickerPet.PickupChecks(item, ItemID.MagicalPumpkinSeed, out ItemPet itemChck))
            {
                if (itemChck.herbBoost == true)
                {
                    for (int i = 0; i < GlobalPet.Randomizer(Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant ? squash.squashlingRareChance : squash.squashlingCommonChance) * item.stack; i++)
                    {
                        player.QuickSpawnItemDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.HarvestingItem), item.type, 1);
                    }
                }
            }
        }
    }
    public sealed class MagicalPumpkinSeed : PetTooltip
    {
        public override PetEffect PetsEffect => squashling;
        public static Squashling squashling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Squashling pet))
                    return pet;
                else
                    return ModContent.GetInstance<Squashling>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.MagicalPumpkinSeed")
                        .Replace("<plant>", squashling.squashlingCommonChance.ToString())
                        .Replace("<rarePlant>", squashling.squashlingRareChance.ToString())
                        .Replace("<health>", squashling.pumpkinArmorBonusHp.ToString())
                        .Replace("<harvFort>", squashling.pumpkinArmorBonusHarvestingFortune.ToString())
                        .Replace("<pumpkinPieceAmount>", squashling.WornPumpkinAmount.ToString());
    }
}

