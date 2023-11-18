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
    public sealed class ToyGolemEffect : ModPlayer
    {
        GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.GolemPetItem && Player.miscEquips[1].TryGetGlobalItem(out ToyGolem toyGolem))
            {
                Player.lifeRegen += toyGolem.CurrentRegen;
                Pet.miningExpBoost += toyGolem.CurrentMiningExp;
                Player.statLifeMax2 += (int)(Player.statLifeMax2 * toyGolem.CurrentHealth);
            }
        }
    }
    public sealed class ToyGolem : GlobalItem
    {
        public int regenPerRoll = 1;
        public int regenMaxRoll = 4;
        public int regenRoll = 0;
        public int CurrentRegen => regenPerRoll * regenRoll;

        public float baseHealth = 0.025f;
        public float healthPerRoll = 0.0025f;
        public int healthMaxRoll = 35;
        public int healthRoll = 0;
        public float CurrentHealth => baseHealth + healthPerRoll * healthRoll;

        public float baseMiningExp = 0.07f;
        public float miningExpPerRoll = 0.012f;
        public int miningExpMaxRoll = 20;
        public int miningExpRoll = 0;
        public float CurrentMiningExp => baseMiningExp + miningExpPerRoll * miningExpRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.GolemPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (regenRoll <= 0)
                regenRoll = Main.rand.Next(regenMaxRoll) + 1;
            if (miningExpRoll <= 0)
                miningExpRoll = Main.rand.Next(miningExpMaxRoll) + 1;
            if (healthRoll <= 0)
                healthRoll = Main.rand.Next(healthMaxRoll) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("GolemRegen", regenRoll);
            tag.Add("GolemHealth", healthRoll);
            tag.Add("GolemExp", miningExpRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            regenRoll = tag.GetInt("GolemRegen");
            healthRoll = tag.GetInt("GolemHealth");
            miningExpRoll = tag.GetInt("GolemExp");
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.ToyGolem")

                        .Replace("<regenPer>", regenPerRoll.ToString())

                        .Replace("<healthBase>", Math.Round(baseHealth * 100, 2).ToString())
                        .Replace("<healthPer>", Math.Round(healthPerRoll * 100, 2).ToString())

                        .Replace("<expBase>", Math.Round(baseMiningExp * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(miningExpPerRoll * 100, 2).ToString())

                        .Replace("<currentRegen>", GlobalPet.LightPetRarityColorConvert(CurrentRegen.ToString(), regenRoll, regenMaxRoll))
                        .Replace("<regenRoll>", GlobalPet.LightPetRarityColorConvert(regenRoll.ToString(), regenRoll, regenMaxRoll))
                        .Replace("<regenMaxRoll>", GlobalPet.LightPetRarityColorConvert(regenMaxRoll.ToString(), regenRoll, regenMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentMiningExp * 100, 2).ToString(), miningExpRoll, miningExpMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(miningExpRoll.ToString(), miningExpRoll, miningExpMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(miningExpMaxRoll.ToString(), miningExpRoll, miningExpMaxRoll))

                        .Replace("<currentHealth>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentHealth*100,2).ToString(), healthRoll, healthMaxRoll))
                        .Replace("<healthRoll>", GlobalPet.LightPetRarityColorConvert(healthRoll.ToString(), healthRoll, healthMaxRoll))
                        .Replace("<healthMaxRoll>", GlobalPet.LightPetRarityColorConvert(healthMaxRoll.ToString(), healthRoll, healthMaxRoll))
                        ));
        }
    }
}
