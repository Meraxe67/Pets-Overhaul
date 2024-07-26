using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// ModPlayer Class that includes Dictionaries and Methods used by Pets Overhaul's sound effects.
    /// </summary>
    public sealed class PetSounds : ModPlayer
    {
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

        public ReLogic.Utilities.SlotId PlayAmbientSoundFromItemId(int itemId)
        {
            SoundStyle petAmbientSound = SoundID.MenuClose;

            if (PetItemIdToAmbientSound.ContainsKey(itemId))
            {
                petAmbientSound = PetItemIdToAmbientSound[itemId][Main.rand.Next(PetItemIdToAmbientSound[itemId].Length)];
            }

            return petAmbientSound == SoundID.MenuClose ? ReLogic.Utilities.SlotId.Invalid : SoundEngine.PlaySound(petAmbientSound, Player.Center);
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
