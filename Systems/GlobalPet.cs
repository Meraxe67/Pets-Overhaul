using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// ModPlayer class that contains many useful Methods and fields for Pet implementation, this is the central class of Pets Overhaul.
    /// </summary>
    public sealed class GlobalPet : ModPlayer
    {
        public static InputMode PlayerInputMode => PlayerInput.CurrentProfile.InputModes.ContainsKey(InputMode.Keyboard) ? InputMode.Keyboard : InputMode.XBoxGamepad;
        /// <summary>
        /// Modify this value if you want to reduce or increase lifesteal & healing by Pets for any reason, such as a Mod applying an effect that reduces healings. Basically a modifier on heals from Pets. Used in PetRecovery().
        /// </summary>
        public float petHealMultiplier = 1f;
        /// <summary>
        /// Modify this value if you want to reduce or increase shields applied by Pets for any reason, such as increasing Shield gain with a condition. Basically a modifier on shields coming from Pets. Used in AddShield().
        /// </summary>
        public float petShieldMultiplier = 1f;
        /// <summary>
        /// Influences the chance to increase stack of the item from your pet that doesn't fit into any other fortune category. This also increases all other fortunes with half effectiveness.
        /// </summary>
        public int globalFortune = 0;
        /// <summary>
        /// Influences the chance to increase stack of the item that your Harvesting Pet gave.
        /// </summary>
        public int harvestingFortune = 0;
        /// <summary>
        /// Influences the chance to increase stack of the item that your Mining Pet gave.
        /// </summary>
        public int miningFortune = 0;
        /// <summary>
        /// Influences the chance to increase stack of the item that your Fishing Pet gave.
        /// </summary>
        public int fishingFortune = 0;
        /// <summary>
        /// Contains list of debuffs that are related to burning.
        /// </summary>
        public static List<int> BurnDebuffs = [BuffID.Burning, BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.CursedInferno, BuffID.ShadowFlame, BuffID.Frostburn2];
        /// <summary>
        /// Contains list of enemies that are associated with Corruption biome.
        /// </summary>
        public static List<int> CorruptEnemies = [NPCID.EaterofSouls, NPCID.LittleEater, NPCID.BigEater, NPCID.DevourerHead, NPCID.DevourerBody, NPCID.DevourerTail, NPCID.EaterofWorldsHead, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.Corruptor, NPCID.CorruptSlime, NPCID.Slimeling, NPCID.Slimer, NPCID.Slimer2, NPCID.SeekerHead, NPCID.SeekerBody, NPCID.SeekerTail, NPCID.DarkMummy, NPCID.CursedHammer, NPCID.Clinger, NPCID.BigMimicCorruption, NPCID.DesertGhoulCorruption, NPCID.SandsharkCorrupt, NPCID.PigronCorruption, NPCID.CorruptGoldfish, NPCID.CorruptBunny, NPCID.CorruptPenguin, NPCID.DesertDjinn];

        /// <summary>
        /// Contains list of enemies that are associated with Crimson biome.
        /// </summary>
        public static List<int> CrimsonEnemies = [NPCID.BloodCrawler, NPCID.BloodCrawlerWall, NPCID.FaceMonster, NPCID.Crimera, NPCID.LittleCrimera, NPCID.BigCrimera, NPCID.BrainofCthulhu, NPCID.Creeper, NPCID.Herpling, NPCID.Crimslime, NPCID.BigCrimslime, NPCID.LittleCrimslime, NPCID.BloodJelly, NPCID.BloodFeeder, NPCID.BloodMummy, NPCID.CrimsonAxe, NPCID.IchorSticker, NPCID.FloatyGross, NPCID.BigMimicCrimson, NPCID.DesertGhoulCrimson, NPCID.SandsharkCrimson, NPCID.PigronCrimson, NPCID.CrimsonGoldfish, NPCID.CrimsonBunny, NPCID.CrimsonPenguin, NPCID.DesertDjinn];

        /// <summary>
        /// Contains list of enemies that are associated with Hallow biome. 
        /// </summary>
        public static List<int> HallowEnemies = [NPCID.Pixie, NPCID.Unicorn, NPCID.RainbowSlime, NPCID.Gastropod, NPCID.LightMummy, NPCID.QueenSlimeBoss, NPCID.QueenSlimeMinionBlue, NPCID.QueenSlimeMinionPink, NPCID.QueenSlimeMinionPurple, NPCID.HallowBoss, NPCID.IlluminantSlime, NPCID.IlluminantBat, NPCID.ChaosElemental, NPCID.EnchantedSword, NPCID.BigMimicHallow, NPCID.DesertGhoulHallow, NPCID.PigronHallow, NPCID.SandsharkHallow];

        public Color skin;
        public bool skinColorChanged = false;
        /// <summary>
        /// Is cleared at the end of PostUpdate hook. Main use is for Weighted Lists with usage of ItemWeight() Method. Retrieve a value using Main.rand.Next() for its index for Weighted list usage.
        /// </summary>
        public static List<int> ItemPool = new();

        public List<(int shieldAmount, int shieldTimer)> petShield = new();
        public int currentShield = 0;
        public int shieldToBeReduced = 0;
        public bool jumpRegistered = false;
        public int petSwapCooldown = 600;
        internal int previousPetItem = 0;
        /// <summary>
        /// This field ticks down every frame in PreUpdate() hook. Does not go below -1. Plays 'cooldown refreshment' sound effect upon reaching 0 and displays Timer while higher than 0. Usually is recommended to use Mod's timer mechanic for timers that the Player should be aware of.
        /// </summary>
        public int timer = -1;
        /// <summary>
        /// Use this field to set how long the Pet's cooldown will be.
        /// </summary>
        public int timerMax = 0;
        /// <summary>
        /// Increase this value to reduce ability cooldowns. Eg. 0.1f increases how fast ability will return by 10%. Negative values will increase the cooldowns. Negative is capped at -0.9f. Do not use this to directly reduce a cooldown, use the timer field instead. Ability Haste reduces timerMax with a more balanced calculation.
        /// </summary>
        public float abilityHaste = 0;
        /// <summary>
        /// Used to change alternating color of maximum Light Pet Rolls alongside colorSwitched, increases 0.01f every frame, until hitting 1f, where it decreases 0.01f every frame and so on.
        /// </summary>
        public static float ColorVal { get; internal set; }

        /// <summary>
        /// Used to change alternating color of maximum Light Pet Rolls alongside colorVal
        /// </summary>
        private static bool colorSwitched = false;

        //Methods etc.
        #region
        /// <summary>
        /// Sets the Pet ability cooldown while taking ability haste into consideration. Recommended to use this in PreUpdate, combined with condition that expected Pet is in use.
        /// </summary>
        public void SetPetAbilityTimer(int cooldown)
        {
            if (abilityHaste < -0.9f)
            {
                abilityHaste = -0.9f;
            }

            timerMax = (int)(cooldown * (1 / (1 + abilityHaste)));
        }
        public static IEntitySource GetSource_Pet(EntitySourcePetIDs typeId, string context = null)
        {
            return new EntitySource_Pet
            {
                ContextType = typeId,
                Context = context
            };
        }
        /// <summary>
        /// Add an id to pool List with how many times it should be added as the weight.
        /// </summary>
        public static void ItemWeight(int itemId, int weight)
        {
            for (int i = 0; i < weight; i++)
            {
                ItemPool.Add(itemId);
            }
        }
        /// <summary>
        /// Runs all of the standart OnPickup's checks for the Pet to work with no problems.
        /// </summary>
        public bool PickupChecks(Item item, int petitemid, out ItemPet itemPet)
        {
            if (PetInUse(petitemid) && Player.CanPullItem(item, Player.ItemSpace(item)) && item.TryGetGlobalItem(out ItemPet petItemCheck) && petItemCheck.pickedUpBefore == false)
            {
                itemPet = petItemCheck;
                return true;
            }
            itemPet = null;
            return false;
        }
        /// <summary>
        /// Spawns coins accordingly to the given value and converts it to a higher coin tier if possible. Source of spawned coins will be globalItem. Recommended use is with GlobalPet.Randomizer() to achieve more precise and 'natural' values. (100x the intended coin value)
        /// </summary>
        public void GiveCoins(int coinAmount)
        {
            if (coinAmount > 1000000)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.PlatinumCoin, coinAmount / 1000000);
                coinAmount %= 1000000;
            }
            if (coinAmount > 10000)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.GoldCoin, coinAmount / 10000);
                coinAmount %= 10000;
            }
            if (coinAmount > 100)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.SilverCoin, coinAmount / 100);
                coinAmount %= 100;
            }
            Player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.GlobalItem), ItemID.CopperCoin, coinAmount);
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            if (item.TryGetGlobalItem(out ItemPet fortune) && fortune.pickedUpBefore == false && player.CanPullItem(item, player.ItemSpace(item)))
            {
                if (fortune.globalDrop)
                {
                    for (int i = 0; i < GlobalPet.Randomizer(PickerPet.globalFortune * item.stack); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.GlobalItem), item.type, 1);
                    }
                }

                if (fortune.harvestingDrop)
                {
                    for (int i = 0; i < GlobalPet.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.harvestingFortune * 10) * item.stack, 1000); i++) //Multiplied by 10 and divided by 1000 since we divide globalFortune by 2, to get more precise numbers.
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.HarvestingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.miningDrop)
                {
                    for (int i = 0; i < GlobalPet.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.miningFortune * 10) * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.MiningFortuneItem), item.type, 1);
                    }
                }

                if (fortune.fishingDrop)
                {
                    for (int i = 0; i < GlobalPet.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.fishingFortune) * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.FishingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.herbBoost)
                {
                    for (int i = 0; i < GlobalPet.Randomizer((PickerPet.globalFortune + PickerPet.harvestingFortune) * 10 / 2 * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.HarvestingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.oreBoost)
                {
                    for (int i = 0; i < GlobalPet.Randomizer((PickerPet.globalFortune + PickerPet.miningFortune) * 10 / 2 * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.MiningFortuneItem), item.type, 1);
                    }
                }
                // Fish is below at ModifyCaughtFish()
            }
        }
        /// <summary>
        /// Checks if the given Pet Item is in use and checks if pet has been lately swapped or not.
        /// </summary>
        public bool PetInUseWithSwapCd(int petItemID)
        {
            return Player.miscEquips[0].type == petItemID && Player.HasBuff(ModContent.BuffType<ObliviousPet>()) == false;
        }
        /// <summary>
        /// Checks if the given Pet Item is in use without being affected by swapping cooldown.
        /// </summary>
        public bool PetInUse(int petItemID)
        {
            return Player.miscEquips[0].type == petItemID;
        }
        public static bool LifestealCheck(NPC npc)
        {
            return !npc.friendly && !npc.SpawnedFromStatue && npc.type != NPCID.TargetDummy && npc.canGhostHeal;
        }
        public bool AbilityPressCheck()
        {
            return Player.dead == false && timer <= 0 && PetKeybinds.UsePetAbility.JustPressed;
        }
        /// <summary>
        /// Randomizes the given number. numToBeRandomized / randomizeTo returns how many times its 100% chance and rolls if the leftover, non-100% amount is true. Randomizer(250) returns +2 and +1 more with 50% chance.
        /// randomizeTo is converted to positive if its negative for proper usage of Method.
        /// </summary>
        public static int Randomizer(int numToBeRandomized, int randomizeTo = 100)
        {
            if (randomizeTo < 0)
                randomizeTo *= -1;

            int a = 0;
            a = numToBeRandomized / randomizeTo;
            numToBeRandomized %= randomizeTo;

            if (numToBeRandomized < 0 && Main.rand.NextBool(numToBeRandomized * -1, randomizeTo))
            {
                a--;
            }
            else if (Main.rand.NextBool(numToBeRandomized, randomizeTo))
            {
                a++;
            }
            return a;
        }
        /// <summary>
        /// Sets active of oldest Main.combatText to false.
        /// </summary>
        /// <returns>Index of removed combatText.</returns>
        public static int RemoveOldestCombatText()
        {
            int textLife = 6000;
            int textToRemove = 100;
            for (int i = 0; i < Main.maxCombatText; i++)
            {
                if (Main.combatText[i].lifeTime < textLife)
                {
                    textLife = Main.combatText[i].lifeTime;
                    textToRemove = i;
                }
            }
            Main.combatText[textToRemove].active = false;
            return textToRemove;
        }
        /// <summary>
        /// Adds to petShield list and applies petShieldMultiplier. Does not allow for values lower than 1 to be added.
        /// </summary>
        /// <param name="shieldAmount">Pet Shield to be added to Player.</param>
        /// <param name="shieldDuration">Duration of this individual Shield on the Player.</param>
        /// <param name="applyPetShieldMult">Set to false to prevent from petShieldMultiplier from being applied..</param>
        /// <returns>Value of the added shield, -1 if failed to add.</returns>
        public int AddShield(int shieldAmount, int shieldDuration, bool applyPetShieldMult = true)
        {
            int shield = shieldAmount;
            if (applyPetShieldMult)
            {
                shield = (int)(shield * petShieldMultiplier);
            }
            if (shield > 0 && shieldDuration > 0)
            {
                petShield.Add((shield, shieldDuration));
                return shield;
            }
            return -1;
        }
        /// <summary>
        /// Used for Healing and Mana recovery purposes. Non converted amount can still grant +1, depending on a roll. Example: PetRecovery(215, 0.05f) will heal you for 10 health and 75% chance to heal +1 more, resulting in 11 health recovery.
        /// </summary>
        /// <param name="baseAmount">Base amount of value to be recovered</param>
        /// <param name="percentageAmount">% of baseAmount to be converted to recovery.</param>
        /// <param name="flatIncrease">Amount to be included that will not go through any calculations & complications.</param>
        /// <param name="manaSteal">Whether or not if it will recover health or mana. petHealMultiplier and Moon Leech debuff will be disabled if set to True.</param>
        /// <param name="isLifesteal">Should be set to false if this is not a Life Steal, it won't use vanilla Life steal cap, won't be affected by Moon Leech debuff and won't modify player.lifeSteal if set to false.</param>
        /// <param name="doHeal">Should be set to false if intended to simply return a value but not do anything at all.</param>
        /// <returns>Returns amount calculated, irrelevant to Player's health cap, or the lifeSteal cap etc.</returns>
        public int PetRecovery(int baseAmount, float percentageAmount, int flatIncrease = 0, bool manaSteal = false, bool isLifesteal = true, bool doHeal = true)
        {
            float num = baseAmount * (manaSteal ? percentageAmount : (petHealMultiplier * ((isLifesteal && Player.HasBuff(BuffID.MoonLeech)) ? (percentageAmount * 0.33f) : percentageAmount)));
            int calculatedAmount = (int)num;
            if (Main.rand.NextFloat(0, 1) < num % 1)
            {
                calculatedAmount++;
            }

            calculatedAmount += flatIncrease;
            num = calculatedAmount;
            if (doHeal == true && calculatedAmount > 0)
            {
                if (manaSteal == false)
                {
                    Player.HealEffect(calculatedAmount);
                    if (calculatedAmount > Player.statLifeMax2 - Player.statLife)
                    {
                        calculatedAmount = Player.statLifeMax2 - Player.statLife;
                    }

                    if (isLifesteal == true)
                    {
                        if (calculatedAmount > Player.lifeSteal)
                        {
                            calculatedAmount = (int)Player.lifeSteal;
                        }

                        if (Player.lifeSteal > 0)
                        {
                            Player.statLife += calculatedAmount;
                            Player.lifeSteal -= calculatedAmount;
                        }
                    }
                    else
                    {
                        Player.statLife += calculatedAmount;
                    }
                }
                else
                {
                    Player.ManaEffect(calculatedAmount);
                    if (calculatedAmount > Player.statManaMax2 - Player.statMana)
                    {
                        calculatedAmount = Player.statManaMax2 - Player.statMana;
                    }

                    Player.statMana += calculatedAmount;
                }
            }

            return (int)num;
        }
        public static bool KingSlimePetActive(out Player owner)
        {
            bool anyPlayerHasSlimePet = false;
            owner = null;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player plr = Main.player[i];
                if (plr.active && plr.whoAmI != 255 && plr.miscEquips[0].type == ItemID.KingSlimePetItem)
                {
                    anyPlayerHasSlimePet = true;
                    owner = plr;
                    break;
                }
            }
            return anyPlayerHasSlimePet == true;
        }
        public static bool QueenSlimePetActive(out Player owner)
        {
            bool anyPlayerHasSlimePet = false;
            owner = null;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player plr = Main.player[i];
                if (plr.active && plr.whoAmI != 255 && plr.miscEquips[0].type == ItemID.QueenSlimePetItem)
                {
                    anyPlayerHasSlimePet = true;
                    owner = plr;
                    break;
                }
            }
            return anyPlayerHasSlimePet == true;
        }
        public static bool DualSlimePetActive(out Player owner)
        {
            bool anyPlayerHasSlimePet = false;
            owner = null;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player plr = Main.player[i];
                if (plr.active && plr.whoAmI != 255 && plr.miscEquips[0].type == ItemID.ResplendentDessert)
                {
                    anyPlayerHasSlimePet = true;
                    owner = plr;
                    break;
                }
            }
            return anyPlayerHasSlimePet == true;
        }
        public static void HandleShieldBlockMessage(BinaryReader reader, int whoAmI, int damageAmount)
        {
            int player = reader.ReadByte();
            if (Main.netMode == NetmodeID.Server)
            {
                player = whoAmI;
            }

            Main.player[player].GetModPlayer<GlobalPet>().ShieldFullBlockEffect(damageAmount);

            if (Main.netMode == NetmodeID.Server)
            {
                SendShieldBlockToServer(player, damageAmount);
            }
        }
        public static void SendShieldBlockToServer(int whoAmI, int dmgAmount)
        {
            ModPacket packet = ModContent.GetInstance<PetsOverhaul>().GetPacket();
            packet.Write((byte)MessageType.ShieldFullAbsorb);
            packet.Write(dmgAmount);
            packet.Write((byte)whoAmI);
            packet.Send(ignoreClient: whoAmI);
        }
        public void ShieldFullBlockEffect(int damage)
        {
            CombatText.NewText(Player.Hitbox, Color.Cyan, -damage, true);
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath43 with { PitchVariance = 0.4f, Pitch = -0.8f, Volume = 0.2f }, Player.Center);
            }
            if (damage <= 1)
            {
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 40 : 20);
            }
            else
            {
                Player.SetImmuneTimeForAllTypes(Player.longInvince ? 80 : 40);
            }
            if (Player.whoAmI == Main.myPlayer)
            {
                shieldToBeReduced += damage;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    SendShieldBlockToServer(Player.whoAmI, damage);
                }
            }
        }
        #endregion

        //Overrides
        #region 
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
            On_Player.DoBootsEffect_PlaceFlowersOnTile += On_Player_DoBootsEffect_PlaceFlowersOnTile;
        }

        public static bool On_Player_DoBootsEffect_PlaceFlowersOnTile(On_Player.orig_DoBootsEffect_PlaceFlowersOnTile orig, Player self, int X, int Y)
        {
            bool PlacedFlower = orig(self, X, Y);
            if (PlacedFlower)
            {
                TilePlacement.AddToList(X, Y);
            }
            return PlacedFlower;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("SkinColor", skin);
            tag.Add("SkinColorChanged", skinColorChanged);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("SkinColor", out Color skinColor))
            {
                skin = skinColor;
            }

            if (tag.TryGet("SkinColorChanged", out bool skinChanged))
            {
                skinColorChanged = skinChanged;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ModContent.GetInstance<PetPersonalization>().DifficultAmount != 0)
            {
                modifiers.FinalDamage *= 1f - ModContent.GetInstance<PetPersonalization>().DifficultAmount * 0.01f;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (ModContent.GetInstance<PetPersonalization>().DifficultAmount != 0)
            {
                modifiers.FinalDamage *= 1f + ModContent.GetInstance<PetPersonalization>().DifficultAmount * 0.01f;
            }
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
            {
                if (info.Damage > currentShield && currentShield > 0)
                {
                    CombatText.NewText(Player.Hitbox, Color.Cyan, -currentShield, true);
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath43 with { PitchVariance = 0.4f, Pitch = -0.8f, Volume = 0.2f }, Player.Center);
                    }

                    info.Damage -= currentShield;
                    shieldToBeReduced += currentShield;
                }
            };
            if (ModContent.GetInstance<PetPersonalization>().HurtSoundEnabled && Player.GetModPlayer<PetSounds>().PetItemIdToHurtSound.ContainsKey(Player.miscEquips[0].type))
            {
                modifiers.DisableSound();
            }

        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (ModContent.GetInstance<PetPersonalization>().HurtSoundEnabled)
            {
                Player.GetModPlayer<PetSounds>().PlayHurtSoundFromItemId(Player.miscEquips[0].type);
            }
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if (currentShield > 0 && info.Damage <= currentShield - shieldToBeReduced)
            {
                info.SoundDisabled = true;
                ShieldFullBlockEffect(info.Damage);
                return true;
            }
            return base.ConsumableDodge(info);
        }
        public override void ResetEffects()
        {
            petSwapCooldown = 600;

            fishingFortune = 0;
            harvestingFortune = 0;
            miningFortune = 0;
            globalFortune = 0;

            abilityHaste = 0;
            petHealMultiplier = 1f;
            petShieldMultiplier = 1f;
        }
        public override void PreUpdate()
        {
            if (Main.mouseItem.TryGetGlobalItem(out ItemPet item) && item.pickedUpBefore == false) //Player's hand slot is not being reckognized as 'inventory' in UpdateInventory() of GlobalItem, so manually updating the Hand slot
            {
                item.pickedUpBefore = true;
            }

            if (ItemPet.updateReplacedTile.Count > 0)
            {
                ItemPet.updateReplacedTile.Clear();
            }

            if (ColorVal >= 1f)
            {
                colorSwitched = true;
            }
            else if (ColorVal <= 0f)
            {
                colorSwitched = false;
            }

            ColorVal += colorSwitched ? -0.01f : 0.01f;

            if (Player.jump == 0)
            {
                jumpRegistered = false;
            }
            timer--;
            if (timer < -1)
            {
                timer = -1;
            }

            if (timer == 0)
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled && (ModContent.GetInstance<PetPersonalization>().LowCooldownSoundEnabled == false && timerMax > ModContent.GetInstance<PetPersonalization>().LowCooldownTreshold || ModContent.GetInstance<PetPersonalization>().LowCooldownSoundEnabled))
                {
                    SoundEngine.PlaySound(SoundID.MaxMana with { PitchVariance = 0.3f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.Center);
                }
            }
        }
        public override void PostUpdate()
        {
            if (petShield.Count > 0)
            {
                while (shieldToBeReduced > 0 && petShield.Count > 0)
                {
                    (int shieldAmount, int shieldTimer) value = petShield.Find(x => x.shieldTimer == petShield.Min(x => x.shieldTimer));
                    int index = petShield.IndexOf(value);
                    if (index != -1 && value.shieldAmount <= shieldToBeReduced)
                    {
                        shieldToBeReduced -= value.shieldAmount;
                        petShield.RemoveAt(index);
                    }
                    else if (index != -1 && value.shieldAmount > shieldToBeReduced)
                    {
                        value.shieldAmount -= shieldToBeReduced;
                        shieldToBeReduced = 0;
                        petShield[index] = value;
                    }
                }

                shieldToBeReduced = 0;
                currentShield = 0;

                petShield.RemoveAll(x => x.shieldTimer < 1 || x.shieldAmount <= 0);
                petShield.ForEach(x => currentShield += x.shieldAmount);

                for (int i = 0; i < petShield.Count; i++)
                {
                    (int shieldAmount, int shieldTimer) shieldValue = petShield[i];
                    shieldValue.shieldTimer--;
                    petShield[i] = shieldValue;
                }
            }

            if (ItemPool.Count > 0)
            {
                ItemPool.Clear();
            }
        }
        public override void OnEnterWorld()
        {
            previousPetItem = Player.miscEquips[0].type;
            if (ModContent.GetInstance<PetPersonalization>().EnableNotice)
            {
                Main.NewText(Language.GetTextValue("Mods.PetsOverhaul.Notice"));
            }
            if (ModContent.GetInstance<PetPersonalization>().EnableModNotice)
            {
                if (ModLoader.TryGetMod("PetsOverhaulCalamityAddon", out _) == false && ModLoader.TryGetMod("CalamityMod", out _) == true)
                    Main.NewText(Language.GetTextValue("Mods.PetsOverhaul.CalamityDetected"));
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (ModContent.GetInstance<PetPersonalization>().DeathSoundEnabled)
            {
                playSound = Player.GetModPlayer<PetSounds>().PlayKillSoundFromItemId(Player.miscEquips[0].type) == ReLogic.Utilities.SlotId.Invalid;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }
        public override void UpdateEquips()
        {
            if (Player.miscEquips[0].type == ItemID.None)
            {
                timerMax = 0;
                return;
            }

            if (previousPetItem != Player.miscEquips[0].type)
            {
                timerMax = 0;
                if (ModContent.GetInstance<PetPersonalization>().SwapCooldown)
                {
                    Player.AddBuff(ModContent.BuffType<ObliviousPet>(), petSwapCooldown);
                }

                previousPetItem = Player.miscEquips[0].type;
            }
            if (ModContent.GetInstance<PetPersonalization>().PassiveSoundEnabled && Main.rand.NextBool(3600))
            {
                Player.GetModPlayer<PetSounds>().PlayAmbientSoundFromItemId(Player.miscEquips[0].type);
            }
        }
        public override void UpdateDead()
        {
            timer = -1;
            petShield.Clear();
        }
        public override void ModifyCaughtFish(Item fish)
        {
            for (int i = 0; i < GlobalPet.Randomizer((globalFortune + fishingFortune) * 10 / 2 * fish.stack, 1000); i++)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySourcePetIDs.FishingFortuneItem), fish.type, 1);
            }
        }
        #endregion
    }
}
