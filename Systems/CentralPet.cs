using Microsoft.Xna.Framework;
using PetsOverhaul.Buffs;
using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.PetEffects.Vanilla;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using System;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// ModPlayer class that contains useful Methods and fields for Pet implementation, works as a central class.
    /// </summary>
    public sealed class GlobalPet : ModPlayer
    {
        public static InputMode PlayerInputMode => PlayerInput.CurrentProfile.InputModes.ContainsKey(InputMode.Keyboard) ? InputMode.Keyboard : InputMode.XBoxGamepad;
        public bool jojaColaCaught = false;
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
        /// Influences the multiplier of the exp you gain for your Junimo Pet's Harvesting Skill.
        /// </summary>
        public float harvestingExpBoost = 1f;
        /// <summary>
        /// Influences the multiplier of the exp you gain for your Junimo Pet's Mining Skill.
        /// </summary>
        public float miningExpBoost = 1f;
        /// <summary>
        /// Influences the multiplier of the exp you gain for your Junimo Pet's Fishing Skill.
        /// </summary>
        public float fishingExpBoost = 1f;

        public bool[] burnDebuffs = BuffID.Sets.Factory.CreateBoolSet(false, BuffID.Burning, BuffID.OnFire, BuffID.OnFire3, BuffID.Frostburn, BuffID.CursedInferno, BuffID.ShadowFlame, BuffID.Frostburn2);
        public Color skin;
        public bool skinColorChanged = false;
        /// <summary>
        /// Is cleared at the end of PostUpdate hook. Main use is for Weighted Lists with usage of ItemWeight() Method. Retrieve a value using Main.rand.Next() for its index for Weighted list usage.
        /// </summary>
        public static List<int> pool = new();
        public List<(int shieldAmount, int shieldTimer)> petShield = new();
        public int currentShield = 0;
        public int shieldToBeReduced = 0;
        public bool jumpRegistered = false;
        public int petSwapCooldown = 600;
        internal int previousPetItem = 0;
        /// <summary>
        /// This field ticks down every frame in PreUpdate() hook. Does not go below -1. Plays 'cooldown refreshment' sound effect upon reaching 0 amd displays Timer while higher than 0. Usually is recommended to use Mod's timer mechanic for timers that the Player should be aware of.
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
        public static float colorVal = 0;

        /// <summary>
        /// Used to change alternating color of maximum Light Pet Rolls alongside colorVal
        /// </summary>
        private static bool colorSwitched = false;
        public static readonly Color lowQuality = new(130, 130, 130);
        public static readonly Color midQuality = new(77, 117, 154);
        public static readonly Color highQuality = new(252, 194, 0);
        /// <summary>
        /// Alternates between (165, 249, 255) and (255, 207, 249) every frame.
        /// </summary>
        public static Color maxQuality = new(165, 249, 255);
        /// <summary>
        /// Converts given text to be corresponding color of Light Pet quality values
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <param name="currentRoll">Current roll of the stat</param>
        /// <param name="maxRoll">Maximum roll of the stat</param>
        /// <returns>Text with its color changed depending on quality amount</returns>
        public static string LightPetRarityColorConvert(string text, int currentRoll, int maxRoll)
        {
            if (currentRoll == maxRoll)
            {
                return $"[c/{maxQuality.Hex3()}:{text}]";
            }
            else if (currentRoll > maxRoll * 0.66f)
            {
                return $"[c/{highQuality.Hex3()}:{text}]";
            }
            else if (currentRoll > maxRoll * 0.33f)
            {
                return $"[c/{midQuality.Hex3()}:{text}]";
            }
            else
            {
                return $"[c/{lowQuality.Hex3()}:{text}]";
            }
        }

        public static IEntitySource GetSource_Pet(EntitySource_Pet.TypeId typeId, string context = null)
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
                pool.Add(itemId);
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
        /// Spawns coins accordingly to the given value and converts it to a higher coin tier if possible. Source of spawned coins will be globalItem. Recommended use is with ItemPet.Randomizer() to achieve more precise and 'natural' values. (100x the intended coin value)
        /// </summary>
        public void GiveCoins(int coinAmount)
        {
            if (coinAmount > 1000000)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.PlatinumCoin, coinAmount / 1000000);
                coinAmount %= 1000000;
            }
            if (coinAmount > 10000)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.GoldCoin, coinAmount / 10000);
                coinAmount %= 10000;
            }
            if (coinAmount > 100)
            {
                Player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.SilverCoin, coinAmount / 100);
                coinAmount %= 100;
            }
            Player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.CopperCoin, coinAmount);
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            if (item.TryGetGlobalItem(out ItemPet fortune) && fortune.pickedUpBefore == false && player.CanPullItem(item, player.ItemSpace(item)))
            {
                if (fortune.globalDrop)
                {
                    for (int i = 0; i < ItemPet.Randomizer(PickerPet.globalFortune * item.stack); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.globalItem), item.type, 1);
                    }
                }

                if (fortune.harvestingDrop)
                {
                    for (int i = 0; i < ItemPet.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.harvestingFortune * 10) * item.stack, 1000); i++) //Multiplied by 10 and divided by 1000 since we divide globalFortune by 2, to get more precise numbers.
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.harvestingFortuneItem), item.type, 1);
                    }
                }

                if (fortune.miningDrop)
                {
                    for (int i = 0; i < ItemPet.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.miningFortune * 10) * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.miningFortuneItem), item.type, 1);
                    }
                }

                if (fortune.fishingDrop)
                {
                    for (int i = 0; i < ItemPet.Randomizer((PickerPet.globalFortune * 10 / 2 + PickerPet.fishingFortune) * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GetSource_Pet(EntitySource_Pet.TypeId.fishingFortuneItem), item.type, 1);
                    }
                }
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
        public override void SaveData(TagCompound tag)
        {
            tag.Add("SkinColor", skin);
            tag.Add("SkinColorChanged", skinColorChanged);
            tag.Add("JojaColaCaughtBefore", jojaColaCaught);
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

            if (tag.TryGet("jojaColaCaught", out bool jojaCaught))
            {
                jojaColaCaught = jojaCaught;
            }
        }
        public static bool LifestealCheck(NPC npc)
        {
            return !npc.friendly && !npc.SpawnedFromStatue && npc.type != NPCID.TargetDummy;
        }
        /// <summary>
        /// percentageAmount% of baseAmount is converted to healing, non converted amount can still grant +1, depending on a roll. If manaSteal is set to true, 
        /// everything same will be done for Mana instead of Health. Example: Lifesteal(215, 0.05f) will heal you for 10 health and 75% chance to heal 11 health 
        /// instead. It returns its actual amount before MaxHealth/LifeSteal limitations, meaning you can combine it with another Lifesteal method to set the flatIncrease to something else. Lihzahrd uses 
        /// this with its %heallth healing alongside its regular Lifesteal. flatIncrease does not go into any calculations. Will show its actual value as recovered, 
        /// however, it will not increase the lifeSteal field or health/mana itself more than Maximum Health/mana cap or the lifeSteal field itself if respectLifeStealCap is true.
        /// Does not actually lifesteal if doLifesteal is set to false, so it can safely return an integer value.
        /// </summary>
        public int Lifesteal(int baseAmount, float percentageAmount, int flatIncrease = 0, bool manaSteal = false, bool respectLifeStealCap = true, bool doLifesteal = true)
        {
            float num = baseAmount * (Player.HasBuff(BuffID.MoonLeech) ? percentageAmount * 0.33f : percentageAmount);
            int calculatedAmount = (int)num;
            if (Main.rand.NextFloat(0, 1) < num % 1)
            {
                calculatedAmount++;
            }

            calculatedAmount += flatIncrease;
            num = calculatedAmount;
            if (calculatedAmount > 0 && doLifesteal == true)
            {
                if (manaSteal == false)
                {
                    Player.HealEffect(calculatedAmount);
                    if (calculatedAmount > Player.statLifeMax2 - Player.statLife)
                    {
                        calculatedAmount = Player.statLifeMax2 - Player.statLife;
                    }

                    if (respectLifeStealCap == true)
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (ModContent.GetInstance<Personalization>().DifficultAmount != 0)
            {
                modifiers.FinalDamage *= 1f - ModContent.GetInstance<Personalization>().DifficultAmount * 0.01f;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (ModContent.GetInstance<Personalization>().DifficultAmount != 0)
            {
                modifiers.FinalDamage *= 1f + ModContent.GetInstance<Personalization>().DifficultAmount * 0.01f;
            }

            modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
            {
                if (info.Damage > currentShield && currentShield > 0)
                {
                    CombatText.NewText(Player.Hitbox, Color.Cyan, -currentShield, true);
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath43 with { PitchVariance = 0.4f, Pitch = -0.8f, Volume = 0.2f }, Player.position);
                    }

                    info.Damage -= currentShield;
                    shieldToBeReduced += currentShield;
                }
            };
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
            packet.Write((byte)PetsOverhaul.MessageType.shieldFullAbsorb);
            packet.Write(dmgAmount);
            packet.Write((byte)whoAmI);
            packet.Send(ignoreClient: whoAmI);
        }
        public void ShieldFullBlockEffect(int damage)
        {
            CombatText.NewText(Player.Hitbox, Color.Cyan, -damage, true);
            if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath43 with { PitchVariance = 0.4f, Pitch = -0.8f, Volume = 0.2f }, Player.position);
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

            if (colorVal >= 1f)
            {
                colorSwitched = true;
            }
            else if (colorVal <= 0f)
            {
                colorSwitched = false;
            }

            colorVal += colorSwitched ? -0.01f : 0.01f;
            maxQuality = Color.Lerp(new Color(165, 249, 255), new Color(255, 207, 249), colorVal);

            fishingFortune = 0;
            harvestingFortune = 0;
            miningFortune = 0;
            globalFortune = 0;
            fishingExpBoost = 1f;
            harvestingExpBoost = 1f;
            miningExpBoost = 1f;

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
                if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false && (ModContent.GetInstance<Personalization>().LowCooldownSoundDisabled && timerMax > 90 || ModContent.GetInstance<Personalization>().LowCooldownSoundDisabled == false))
                {
                    SoundEngine.PlaySound(SoundID.MaxMana with { PitchVariance = 0.3f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Player.position);
                }
            }
            if (abilityHaste < -0.9f)
            {
                abilityHaste = -0.9f;
            }

            timerMax = (int)(timerMax * (1 / (1 + abilityHaste)));
            petSwapCooldown = 600;
            abilityHaste = 0;
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

            if (pool.Count > 0)
            {
                pool.Clear();
            }
        }
        public override void OnEnterWorld()
        {
            previousPetItem = Player.miscEquips[0].type;
            if (ModContent.GetInstance<Personalization>().DisableNotice == false)
            {
                Main.NewText(Language.GetTextValue("Mods.PetsOverhaul.Notice"));
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {

            if (ModContent.GetInstance<Personalization>().HurtSoundDisabled == false)
            {
                info.SoundDisabled = Player.GetModPlayer<PetRegistry>().PlayHurtSoundFromItemId(Player.miscEquips[0].type) != ReLogic.Utilities.SlotId.Invalid;
            }
        }
        public override void UpdateEquips()
        {
            if (Player.miscEquips[0].type == ItemID.None)
            {
                return;
            }

            if (previousPetItem != Player.miscEquips[0].type)
            {
                if (ModContent.GetInstance<Personalization>().SwapCooldown == false)
                {
                    Player.AddBuff(ModContent.BuffType<ObliviousPet>(), petSwapCooldown);
                }

                previousPetItem = Player.miscEquips[0].type;
            }
            if (ModContent.GetInstance<Personalization>().PassiveSoundDisabled == false && Main.rand.NextBool(3600))
            {
                Player.GetModPlayer<PetRegistry>().PlayEquipSoundFromItemId(Player.miscEquips[0].type);
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            timer = -1;
            petShield.Clear();
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (ModContent.GetInstance<Personalization>().DeathSoundDisabled == false)
            {
                playSound = Player.GetModPlayer<PetRegistry>().PlayKillSoundFromItemId(Player.miscEquips[0].type) == ReLogic.Utilities.SlotId.Invalid;
            }

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (ModContent.GetInstance<Personalization>().JojaColaEasyOff == false && jojaColaCaught == false && Main.rand.NextBool(5))
            {
                itemDrop = ItemID.JojaCola;
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (fish.type == ItemID.JojaCola)
            {
                jojaColaCaught = true;
            }
        }
    }
    /// <summary>
    /// GlobalItem class that contains many useful booleans and methods for mainly gathering and item randomizing purposes
    /// </summary>
    public sealed class ItemPet : GlobalItem
    {
        public override bool InstancePerEntity => true;
        /// <summary>
        /// Includes tiles that are considered 'soil' tiles, except Dirt. Used by Dirtiest Block.
        /// </summary>
        public static bool[] commonTiles = TileID.Sets.Factory.CreateBoolSet(false, TileID.Mud, TileID.SnowBlock, TileID.Ash, TileID.ClayBlock, TileID.Marble, TileID.Granite, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone, TileID.Sand, TileID.Ebonsand, TileID.Crimsand, TileID.Pearlsand, TileID.CorruptSandstone, TileID.Sandstone, TileID.CrimsonSandstone, TileID.HallowSandstone, TileID.HardenedSand, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand, TileID.HallowHardenedSand, TileID.IceBlock, TileID.CorruptIce, TileID.FleshIce, TileID.HallowedIce, TileID.Stone, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone);
        /// <summary>
        /// Includes Gem tiles.
        /// </summary>
        public static bool[] gemTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.AmberStoneBlock, TileID.Diamond, TileID.ExposedGems, TileID.Crystals);
        /// <summary>
        /// Includes tiles that are extractable by an Extractinator and a few other stuff that aren't recognized as ores such as Obsidian and Luminite
        /// </summary>
        public static bool[] extractableAndOthers = TileID.Sets.Factory.CreateBoolSet(false, TileID.DesertFossil, TileID.Slush, TileID.Silt, TileID.Obsidian, TileID.LunarOre);
        /// <summary>
        /// Includes tiles that counts as trees.
        /// </summary>
        public static bool[] treeTile = TileID.Sets.Factory.CreateBoolSet(false, TileID.TreeAmber, TileID.TreeAmethyst, TileID.TreeAsh, TileID.TreeDiamond, TileID.TreeEmerald, TileID.TreeRuby, TileID.Trees, TileID.TreeSapphire, TileID.TreeTopaz, TileID.MushroomTrees, TileID.PalmTree, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow, TileID.Bamboo, TileID.Cactus);
        /// <summary>
        /// Contains items dropped by gemstone trees. Current only use is Caveling Gardener.
        /// </summary>
        public static bool[] gemstoneTreeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.GemTreeAmberSeed, ItemID.GemTreeAmethystSeed, ItemID.GemTreeDiamondSeed, ItemID.GemTreeEmeraldSeed, ItemID.GemTreeRubySeed, ItemID.GemTreeSapphireSeed, ItemID.GemTreeTopazSeed, ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Amber, ItemID.Diamond, ItemID.StoneBlock);
        /// <summary>
        /// Contains items dropped by trees. Current only use is Blue Chicken.
        /// </summary>
        public static bool[] treeItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Acorn, ItemID.BambooBlock, ItemID.Cactus, ItemID.Wood, ItemID.AshWood, ItemID.BorealWood, ItemID.PalmWood, ItemID.Ebonwood, ItemID.Shadewood, ItemID.RichMahogany, ItemID.Pearlwood, ItemID.SpookyWood);
        /// <summary>
        /// Contains items dropped by trees. Current only use is Blue Chicken.
        /// </summary>
        public static bool[] seaPlantItem = ItemID.Sets.Factory.CreateBoolSet(false, ItemID.Coral, ItemID.Seashell, ItemID.Starfish, ItemID.LightningWhelkShell, ItemID.TulipShell, ItemID.JunoniaShell);
        /// <summary>
        /// Adds coordinates in this to PlayerPlacedBlockList if a tile has been replaced and item is obtained through that way. In GlobalTile, Add to this in the CanReplace() hook.
        /// </summary>
        public static List<Point16> updateReplacedTile = new();

        //Checks to determine which Pet should benefit
        public bool itemFromNpc = false;
        public bool herbBoost = false;
        public bool oreBoost = false;
        public bool commonBlock = false;
        public bool blockNotByPlayer = false;
        public bool pickedUpBefore = false;
        public bool itemFromBag = false;
        public bool itemFromBoss = false;
        public bool harvestingDrop = false;
        public bool miningDrop = false;
        public bool fishingDrop = false;
        public bool globalDrop = false;
        public bool fortuneHarvestingDrop = false;
        public bool fortuneMiningDrop = false;
        public bool fortuneFishingDrop = false;

        /// <summary>
        /// 1000 is 10 exp.
        /// </summary>
        public const int MinimumExpForRarePlant = 1000;

        /// <summary>
        /// Randomizes the given number. numToBeRandomized / randomizeTo returns how many times its 100% chance and rolls if the leftover, non-100% amount is true. Randomizer(250) returns +2 and +1 more with 50% chance.
        /// </summary>
        public static int Randomizer(int numToBeRandomized, int randomizeTo = 100)
        {
            int a = 0;
            if (numToBeRandomized >= randomizeTo)
            {
                a = numToBeRandomized / randomizeTo;
                numToBeRandomized %= randomizeTo;
            }
            if (Main.rand.NextBool(numToBeRandomized, randomizeTo))
            {
                a++;
            }

            return a;

        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (pickedUpBefore == false)
            {
                pickedUpBefore = true;
            }
        }
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (source is EntitySource_Pet petSource)
            {
                globalDrop = petSource.ContextType == EntitySource_Pet.TypeId.globalItem;

                harvestingDrop = petSource.ContextType == EntitySource_Pet.TypeId.harvestingItem;

                miningDrop = petSource.ContextType == EntitySource_Pet.TypeId.miningItem;

                fishingDrop = petSource.ContextType == EntitySource_Pet.TypeId.fishingItem;

                fortuneHarvestingDrop = petSource.ContextType == EntitySource_Pet.TypeId.harvestingFortuneItem;

                fortuneMiningDrop = petSource.ContextType == EntitySource_Pet.TypeId.miningFortuneItem;

                fortuneFishingDrop = petSource.ContextType == EntitySource_Pet.TypeId.fishingFortuneItem;
            }
            else if (source is EntitySource_TileBreak || source is EntitySource_ShakeTree)
            {
                herbBoost = Junimo.HarvestingXpPerGathered.Exists(x => x.plantList.Contains(item.type));

                if (source is EntitySource_TileBreak brokenTile)
                {
                    ushort tileType = Main.tile[brokenTile.TileCoords].TileType;

                    if (PlayerPlacedBlockList.placedBlocksByPlayer.Remove(new Point16(brokenTile.TileCoords)) == false)
                    {
                        oreBoost = TileID.Sets.Ore[tileType] || gemTile[tileType] || extractableAndOthers[tileType] || item.type == ItemID.LifeCrystal;
                        commonBlock = TileID.Sets.Conversion.Moss[tileType] || commonTiles[tileType];
                        blockNotByPlayer = true;
                    }

                    if (TileID.Sets.CountsAsGemTree[tileType] == false && gemstoneTreeItem[item.type] || treeTile[tileType] == false && treeItem[item.type] || blockNotByPlayer == false && seaPlantItem[item.type])
                    {
                        herbBoost = false;
                    }

                    if (updateReplacedTile.Count > 0)
                    {
                        PlayerPlacedBlockList.placedBlocksByPlayer.AddRange(updateReplacedTile);
                    }
                }
            }
            else if (source is EntitySource_Loot lootSource && lootSource.Entity is NPC npc)
            {
                if (npc.boss == true || NpcPet.nonBossTrueBosses[npc.type])
                {
                    itemFromBoss = true;
                }
                else
                {
                    itemFromNpc = true;
                }
            }
            else if (source is EntitySource_ItemOpen)
            {
                itemFromBag = true;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            BitsByte sources1 = new(blockNotByPlayer, pickedUpBefore, itemFromNpc, itemFromBoss, itemFromBag, herbBoost, oreBoost, commonBlock);
            BitsByte sources2 = new(globalDrop, harvestingDrop, miningDrop, fishingDrop, fortuneHarvestingDrop, fortuneMiningDrop, fortuneFishingDrop);
            writer.Write(sources1);
            writer.Write(sources2);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            BitsByte sources1 = reader.ReadByte();
            sources1.Retrieve(ref blockNotByPlayer, ref pickedUpBefore, ref itemFromNpc, ref itemFromBoss, ref itemFromBag, ref herbBoost, ref oreBoost, ref commonBlock);
            BitsByte sources2 = reader.ReadByte();
            sources2.Retrieve(ref globalDrop, ref harvestingDrop, ref miningDrop, ref fishingDrop, ref fortuneHarvestingDrop, ref fortuneMiningDrop, ref fortuneFishingDrop);
        }
    }
    /// <summary>
    /// GlobalNPC class that contains useful booleans and methods such as Slow() and seaCreature
    /// </summary>
    public sealed class NpcPet : GlobalNPC
    {
        public enum SlowId
        {
            Grinch = 0, Snowman = 1, QueenSlime = 2, Deerclops = 3, IceQueen = 4, PikachuStatic = 5, Misc = 6
        }
        public List<(SlowId, float slowAmount, int slowTime)> SlowList = new();
        /// <summary>
        /// If you need to find out how much current cumulative slow amount is, use this.
        /// </summary>
        public float SlowAmount { get; internal set; }
        public bool seaCreature;
        public int playerThatFishedUp;
        public int maulCounter;
        public int curseCounter;
        /// <summary>
        /// Contains all Vanilla bosses that does not return npc.boss = true
        /// </summary>
        public static bool[] nonBossTrueBosses = NPCID.Sets.Factory.CreateBoolSet(false, NPCID.TheDestroyer, NPCID.TheDestroyerBody, NPCID.TheDestroyerTail, NPCID.EaterofWorldsBody, NPCID.EaterofWorldsTail, NPCID.EaterofWorldsHead, NPCID.LunarTowerSolar, NPCID.LunarTowerNebula, NPCID.LunarTowerStardust, NPCID.LunarTowerVortex, NPCID.TorchGod, NPCID.Retinazer, NPCID.Spazmatism);
        public Vector2 FlyingVelo { get; internal set; }
        public float GroundVelo { get; internal set; }
        public bool VeloChangedFlying { get; internal set; }
        public bool VeloChangedFlying2 { get; internal set; }
        public bool VeloChangedGround { get; internal set; }
        public bool VeloChangedGround2 { get; internal set; }

        public override bool InstancePerEntity => true;
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.EyeofCthulhu && MasteryShardCheck.masteryShardObtained1 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillEoC(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.WallofFlesh && MasteryShardCheck.masteryShardObtained2 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillWoF(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.Golem && MasteryShardCheck.masteryShardObtained3 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillGolem(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.SkeletronHead && MasteryShardCheck.masteryShardObtained4 == false)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new FirstKillSkeletron(), ModContent.ItemType<MasteryShard>()));
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                if (MasteryShardCheck.masteryShardObtained5 == false)
                {
                    npcLoot.Add(ItemDropRule.ByCondition(new FirstKillMoonLord(), ModContent.ItemType<MasteryShard>()));
                }

                if (Main.expertMode == false && Main.masterMode == false)
                {
                    npcLoot.Add(ItemDropRule.Common(ItemID.SuspiciousLookingTentacle));
                }
            }
        }
        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.EyeofCthulhu)
            {
                MasteryShardCheck.masteryShardObtained1 = true;
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                MasteryShardCheck.masteryShardObtained2 = true;
            }
            if (npc.type == NPCID.Golem)
            {
                MasteryShardCheck.masteryShardObtained3 = true;
            }
            if (npc.type == NPCID.SkeletronHead)
            {
                MasteryShardCheck.masteryShardObtained4 = true;
            }
            if (npc.type == NPCID.MoonLordCore)
            {
                MasteryShardCheck.masteryShardObtained5 = true;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.active)
            {
                if (VeloChangedGround == true)
                {
                    npc.velocity.X = GroundVelo;
                    VeloChangedGround2 = true;
                }
                else
                {
                    VeloChangedGround2 = false;
                }

                if (VeloChangedGround2 == false)
                {
                    GroundVelo = npc.velocity.X;
                }

                if (VeloChangedFlying == true)
                {
                    npc.velocity = FlyingVelo;
                    VeloChangedFlying2 = true;
                }
                else
                {
                    VeloChangedFlying2 = false;
                }

                if (VeloChangedFlying2 == false)
                {
                    FlyingVelo = npc.velocity;
                }
            }

            return base.PreAI(npc);
        }
        public override void PostAI(NPC npc)
        {
            if (npc.active)
            {
                SlowAmount = 0;
                if (SlowList.Count > 0)
                {
                    SlowList.ForEach(x => SlowAmount += x.slowAmount);
                    for (int i = 0; i < SlowList.Count; i++) //List'lerde struct'lar bir nevi readonly olarak çalıştığından, değeri alıp tekrar atıyoruz
                    {
                        (SlowId, float slowAmount, int slowTime) slow = SlowList[i];
                        slow.slowTime--;
                        SlowList[i] = slow;
                    }
                    int indexToRemove = SlowList.FindIndex(x => x.slowTime <= 0);
                    if (indexToRemove != -1)
                    {
                        SlowList.RemoveAt(indexToRemove);
                    }
                }
                if (SlowAmount != 0)
                {
                    Slow(npc, SlowAmount);
                }
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_FishedOut fisherman && fisherman.Fisher is Player player)
            {
                playerThatFishedUp = player.whoAmI;
                seaCreature = true;
            }
            else if (npc.type == NPCID.DukeFishron && source is EntitySource_BossSpawn fisher && fisher.Target is Player player2)
            {
                playerThatFishedUp = player2.whoAmI;
                seaCreature = true;
            }
            else
            {
                seaCreature = false;
            }
        }
        /// <summary>
        /// Slows if enemy is not a boss or a friendly npc. Also does not slow an npc's vertical speed if they are affected by gravity, but does so if they arent. Due to the formula, you may use a positive number for slowAmount freely and as much as you want, it almost will never completely stop an enemy. Negative values however, easily can get out of hand and cause unwanted effects. Due to that, a cap of -0.9f exists for negative values, which 10x's the speed.
        /// </summary>
        private void Slow(NPC npc, float slow)
        {
            if (slow < -0.9f)
            {
                slow = -0.9f;
            }

            FlyingVelo = npc.velocity;
            GroundVelo = npc.velocity.X;
            if (npc.noGravity == false)
            {
                npc.velocity.X *= 1 / (1 + slow);
                VeloChangedGround = true;
            }
            else
            {
                VeloChangedGround = false;
            }

            if (npc.noGravity)
            {
                npc.velocity *= 1 / (1 + slow);
                VeloChangedFlying = true;
            }
            else
            {
                VeloChangedFlying = false;
            }
        }
        /// <summary>
        /// Use this to add Slow to the NPC. Also checks if the NPC is a boss or a friendly npc or not.
        /// </summary>
        public void AddSlow(SlowId slowType, float slowValue, int slowTimer, NPC npc)
        {
            if (npc.active && (npc.townNPC == false || npc.isLikeATownNPC == false || npc.friendly == false) && npc.boss == false && nonBossTrueBosses[npc.type] == false)
            {
                int indexToReplace;
                if (SlowList.Exists(x => x.Item1 == slowType && x.slowAmount < slowValue))
                {
                    indexToReplace = SlowList.FindIndex(x => x.Item1 == slowType && x.slowAmount < slowValue);
                    SlowList[indexToReplace] = (slowType, slowValue, slowTimer);
                }
                else if (SlowList.Exists(x => x.Item1 == slowType && x.slowAmount == slowValue && x.slowTime < slowTimer))
                {
                    indexToReplace = SlowList.FindIndex(x => x.Item1 == slowType && x.slowAmount == slowValue && x.slowTime < slowTimer);
                    SlowList[indexToReplace] = (slowType, slowValue, slowTimer);
                }
                else if (SlowList.Exists(x => x.Item1 == slowType) == false)
                {
                    SlowList.Add((slowType, slowValue, slowTimer));
                }
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<Mauled>()))
            {
                for (int i = 0; i < maulCounter; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 3f), 75, default, Main.rand.NextFloat(0.5f, 0.8f));
                    dust.velocity *= 0.8f;
                }
            }
            if (npc.HasBuff(ModContent.BuffType<QueensDamnation>()))
            {
                if (curseCounter >= 20)
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Wraith, Main.rand.NextFloat(0f, 0.6f), Main.rand.NextFloat(-1f, 2f), 75, default, Main.rand.NextFloat(0.7f, 1f));
                    dust.velocity *= 0.2f;
                }
                else
                {
                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Pixie, Main.rand.NextFloat(0f, 0.6f), Main.rand.NextFloat(-1f, 2f), 150, default, Main.rand.NextFloat(0.5f, 0.7f));
                    dust.velocity *= 0.2f;
                }
            }
        }
    }
    public sealed class ProjectileSourceChecks : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool isPlanteraProjectile = false;
        public bool petProj = false;
        public bool isFromSentry = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            isPlanteraProjectile = false;
            petProj = false;
            isFromSentry = false;
            if (source is EntitySource_ItemUse item && (item.Item.type == ItemID.VenusMagnum || item.Item.type == ItemID.NettleBurst || item.Item.type == ItemID.LeafBlower || item.Item.type == ItemID.FlowerPow || item.Item.type == ItemID.WaspGun || item.Item.type == ItemID.Seedler || item.Item.type == ItemID.GrenadeLauncher))
            {
                isPlanteraProjectile = true;
            }
            else if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && (proj.type == ProjectileID.Pygmy || proj.type == ProjectileID.Pygmy2 || proj.type == ProjectileID.Pygmy3 || proj.type == ProjectileID.Pygmy4 || proj.type == ProjectileID.FlowerPow || proj.type == ProjectileID.SeedlerNut))
            {
                isPlanteraProjectile = true;
            }
            if (source is EntitySource_Pet { ContextType: EntitySource_Pet.TypeId.petProjectile })
            {
                petProj = true;
            }
            if (source is EntitySource_Parent parent2 && parent2.Entity is Projectile proj2 && proj2.sentry)
            {
                isFromSentry = true;
            }
        }
    }
}
