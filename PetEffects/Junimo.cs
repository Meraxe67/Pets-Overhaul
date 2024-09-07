using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.PetEffects
{
    public sealed class Junimo : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int maxLvls = 50;
        public int maxXp = 2147480000;
        public float miningResistPerLevel = 0.0022f;
        public float fishingDamagePerLevel = 0.0025f;
        public double harvestingHealthperLevel = 1.5;
        public float harvestingCoin = 0.6f;
        public float miningCoin = 0.4f;
        public float fishingCoin = 6f;
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
100,
214,
344,
492,
660,
851,
1067,
1311,
1586,
1895,
2242,
2630,
3063,
3545,
4080,
4672,
5326,
6046,
6837,
7703,
8649,
9680,
10800,
12014,
13326,
14740,
16260,
17889,
19630,
21485,
23456,
25545,
27753,
30080,
32525,
35087,
37764,
40553,
43450,
46451,
49551,
52744,
56023,
59380,
62807,
66295,
69835,
73417,
77031,
        };

        public int junimoMiningLevel = 1;
        public int junimoMiningExp = 0;
        public int[] junimoMiningLevelsToXp = new int[] {
0,
90,
193,
311,
445,
597,
769,
964,
1184,
1432,
1711,
2024,
2374,
2764,
3198,
3680,
4214,
4804,
5454,
6168,
6950,
7804,
8734,
9744,
10838,
12020,
13294,
14663,
16130,
17698,
19369,
21145,
23027,
25016,
27112,
29314,
31621,
34031,
36542,
39150,
41851,
44641,
47514,
50464,
53484,
56567,
59705,
62890,
66113,
69365,

        };

        public int junimoFishingLevel = 1;
        public int junimoFishingExp = 0;
        public int[] junimoFishingLevelsToXp = new int[] {
            0,
10,
21,
33,
46,
60,
75,
92,
111,
132,
155,
180,
208,
239,
273,
310,
350,
394,
442,
494,
550,
611,
677,
748,
824,
906,
994,
1088,
1188,
1294,
1406,
1525,
1651,
1784,
1924,
2071,
2225,
2385,
2551,
2723,
2901,
3084,
3272,
3465,
3662,
3863,
4067,
4274,
4483,
4693,
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

        //public int baseRoll = 100; //1
        //public int rollChancePerLevel = 10; // 0.1
        //public static void AnglerQuestPool()
        //{
        //    GlobalPet.ItemWeight(ItemID.ApprenticeBait, 3000);
        //    GlobalPet.ItemWeight(ItemID.JourneymanBait, 300);
        //    GlobalPet.ItemWeight(ItemID.MasterBait, 50);
        //    GlobalPet.ItemWeight(ItemID.FishingPotion, 300);
        //    GlobalPet.ItemWeight(ItemID.SonarPotion, 330);
        //    GlobalPet.ItemWeight(ItemID.CratePotion, 300);
        //    GlobalPet.ItemWeight(ItemID.GoldCoin, 80);
        //    GlobalPet.ItemWeight(ItemID.PlatinumCoin, 1);

        //    GlobalPet.ItemWeight(ItemID.JojaCola, 150);
        //    GlobalPet.ItemWeight(ItemID.FrogLeg, 12);
        //    GlobalPet.ItemWeight(ItemID.BalloonPufferfish, 13);
        //    GlobalPet.ItemWeight(ItemID.PurpleClubberfish, 13);
        //    GlobalPet.ItemWeight(ItemID.ReaverShark, 10);
        //    GlobalPet.ItemWeight(ItemID.Rockfish, 10);
        //    GlobalPet.ItemWeight(ItemID.SawtoothShark, 10);
        //    GlobalPet.ItemWeight(ItemID.Swordfish, 20);
        //    GlobalPet.ItemWeight(ItemID.ZephyrFish, 4);
        //    GlobalPet.ItemWeight(ItemID.Oyster, 15);
        //    GlobalPet.ItemWeight(ItemID.WhitePearl, 10);
        //    GlobalPet.ItemWeight(ItemID.BlackPearl, 3);
        //    GlobalPet.ItemWeight(ItemID.PinkPearl, 1);
        //    GlobalPet.ItemWeight(ItemID.BottomlessLavaBucket, 3);
        //    GlobalPet.ItemWeight(ItemID.LavaAbsorbantSponge, 3);
        //    GlobalPet.ItemWeight(ItemID.DemonConch, 8);

        //    GlobalPet.ItemWeight(ItemID.PrincessFish, 500);
        //    GlobalPet.ItemWeight(ItemID.Stinkfish, 500);
        //    GlobalPet.ItemWeight(ItemID.ArmoredCavefish, 200);
        //    GlobalPet.ItemWeight(ItemID.FlarefinKoi, 75);
        //    GlobalPet.ItemWeight(ItemID.ChaosFish, 100);
        //    GlobalPet.ItemWeight(ItemID.Damselfish, 300);
        //    GlobalPet.ItemWeight(ItemID.DoubleCod, 250);
        //    GlobalPet.ItemWeight(ItemID.Ebonkoi, 200);
        //    GlobalPet.ItemWeight(ItemID.FrostMinnow, 300);
        //    GlobalPet.ItemWeight(ItemID.Hemopiranha, 200);
        //    GlobalPet.ItemWeight(ItemID.Honeyfin, 150);
        //    GlobalPet.ItemWeight(ItemID.BlueJellyfish, 50);
        //    GlobalPet.ItemWeight(ItemID.Obsidifish, 300);
        //    GlobalPet.ItemWeight(ItemID.GoldenCarp, 10);
        //    GlobalPet.ItemWeight(ItemID.SpecularFish, 550);
        //    GlobalPet.ItemWeight(ItemID.CrimsonTigerfish, 350);
        //    GlobalPet.ItemWeight(ItemID.VariegatedLardfish, 335);
        //    if (Main.hardMode)
        //    {
        //        GlobalPet.ItemWeight(ItemID.GreenJellyfish, 50);
        //        GlobalPet.ItemWeight(ItemID.Prismite, 50);
        //        GlobalPet.ItemWeight(ItemID.Toxikarp, 2);
        //        GlobalPet.ItemWeight(ItemID.Bladetongue, 2);
        //        GlobalPet.ItemWeight(ItemID.CrystalSerpent, 2);
        //        GlobalPet.ItemWeight(ItemID.ScalyTruffle, 1);
        //        GlobalPet.ItemWeight(ItemID.ObsidianSwordfish, 1);
        //    }
        //}
        /// <summary>
        /// Updates currently existing classes popup experience text or creates a new one if its nonexistent. This won't do anything if Config option to disable Exp popups are true. Returns Index of newly created Popup text, or will still return same index as before, if it already exists.
        /// </summary>
        public int PopupExp(int classIndex, int classExp, Color color)
        {
            if (Main.showItemText)
            {
                Vector2 popupVelo = new(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-6, -1));
                if (classIndex > -1)
                {
                    Main.popupText[classIndex].name = "+" + classExp.ToString() + " exp";
                    Main.popupText[classIndex].position = Player.Center;
                    Main.popupText[classIndex].velocity = popupVelo;
                    Main.popupText[classIndex].lifeTime = classExp + 180 > 700 ? 700 : classExp + 180;
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
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Junimo juni = player.GetModPlayer<Junimo>();
            if (PickerPet.PickupChecks(item, ItemID.JunimoPetItem, out ItemPet itemChck))
            {
                if (itemChck.harvestingDrop || itemChck.fortuneHarvestingDrop || itemChck.herbBoost)
                {
                    int value = HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount;
                    value = value * item.stack;
                    PickerPet.GiveCoins(GlobalPet.Randomizer((int)(juni.harvestingCoin * juni.junimoHarvestingLevel * value)));
                    value = GlobalPet.Randomizer(value);
                    juni.junimoHarvestingExp += value;
                    juni.popupExpHarv += value;
                    juni.popupIndexHarv = juni.PopupExp(juni.popupIndexHarv, juni.popupExpHarv, new Color(205, 255, 0));
                }
                else if (itemChck.fishingDrop || itemChck.fortuneFishingDrop)
                {
                    int value;
                    int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.fishList.Contains(item.type)));
                    value = index == -1
                        ? juni.defaultExps * item.stack
                        : FishingXpPerCaught[index].expAmount * item.stack;
                    PickerPet.GiveCoins(GlobalPet.Randomizer((int)(juni.fishingCoin * juni.junimoFishingLevel * value)));
                    value = GlobalPet.Randomizer(value);
                    juni.junimoFishingExp += value;
                    juni.popupExpFish += value;
                    juni.popupIndexFish = juni.PopupExp(juni.popupIndexFish, juni.popupExpFish, new Color(3, 130, 233));
                }
                else if (itemChck.blockNotByPlayer && (itemChck.oreBoost || itemChck.miningDrop || itemChck.fortuneMiningDrop))
                {
                    int index = MiningXpPerBlock.IndexOf(MiningXpPerBlock.Find(x => x.oreList.Contains(item.type)));
                    int value = index == -1
                        ? juni.defaultExps * item.stack
                        : MiningXpPerBlock[index].expAmount * item.stack;
                    PickerPet.GiveCoins(GlobalPet.Randomizer((int)(juni.miningCoin * juni.junimoMiningLevel * value)));
                    value = GlobalPet.Randomizer(value);
                    juni.junimoMiningExp += value;
                    juni.popupExpMining += value;
                    juni.popupIndexMining = juni.PopupExp(juni.popupIndexMining, juni.popupExpMining, new Color(150, 168, 176));
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.JunimoPetItem))
            {
                Player.endurance += junimoMiningLevel * miningResistPerLevel;
                Player.GetDamage<GenericDamageClass>() *= 1f + junimoFishingLevel * fishingDamagePerLevel;
                Player.statLifeMax2 += (int)(harvestingHealthperLevel * junimoHarvestingLevel);
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
            if (Player.whoAmI == Main.myPlayer && Player.miscEquips[0].type == ItemID.JunimoPetItem)
            {
                int value;
                int index = FishingXpPerKill.IndexOf(FishingXpPerKill.Find(x => x.enemyList.Contains(npcId)));
                value = index == -1
                    ? GlobalPet.Randomizer(defaultSeaCreatureExp)
                    : GlobalPet.Randomizer(FishingXpPerKill[index].expAmount);

                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
            }
        }
        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            if (anglerQuestDayCheck == false && Pet.PetInUse(ItemID.JunimoPetItem))
            {
                int value = GlobalPet.Randomizer(anglerQuestExp);
                junimoFishingExp += value;
                popupExpFish += value;
                anglerQuestDayCheck = true;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (Pet.PetInUse(ItemID.JunimoPetItem))
            {
                int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.fishList.Contains(fish.type)));
                int value = index == -1
                    ? defaultExps * fish.stack
                    : FishingXpPerCaught[index].expAmount * fish.stack;
                Pet.GiveCoins(GlobalPet.Randomizer((int)(fishingCoin * junimoFishingLevel * value)));
                value = GlobalPet.Randomizer(value);
                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, new Color(3, 130, 233));
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

            bool soundOff = ModContent.GetInstance<Personalization>().AbilitySoundDisabled;
            if (junimoHarvestingLevel < maxLvls && junimoHarvestingExp >= junimoHarvestingLevelsToXp[junimoHarvestingLevel])
            {
                junimoHarvestingLevel++;
                if (soundOff == false)
                {
                    SoundEngine.PlaySound(SoundID.Item35 with { PitchVariance = 0.2f, Pitch = 0.5f }, Player.position);
                }

                popupMessage.Color = new Color(205, 255, 0);
                popupMessage.Text = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoLevel")
                    .Replace("<class>", Language.GetTextValue($"Mods.PetsOverhaul.Classes.Harvesting"))
                    .Replace("<upOrMax>", junimoHarvestingLevel >= maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxed") : Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoUp"));
                PopupText.NewText(popupMessage, Player.position);
            }
            if (junimoMiningLevel < maxLvls && junimoMiningExp >= junimoMiningLevelsToXp[junimoMiningLevel])
            {
                junimoMiningLevel++;
                if (soundOff == false)
                {
                    SoundEngine.PlaySound(SoundID.Item35 with
                    {
                        PitchVariance = 0.2f,
                        Pitch = 0.5f
                    }, Player.position);
                }

                popupMessage.Color = new Color(150, 168, 176);
                popupMessage.Text = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoLevel")
                    .Replace("<class>", Language.GetTextValue($"Mods.PetsOverhaul.Classes.Mining"))
                    .Replace("<upOrMax>", junimoHarvestingLevel >= maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxed") : Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoUp"));
                PopupText.NewText(popupMessage, Player.position);
            }

            if (junimoFishingLevel < maxLvls && junimoFishingExp >= junimoFishingLevelsToXp[junimoFishingLevel])
            {
                junimoFishingLevel++;
                if (soundOff == false)
                {
                    SoundEngine.PlaySound(SoundID.Item35 with
                    {
                        PitchVariance = 0.2f,
                        Pitch = 0.5f
                    }, Player.position);
                }

                popupMessage.Color = new Color(3, 130, 233);
                popupMessage.Text = Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoLevel")
                    .Replace("<class>", Language.GetTextValue($"Mods.PetsOverhaul.Classes.Fishing"))
                    .Replace("<upOrMax>", junimoHarvestingLevel >= maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxed") : Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoUp"));
                PopupText.NewText(popupMessage, Player.position);
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
                    packet.Write((byte)MessageType.SeaCreatureOnKill);
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
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Junimo junimo = Main.LocalPlayer.GetModPlayer<Junimo>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoPetItem")
                .Replace("<class>", PetTextsColors.ClassText(junimo.PetClassPrimary, junimo.PetClassSecondary))
                        .Replace("<maxLvl>", junimo.maxLvls.ToString())
                        .Replace("<harvestingProfit>", Math.Round(junimo.harvestingCoin * junimo.junimoHarvestingLevel, 2).ToString())
                        .Replace("<flatHealth>", Math.Round(junimo.junimoHarvestingLevel * junimo.harvestingHealthperLevel, 1).ToString())
                        .Replace("<harvestLevel>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()}:{junimo.junimoHarvestingLevel}]")
                        .Replace("<harvestNext>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Harvesting).Hex3()}:{(junimo.junimoHarvestingLevel >= junimo.maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoHarvestingLevelsToXp[junimo.junimoHarvestingLevel] - junimo.junimoHarvestingExp).ToString())}]")
                        .Replace("<harvestCurrent>", junimo.junimoHarvestingExp.ToString())
                        .Replace("<miningProfit>", Math.Round(junimo.miningCoin * junimo.junimoMiningLevel, 2).ToString())
                        .Replace("<bonusReduction>", Math.Round(junimo.junimoMiningLevel * junimo.miningResistPerLevel * 100, 2).ToString())
                        .Replace("<miningLevel>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()}:{junimo.junimoMiningLevel}]")
                        .Replace("<miningNext>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Mining).Hex3()}:{(junimo.junimoMiningLevel >= junimo.maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoMiningLevelsToXp[junimo.junimoMiningLevel] - junimo.junimoMiningExp).ToString())}]")
                        .Replace("<miningCurrent>", junimo.junimoMiningExp.ToString())
                        .Replace("<fishingProfit>", Math.Round(junimo.fishingCoin * junimo.junimoFishingLevel, 2).ToString())
                        .Replace("<bonusDamage>", Math.Round(junimo.junimoFishingLevel * junimo.fishingDamagePerLevel * 100, 2).ToString())
                        .Replace("<fishingLevel>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()}:{junimo.junimoFishingLevel}]")
                        .Replace("<fishingNext>", $"[c/{PetTextsColors.ClassEnumToColor(PetClasses.Fishing).Hex3()}:{(junimo.junimoFishingLevel >= junimo.maxLvls ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxLevelText") : (junimo.junimoFishingLevelsToXp[junimo.junimoFishingLevel] - junimo.junimoFishingExp).ToString())}]")
                        .Replace("<fishingCurrent>", junimo.junimoFishingExp.ToString())
                        ));
        }
    }
}
