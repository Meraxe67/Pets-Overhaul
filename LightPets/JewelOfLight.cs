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
    public sealed class JewelOfLightEffect : ModPlayer
    {
        public GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.FairyQueenPetItem && Player.miscEquips[1].TryGetGlobalItem(out JewelOfLight empress))
            {
                Player.moveSpeed += empress.CurrentMoveSpd;
                Pet.fishingExpBoost += empress.CurrentFishingExp;
                if (Player.equippedWings != null)
                    Player.wingTimeMax += empress.CurrentFlightTime;
            }
        }
    }
    public sealed class JewelOfLight : GlobalItem
    {
        public float baseMoveSpd = 0.07f;
        public float moveSpdPerRoll = 0.015f;
        public int moveSpdMaxRoll = 8;
        public int moveSpdRoll = 0;
        public float CurrentMoveSpd => baseMoveSpd + moveSpdPerRoll * moveSpdRoll;

        public int baseWing = 30;
        public int wingPerRoll = 6;
        public int wingMaxRoll = 15;
        public int wingRoll = 0;
        public int CurrentFlightTime => baseWing + wingPerRoll * wingRoll;

        public float baseFishingExp = 0.07f;
        public float fishingExpPerRoll = 0.012f;
        public int fishingExpMaxRoll = 20;
        public int fishingExpRoll = 0;
        public float CurrentFishingExp => baseFishingExp + fishingExpPerRoll * fishingExpRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.FairyQueenPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (moveSpdRoll <= 0)
                moveSpdRoll = Main.rand.Next(moveSpdMaxRoll) + 1;
            if (fishingExpRoll <= 0)
                fishingExpRoll = Main.rand.Next(fishingExpMaxRoll) + 1;
            if (wingRoll <= 0)
                wingRoll = Main.rand.Next(wingMaxRoll) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("EmpressMoveSpd", moveSpdRoll);
            tag.Add("EmpressWing", wingRoll);
            tag.Add("EmpressExp", fishingExpRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            moveSpdRoll = tag.GetInt("EmpressMoveSpd");
            wingRoll = tag.GetInt("EmpressWing");
            fishingExpRoll = tag.GetInt("EmpressExp");
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.JewelOfLight")

                        .Replace("<moveSpdBase>", Math.Round(baseMoveSpd * 100, 2).ToString())
                        .Replace("<moveSpdPer>", Math.Round(moveSpdPerRoll * 100, 2).ToString())

                        .Replace("<wingBase>", Math.Round(baseWing / 60f, 2).ToString())
                        .Replace("<wingPer>", Math.Round(wingPerRoll / 60f, 2).ToString())

                        .Replace("<expBase>", Math.Round(baseFishingExp * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(fishingExpPerRoll * 100, 2).ToString())

                        .Replace("<currentMoveSpd>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentMoveSpd * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), moveSpdRoll, moveSpdMaxRoll))
                        .Replace("<moveSpdRoll>", GlobalPet.LightPetRarityColorConvert(moveSpdRoll.ToString(), moveSpdRoll, moveSpdMaxRoll))
                        .Replace("<moveSpdMaxRoll>", GlobalPet.LightPetRarityColorConvert(moveSpdMaxRoll.ToString(), moveSpdRoll, moveSpdMaxRoll))

                        .Replace("<currentWing>", GlobalPet.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + Math.Round(CurrentFlightTime / 60f, 2).ToString(), wingRoll, wingMaxRoll))
                        .Replace("<wingRoll>", GlobalPet.LightPetRarityColorConvert(wingRoll.ToString(), wingRoll, wingMaxRoll))
                        .Replace("<wingMaxRoll>", GlobalPet.LightPetRarityColorConvert(wingMaxRoll.ToString(), wingRoll, wingMaxRoll))

                        .Replace("<currentExp>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentFishingExp * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), fishingExpRoll, fishingExpMaxRoll))
                        .Replace("<expRoll>", GlobalPet.LightPetRarityColorConvert(fishingExpRoll.ToString(), fishingExpRoll, fishingExpMaxRoll))
                        .Replace("<expMaxRoll>", GlobalPet.LightPetRarityColorConvert(fishingExpMaxRoll.ToString(), fishingExpRoll, fishingExpMaxRoll))

                        ));
        }
    }
}
