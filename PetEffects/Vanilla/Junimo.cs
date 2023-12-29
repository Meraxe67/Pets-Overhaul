using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Junimo : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int maxLvls = 50;
        public int maxXp = 2147480000;
        public float expBoostWhileInUse = 0.33f;
        public float miningResistPerLevel = 0.0014f;
        public float fishingDamagePerLevel = 0.0015f;
        public float harvestHpPercentPerLevel = 0.0016f;
        public float fishingPowerPerLevel = 0.005f;
        public float miningOrePerLevel = 1.2f;
        public float harvestingExpToCoinPerLevel = 0.6f;
        public int popupExpHarv = 0; //Represents current existing exp value on popup texts
        public int popupExpMining = 0;
        public int popupExpFish = 0;
        public int popupIndexHarv = -1;
        public int popupIndexMining = -1;
        public int popupIndexFish = -1;
        public bool anglerQuestDayCheck = false;
        /// <summary>
        /// Default exp value used by Mining and Fishing
        /// </summary>
        public int defaultExps = 100;
        public int junimoHarvestingLevel = 1;
        public int junimoHarvestingExp = 0;
        public int[] junimoHarvestingLevelsToXp = new int[]
        {
            0,
            125,
            275,
            454,
            667,
            920,
            1219,
            1571,
            1984,
            2468,
            3033,
            3690,
            4452,
            5332,
            6345,
            7507,
            8836,
            10351,
            12072,
            14020,
            16217,
            18686,
            21451,
            24536,
            27966,
            31766,
            35961,
            40575,
            45631,
            51152,
            57158,
            63668,
            70698,
            78262,
            86370,
            95029,
            104242,
            114007,
            124318,
            135165,
        };

        public int junimoMiningLevel = 1;
        public int junimoMiningExp = 0;
        public int[] junimoMiningLevelsToXp = new int[] {
            0,
            115,
            253,
            418,
            614,
            846,
            1120,
            1443,
            1822,
            2266,
            2784,
            3386,
            4084,
            4890,
            5818,
            6883,
            8101,
            9489,
            11065,
            12849,
            14861,
            17122,
            19654,
            22479,
            25620,
            29100,
            32941,
            37166,
            41796,
            46851,
            52350,
            58310,
            64746,
            71671,
            79094,
            87021,
            95455,
            104395,
            113835,
            123765,
        };

        public int junimoFishingLevel = 1;
        public int junimoFishingExp = 0;
        public int[] junimoFishingLevelsToXp = new int[] {
            0,
            10,
            22,
            36,
            52,
            71,
            93,
            118,
            147,
            180,
            218,
            262,
            313,
            371,
            437,
            512,
            597,
            693,
            802,
            925,
            1063,
            1218,
            1391,
            1584,
            1798,
            2035,
            2296,
            2583,
            2897,
            3239,
            3611,
            4014,
            4449,
            4917,
            5418,
            5953,
            6522,
            7125,
            7761,
            8430,
        };

        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] oreList)> MiningXpPerBlock = new()
        {
            { (90, new int[] { ItemID.Obsidian, ItemID.SiltBlock, ItemID.SlushBlock, ItemID.DesertFossil }) },
            { (250, new int[] { ItemID.CopperOre, ItemID.TinOre }) },
            { (325, new int[] { ItemID.IronOre, ItemID.LeadOre, ItemID.Amethyst }) },
            { (400, new int[] { ItemID.SilverOre, ItemID.TungstenOre, ItemID.Topaz, ItemID.Sapphire, ItemID.Meteorite }) },
            { (475, new int[] { ItemID.GoldOre, ItemID.PlatinumOre, ItemID.Emerald, ItemID.Ruby, ItemID.Hellstone }) },
            { (550, new int[] { ItemID.CrimtaneOre, ItemID.DemoniteOre, ItemID.Diamond, ItemID.Amber }) },
            { (750, new int[] { ItemID.CobaltOre, ItemID.PalladiumOre }) },
            { (900, new int[] { ItemID.MythrilOre, ItemID.OrichalcumOre }) },
            { (1050, new int[] { ItemID.AdamantiteOre, ItemID.TitaniumOre, ItemID.CrystalShard }) },
            { (1200, new int[] { ItemID.ChlorophyteOre }) },
            { (1300, new int[] { ItemID.LunarOre }) },
            { (2500, new int[] { ItemID.LifeCrystal }) },
            { (12500, new int[] { ItemID.QueenSlimeCrystal }) },
            { (100000, new int[] { ItemID.DirtiestBlock }) }
        };
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] plantList)> HarvestingXpPerGathered = new()
        {
            { (69, new int[] { ItemID.RottenEgg }) },
            { (110, new int[] { ItemID.Acorn }) },
            { (125, new int[] { ItemID.AshGrassSeeds, ItemID.BlinkrootSeeds, ItemID.CorruptSeeds, ItemID.CrimsonSeeds, ItemID.DaybloomSeeds, ItemID.DeathweedSeeds, ItemID.FireblossomSeeds, ItemID.GrassSeeds, ItemID.HallowedSeeds, ItemID.JungleGrassSeeds, ItemID.MoonglowSeeds, ItemID.MushroomGrassSeeds, ItemID.ShiverthornSeeds, ItemID.WaterleafSeeds }) },
            { (165, new int[] { ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.PalmWood, ItemID.Ebonwood, ItemID.Shadewood, ItemID.StoneBlock, ItemID.RichMahogany }) },
            { (220, new int[] { ItemID.Daybloom, ItemID.Blinkroot, ItemID.Deathweed, ItemID.Fireblossom, ItemID.Moonglow, ItemID.Shiverthorn, ItemID.Waterleaf, ItemID.GlowingMushroom, ItemID.Pumpkin }) },
            { (250, new int[] { ItemID.GemTreeAmberSeed, ItemID.GemTreeAmethystSeed, ItemID.GemTreeDiamondSeed, ItemID.GemTreeEmeraldSeed, ItemID.GemTreeRubySeed, ItemID.GemTreeSapphireSeed, ItemID.GemTreeTopazSeed, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond }) },
            { (300, new int[] { ItemID.Pearlwood, ItemID.SpookyWood, ItemID.Cactus, ItemID.BambooBlock, ItemID.Mushroom, ItemID.VileMushroom, ItemID.ViciousMushroom }) },
            { (500, new int[] { ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemID.LightningWhelkShell, ItemID.TulipShell, ItemID.JunoniaShell, ItemID.JungleSpores }) },
            { (1750, new int[] { ItemID.SpicyPepper, ItemID.Pomegranate, ItemID.Elderberry, ItemID.BlackCurrant, ItemID.Apple, ItemID.Apricot, ItemID.Banana, ItemID.BloodOrange, ItemID.Cherry, ItemID.Coconut, ItemID.Grapefruit, ItemID.Lemon, ItemID.Mango, ItemID.Peach, ItemID.Pineapple, ItemID.Plum, ItemID.Rambutan }) },
            { (2000, new int[] { ModContent.ItemType<Egg>() }) },
            { (2500, new int[] { ItemID.Dragonfruit, ItemID.Starfruit, ItemID.Grapes }) },
            { (3500, new int[] { ItemID.GreenMushroom, ItemID.TealMushroom, ItemID.SkyBlueFlower, ItemID.YellowMarigold, ItemID.BlueBerries, ItemID.LimeKelp, ItemID.PinkPricklyPear, ItemID.OrangeBloodroot, ItemID.StrangePlant1, ItemID.StrangePlant2, ItemID.StrangePlant3, ItemID.StrangePlant4 }) },
            { (5000, new int[] { ItemID.JungleRose, ItemID.ManaFlower }) },
            { (10000, new int[] { ItemID.LifeFruit, ItemID.LeafWand, ItemID.LivingWoodWand, ItemID.LivingMahoganyWand, ItemID.LivingMahoganyLeafWand, ItemID.BlueEgg }) },
            { (25000, new int[] { ItemID.EucaluptusSap, ItemID.MagicalPumpkinSeed }) }
        };

        public int defaultSeaCreatureExp = 1500;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] enemyList)> FishingXpPerKill = new()
        {
            { (1500, new int[] { NPCID.EyeballFlyingFish, NPCID.ZombieMerman }) },
            { (3000, new int[] { NPCID.GoblinShark, NPCID.BloodEelBody, NPCID.BloodEelTail, NPCID.BloodEelHead }) },
            { (5000, new int[] { NPCID.BloodNautilus }) },
            { (50000, new int[] { NPCID.DukeFishron }) }
        };
        public int anglerQuestExp = 4000;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] fishList)> FishingXpPerCaught = new()
        {
            { (20, new int[] { ItemID.FishingSeaweed, ItemID.OldShoe, ItemID.TinCan }) },
            { (25, new int[] { ItemID.FrostDaggerfish }) },
            { (35, new int[] { ItemID.BombFish }) },
            { (100, new int[] { ItemID.Flounder, ItemID.Bass, ItemID.RockLobster, ItemID.Trout, ItemID.JojaCola }) },
            { (175, new int[] { ItemID.AtlanticCod, ItemID.CrimsonTigerfish, ItemID.SpecularFish, ItemID.Tuna }) },
            { (200, new int[] { ItemID.Salmon, ItemID.NeonTetra }) },
            { (250, new int[] { ItemID.ArmoredCavefish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.Ebonkoi, ItemID.FrostMinnow, ItemID.Hemopiranha, ItemID.Shrimp, ItemID.VariegatedLardfish }) },
            { (275, new int[] { ItemID.Honeyfin, ItemID.PrincessFish, ItemID.Oyster }) },
            { (350, new int[] { ItemID.WoodenCrate, ItemID.WoodenCrateHard }) },
            { (375, new int[] { ItemID.Stinkfish, ItemID.BlueJellyfish, ItemID.GreenJellyfish, ItemID.PinkJellyfish, ItemID.Obsidifish, ItemID.Prismite }) },
            { (500, new int[] { ItemID.PurpleClubberfish, ItemID.Swordfish, ItemID.ChaosFish, ItemID.FlarefinKoi, ItemID.IronCrate, ItemID.IronCrateHard }) },
            { (600, new int[] { ItemID.SawtoothShark, ItemID.Rockfish, ItemID.ReaverShark, ItemID.AlchemyTable, ItemID.HallowedFishingCrate, ItemID.HallowedFishingCrateHard, ItemID.GoldenCarp }) },
            { (700, new int[] { ItemID.JungleFishingCrate, ItemID.JungleFishingCrateHard, ItemID.CorruptFishingCrate, ItemID.CorruptFishingCrateHard, ItemID.CrimsonFishingCrate, ItemID.CrimsonFishingCrateHard, ItemID.DungeonFishingCrate, ItemID.DungeonFishingCrateHard, ItemID.FloatingIslandFishingCrate, ItemID.FloatingIslandFishingCrateHard, ItemID.FrozenCrate, ItemID.FrozenCrateHard, ItemID.OasisCrate, ItemID.OasisCrateHard, ItemID.OceanCrate, ItemID.OceanCrateHard }) },
            { (900, new int[] { ItemID.GoldenCrate, ItemID.GoldenCrateHard, ItemID.LavaCrate, ItemID.LavaCrateHard }) },
            { (1000, new int[] { ItemID.BalloonPufferfish, ItemID.FrogLeg }) },
            { (1250, new int[] { ItemID.DreadoftheRedSea, ItemID.CombatBook, ItemID.ZephyrFish }) },
            { (1500, new int[] { ItemID.BottomlessLavaBucket, ItemID.LavaAbsorbantSponge, ItemID.DemonConch }) },
            { (2000, new int[] { ItemID.LadyOfTheLake, ItemID.Toxikarp, ItemID.Bladetongue, ItemID.CrystalSerpent }) },
            { (2500, new int[] { ItemID.ObsidianSwordfish, ItemID.ScalyTruffle }) }
        };

        public int baseRoll = 100; //1
        public int rollChancePerLevel = 10; // 0.1
        public static void AnglerQuestPool()
        {
            GlobalPet.ItemWeight(ItemID.ApprenticeBait, 3000);
            GlobalPet.ItemWeight(ItemID.JourneymanBait, 300);
            GlobalPet.ItemWeight(ItemID.MasterBait, 50);
            GlobalPet.ItemWeight(ItemID.FishingPotion, 300);
            GlobalPet.ItemWeight(ItemID.SonarPotion, 330);
            GlobalPet.ItemWeight(ItemID.CratePotion, 300);
            GlobalPet.ItemWeight(ItemID.GoldCoin, 80);
            GlobalPet.ItemWeight(ItemID.PlatinumCoin, 1);

            GlobalPet.ItemWeight(ItemID.JojaCola, 150);
            GlobalPet.ItemWeight(ItemID.FrogLeg, 12);
            GlobalPet.ItemWeight(ItemID.BalloonPufferfish, 13);
            GlobalPet.ItemWeight(ItemID.PurpleClubberfish, 13);
            GlobalPet.ItemWeight(ItemID.ReaverShark, 10);
            GlobalPet.ItemWeight(ItemID.Rockfish, 10);
            GlobalPet.ItemWeight(ItemID.SawtoothShark, 10);
            GlobalPet.ItemWeight(ItemID.Swordfish, 20);
            GlobalPet.ItemWeight(ItemID.ZephyrFish, 4);
            GlobalPet.ItemWeight(ItemID.Oyster, 15);
            GlobalPet.ItemWeight(ItemID.WhitePearl, 10);
            GlobalPet.ItemWeight(ItemID.BlackPearl, 3);
            GlobalPet.ItemWeight(ItemID.PinkPearl, 1);
            GlobalPet.ItemWeight(ItemID.BottomlessLavaBucket, 3);
            GlobalPet.ItemWeight(ItemID.LavaAbsorbantSponge, 3);
            GlobalPet.ItemWeight(ItemID.DemonConch, 8);

            GlobalPet.ItemWeight(ItemID.PrincessFish, 500);
            GlobalPet.ItemWeight(ItemID.Stinkfish, 500);
            GlobalPet.ItemWeight(ItemID.ArmoredCavefish, 200);
            GlobalPet.ItemWeight(ItemID.FlarefinKoi, 75);
            GlobalPet.ItemWeight(ItemID.ChaosFish, 100);
            GlobalPet.ItemWeight(ItemID.Damselfish, 300);
            GlobalPet.ItemWeight(ItemID.DoubleCod, 250);
            GlobalPet.ItemWeight(ItemID.Ebonkoi, 200);
            GlobalPet.ItemWeight(ItemID.FrostMinnow, 300);
            GlobalPet.ItemWeight(ItemID.Hemopiranha, 200);
            GlobalPet.ItemWeight(ItemID.Honeyfin, 150);
            GlobalPet.ItemWeight(ItemID.BlueJellyfish, 50);
            GlobalPet.ItemWeight(ItemID.Obsidifish, 300);
            GlobalPet.ItemWeight(ItemID.GoldenCarp, 10);
            GlobalPet.ItemWeight(ItemID.SpecularFish, 550);
            GlobalPet.ItemWeight(ItemID.CrimsonTigerfish, 350);
            GlobalPet.ItemWeight(ItemID.VariegatedLardfish, 335);
            if (Main.hardMode)
            {
                GlobalPet.ItemWeight(ItemID.GreenJellyfish, 50);
                GlobalPet.ItemWeight(ItemID.Prismite, 50);
                GlobalPet.ItemWeight(ItemID.Toxikarp, 2);
                GlobalPet.ItemWeight(ItemID.Bladetongue, 2);
                GlobalPet.ItemWeight(ItemID.CrystalSerpent, 2);
                GlobalPet.ItemWeight(ItemID.ScalyTruffle, 1);
                GlobalPet.ItemWeight(ItemID.ObsidianSwordfish, 1);
            }
        }
        /// <summary>
        /// Updates currently existing classes popup experience text or creates a new one if its nonexistent. This won't do anything if Config option to disable Exp popups are true. Returns Index of newly created Popup text, or will still return same index as before, if it already exists.
        /// </summary>
        public int PopupExp(int classIndex, int classExp, Color color)
        {
            if (ModContent.GetInstance<Personalization>().JunimoExpGainMessage == false && Main.showItemText)
            {
                Vector2 popupVelo = new(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-6, -1));
                if (classIndex > -1)
                {
                    Main.popupText[classIndex].name = "+" + classExp.ToString() + " exp";
                    Main.popupText[classIndex].position = Player.Center;
                    Main.popupText[classIndex].velocity = popupVelo;
                    Main.popupText[classIndex].lifeTime = classExp + 180 > 700 ? 700 : (classExp + 180);
                    Main.popupText[classIndex].rotation = Main.rand.NextFloat(0.2f);
                }
                else
                {
                    classIndex = PopupText.NewText(new AdvancedPopupRequest() with { Velocity = popupVelo, DurationInFrames = 180, Color = color, Text = "+" + classExp.ToString() + " exp" }, Player.Center);
                    Main.popupText[classIndex].rotation = Main.rand.NextFloat(0.2f);
                }
            }
            return classIndex;
        }
        public static bool JunimoExpCheck(Player player)
        {
            return ModContent.GetInstance<Personalization>().JunimoExpWhileNotInInv == false || player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem) || player.miscEquips[0].type == ItemID.JunimoPetItem;
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Junimo juni = player.GetModPlayer<Junimo>();
            if (player.CanPullItem(item, player.ItemSpace(item)) && item.TryGetGlobalItem(out ItemPet itemChck) && itemChck.pickedUpBefore == false)
            {
                if (itemChck.herbBoost)
                {
                    int value = HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount;

                    if (player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem) || PickerPet.PetInUse(ItemID.JunimoPetItem))
                    {
                        PickerPet.GiveCoins(ItemPet.Randomizer((int)(juni.harvestingExpToCoinPerLevel * juni.junimoHarvestingLevel * value * item.stack)));
                    }
                    if (JunimoExpCheck(player))
                    {
                        value = ItemPet.Randomizer((int)(value * item.stack * PickerPet.harvestingExpBoost));
                        juni.junimoHarvestingExp += value;
                        juni.popupExpHarv += value;
                        juni.popupIndexHarv = juni.PopupExp(juni.popupIndexHarv, juni.popupExpHarv, new Color(205, 255, 0));
                    }
                }
                else if (itemChck.oreBoost)
                {
                    if (JunimoExpCheck(player))
                    {
                        int index = MiningXpPerBlock.IndexOf(MiningXpPerBlock.Find(x => x.oreList.Contains(item.type)));
                        int value = index == -1
                            ? ItemPet.Randomizer((int)(PickerPet.miningExpBoost * juni.defaultExps * item.stack))
                            : ItemPet.Randomizer((int)(MiningXpPerBlock[index].expAmount * item.stack * PickerPet.miningExpBoost));
                        juni.junimoMiningExp += value;
                        juni.popupExpMining += value;
                        juni.popupIndexMining = juni.PopupExp(juni.popupIndexMining, juni.popupExpMining, new Color(150, 168, 176));
                    }
                    if (player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem) || PickerPet.PetInUse(ItemID.JunimoPetItem))
                    {
                        for (int i = 0; i < ItemPet.Randomizer((int)(juni.junimoMiningLevel * juni.miningOrePerLevel) * item.stack); i++)
                        {
                            player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), item.type, 1);
                        }
                    }
                }

                if (JunimoExpCheck(player))
                {
                    if (itemChck.harvestingDrop || itemChck.fortuneHarvestingDrop)
                    {
                        int value;
                        int index = HarvestingXpPerGathered.IndexOf(HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)));
                        value = index == -1
                            ? juni.defaultExps
                            : ItemPet.Randomizer((int)(HarvestingXpPerGathered[index].expAmount * item.stack * PickerPet.harvestingExpBoost));
                        juni.junimoHarvestingExp += value;
                        juni.popupExpHarv += value;
                        juni.popupIndexHarv = juni.PopupExp(juni.popupIndexHarv, juni.popupExpHarv, new Color(205, 255, 0));
                    }
                    if (itemChck.miningDrop || itemChck.fortuneMiningDrop)
                    {
                        int value;
                        int index = MiningXpPerBlock.IndexOf(MiningXpPerBlock.Find(x => x.oreList.Contains(item.type)));
                        value = index == -1
                            ? juni.defaultExps
                            : ItemPet.Randomizer((int)(MiningXpPerBlock[index].expAmount * item.stack * PickerPet.miningExpBoost));
                        juni.junimoMiningExp += value;
                        juni.popupExpMining += value;
                        juni.popupIndexMining = juni.PopupExp(juni.popupIndexMining, juni.popupExpMining, new Color(150, 168, 176));
                    }
                    if (itemChck.fishingDrop || itemChck.fortuneFishingDrop)
                    {
                        int value;
                        int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.fishList.Contains(item.type)));
                        value = index == -1
                            ? juni.defaultExps
                            : ItemPet.Randomizer((int)(PickerPet.fishingExpBoost * FishingXpPerCaught[index].expAmount * item.stack));
                        juni.junimoFishingExp += value;
                        juni.popupExpFish += value;
                        juni.popupIndexFish = juni.PopupExp(juni.popupIndexFish, juni.popupExpFish, new Color(3, 130, 233));
                    }
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUse(ItemID.JunimoPetItem) || Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem))
            {
                int noSwapCd = (Player.HasBuff(ModContent.BuffType<ObliviousPet>()) == false && Pet.PetInUse(ItemID.JunimoPetItem)) ? 2 : 1;
                Player.endurance += junimoMiningLevel * miningResistPerLevel * noSwapCd;
                Player.GetDamage<GenericDamageClass>() *= 1f + junimoFishingLevel * fishingDamagePerLevel * noSwapCd;
                Player.statLifeMax2 += Player.statLifeMax2 * harvestHpPercentPerLevel > 1 ? (int)(Player.statLifeMax2 * junimoHarvestingLevel * harvestHpPercentPerLevel * noSwapCd) : Player.statLifeMax2 += junimoHarvestingLevel * noSwapCd;
            }

            if (Pet.PetInUse(ItemID.JunimoPetItem))
            {
                Pet.fishingExpBoost += expBoostWhileInUse;
                Pet.miningExpBoost += expBoostWhileInUse;
                Pet.harvestingExpBoost += expBoostWhileInUse;
            }
        }
        public static void RunSeaCreatureOnKill(BinaryReader reader, int whoAmI, int npcId)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }
            Main.player[player].GetModPlayer<Junimo>().ExpOnSeaCreatureKill(npcId);
        }
        public void ExpOnSeaCreatureKill(int npcId)
        {
            if (Player.whoAmI == Main.myPlayer && JunimoExpCheck(Player))
            {
                int value;
                int index = FishingXpPerKill.IndexOf(FishingXpPerKill.Find(x => x.enemyList.Contains(npcId)));
                value = index == -1
                    ? ItemPet.Randomizer((int)(defaultSeaCreatureExp * Pet.fishingExpBoost))
                    : ItemPet.Randomizer((int)(FishingXpPerKill[index].expAmount * Pet.fishingExpBoost));

                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
            }
        }
        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            if (anglerQuestDayCheck == false && (Pet.PetInUse(ItemID.JunimoPetItem) || Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem)))
            {
                AnglerQuestPool();
                if (GlobalPet.pool.Count > 0)
                {
                    for (int i = 0; i < ItemPet.Randomizer(Main.rand.Next(baseRoll + junimoFishingLevel * rollChancePerLevel, (int)((baseRoll + junimoFishingLevel * rollChancePerLevel) * Main.rand.NextFloat(1f, 1.5f)))); i++)
                    {
                        Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), GlobalPet.pool[Main.rand.Next(GlobalPet.pool.Count)], 1);
                    }
                }

                int value = ItemPet.Randomizer((int)(Pet.fishingExpBoost * anglerQuestExp));
                junimoFishingExp += value;
                popupExpFish += value;
                anglerQuestDayCheck = true;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (JunimoExpCheck(Player))
            {
                int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.fishList.Contains(fish.type)));
                int value = index == -1
                    ? ItemPet.Randomizer((int)(Pet.fishingExpBoost * defaultExps * fish.stack))
                    : ItemPet.Randomizer((int)(Pet.fishingExpBoost * FishingXpPerCaught[index].expAmount * fish.stack));
                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
            }
        }
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (Pet.PetInUse(ItemID.JunimoPetItem) || Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem))
            {
                fishingLevel += junimoFishingLevel * fishingPowerPerLevel;
            }
        }
        public override void PreUpdate()
        {
            if (popupIndexHarv > -1 && Main.popupText[popupIndexHarv].lifeTime <= 0)
            {
                popupIndexHarv = -1;
                popupExpHarv = 0;
            }
            if (popupIndexMining > -1 && Main.popupText[popupIndexMining].lifeTime <= 0)
            {
                popupIndexMining = -1;
                popupExpMining = 0;
            }
            if (popupIndexFish > -1 && Main.popupText[popupIndexFish].lifeTime <= 0)
            {
                popupIndexFish = -1;
                popupExpFish = 0;
            }
            if (Main.dayTime == true && Main.time == 0)
            {
                anglerQuestDayCheck = false;
            }

            junimoHarvestingLevel = Math.Clamp(junimoHarvestingLevel, 1, maxLvls);
            junimoMiningLevel = Math.Clamp(junimoMiningLevel, 1, maxLvls);
            junimoFishingLevel = Math.Clamp(junimoFishingLevel, 1, maxLvls);

            junimoHarvestingExp = Math.Clamp(junimoHarvestingExp, 0, maxXp);
            junimoMiningExp = Math.Clamp(junimoMiningExp, 0, maxXp);
            junimoFishingExp = Math.Clamp(junimoFishingExp, 0, maxXp);

            AdvancedPopupRequest popupMessage = new()
            {
                DurationInFrames = 300,
                Velocity = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-15, -10))
            };

            bool notificationOff = ModContent.GetInstance<Personalization>().JunimoNotifOff;
            bool soundOff = ModContent.GetInstance<Personalization>().AbilitySoundDisabled;
            if (junimoHarvestingLevel < maxLvls && junimoHarvestingExp >= junimoHarvestingLevelsToXp[junimoHarvestingLevel])
            {
                junimoHarvestingLevel++;
                if (notificationOff == false)
                {
                    if (soundOff == false)
                    {
                        SoundEngine.PlaySound(SoundID.Item35 with { PitchVariance = 0.2f, Pitch = 0.5f }, Player.position);
                    }

                    popupMessage.Color = new Color(205, 255, 0);
                    popupMessage.Text = $"Junimo harvesting level {(junimoHarvestingLevel >= maxLvls ? "maxed" : "up")}!";
                    PopupText.NewText(popupMessage, Player.position);
                }
            }
            if (junimoMiningLevel < maxLvls && junimoMiningExp >= junimoMiningLevelsToXp[junimoMiningLevel])
            {
                junimoMiningLevel++;
                if (notificationOff == false)
                {
                    if (soundOff == false)
                    {
                        SoundEngine.PlaySound(SoundID.Item35 with
                        {
                            PitchVariance = 0.2f,
                            Pitch = 0.5f
                        }, Player.position);
                    }

                    popupMessage.Color = new Color(150, 168, 176);
                    popupMessage.Text = $"Junimo mining level {(junimoMiningLevel >= maxLvls ? "maxed" : "up")}!";
                    PopupText.NewText(popupMessage, Player.position);
                }
            }

            if (junimoFishingLevel < maxLvls && junimoFishingExp >= junimoFishingLevelsToXp[junimoFishingLevel])
            {
                junimoFishingLevel++;
                if (notificationOff == false)
                {
                    if (soundOff == false)
                    {
                        SoundEngine.PlaySound(SoundID.Item35 with
                        {
                            PitchVariance = 0.2f,
                            Pitch = 0.5f
                        }, Player.position);
                    }

                    popupMessage.Color = new Color(3, 130, 233);
                    popupMessage.Text = $"Junimo fishing level {(junimoFishingLevel >= maxLvls ? "maxed" : "up")}!";
                    PopupText.NewText(popupMessage, Player.position);
                }
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("AnglerCheck", anglerQuestDayCheck);
            tag.Add("harvestlvl", junimoHarvestingLevel);
            tag.Add("harvestexp", junimoHarvestingExp);
            tag.Add("mininglvl", junimoMiningLevel);
            tag.Add("miningexp", junimoMiningExp);
            tag.Add("fishinglvl", junimoFishingLevel);
            tag.Add("fishingexp", junimoFishingExp);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("AnglerCheck", out bool anglerCheck))
            {
                anglerQuestDayCheck = anglerCheck;
            }

            if (tag.TryGet("harvestlvl", out int harvLvl))
            {
                junimoHarvestingLevel = harvLvl;
            }

            if (tag.TryGet("harvestexp", out int harvExp))
            {
                junimoHarvestingExp = harvExp;
            }

            if (tag.TryGet("mininglvl", out int miningLvl))
            {
                junimoMiningLevel = miningLvl;
            }

            if (tag.TryGet("miningexp", out int miningExp))
            {
                junimoMiningExp = miningExp;
            }

            if (tag.TryGet("fishinglvl", out int fishLvl))
            {
                junimoFishingLevel = fishLvl;
            }

            if (tag.TryGet("fishingexp", out int fishExp))
            {
                junimoFishingExp = fishExp;
            }
        }
    }

    public sealed class JunimoKilledSeaCreature : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc)
        {
            if (npc.TryGetGlobalNPC(out NpcPet npcPet) && npcPet.seaCreature && npc.friendly == false)
            {
                int playerId = npcPet.playerThatFishedUp;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
                    packet.Write((byte)PetsOverhaul.MessageType.seaCreatureOnKill);
                    packet.Write(npc.type);
                    packet.Write((byte)playerId);
                    packet.Send(toClient: playerId);
                }
                else
                {
                    Main.player[playerId].GetModPlayer<Junimo>().ExpOnSeaCreatureKill(npc.type);
                }
            }
        }
    }

    public sealed class JunimoPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.JunimoPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Junimo junimo = Main.LocalPlayer.GetModPlayer<Junimo>();
            int noSwapCd = (junimo.Player.HasBuff(ModContent.BuffType<ObliviousPet>()) == false && junimo.Pet.PetInUse(ItemID.JunimoPetItem)) ? 2 : 1;
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoPetItem")
                        .Replace("<maxLvl>", junimo.maxLvls.ToString())
                        .Replace("<expOutsideInvActiveOrNo>", ModContent.GetInstance<Personalization>().JunimoExpWhileNotInInv ? Language.GetTextValue("Mods.PetsOverhaul.Config.JunimoExpActive") : Language.GetTextValue("Mods.PetsOverhaul.Config.JunimoExpInactive"))
                        .Replace("<harvestingProfit>", Math.Round(junimo.harvestingExpToCoinPerLevel * junimo.junimoHarvestingLevel, 2).ToString())
                        .Replace("<bonusHealth>", Math.Round(junimo.junimoHarvestingLevel * junimo.harvestHpPercentPerLevel * 100 * noSwapCd, 2).ToString())
                        .Replace("<flatHealth>", (junimo.junimoHarvestingLevel * noSwapCd).ToString())
                        .Replace("<harvestLevel>", junimo.junimoHarvestingLevel.ToString())
                        .Replace("<harvestNext>", junimo.junimoHarvestingLevel >= junimo.maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoHarvestingLevelsToXp[junimo.junimoHarvestingLevel] - junimo.junimoHarvestingExp).ToString())
                        .Replace("<harvestCurrent>", junimo.junimoHarvestingExp.ToString())
                        .Replace("<miningBonusDrop>", Math.Round(junimo.junimoMiningLevel * junimo.miningOrePerLevel, 2).ToString())
                        .Replace("<bonusReduction>", Math.Round(junimo.junimoMiningLevel * noSwapCd * junimo.miningResistPerLevel * 100, 2).ToString())
                        .Replace("<miningLevel>", junimo.junimoMiningLevel.ToString())
                        .Replace("<miningNext>", junimo.junimoMiningLevel >= junimo.maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoMiningLevelsToXp[junimo.junimoMiningLevel] - junimo.junimoMiningExp).ToString())
                        .Replace("<miningCurrent>", junimo.junimoMiningExp.ToString())
                        .Replace("<fishingPower>", Math.Round(junimo.junimoFishingLevel * junimo.fishingPowerPerLevel, 2).ToString())
                        .Replace("<bonusDamage>", Math.Round(junimo.junimoFishingLevel * noSwapCd * junimo.fishingDamagePerLevel * 100, 2).ToString())
                        .Replace("<fishingLevel>", junimo.junimoFishingLevel.ToString())
                        .Replace("<fishingNext>", junimo.junimoFishingLevel >= junimo.maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoFishingLevelsToXp[junimo.junimoFishingLevel] - junimo.junimoFishingExp).ToString())
                        .Replace("<fishingCurrent>", junimo.junimoFishingExp.ToString())
                        .Replace("<rollTime>", Math.Round((junimo.baseRoll + junimo.junimoFishingLevel * junimo.rollChancePerLevel) / 100f, 2).ToString())
                        ));
        }
    }
}
