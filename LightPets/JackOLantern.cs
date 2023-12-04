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
    public sealed class JackOLanternEffect : ModPlayer
    {
        public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.PumpkingPetItem && Player.miscEquips[1].TryGetGlobalItem(out JackOLantern jackOLantern))
            {
                Player.GetAttackSpeed<GenericDamageClass>() += jackOLantern.CurrentAtkSpd;
                Pet.harvestingExpBoost += jackOLantern.CurrentHarvExp;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Player.miscEquips[1].type == ItemID.PumpkingPetItem && Player.miscEquips[1].TryGetGlobalItem(out JackOLantern jackOLantern))
            {
                luck += jackOLantern.CurrentLuck;
            }

        }
    }
    public sealed class JackOLantern : GlobalItem
    {
        public float baseAtkSpd = 0.04f;
        public float atkSpdPerRoll = 0.003f;
        public int atkSpdMaxRoll = 30;
        public int atkSpdRoll = 0;
        public float CurrentAtkSpd => baseAtkSpd + atkSpdPerRoll * atkSpdRoll;

        public float baseLuck = 0.03f;
        public float luckPerRoll = 0.01f;
        public int luckMaxRoll = 15;
        public int luckRoll = 0;
        public float CurrentLuck => baseLuck + luckPerRoll * luckRoll;

        public float baseHarvExp = 0.07f;
        public float harvExpPerRoll = 0.012f;
        public int harvExpMaxRoll = 20;
        public int harvExpRoll = 0;
        public float CurrentHarvExp => baseHarvExp + harvExpPerRoll * harvExpRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.PumpkingPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (atkSpdRoll <= 0)
                atkSpdRoll = Main.rand.Next(atkSpdMaxRoll) + 1;
            if (harvExpRoll <= 0)
                harvExpRoll = Main.rand.Next(harvExpMaxRoll) + 1;
            if (luckRoll <= 0)
                luckRoll = Main.rand.Next(luckMaxRoll) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("PumpkinAtkSpd", atkSpdRoll);
            tag.Add("PumpkinLuck", luckRoll);
            tag.Add("PumpkinExp", harvExpRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            atkSpdRoll = tag.GetInt("PumpkinAtkSpd");
            luckRoll = tag.GetInt("PumpkinLuck");
            harvExpRoll = tag.GetInt("PumpkinExp");
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.JackOLantern")

                        .Replace("<atkSpdBase>", Math.Round(baseAtkSpd * 100, 2).ToString())
                        .Replace("<atkSpdPer>", Math.Round(atkSpdPerRoll * 100, 2).ToString())

                        .Replace("<expBase>", Math.Round(baseHarvExp * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(harvExpPerRoll * 100, 2).ToString())

                        .Replace("<luckBase>", baseLuck.ToString())
                        .Replace("<luckPer>", luckPerRoll.ToString())

                        .Replace("<currentAtkSpd>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentAtkSpd * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), atkSpdRoll, atkSpdMaxRoll))
                        .Replace("<atkSpdRoll>", GlobalPet.LightPetRarityColorConvert(atkSpdRoll.ToString(), atkSpdRoll, atkSpdMaxRoll))
                        .Replace("<atkSpdMaxRoll>", GlobalPet.LightPetRarityColorConvert(atkSpdMaxRoll.ToString(), atkSpdRoll, atkSpdMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentHarvExp * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), harvExpRoll, harvExpMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(harvExpRoll.ToString(), harvExpRoll, harvExpMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(harvExpMaxRoll.ToString(), harvExpRoll, harvExpMaxRoll))

                        .Replace("<currentLuck>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentLuck.ToString(), luckRoll, luckMaxRoll))
                        .Replace("<luckRoll>", GlobalPet.LightPetRarityColorConvert(luckRoll.ToString(), luckRoll, luckMaxRoll))
                        .Replace("<luckMaxRoll>", GlobalPet.LightPetRarityColorConvert(luckMaxRoll.ToString(), luckRoll, luckMaxRoll))
                        ));
        }
    }
}
