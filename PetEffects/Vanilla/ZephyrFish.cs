using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class ZephyrFish : ModPlayer
    {
        public float powerPerQuest = 0.004f;
        public float maxQuestPower = 0.4f;
        public int baseChance = 40;
        public int windChance = 120;
        public int speedMult = 20;
        public bool amplifiedFishingChance { get; internal set; }
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUse(ItemID.ZephyrFish))
            {
                if (Main.windSpeedCurrent < 0)
                {
                    Player.fishingSkill += (int)(Main.windSpeedCurrent * -speedMult);
                }

                if (Main.windSpeedCurrent > 0)
                {
                    Player.fishingSkill += (int)(Main.windSpeedCurrent * speedMult);
                }
            }
        }
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (Pet.PetInUse(ItemID.ZephyrFish))
            {
                if (Player.anglerQuestsFinished * 0.004f >= maxQuestPower)
                {
                    fishingLevel += maxQuestPower;
                }
                else
                {
                    fishingLevel += Player.anglerQuestsFinished * 0.004f;
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Pet.PetInUse(ItemID.ZephyrFish) && item.fishingPole > 0)
            {
                amplifiedFishingChance = false;
            }
            return true;
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (Pet.PetInUse(ItemID.ZephyrFish))
            {
                if (Main.windSpeedCurrent > 0.2f && (attempt.heightLevel == 0 || attempt.heightLevel == 1) && attempt.X > Player.position.X / 16)
                {
                    amplifiedFishingChance = true;
                }
                else if (Main.windSpeedCurrent < -0.2f && (attempt.heightLevel == 0 || attempt.heightLevel == 1) && attempt.X < Player.position.X / 16)
                {
                    amplifiedFishingChance = true;
                }
            }
        }

        public override void ModifyCaughtFish(Item fish)
        {
            if (Pet.PetInUse(ItemID.ZephyrFish) && fish.maxStack != 1)
            {
                if (amplifiedFishingChance)
                {
                    for (int i = 0; i < ItemPet.Randomizer(windChance * fish.stack); i++)
                    {
                        Player.QuickSpawnItem(Player.GetSource_Misc("FishingItem"), fish, 1);
                    }
                }
                else
                {
                    for (int i = 0; i < ItemPet.Randomizer(baseChance * fish.stack); i++)
                    {
                        Player.QuickSpawnItem(Player.GetSource_Misc("FishingItem"), fish, 1);
                    }
                }
            }
        }
    }
    public sealed class ZephyrFishItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ZephyrFish;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            ZephyrFish zephyrFish = Main.LocalPlayer.GetModPlayer<ZephyrFish>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.ZephyrFish")
                        .Replace("<windFish>", Math.Round(zephyrFish.speedMult / 8f, 5).ToString())
                        .Replace("<regularChance>", zephyrFish.baseChance.ToString())
                        .Replace("<windChance>", zephyrFish.windChance.ToString())
                        .Replace("<anglerPower>", Math.Round(zephyrFish.powerPerQuest * 100, 5).ToString())
                        .Replace("<maxAnglerPower>", Math.Round(zephyrFish.maxQuestPower * 100, 5).ToString())
                        .Replace("<anglerQuests>", Main.LocalPlayer.anglerQuestsFinished.ToString())
                        .Replace("<currentAnglerPower>", Math.Round(zephyrFish.powerPerQuest * Main.LocalPlayer.anglerQuestsFinished * 100, 5).ToString())
                        ));
        }
    }
}
