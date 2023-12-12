using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// ModPlayer class that implements several helperfunctions to reduce repitition.
    /// </summary>
    public sealed class PetRegistry : ModPlayer
    {
        public static Dictionary<string, int> LightPetNamesAndItems = new Dictionary<string, int>
        {
            {"Flickerwick",ItemID.DD2PetGhost },
            {"Crimson Heart",ItemID.CrimsonHeart },
            {"Fairy",ItemID.FairyBell},
            {"Jack 'O Lantern",ItemID.PumpkingPetItem },
            {"Fairy Princess",ItemID.FairyQueenPetItem },
            {"Magic Lantern",ItemID.MagicLantern },
            {"Shadow Orb",ItemID.ShadowOrb },
            {"Suspicious Looking Eye",ItemID.SuspiciousLookingTentacle },
            {"Toy Golem",ItemID.GolemPetItem},
            {"Wisp",ItemID.WispinaBottle },
        };

        public static Dictionary<string, int> PetNamesAndItems = new Dictionary<string, int>
        {
            {"Turtle", ItemID.Seaweed},
            {"Baby Dinosaur", ItemID.AmberMosquito},
            {"Baby Eater", ItemID.EatersBone},
            {"Baby Face Monster", ItemID.BoneRattle},
            {"Baby Grinch", ItemID.BabyGrinchMischiefWhistle},
            {"Baby Hornet", ItemID.Nectar},
            {"Baby Imp", ItemID.HellCake},
            {"Baby Penguin", ItemID.Fish},
            {"Baby Red Panda", ItemID.BambooLeaf},
            {"Dungeon Guardian", ItemID.BoneKey},
            {"Baby Snowman", ItemID.ToySled},
            {"Baby Truffle", ItemID.StrangeGlowingMushroom},
            {"Baby Werewolf", ItemID.FullMoonSqueakyToy},
            {"Bernie", ItemID.BerniePetItem},
            {"Black Cat", ItemID.UnluckyYarn},
            {"Blue Chicken", ItemID.BlueEgg},
            {"Bunny", ItemID.Carrot},
            {"Caveling Gardener", ItemID.GlowTulip},
            {"Chester", ItemID.ChesterPetItem},
            {"Companion Cube", ItemID.CompanionCube},
            {"Cursed Sapling", ItemID.CursedSapling},
            {"Dirtiest Block", ItemID.DirtiestBlock},
            {"Dynamite Kitten", ItemID.BallOfFuseWire},
            {"Estee", ItemID.CelestialWand},
            {"Eyeball Spring", ItemID.EyeSpring},
            {"Fennec Fox", ItemID.ExoticEasternChewToy},
            {"Glittery Butterfly", ItemID.BedazzledNectar},
            {"Glommer", ItemID.GlommerPetItem},
            {"Hoardagron", ItemID.DD2PetDragon},
            {"Junimo", ItemID.JunimoPetItem},
            {"Lil' Harpy", ItemID.BirdieRattle},
            {"Lizard", ItemID.LizardEgg},
            {"Mini Minotaur", ItemID.TartarSauce},
            {"Parrot", ItemID.ParrotCracker},
            {"Pig Man", ItemID.PigPetItem},
            {"Plantero", ItemID.MudBud},
            {"Propeller Gato", ItemID.DD2PetGato},
            {"Puppy", ItemID.DogWhistle},
            {"Sapling", ItemID.Seedling},
            {"Spider", ItemID.SpiderEgg},
            {"Shadow Mimic", ItemID.OrnateShadowKey},
            {"SharkPup", ItemID.SharkBait},
            {"Spiffo", ItemID.SpiffoPlush},
            {"Squashling", ItemID.MagicalPumpkinSeed},
            {"Sugar Glider", ItemID.EucaluptusSap},
            {"Tiki Spirit", ItemID.TikiTotem},
            {"Volt Bunny", ItemID.LightningCarrot},
            {"Zephyr Fish", ItemID.ZephyrFish},
            {"Suspicious Eye", ItemID.EyeOfCthulhuPetItem},
            {"Spider Brain", ItemID.BrainOfCthulhuPetItem},
            {"Eater of Worms", ItemID.EaterOfWorldsPetItem},
            {"Slime Prince", ItemID.KingSlimePetItem},
            {"HoneyBee", ItemID.QueenBeePetItem},
            {"Tiny Deerclops", ItemID.DeerclopsPetItem},
            {"Skeletron Jr.", ItemID.SkeletronPetItem},
            {"Slime Princess", ItemID.QueenSlimePetItem},
            {"Mini Prime", ItemID.SkeletronPrimePetItem},
            {"Destroyer", ItemID.DestroyerPetItem},
            {"Rez and Spaz", ItemID.TwinsPetItem},
            {"Everscream Sapling", ItemID.EverscreamPetItem},
            {"Alien Skater", ItemID.MartianPetItem},
            {"Baby Ogre", ItemID.DD2OgrePetItem},
            {"Tiny Fishron", ItemID.DukeFishronPetItem},
            {"Phantasmal Dragon", ItemID.LunaticCultistPetItem},
            {"Itsy Betsy", ItemID.DD2BetsyPetItem},
            {"Ice Queen", ItemID.IceQueenPetItem},
            {"Plantera Seedling", ItemID.PlanteraPetItem},
            {"Moonling", ItemID.MoonLordPetItem},
            {"Slime Royals", ItemID.ResplendentDessert},
        };

        public Dictionary<int, SoundStyle[]> PetItemIdToHurtSound = new Dictionary<int, SoundStyle[]>() {
            {
                ItemID.Seaweed,
                    new SoundStyle[] {
                        SoundID.NPCHit24 with {
                            PitchVariance = 0.4f
                        }
                    }
            }, {
                ItemID.LunaticCultistPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit55 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.LizardEgg,
                new SoundStyle[] {
                    SoundID.NPCHit26 with {
                        PitchVariance = 0.6f
                    }
                }
            }, {
                ItemID.BoneKey,
                new SoundStyle[] {
                    SoundID.NPCHit2 with {
                        PitchVariance = 0.05f, Pitch = 0.1f
                    }
                }
            }, {
                ItemID.SkeletronPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit2 with {
                        PitchVariance = 0.05f, Pitch = 0.1f
                    }
                }
            }, {
                ItemID.ToySled,
                new SoundStyle[] {
                    SoundID.NPCHit11 with {
                        Pitch = -0.5f, PitchVariance = 0.2f
                    }
                }
            }, {
                ItemID.FullMoonSqueakyToy,
                new SoundStyle[] {
                    SoundID.NPCHit6 with {
                        PitchVariance = 0.4f
                    }
                }
            }, {
                ItemID.CursedSapling,
                new SoundStyle[] {
                    SoundID.NPCHit7 with {
                        PitchVariance = 0.4f
                    }
                }
            }, {
                ItemID.CelestialWand,
                new SoundStyle[] {
                    SoundID.NPCHit5 with {
                        PitchVariance = 0.2f, Pitch = 0.5f
                    }
                }
            }, {
                ItemID.UnluckyYarn,
                new SoundStyle[] {
                    SoundID.Meowmere with {
                        PitchVariance = 0.4f, Pitch = 0.6f
                    }
                }
            }, {
                ItemID.CompanionCube,
                new SoundStyle[] {
                    SoundID.NPCHit55 with {
                        Pitch = -0.3f, PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.ParrotCracker,
                new SoundStyle[] {
                    SoundID.NPCHit46 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.GlommerPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit35 with {
                        PitchVariance = 0.2f, Pitch = -0.5f
                    }
                }
            }, {
                ItemID.SpiderEgg,
                new SoundStyle[] {
                    SoundID.NPCHit29 with {
                        PitchVariance = 0.3f
                    }
                }
            }, {
                ItemID.OrnateShadowKey,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.DestroyerPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.SkeletronPrimePetItem,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.TwinsPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit4 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.PigPetItem,
                new SoundStyle[] {
                    SoundID.Zombie39 with {
                        PitchVariance = 0.3f
                    }
                }
            }, {
                ItemID.LightningCarrot,
                new SoundStyle[] {
                    SoundID.NPCHit34 with {
                        PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.BrainOfCthulhuPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit9 with {
                        Pitch = 0.1f, PitchVariance = 0.4f
                    }
                }
            }, {
                ItemID.DD2OgrePetItem,
                new SoundStyle[] {
                    SoundID.DD2_OgreHurt with {
                        PitchVariance = 0.7f, Volume = 0.7f
                    }
                }
            }, {
                ItemID.MartianPetItem,
                new SoundStyle[] {
                    SoundID.NPCHit39 with {
                        Pitch = 0.2f, PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.DD2BetsyPetItem,
                new SoundStyle[] {
                    SoundID.DD2_BetsyHurt with {
                        Pitch = 0.3f, PitchVariance = 0.5f
                    }
                }
            }, {
                ItemID.DukeFishronPetItem,
                new SoundStyle[] {
                    SoundID.Zombie39 with {
                        PitchVariance = 0.8f
                    }
                }
            }, {
                ItemID.ChesterPetItem,
                new SoundStyle[] {
                    SoundID.ChesterOpen with {
                        PitchVariance = 0.2f, Pitch = -0.6f
                    }, SoundID.ChesterClose with {
                        PitchVariance = 0.2f, Pitch = -0.6f
                    }
                }
            }, {
                ItemID.MoonLordPetItem,
                new SoundStyle[] {
                    SoundID.Zombie100 with {
                        PitchVariance = 0.5f, Volume = 0.5f
                    }, SoundID.Zombie101 with {
                        PitchVariance = 0.5f, Volume = 0.5f
                    }, SoundID.Zombie102 with {
                        PitchVariance = 0.5f, Volume = 0.5f
                    }
                }
            },
        };
        public Dictionary<int, SoundStyle[]> PetItemIdToAmbientSound = new Dictionary<int, SoundStyle[]>() {
            {
                ItemID.LizardEgg,
                    new SoundStyle[] {
                        SoundID.Zombie37 with {
                                PitchVariance = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                            },
                            SoundID.Zombie36 with {
                                PitchVariance = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                            }
                    }
            }, {
                ItemID.ParrotCracker,
                new SoundStyle[] {
                    SoundID.Cockatiel with {
                            PitchVariance = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.8f
                        },
                        SoundID.Macaw with {
                            PitchVariance = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.8f
                        }
                }
            }, {
                ItemID.BoneRattle,
                new SoundStyle[] {
                    SoundID.Zombie8 with {
                        PitchVariance = 0.5f, Pitch = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                    }
                }
            }, {
                ItemID.DukeFishronPetItem,
                new SoundStyle[] {
                    SoundID.Zombie20 with {
                        PitchVariance = 0.5f, Pitch = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                    }
                }
            }, {
                ItemID.MartianPetItem,
                new SoundStyle[] {
                    SoundID.Zombie59 with {
                            PitchVariance = 0.5f, Pitch = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                        },
                        SoundID.Zombie60 with {
                            PitchVariance = 0.5f, Pitch = 0.5f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                        }
                }
            }, {
                ItemID.QueenBeePetItem,
                new SoundStyle[] {
                    SoundID.Zombie50 with {
                            PitchVariance = 0.2f, Pitch = 0.9f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                        },
                        SoundID.Zombie51 with {
                            PitchVariance = 0.2f, Pitch = 0.9f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                        },
                        SoundID.Zombie52 with {
                            PitchVariance = 0.2f, Pitch = 0.9f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew, Volume = 0.5f
                        }
                }
            }
        };
        public Dictionary<int, SoundStyle> PetItemidToKillSound = new Dictionary<int, SoundStyle>
        {
            {
                ItemID.Seaweed,
                SoundID.NPCDeath27 with { PitchVariance = 0.4f }
            }, {
                ItemID.LunaticCultistPetItem,
                SoundID.NPCDeath59 with { Pitch = -0.2f, PitchVariance = 0.2f }
            }, {
                ItemID.SpiderEgg,
                SoundID.NPCDeath47 with { PitchVariance = 0.5f }
            }, {
                ItemID.LightningCarrot,
                SoundID.Item94 with { PitchVariance = 0.3f }
            }, {
                ItemID.LizardEgg,
                SoundID.NPCDeath29 with { PitchVariance = 0.3f }
            }, {
                ItemID.CursedSapling,
                SoundID.NPCDeath5 with { PitchVariance = 0.5f }
            }, {
                ItemID.EverscreamPetItem,
                SoundID.NPCDeath5 with { PitchVariance = 0.5f }
            }, {
                ItemID.PigPetItem,
                SoundID.NPCDeath20 with { Pitch = 0.5f, PitchVariance = 0.3f }
            }, {
                ItemID.ParrotCracker,
                SoundID.NPCDeath48 with { PitchVariance = 0.5f }
            }, {
                ItemID.BrainOfCthulhuPetItem,
                SoundID.NPCDeath11 with { Pitch = -0.2f, PitchVariance = 0.2f }
            }, {
                ItemID.DD2OgrePetItem,
                SoundID.DD2_OgreDeath with { PitchVariance = 0.7f, Volume = 0.7f }
            }, {
                ItemID.MartianPetItem,
                SoundID.NPCDeath57 with { Pitch = -0.3f, PitchVariance = 0.5f }
            }, {
                ItemID.DD2BetsyPetItem,
                SoundID.DD2_BetsyScream with { Pitch = -0.5f, PitchVariance = 0.2f }
            }, {
                ItemID.DukeFishronPetItem,
                SoundID.NPCDeath20 with { Pitch = -0.2f, PitchVariance = 0.3f }
            }, {
                ItemID.MoonLordPetItem,
                SoundID.NPCDeath62 with { PitchVariance = 0.5f, Volume = 0.8f }
            }
        };
        public ReLogic.Utilities.SlotId PlayHurtSoundFromItemId(int itemId)
        {
            SoundStyle itemsHurtSound = SoundID.MenuClose;

            if (PetItemIdToHurtSound.ContainsKey(itemId))
            {
                itemsHurtSound = PetItemIdToHurtSound[itemId][Main.rand.Next(PetItemIdToHurtSound[itemId].Length)];
            }
            else if (itemId == ItemID.BerniePetItem)
            {
                itemsHurtSound = Player.Male == true ? (SoundID.DSTMaleHurt with { PitchVariance = 0.2f }) : (SoundID.DSTFemaleHurt with { PitchVariance = 0.2f });
            }

            return itemsHurtSound == SoundID.MenuClose ? ReLogic.Utilities.SlotId.Invalid : SoundEngine.PlaySound(itemsHurtSound, Player.Center);
        }

        public ReLogic.Utilities.SlotId PlayEquipSoundFromItemId(int itemId)
        {
            SoundStyle petSummonSound = SoundID.MenuClose;

            if (PetItemIdToAmbientSound.ContainsKey(itemId))
            {
                petSummonSound = PetItemIdToAmbientSound[itemId][Main.rand.Next(PetItemIdToAmbientSound[itemId].Length)];
            }

            return petSummonSound == SoundID.MenuClose ? ReLogic.Utilities.SlotId.Invalid : SoundEngine.PlaySound(petSummonSound, Player.Center);
        }

        public ReLogic.Utilities.SlotId PlayKillSoundFromItemId(int itemId)
        {
            SoundStyle petKillSound = SoundID.MenuClose;

            if (PetItemidToKillSound.ContainsKey(itemId))
            {
                petKillSound = PetItemidToKillSound[itemId];
            }
            else if (itemId == ItemID.CompanionCube)
            {
                petKillSound = Main.rand.NextBool(25)
                    ? (SoundID.NPCDeath61 with { PitchVariance = 0.5f, Volume = 0.7f })
                    : (SoundID.NPCDeath59 with { PitchVariance = 0.5f });
            }

            return petKillSound == SoundID.MenuClose ? ReLogic.Utilities.SlotId.Invalid : SoundEngine.PlaySound(petKillSound, Player.Center);
        }
    }
}
