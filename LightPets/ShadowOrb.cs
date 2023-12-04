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
    public sealed class ShadowOrbEffect : ModPlayer
    {
        public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.ShadowOrb && Player.miscEquips[1].TryGetGlobalItem(out ShadowOrb shadowOrb))
            {
                Player.statManaMax2 += shadowOrb.CurrentMana;
                Pet.harvestingExpBoost += shadowOrb.CurrentHarvExp;
                Pet.harvestingFortune += shadowOrb.CurrentHarvFort;
            }
        }
    }
    public sealed class ShadowOrb : GlobalItem
    {
        public int baseMana = 20;
        public int manaPerRoll = 2;
        public int manaMaxRoll = 10;
        public int manaRoll = 0;
        public int CurrentMana => baseMana + manaPerRoll * manaRoll;

        public float baseHarvExp = 0.05f;
        public float harvExpPerRoll = 0.01f;
        public int harvExpMaxRoll = 15;
        public int harvExpRoll = 0;
        public float CurrentHarvExp => baseHarvExp + harvExpPerRoll * harvExpRoll;

        public int baseHarvFort = 5;
        public int harvFortPerRoll = 1;
        public int harvFortMaxRoll = 15;
        public int harvFortRoll = 0;
        public int CurrentHarvFort => baseHarvFort + harvFortPerRoll * harvFortRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShadowOrb;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (manaRoll <= 0)
                manaRoll = Main.rand.Next(manaMaxRoll) + 1;
            if (harvExpRoll <= 0)
                harvExpRoll = Main.rand.Next(harvExpMaxRoll) + 1;
            if (harvFortRoll <= 0)
                harvFortRoll = Main.rand.Next(harvFortMaxRoll) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("ShadowMana", manaRoll);
            tag.Add("ShadowExp", harvExpRoll);
            tag.Add("ShadowFort", harvFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            manaRoll = tag.GetInt("ShadowMana");
            harvExpRoll = tag.GetInt("ShadowExp");
            harvFortRoll = tag.GetInt("ShadowFort");
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.ShadowOrb")

                        .Replace("<manaBase>", baseMana.ToString())
                        .Replace("<manaPer>", manaPerRoll.ToString())

                        .Replace("<expBase>", Math.Round(baseHarvExp * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(harvExpPerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseHarvFort.ToString())
                        .Replace("<fortPer>", harvFortPerRoll.ToString())

                        .Replace("<currentMana>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMana.ToString(), manaRoll, manaMaxRoll))
                        .Replace("<manaRoll>", GlobalPet.LightPetRarityColorConvert(manaRoll.ToString(), manaRoll, manaMaxRoll))
                        .Replace("<manaMaxRoll>", GlobalPet.LightPetRarityColorConvert(manaMaxRoll.ToString(), manaRoll, manaMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentHarvExp * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), harvExpRoll, harvExpMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(harvExpRoll.ToString(), harvExpRoll, harvExpMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(harvExpMaxRoll.ToString(), harvExpRoll, harvExpMaxRoll))

                        .Replace("<currentFort>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentHarvFort.ToString(), harvFortRoll, harvFortMaxRoll))
                        .Replace("<fortRoll>", GlobalPet.LightPetRarityColorConvert(harvFortRoll.ToString(), harvFortRoll, harvFortMaxRoll))
                        .Replace("<fortMaxRoll>", GlobalPet.LightPetRarityColorConvert(harvFortMaxRoll.ToString(), harvFortRoll, harvFortMaxRoll))
                        ));
        }
    }
}
