using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class BlueChicken : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int blueEggTimer = 28800;
        public float tipsyMovespd = 0.1f;
        public int plantChance = 25;
        public int rarePlantChance = 50;
        public int treeChance = 10;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUse(ItemID.BlueEgg))
            {
                if (Main.rand.NextBool(blueEggTimer))
                {
                    if (Main.rand.NextBool(200))
                    {
                       if (Main.rand.NextBool(2,5))
                            Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), ItemID.BlueEgg);
                       else
                            Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), ItemID.RottenEgg);
                    }
                    else
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), ModContent.ItemType<Egg>());
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath3 with { PitchVariance = 0.1f, Pitch = 0.9f }, Player.position);
                    }
                }
            }
        }
        public static void PoolPlant()
        {
            GlobalPet.ItemWeight(ItemID.GrassSeeds, 300);
            GlobalPet.ItemWeight(ItemID.JungleGrassSeeds, 275);
            GlobalPet.ItemWeight(ItemID.AshGrassSeeds, 250);
            GlobalPet.ItemWeight(ItemID.CorruptSeeds, 225);
            GlobalPet.ItemWeight(ItemID.CrimsonSeeds, 225);
            GlobalPet.ItemWeight(ItemID.MushroomGrassSeeds, 125);
            GlobalPet.ItemWeight(ItemID.BlinkrootSeeds, 125);
            GlobalPet.ItemWeight(ItemID.DaybloomSeeds, 125);
            GlobalPet.ItemWeight(ItemID.DeathweedSeeds, 125);
            GlobalPet.ItemWeight(ItemID.FireblossomSeeds, 125);
            GlobalPet.ItemWeight(ItemID.MoonglowSeeds, 125);
            GlobalPet.ItemWeight(ItemID.ShiverthornSeeds, 125);
            GlobalPet.ItemWeight(ItemID.WaterleafSeeds, 125);
            GlobalPet.ItemWeight(ItemID.PumpkinSeed, 75);
            GlobalPet.ItemWeight(ItemID.SpicyPepper, 20);
            GlobalPet.ItemWeight(ItemID.Pomegranate, 20);
            GlobalPet.ItemWeight(ItemID.Elderberry, 20);
            GlobalPet.ItemWeight(ItemID.BlackCurrant, 20);
            GlobalPet.ItemWeight(ItemID.Rambutan, 20);
            GlobalPet.ItemWeight(ItemID.MagicalPumpkinSeed, 2);
            //ItemWeight(ItemID.Grapes, 1);
        }
        public static void PoolRarePlant()
        {
            GlobalPet.ItemWeight(ItemID.JungleSpores, 60);
            GlobalPet.ItemWeight(ItemID.SpicyPepper, 24);
            GlobalPet.ItemWeight(ItemID.Pomegranate, 24);
            GlobalPet.ItemWeight(ItemID.Elderberry, 24);
            GlobalPet.ItemWeight(ItemID.BlackCurrant, 24);
            GlobalPet.ItemWeight(ItemID.Rambutan, 24);
            GlobalPet.ItemWeight(ItemID.StrangePlant1, 4);
            GlobalPet.ItemWeight(ItemID.StrangePlant2, 4);
            GlobalPet.ItemWeight(ItemID.StrangePlant3, 4);
            GlobalPet.ItemWeight(ItemID.StrangePlant4, 4);
            GlobalPet.ItemWeight(ItemID.MagicalPumpkinSeed, 1);
            //ItemWeight(ItemID.Grapes, 1);
        }
        public static void PoolTree()
        {
            GlobalPet.ItemWeight(ItemID.Acorn, 300);
            GlobalPet.ItemWeight(ItemID.Wood, 300);
            GlobalPet.ItemWeight(ItemID.BorealWood, 250);
            GlobalPet.ItemWeight(ItemID.RichMahogany, 250);
            GlobalPet.ItemWeight(ItemID.Ebonwood, 250);
            GlobalPet.ItemWeight(ItemID.Shadewood, 250);
            GlobalPet.ItemWeight(ItemID.PalmWood, 250);
            GlobalPet.ItemWeight(ItemID.AshWood, 150);
            GlobalPet.ItemWeight(ItemID.Apple, 15);
            GlobalPet.ItemWeight(ItemID.Apricot, 15);
            GlobalPet.ItemWeight(ItemID.Banana, 15);
            GlobalPet.ItemWeight(ItemID.BloodOrange, 15);
            GlobalPet.ItemWeight(ItemID.Cherry, 15);
            GlobalPet.ItemWeight(ItemID.Coconut, 15);
            GlobalPet.ItemWeight(ItemID.Grapefruit, 15);
            GlobalPet.ItemWeight(ItemID.Lemon, 15);
            GlobalPet.ItemWeight(ItemID.Mango, 15);
            GlobalPet.ItemWeight(ItemID.Peach, 15);
            GlobalPet.ItemWeight(ItemID.Pineapple, 15);
            GlobalPet.ItemWeight(ItemID.Plum, 15);
            GlobalPet.ItemWeight(ItemID.GemTreeAmethystSeed, 15);
            GlobalPet.ItemWeight(ItemID.GemTreeTopazSeed, 14);
            GlobalPet.ItemWeight(ItemID.GemTreeSapphireSeed, 13);
            GlobalPet.ItemWeight(ItemID.GemTreeEmeraldSeed, 12);
            GlobalPet.ItemWeight(ItemID.GemTreeRubySeed, 11);
            GlobalPet.ItemWeight(ItemID.GemTreeAmberSeed, 10);
            GlobalPet.ItemWeight(ItemID.GemTreeDiamondSeed, 9);
            GlobalPet.ItemWeight(ItemID.EucaluptusSap, 1);
            //ItemWeight(ItemID.Dragonfruit, 3);
            //ItemWeight(ItemID.Starfruit, 1);
        }
        public override bool OnPickup(Item item)
        {
            if (Pet.PickupChecks(item, ItemID.BlueEgg, out ItemPet itemChck))
            {
                if (itemChck.herbBoost)
                {
                    if (Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant)
                    {
                        PoolRarePlant();
                        if (GlobalPet.pool.Count > 0)
                            for (int i = 0; i < ItemPet.Randomizer(rarePlantChance * item.stack); i++)
                            {
                                Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), GlobalPet.pool[Main.rand.Next(GlobalPet.pool.Count)], 1);
                            }
                    }
                    else if (ItemPet.treeItem[item.type])
                    {
                        PoolTree();
                        if (GlobalPet.pool.Count > 0)
                            for (int i = 0; i < ItemPet.Randomizer(treeChance * item.stack); i++)
                            {
                                Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), GlobalPet.pool[Main.rand.Next(GlobalPet.pool.Count)], 1);
                            }
                    }
                    else
                    {
                        PoolPlant();
                        if (GlobalPet.pool.Count > 0)
                            for (int i = 0; i < ItemPet.Randomizer(plantChance * item.stack); i++)
                            {
                                Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), GlobalPet.pool[Main.rand.Next(GlobalPet.pool.Count)], 1);
                            }
                    }
                }
            }
            return true;
        }
    }
    public sealed class BlueEgg : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.BlueEgg;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            BlueChicken blueChicken = Main.LocalPlayer.GetModPlayer<BlueChicken>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.BlueEgg")
                .Replace("<plantChance>", blueChicken.plantChance.ToString())
                .Replace("<rarePlantChance>", blueChicken.rarePlantChance.ToString())
                .Replace("<choppableChance>", blueChicken.treeChance.ToString())
                .Replace("<eggMinute>", (blueChicken.blueEggTimer / 3600).ToString())
            ));
        }
    }
}