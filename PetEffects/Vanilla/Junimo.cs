using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
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
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int maxLvls = 27;
        public int maxXp = 2147480000;
        public float harvestingExpToCoinMult = 1.2f;
        public int junimoHarvestingLevel = 1;
        public int junimoHarvestingExp = 0;
        public int popupExpHarv = 0; //Represents current existing exp value on popup texts
        public int popupExpMining = 0;
        public int popupExpFish = 0;
        public int popupIndexHarv = 0;
        public int popupIndexMining = 0;
        public int popupIndexFish = 0;
        public int[] junimoHarvestingLevelsToXp = new int[]
        {
            0,
            20,
            50,
            110,
            200,
            325,
            500,
            700,
            950,
            1275,
            1700,
            2175,
            2700,
            3300,
            4000,
            4700,
            5600,
            6700,
            8000,
            9500,
            11250,
            13500,
            16500,
            20000,
            25000,
            32500,
            42500
        };

        public int junimoMiningLevel = 1;
        public int junimoMiningExp = 0;
        public int[] junimoMiningLevelsToXp = new int[] {
            0,
            15,
            40,
            80,
            135,
            200,
            290,
            400,
            550,
            750,
            1100,
            1550,
            2200,
            2900,
            3800,
            5000,
            6500,
            8500,
            11000,
            14000,
            18000,
            22000,
            27000,
            33000,
            40000,
            49000,
            60000
        };

        public int junimoFishingLevel = 1;
        public int junimoFishingExp = 0;
        public int[] junimoFishingLevelsToXp = new int[] {
            0,
            5,
            15,
            30,
            50,
            75,
            105,
            140,
            190,
            240,
            300,
            375,
            460,
            555,
            675,
            800,
            950,
            1150,
            1400,
            1700,
            2100,
            2500,
            3000,
            3750,
            4750,
            6000,
            7750
        };

        public int junimoInUseMultiplier = 1;
        public bool anglerQuestDayCheck = false;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] oreList)> MiningXpPerBlock = new List<(int, int[])>
        {
            {(90, new int[]{ ItemID.Obsidian, ItemID.SiltBlock, ItemID.SlushBlock, ItemID.DesertFossil } )},
            {(200, new int[]{ ItemID.CopperOre, ItemID.TinOre } )},
            {(300, new int[]{ ItemID.IronOre, ItemID.LeadOre, ItemID.Amethyst } )},
            {(400, new int[]{ ItemID.SilverOre, ItemID.TungstenOre, ItemID.Topaz, ItemID.Sapphire, ItemID.Meteorite } )},
            {(470, new int[]{ ItemID.GoldOre, ItemID.PlatinumOre, ItemID.Emerald, ItemID.Ruby, ItemID.Hellstone } )},
            {(550, new int[]{ ItemID.CrimtaneOre, ItemID.DemoniteOre, ItemID.Diamond, ItemID.Amber } )},
            {(750, new int[]{ ItemID.CobaltOre, ItemID.PalladiumOre } )},
            {(850, new int[]{ ItemID.CrystalShard } )},
            {(900, new int[]{ ItemID.MythrilOre, ItemID.OrichalcumOre } )},
            {(1050, new int[]{ ItemID.AdamantiteOre, ItemID.TitaniumOre }) },
            {(1200, new int[]{ ItemID.ChlorophyteOre } )},
            {(1300, new int[]{ ItemID.LunarOre } )},
            {(2500, new int[]{ ItemID.LifeCrystal } )},
            {(12500, new int[]{ ItemID.QueenSlimeCrystal } )},
            {(100000,new int[]{ItemID.DirtiestBlock}) }
        };
        public int defaultSeaCreatureExp = 2000;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] enemyList)> FishingXpPerKill = new List<(int, int[])>
        {
            {(1500, new int[]{ NPCID.EyeballFlyingFish, NPCID.ZombieMerman }) },
            {(3000, new int[]{ NPCID.GoblinShark, NPCID.BloodEelBody, NPCID.BloodEelTail, NPCID.BloodEelHead }) },
            {(5000, new int[]{ NPCID.BloodNautilus } )}
        };
        public int anglerQuestExp = 4000;
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] fishList)> FishingXpPerCaught = new List<(int, int[])>
        {
            {(10, new int[]{ ItemID.FishingSeaweed, ItemID.OldShoe, ItemID.TinCan }) },
            {(25, new int[]{ ItemID.FrostDaggerfish,ItemID.BombFish }) },
            {(35, new int[]{ ItemID.BombFish }) },
            {(100, new int[]{ ItemID.Flounder,ItemID.Bass,ItemID.RockLobster,ItemID.Trout,ItemID.JojaCola }) },
            {(175, new int[]{ ItemID.AtlanticCod, ItemID.CrimsonTigerfish, ItemID.SpecularFish, ItemID.Tuna }) },
            {(200, new int[]{ ItemID.Salmon, ItemID.NeonTetra}) },
            {(250, new int[]{ ItemID.ArmoredCavefish,ItemID.Damselfish,ItemID.DoubleCod,ItemID.Ebonkoi,ItemID.FrostMinnow,ItemID.Hemopiranha,ItemID.Shrimp,ItemID.VariegatedLardfish }) },
            {(275, new int[]{ ItemID.Honeyfin,ItemID.PrincessFish,ItemID.Oyster }) },
            {(350, new int[]{ ItemID.WoodenCrate,ItemID.WoodenCrateHard }) },
            {(375, new int[]{ ItemID.Stinkfish, ItemID.BlueJellyfish,ItemID.GreenJellyfish,ItemID.PinkJellyfish,ItemID.Obsidifish,ItemID.Prismite}) },
            {(400, new int[]{ ItemID.PurpleClubberfish,ItemID.Swordfish }) },
            {(475, new int[]{ ItemID.ChaosFish,ItemID.FlarefinKoi,ItemID.IronCrate, ItemID.IronCrateHard }) },
            {(500, new int[]{ ItemID.SawtoothShark,ItemID.Rockfish,ItemID.ReaverShark,ItemID.AlchemyTable }) },
            {(550, new int[]{ ItemID.JungleFishingCrate,ItemID.JungleFishingCrateHard,ItemID.CorruptFishingCrate,ItemID.CorruptFishingCrateHard,ItemID.CrimsonFishingCrate,ItemID.CrimsonFishingCrateHard,ItemID.DungeonFishingCrate,ItemID.DungeonFishingCrateHard,ItemID.FloatingIslandFishingCrate,ItemID.FloatingIslandFishingCrateHard,ItemID.FrozenCrate,ItemID.FrozenCrateHard,ItemID.OasisCrate,ItemID.OasisCrateHard,ItemID.OceanCrate,ItemID.OceanCrateHard }) },
            {(600, new int[]{ ItemID.HallowedFishingCrate,ItemID.HallowedFishingCrateHard, ItemID.GoldenCarp }) },
            {(750, new int[]{ ItemID.GoldenCrate,ItemID.GoldenCrateHard,ItemID.LavaCrate,ItemID.LavaCrateHard }) },
            {(1000, new int[]{ ItemID.BalloonPufferfish,ItemID.FrogLeg}) },
            {(1250, new int[]{ ItemID.DreadoftheRedSea, ItemID.CombatBook,ItemID.ZephyrFish }) },
            {(1500, new int[]{ ItemID.BottomlessLavaBucket,ItemID.LavaAbsorbantSponge,ItemID.DemonConch }) },
            {(2000, new int[]{ ItemID.LadyOfTheLake,ItemID.Toxikarp,ItemID.Bladetongue,ItemID.CrystalSerpent }) },
            {(2500, new int[]{ ItemID.ObsidianSwordfish,ItemID.ScalyTruffle }) },
        };
        /// <summary>
        /// Remember to insert the expAmount as *100 from intended amount, eg. 2.5 exp should be written as 250.
        /// </summary>
        public static List<(int expAmount, int[] plantList)> HarvestingXpPerGathered = new List<(int, int[])>
        {
            {(69, new int[]{ ItemID.RottenEgg} )},
            {(80, new int[]{ ItemID.Acorn}) },
            {(125, new int[]{ ItemID.AshGrassSeeds,ItemID.BlinkrootSeeds,ItemID.CorruptSeeds,ItemID.CrimsonSeeds,ItemID.DaybloomSeeds,ItemID.DeathweedSeeds,ItemID.FireblossomSeeds,ItemID.GrassSeeds,ItemID.HallowedSeeds,ItemID.JungleGrassSeeds,ItemID.MoonglowSeeds,ItemID.MushroomGrassSeeds,ItemID.ShiverthornSeeds,ItemID.WaterleafSeeds }) },
            {(150, new int[]{ ItemID.Wood,ItemID.AshWood,ItemID.BorealWood,ItemID.PalmWood,ItemID.Ebonwood,ItemID.Shadewood,ItemID.StoneBlock,ItemID.RichMahogany}) },
            {(220, new int[]{ ItemID.Daybloom,ItemID.Blinkroot,ItemID.Deathweed,ItemID.Fireblossom,ItemID.Moonglow,ItemID.Shiverthorn,ItemID.Waterleaf,ItemID.Mushroom,ItemID.GlowingMushroom,ItemID.VileMushroom,ItemID.ViciousMushroom,ItemID.Pumpkin,ItemID.Cactus,ItemID.BambooBlock }) },
            {(250, new int[]{ ItemID.GemTreeAmberSeed,ItemID.GemTreeAmethystSeed,ItemID.GemTreeDiamondSeed,ItemID.GemTreeEmeraldSeed,ItemID.GemTreeRubySeed,ItemID.GemTreeSapphireSeed,ItemID.GemTreeTopazSeed,ItemID.Amethyst,ItemID.Topaz,ItemID.Sapphire,ItemID.Emerald,ItemID.Ruby,ItemID.Amber,ItemID.Diamond }) },
            {(300, new int[]{ ItemID.Pearlwood,ItemID.SpookyWood}) },
            {(350, new int[]{ ItemID.JungleSpores}) },
            {(500, new int[]{ ItemID.Coral,ItemID.Seashell,ItemID.Starfish}) },
            {(1250, new int[]{ ItemID.SpicyPepper,ItemID.Pomegranate,ItemID.Elderberry,ItemID.BlackCurrant,ItemID.Apple,ItemID.Apricot,ItemID.Banana,ItemID.BloodOrange,ItemID.Cherry,ItemID.Coconut,ItemID.Grapefruit,ItemID.Lemon,ItemID.Mango,ItemID.Peach,ItemID.Pineapple,ItemID.Plum }) },
            {(1500, new int[]{ ModContent.ItemType<Egg>()}) },
            {(2500, new int[]{ ItemID.Dragonfruit,ItemID.Starfruit,ItemID.Grapes}) },
            {(3500, new int[] { ItemID.GreenMushroom,ItemID.TealMushroom,ItemID.SkyBlueFlower,ItemID.YellowMarigold,ItemID.BlueBerries,ItemID.LimeKelp,ItemID.PinkPricklyPear,ItemID.OrangeBloodroot,ItemID.StrangePlant1,ItemID.StrangePlant2,ItemID.StrangePlant3,ItemID.StrangePlant4})},
            {(4000, new int[]{ ItemID.JungleRose,ItemID.ManaFlower }) },
            {(10000, new int[]{ ItemID.LifeFruit,ItemID.LeafWand,ItemID.LivingWoodWand,ItemID.LivingMahoganyWand,ItemID.LivingMahoganyLeafWand}) },
            {(25000, new int[] {ItemID.EucaluptusSap,ItemID.MagicalPumpkinSeed}) }
        };
        /// <summary>
        /// Updates currently existing classes popup experience text or creates a new one if its nonexistent. This won't do anything if Config option to disable Exp popups are true. Returns Index of newly created Popup text, or will still return same index as before, if it already exists.
        /// </summary>
        public int PopupExp(int classIndex, int classExp, Color color)
        {
            if (ModContent.GetInstance<Personalization>().JunimoExpGainMessage == false)
            {
                Vector2 popupVelo = new(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-6, -1));
                if (classIndex != -1)
                {
                    Main.popupText[classIndex].name = "+" + classExp.ToString() + " exp";
                    Main.popupText[classIndex].position = Player.Center;
                    Main.popupText[classIndex].velocity = popupVelo;
                    Main.popupText[classIndex].lifeTime = (classExp + 180) > 700 ? 700 : (classExp + 180);
                    Main.popupText[classIndex].Update(classIndex);
                }
                else
                {
                    classIndex = PopupText.NewText(new AdvancedPopupRequest() with { Velocity = popupVelo, DurationInFrames = 180, Color = color, Text = "+" + classExp.ToString() + " exp" }, Player.Center);
                }

                Main.popupText[classIndex].rotation = Main.rand.NextFloat(0.2f);
                //Main.popupText[classIndex].scale += classExp * 0.01f; - doesnt seem to work -
            }
            return classIndex;
        }
        public bool junimoExpCheck()
        {
            if (ModContent.GetInstance<Personalization>().JunimoExpWhileNotInInv == false || Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem) || Pet.PetInUse(ItemID.JunimoPetItem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (junimoExpCheck() && target.active == false && target.GetGlobalNPC<NpcPet>().seaCreature)
            {
                int value;
                int index = FishingXpPerKill.IndexOf(FishingXpPerKill.Find(x => x.Item2.Contains(target.type)));
                if (index == -1)
                {
                    value = ItemPet.Randomizer((int)(defaultSeaCreatureExp * junimoInUseMultiplier * Pet.fishingExpBoost));
                }
                else
                {
                    value = ItemPet.Randomizer((int)(FishingXpPerKill[index].expAmount * junimoInUseMultiplier * Pet.fishingExpBoost));
                }

                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, Color.LightSkyBlue);
            }

        }
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck))
            {
                if (Player.CanPullItem(item, Player.ItemSpace(item)) && itemChck.pickedUpBefore == false)
                {
                    if (itemChck.rareHerbBoost || itemChck.herbBoost)
                    {
                        int value;
                        int index = HarvestingXpPerGathered.IndexOf(HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)));
                        if (index == -1)
                        {
                            value = 50; //0.5 exp
                        }
                        else
                        {
                            value = HarvestingXpPerGathered[index].expAmount;
                        }

                        if (item.maxStack != 1 && Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem) || Pet.PetInUse(ItemID.JunimoPetItem))
                        {
                            int junimoCash = ItemPet.Randomizer((int)(harvestingExpToCoinMult * junimoHarvestingLevel * value * junimoInUseMultiplier * item.stack), 100);
                            if (junimoCash > 1000000)
                            {
                                Player.QuickSpawnItem(Player.GetSource_Misc("JunimoItem"), ItemID.PlatinumCoin, junimoCash / 1000000);
                                junimoCash %= 1000000;
                            }
                            if (junimoCash > 10000)
                            {
                                Player.QuickSpawnItem(Player.GetSource_Misc("JunimoItem"), ItemID.GoldCoin, junimoCash / 10000);
                                junimoCash %= 10000;
                            }
                            if (junimoCash > 100)
                            {
                                Player.QuickSpawnItem(Player.GetSource_Misc("JunimoItem"), ItemID.SilverCoin, junimoCash / 100);
                                junimoCash %= 100;
                            }
                            Player.QuickSpawnItem(Player.GetSource_Misc("JunimoItem"), ItemID.CopperCoin, junimoCash);
                        }
                        if (junimoExpCheck())
                        {
                            value = ItemPet.Randomizer((int)(value * junimoInUseMultiplier * item.stack * Pet.harvestingExpBoost));
                            junimoHarvestingExp += value;
                            popupExpHarv += value;
                            popupIndexHarv = PopupExp(popupIndexHarv, popupExpHarv, Color.LightGreen);
                        }
                    }
                    else if (itemChck.oreBoost)
                    {
                        if (junimoExpCheck())
                        {
                            int index = MiningXpPerBlock.IndexOf(MiningXpPerBlock.Find(x => x.Item2.Contains(item.type)));
                            int value;
                            if (index == -1)
                            {
                                value = ItemPet.Randomizer((int)(Pet.miningExpBoost * 50 * item.stack * junimoInUseMultiplier));
                            }
                            else
                            {
                                value = ItemPet.Randomizer((int)(MiningXpPerBlock[index].expAmount * junimoInUseMultiplier * item.stack * Pet.miningExpBoost));
                            }

                            junimoMiningExp += value;
                            popupExpMining += value;
                            popupIndexMining = PopupExp(popupIndexMining, popupExpMining, Color.LightGray);
                        }
                        if (item.maxStack != 1 && Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem) || Pet.PetInUse(ItemID.JunimoPetItem))
                        {
                            for (int i = 0; i < ItemPet.Randomizer(junimoMiningLevel * junimoInUseMultiplier * item.stack); i++)
                            {
                                Player.QuickSpawnItem(item.GetSource_Misc("JunimoItem"), item, 1);
                            }
                        }
                    }

                    if (junimoExpCheck())
                    {
                        if (itemChck.harvestingDrop || itemChck.fortuneHarvestingDrop)
                        {
                            int index = HarvestingXpPerGathered.IndexOf(HarvestingXpPerGathered.Find(x => x.Item2.Contains(item.type)));
                            if (index == -1)
                            {

                            }
                            else
                            {
                                int value = ItemPet.Randomizer((int)(HarvestingXpPerGathered[index].expAmount * junimoInUseMultiplier * item.stack * Pet.harvestingExpBoost));
                                junimoHarvestingExp += value;
                                popupExpHarv += value;
                                popupIndexHarv = PopupExp(popupIndexHarv, popupExpHarv, Color.LightGreen);
                            }
                        }
                        if (itemChck.miningDrop || itemChck.fortuneMiningDrop)
                        {
                            int index = MiningXpPerBlock.IndexOf(MiningXpPerBlock.Find(x => x.Item2.Contains(item.type)));
                            if (index == -1)
                            {

                            }
                            else
                            {
                                int value = ItemPet.Randomizer((int)(MiningXpPerBlock[index].expAmount * junimoInUseMultiplier * item.stack * Pet.miningExpBoost));
                                junimoMiningExp += value;
                                popupExpMining += value;
                                popupIndexMining = PopupExp(popupIndexMining, popupExpMining, Color.LightGray);
                            }
                        }
                        if (itemChck.fishingDrop || itemChck.fortuneFishingDrop)
                        {
                            int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.Item2.Contains(item.type)));
                            if (index == -1)
                            {

                            }
                            else
                            {
                                int value = ItemPet.Randomizer((int)(Pet.fishingExpBoost * FishingXpPerCaught[index].expAmount * junimoInUseMultiplier * item.stack));
                                junimoFishingExp += value;
                                popupExpFish += value;
                                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, Color.LightSkyBlue);
                            }
                        }

                    }
                }
            }
            return true;
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUse(ItemID.JunimoPetItem) || Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem))
            {
                int noSwapCd = Player.HasBuff(ModContent.BuffType<ObliviousPet>()) ? 1 : 2;
                Player.endurance += junimoMiningLevel * 0.002f * noSwapCd;
                Player.GetDamage<GenericDamageClass>() *= 1f + junimoFishingLevel * 0.002f * noSwapCd;
                if (Player.statLifeMax2 * junimoHarvestingLevel * 0.0025f * junimoInUseMultiplier > junimoHarvestingLevel * noSwapCd)
                {
                    Player.statLifeMax2 += (int)(Player.statLifeMax2 * junimoHarvestingLevel * 0.0025f * noSwapCd);
                }
                else
                {
                    Player.statLifeMax2 += junimoHarvestingLevel * noSwapCd;
                }
            }

        }
        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
            if (anglerQuestDayCheck == false && junimoExpCheck())
            {
                int value = ItemPet.Randomizer((int)(Pet.fishingExpBoost * anglerQuestExp * junimoInUseMultiplier));
                junimoFishingExp += value;
                popupExpFish += value;
                anglerQuestDayCheck = true;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, Color.LightSkyBlue);
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (junimoExpCheck())
            {
                int index = FishingXpPerCaught.IndexOf(FishingXpPerCaught.Find(x => x.Item2.Contains(fish.type)));
                int value;
                if (index == -1)
                {
                    value = ItemPet.Randomizer((int)(Pet.fishingExpBoost * 50 * junimoInUseMultiplier * fish.stack));
                }
                else
                {
                    value = ItemPet.Randomizer((int)(Pet.fishingExpBoost * FishingXpPerCaught[index].expAmount * junimoInUseMultiplier * fish.stack));
                }

                junimoFishingExp += value;
                popupExpFish += value;
                popupIndexFish = PopupExp(popupIndexFish, popupExpFish, Color.LightSkyBlue);
            }
        }
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (Pet.PetInUse(ItemID.JunimoPetItem))
            {
                fishingLevel += junimoFishingLevel * 0.01f;
            }
            else if (Player.HasItemInInventoryOrOpenVoidBag(ItemID.JunimoPetItem))
            {
                fishingLevel += junimoFishingLevel * 0.005f;
            }
        }
        public override void PreUpdate()
        {
            if (popupIndexHarv != -1 && Main.popupText[popupIndexHarv].lifeTime <= 0)
            {
                popupIndexHarv = -1;
                popupExpHarv = 0;
            }
            if (popupIndexMining != -1 && Main.popupText[popupIndexMining].lifeTime <= 0)
            {
                popupIndexMining = -1;
                popupExpMining = 0;
            }
            if (popupIndexFish != -1 && Main.popupText[popupIndexFish].lifeTime <= 0)
            {
                popupIndexFish = -1;
                popupExpFish = 0;
            }
            if (Main.dayTime == true && Main.time == 0)
            {
                anglerQuestDayCheck = false;
            }

            junimoInUseMultiplier = Pet.PetInUse(ItemID.JunimoPetItem) ? 2 : 1;

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

                    popupMessage.Color = Color.LightGreen;
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

                    popupMessage.Color = Color.LightGray;
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

                    popupMessage.Color = Color.LightSkyBlue;
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
            anglerQuestDayCheck = tag.GetBool("AnglerCheck");
            junimoHarvestingLevel = tag.GetInt("harvestlvl");
            junimoHarvestingExp = tag.GetInt("harvestexp");
            junimoMiningLevel = tag.GetInt("mininglvl");
            junimoMiningExp = tag.GetInt("miningexp");
            junimoFishingLevel = tag.GetInt("fishinglvl");
            junimoFishingExp = tag.GetInt("fishingexp");
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
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Junimo junimo = Main.LocalPlayer.GetModPlayer<Junimo>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoPetItem")
                        .Replace("<maxLvl>", junimo.maxLvls.ToString())
                        .Replace("<expOutsideInvActiveOrNo>", ModContent.GetInstance<Personalization>().JunimoExpWhileNotInInv? "inactive" : "active")
                        .Replace("<harvestingProfit>", Math.Round(junimo.harvestingExpToCoinMult * junimo.junimoInUseMultiplier * junimo.junimoHarvestingLevel, 5).ToString())
                        .Replace("<bonusHealth>", Math.Round(junimo.junimoHarvestingLevel * 0.25f * junimo.junimoInUseMultiplier, 5).ToString())
                        .Replace("<flatHealth>", (junimo.junimoHarvestingLevel * junimo.junimoInUseMultiplier).ToString())
                        .Replace("<harvestLevel>", junimo.junimoHarvestingLevel.ToString())
                        .Replace("<harvestNext>", junimo.junimoHarvestingLevel >= junimo.maxLvls ? "Max Level!" : (junimo.junimoHarvestingLevelsToXp[junimo.junimoHarvestingLevel] - junimo.junimoHarvestingExp).ToString())
                        .Replace("<harvestCurrent>", junimo.junimoHarvestingExp.ToString())
                        .Replace("<miningBonusDrop>", (junimo.junimoMiningLevel * junimo.junimoInUseMultiplier).ToString())
                        .Replace("<bonusReduction>", Math.Round(junimo.junimoMiningLevel * junimo.junimoInUseMultiplier * 0.2f, 5).ToString())
                        .Replace("<miningLevel>", junimo.junimoMiningLevel.ToString())
                        .Replace("<miningNext>", junimo.junimoMiningLevel >= junimo.maxLvls ? "Max Level!" : (junimo.junimoMiningLevelsToXp[junimo.junimoMiningLevel] - junimo.junimoMiningExp).ToString())
                        .Replace("<miningCurrent>", junimo.junimoMiningExp.ToString())
                        .Replace("<fishingPower>", Math.Round(junimo.junimoFishingLevel * junimo.junimoInUseMultiplier * 0.5f, 5).ToString())
                        .Replace("<bonusDamage>", Math.Round(junimo.junimoFishingLevel * junimo.junimoInUseMultiplier * 0.2f, 5).ToString())
                        .Replace("<fishingLevel>", junimo.junimoFishingLevel.ToString())
                        .Replace("<fishingNext>", junimo.junimoFishingLevel >= junimo.maxLvls ? "Max Level!" : (junimo.junimoFishingLevelsToXp[junimo.junimoFishingLevel] - junimo.junimoFishingExp).ToString())
                        .Replace("<fishingCurrent>", junimo.junimoFishingExp.ToString())
                        ));
        }
    }
}
