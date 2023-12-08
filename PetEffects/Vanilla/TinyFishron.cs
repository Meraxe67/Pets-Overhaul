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
    public sealed class TinyFishron : ModPlayer
    {
        public float fishingPowerPenalty = 0.5f;
        public float fpPerQuest = 0.002f;
        public float maxQuestPower = 0.5f;
        public int bobberChance = 105;
        public int stackChance = 10;
        public float multiplier = 1f;

        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            if (Pet.PetInUse(ItemID.DukeFishronPetItem))
            {
                float fishingPowerMult = 0;
                fishingPowerMult += fishingPowerPenalty;
                if (Player.anglerQuestsFinished * fpPerQuest > maxQuestPower)
                {
                    fishingPowerMult += maxQuestPower;
                }
                else
                {
                    fishingPowerMult += Player.anglerQuestsFinished * fpPerQuest;
                }

                fishingLevel += fishingPowerMult;
            }
        }
        public override void ModifyCaughtFish(Item fish)
        {
            if (Pet.PetInUse(ItemID.DukeFishronPetItem) && fish.maxStack != 1)
            {
                for (int i = 0; i < ItemPet.Randomizer(stackChance + (int)(Player.fishingSkill * multiplier) * fish.stack); i++)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.fishingItem), fish, 1);
                }
            }
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Pet.PetInUse(ItemID.DukeFishronPetItem) && item.fishingPole > 0)
            {
                for (int i = 0; i < ItemPet.Randomizer(bobberChance); i++)
                {
                    Vector2 bobberSpeed = velocity + new Vector2(Main.rand.NextFloat(-50f, 50f) * 0.05f, Main.rand.NextFloat(-50f, 50f) * 0.05f);
                    Projectile.NewProjectile(source, position, bobberSpeed, ProjectileID.FishingBobber, 0, 0f, Player.whoAmI);
                }
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
    }
    public sealed class DukeFishronPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DukeFishronPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            TinyFishron tinyFishron = Main.LocalPlayer.GetModPlayer<TinyFishron>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DukeFishronPetItem")
                        .Replace("<baseMult>", tinyFishron.fishingPowerPenalty.ToString())
                        .Replace("<anglerFishingPower>", tinyFishron.fpPerQuest.ToString())
                        .Replace("<flatChance>", tinyFishron.stackChance.ToString())
                        .Replace("<fishingPowerChance>", Math.Round(tinyFishron.multiplier * 100, 2).ToString())
                        .Replace("<bobberChance>", tinyFishron.bobberChance.ToString())
                        .Replace("<anglerQuests>", Main.LocalPlayer.anglerQuestsFinished.ToString())
                        .Replace("<currentAnglerWithBaseMult>", Math.Round(Main.LocalPlayer.anglerQuestsFinished * tinyFishron.fpPerQuest + tinyFishron.fishingPowerPenalty, 2).ToString())
                        ));
        }
    }
}
