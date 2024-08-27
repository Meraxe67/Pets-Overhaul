using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
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
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.MagicLantern && Player.miscEquips[1].TryGetGlobalItem(out MagicLantern magicLantern))
            {
                Player.statDefense += magicLantern.CurrentDef;
                Player.statDefense *= magicLantern.CurrentDefMult + 1f;
                Player.endurance += magicLantern.CurrentDr;
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

        public float baseDr = 0.01f;
        public float drPerRoll = 0.002f;
        public int drMaxRoll = 15;
        public int drRoll = 0;
        public float CurrentDr => baseDr + drPerRoll * drRoll;

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
            {
                defRoll = Main.rand.Next(defMaxRoll) + 1;
            }

            if (defMultRoll <= 0)
            {
                defMultRoll = Main.rand.Next(defMultMaxRoll) + 1;
            }

            if (drRoll <= 0)
            {
                drRoll = Main.rand.Next(drMaxRoll) + 1;
            }

            if (miningFortRoll <= 0)
            {
                miningFortRoll = Main.rand.Next(miningFortMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)defRoll);
            writer.Write((byte)defMultRoll);
            writer.Write((byte)drRoll);
            writer.Write((byte)miningFortRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            defRoll = reader.ReadByte();
            defMultRoll = reader.ReadByte();
            drRoll = reader.ReadByte();
            miningFortRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("LanternDef", defRoll);
            tag.Add("LanternMult", defMultRoll);
            tag.Add("LanternExp", drRoll);
            tag.Add("LanternFort", miningFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("LanternDef", out int def))
            {
                defRoll = def;
            }

            if (tag.TryGet("LanternMult", out int perc))
            {
                defMultRoll = perc;
            }

            if (tag.TryGet("LanternExp", out int exp))
            {
                drRoll = exp;
            }

            if (tag.TryGet("EmpressFort", out int fort))
            {
                miningFortRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.MagicLantern")

                        .Replace("<defPer>", defPerRoll.ToString())

                        .Replace("<defMultBase>", Math.Round(defMultBase * 100, 2).ToString())
                        .Replace("<defMultPer>", Math.Round(defMultPerRoll * 100, 2).ToString())

                        .Replace("<expBase>", Math.Round(baseDr * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(drPerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseMiningFort.ToString())
                        .Replace("<fortPer>", miningFortPerRoll.ToString())

                        .Replace("<currentDef>", PetTextsColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentDef.ToString(), defRoll, defMaxRoll))
                        .Replace("<defRoll>", PetTextsColors.LightPetRarityColorConvert(defRoll.ToString(), defRoll, defMaxRoll))
                        .Replace("<defMaxRoll>", PetTextsColors.LightPetRarityColorConvert(defMaxRoll.ToString(), defRoll, defMaxRoll))

                        .Replace("<currentDefMult>", PetTextsColors.LightPetRarityColorConvert(Math.Round(CurrentDefMult * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), defMultRoll, defMultMaxRoll))
                        .Replace("<defMultRoll>", PetTextsColors.LightPetRarityColorConvert(defMultRoll.ToString(), defMultRoll, defMultMaxRoll))
                        .Replace("<defMultMaxRoll>", PetTextsColors.LightPetRarityColorConvert(defMultMaxRoll.ToString(), defMultRoll, defMultMaxRoll))

                        .Replace("<currentExp>", PetTextsColors.LightPetRarityColorConvert(Math.Round(CurrentDr * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), drRoll, drMaxRoll))
                        .Replace("<expRoll>", PetTextsColors.LightPetRarityColorConvert(drRoll.ToString(), drRoll, drMaxRoll))
                        .Replace("<expMaxRoll>", PetTextsColors.LightPetRarityColorConvert(drMaxRoll.ToString(), drRoll, drMaxRoll))

                        .Replace("<currentFort>", PetTextsColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMiningFort.ToString(), miningFortRoll, miningFortMaxRoll))
                        .Replace("<fortRoll>", PetTextsColors.LightPetRarityColorConvert(miningFortRoll.ToString(), miningFortRoll, miningFortMaxRoll))
                        .Replace("<fortMaxRoll>", PetTextsColors.LightPetRarityColorConvert(miningFortMaxRoll.ToString(), miningFortRoll, miningFortMaxRoll))
                        ));
            if (defRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetTextsColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
