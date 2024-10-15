using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class BabyDinosaur : PetEffect
    {
        public int chance = 175; // 17.5% because its with 1000
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public static void AddItemsToPool()
        {
            GlobalPet.ItemWeight(ItemID.TinOre, 10);
            GlobalPet.ItemWeight(ItemID.CopperOre, 10);
            GlobalPet.ItemWeight(ItemID.Amethyst, 9);
            GlobalPet.ItemWeight(ItemID.IronOre, 9);
            GlobalPet.ItemWeight(ItemID.LeadOre, 9);
            GlobalPet.ItemWeight(ItemID.Topaz, 8);
            GlobalPet.ItemWeight(ItemID.Sapphire, 8);
            GlobalPet.ItemWeight(ItemID.SilverOre, 8);
            GlobalPet.ItemWeight(ItemID.TungstenOre, 8);
            GlobalPet.ItemWeight(ItemID.GoldOre, 7);
            GlobalPet.ItemWeight(ItemID.PlatinumOre, 7);
            GlobalPet.ItemWeight(ItemID.Emerald, 7);
            GlobalPet.ItemWeight(ItemID.Ruby, 7);
            GlobalPet.ItemWeight(ItemID.Diamond, 6);
            GlobalPet.ItemWeight(ItemID.Amber, 6);
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet Pet = player.GetModPlayer<GlobalPet>();
            BabyDinosaur dino = player.GetModPlayer<BabyDinosaur>();
            if (Pet.PickupChecks(item, ItemID.AmberMosquito, out ItemPet itemChck) && itemChck.oreBoost)
            {
                AddItemsToPool();
                if (GlobalPet.ItemPool.Count > 0)
                {
                    for (int i = 0; i < GlobalPet.Randomizer(dino.chance * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), GlobalPet.ItemPool[Main.rand.Next(GlobalPet.ItemPool.Count)], 1);
                    }
                }
            }
        }
    }
    public sealed class AmberMosquito : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.AmberMosquito;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            BabyDinosaur babyDinosaur = Main.LocalPlayer.GetModPlayer<BabyDinosaur>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.AmberMosquito")
                .Replace("<class>", PetTextsColors.ClassText(babyDinosaur.PetClassPrimary, babyDinosaur.PetClassSecondary))
                .Replace("<oreChance>", Math.Round(babyDinosaur.chance / 10f, 2).ToString())
            ));
        }
    }
}
