using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class CrimsonHeartEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.CrimsonHeart && Player.miscEquips[1].TryGetGlobalItem(out CrimsonHeart crimsonHeart))
            {
                Player.statLifeMax2 += crimsonHeart.CurrentHealth;
                Pet.fishingExpBoost += crimsonHeart.CurrentFishExp;
                Pet.fishingFortune += crimsonHeart.CurrentFishFort;
            }
        }
    }
    public sealed class CrimsonHeart : GlobalItem
    {
        public int baseHealth = 10;
        public int healthPerRoll = 1;
        public int healthMaxRoll = 10;
        public int healthRoll = 0;
        public int CurrentHealth => baseHealth + healthPerRoll * healthRoll;

        public float baseFishExp = 0.05f;
        public float fishExpPerRoll = 0.01f;
        public int fishExpMaxRoll = 15;
        public int fishExpRoll = 0;
        public float CurrentFishExp => baseFishExp + fishExpPerRoll * fishExpRoll;

        public int baseFishFort = 5;
        public int fishFortPerRoll = 1;
        public int fishFortMaxRoll = 15;
        public int fishFortRoll = 0;
        public int CurrentFishFort => baseFishFort + fishFortPerRoll * fishFortRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CrimsonHeart;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (healthRoll <= 0)
            {
                healthRoll = Main.rand.Next(healthMaxRoll) + 1;
            }

            if (fishExpRoll <= 0)
            {
                fishExpRoll = Main.rand.Next(fishExpMaxRoll) + 1;
            }

            if (fishFortRoll <= 0)
            {
                fishFortRoll = Main.rand.Next(fishFortMaxRoll) + 1;
            }
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("CrimsonHealth", healthRoll);
            tag.Add("CrimsonExp", fishExpRoll);
            tag.Add("CrimsonFort", fishFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("CrimsonHealth", out int hp))
            {
                healthRoll = hp;
            }

            if (tag.TryGet("CrimsonExp", out int exp))
            {
                fishExpRoll = exp;
            }

            if (tag.TryGet("CrimsonFort", out int fort))
            {
                fishFortRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.CrimsonHeart")

                        .Replace("<hpBase>", baseHealth.ToString())
                        .Replace("<hpPer>", healthPerRoll.ToString())

                        .Replace("<expBase>", Math.Round(baseFishExp * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(fishExpPerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseFishFort.ToString())
                        .Replace("<fortPer>", fishFortPerRoll.ToString())

                        .Replace("<currentHp>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentHealth.ToString(), healthRoll, healthMaxRoll))
                        .Replace("<hpRoll>", GlobalPet.LightPetRarityColorConvert(healthRoll.ToString(), healthRoll, healthMaxRoll))
                        .Replace("<hpMaxRoll>", GlobalPet.LightPetRarityColorConvert(healthMaxRoll.ToString(), healthRoll, healthMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentFishExp * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), fishExpRoll, fishExpMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(fishExpRoll.ToString(), fishExpRoll, fishExpMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(fishExpMaxRoll.ToString(), fishExpRoll, fishExpMaxRoll))

                        .Replace("<currentFort>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentFishFort.ToString(), fishFortRoll, fishFortMaxRoll))
                        .Replace("<fortRoll>", GlobalPet.LightPetRarityColorConvert(fishFortRoll.ToString(), fishFortRoll, fishFortMaxRoll))
                        .Replace("<fortMaxRoll>", GlobalPet.LightPetRarityColorConvert(fishFortMaxRoll.ToString(), fishFortRoll, fishFortMaxRoll))
                        ));
        }
    }
}
