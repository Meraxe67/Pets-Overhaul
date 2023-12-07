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
    public sealed class MagicLanternEffect : ModPlayer
    {
        public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.MagicLantern && Player.miscEquips[1].TryGetGlobalItem(out MagicLantern magicLantern))
            {
                Player.statDefense += magicLantern.CurrentDef;
                Player.statDefense *= magicLantern.CurrentDefMult + 1f;
                Pet.miningExpBoost += magicLantern.CurrentMiningExp;
                Pet.miningFortune += magicLantern.CurrentMiningFort;
            }
        }
    }
    public sealed class MagicLantern : GlobalItem
    {
        public int defPerRoll = 1;
        public int defMaxRoll = 3;
        public int defRoll = 0;
        public int CurrentDef => defPerRoll * defRoll;

        public float defMultBase = 0.02f;
        public float defMultPerRoll = 0.002f;
        public int defMultMaxRoll = 20;
        public int defMultRoll = 0;
        public float CurrentDefMult => defMultBase + defMultPerRoll * defMultRoll;

        public float baseMiningExp = 0.05f;
        public float miningExpPerRoll = 0.01f;
        public int miningExpMaxRoll = 15;
        public int miningExpRoll = 0;
        public float CurrentMiningExp => baseMiningExp + miningExpPerRoll * miningExpRoll;

        public int baseMiningFort = 5;
        public int miningFortPerRoll = 1;
        public int miningFortMaxRoll = 15;
        public int miningFortRoll = 0;
        public int CurrentMiningFort => baseMiningFort + miningFortPerRoll * miningFortRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.MagicLantern;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (defRoll <= 0)
                defRoll = Main.rand.Next(defMaxRoll) + 1;
            if (defMultRoll <= 0)
                defMultRoll = Main.rand.Next(defMultMaxRoll) + 1;
            if (miningExpRoll <= 0)
                miningExpRoll = Main.rand.Next(miningExpMaxRoll) + 1;
            if (miningFortRoll <= 0)
                miningFortRoll = Main.rand.Next(miningFortMaxRoll) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("LanternDef", defRoll);
            tag.Add("LanternMult", defMultRoll);
            tag.Add("LanternExp", miningExpRoll);
            tag.Add("LanternFort", miningFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("LanternDef", out int def))
                defRoll = def;
            if (tag.TryGet("LanternMult", out int perc))
                defMultRoll = perc;
            if (tag.TryGet("LanternExp", out int exp))
                miningExpRoll = exp;
            if (tag.TryGet("EmpressFort", out int fort))
                miningFortRoll = fort;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.MagicLantern")

                        .Replace("<defPer>", defPerRoll.ToString())

                        .Replace("<defMultBase>", Math.Round(defMultBase * 100, 2).ToString())
                        .Replace("<defMultPer>", Math.Round(defMultPerRoll * 100, 2).ToString())

                        .Replace("<expBase>", Math.Round(baseMiningExp * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(miningExpPerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseMiningFort.ToString())
                        .Replace("<fortPer>", miningFortPerRoll.ToString())

                        .Replace("<currentDef>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentDef.ToString(), defRoll, defMaxRoll))
                        .Replace("<defRoll>", GlobalPet.LightPetRarityColorConvert(defRoll.ToString(), defRoll, defMaxRoll))
                        .Replace("<defMaxRoll>", GlobalPet.LightPetRarityColorConvert(defMaxRoll.ToString(), defRoll, defMaxRoll))

                        .Replace("<currentDefMult>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentDefMult * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), defMultRoll, defMultMaxRoll))
                        .Replace("<defMultRoll>", GlobalPet.LightPetRarityColorConvert(defMultRoll.ToString(), defMultRoll, defMultMaxRoll))
                        .Replace("<defMultMaxRoll>", GlobalPet.LightPetRarityColorConvert(defMultMaxRoll.ToString(), defMultRoll, defMultMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentMiningExp * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), miningExpRoll, miningExpMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(miningExpRoll.ToString(), miningExpRoll, miningExpMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(miningExpMaxRoll.ToString(), miningExpRoll, miningExpMaxRoll))

                        .Replace("<currentFort>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMiningFort.ToString(), miningFortRoll, miningFortMaxRoll))
                        .Replace("<fortRoll>", GlobalPet.LightPetRarityColorConvert(miningFortRoll.ToString(), miningFortRoll, miningFortMaxRoll))
                        .Replace("<fortMaxRoll>", GlobalPet.LightPetRarityColorConvert(miningFortMaxRoll.ToString(), miningFortRoll, miningFortMaxRoll))
                        ));
        }
    }
}
