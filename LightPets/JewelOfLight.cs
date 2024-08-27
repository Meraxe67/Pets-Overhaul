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
    public sealed class JewelOfLightEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].type == ItemID.FairyQueenPetItem && Player.miscEquips[1].TryGetGlobalItem(out JewelOfLight empress))
            {
                Player.moveSpeed += empress.CurrentMoveSpd;
                if (Player.equippedWings != null)
                {
                    Player.wingTimeMax += empress.CurrentFlightTime;
                }
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (Player.miscEquips[1].type == ItemID.FairyQueenPetItem && Player.miscEquips[1].TryGetGlobalItem(out JewelOfLight empress))
            {
                Player.runAcceleration += empress.CurrentAcc;
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

        public float baseAcc = 0.05f; //nerf acc amount, make this only additive acceleration
        public float accPerRoll = 0.005f;
        public int accMaxRoll = 20;
        public int accRoll = 0;
        public float CurrentAcc => baseAcc + accPerRoll * accRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.FairyQueenPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (moveSpdRoll <= 0)
            {
                moveSpdRoll = Main.rand.Next(moveSpdMaxRoll) + 1;
            }

            if (accRoll <= 0)
            {
                accRoll = Main.rand.Next(accMaxRoll) + 1;
            }

            if (wingRoll <= 0)
            {
                wingRoll = Main.rand.Next(wingMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)moveSpdRoll);
            writer.Write((byte)accRoll);
            writer.Write((byte)wingRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            moveSpdRoll = reader.ReadByte();
            accRoll = reader.ReadByte();
            wingRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("EmpressMoveSpd", moveSpdRoll);
            tag.Add("EmpressWing", wingRoll);
            tag.Add("EmpressExp", accRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("EmpressMoveSpd", out int spd))
            {
                moveSpdRoll = spd;
            }

            if (tag.TryGet("EmpressWing", out int wing))
            {
                wingRoll = wing;
            }

            if (tag.TryGet("EmpressExp", out int exp))
            {
                accRoll = exp;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.JewelOfLight")

                        .Replace("<moveSpdBase>", Math.Round(baseMoveSpd * 100, 2).ToString())
                        .Replace("<moveSpdPer>", Math.Round(moveSpdPerRoll * 100, 2).ToString())

                        .Replace("<wingBase>", Math.Round(baseWing / 60f, 2).ToString())
                        .Replace("<wingPer>", Math.Round(wingPerRoll / 60f, 2).ToString())

                        .Replace("<expBase>", Math.Round(baseAcc * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(accPerRoll * 100, 2).ToString())

                        .Replace("<currentMoveSpd>", PetTextsColors.LightPetRarityColorConvert(Math.Round(CurrentMoveSpd * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), moveSpdRoll, moveSpdMaxRoll))
                        .Replace("<moveSpdRoll>", PetTextsColors.LightPetRarityColorConvert(moveSpdRoll.ToString(), moveSpdRoll, moveSpdMaxRoll))
                        .Replace("<moveSpdMaxRoll>", PetTextsColors.LightPetRarityColorConvert(moveSpdMaxRoll.ToString(), moveSpdRoll, moveSpdMaxRoll))

                        .Replace("<currentWing>", PetTextsColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + Math.Round(CurrentFlightTime / 60f, 2).ToString(), wingRoll, wingMaxRoll))
                        .Replace("<wingRoll>", PetTextsColors.LightPetRarityColorConvert(wingRoll.ToString(), wingRoll, wingMaxRoll))
                        .Replace("<wingMaxRoll>", PetTextsColors.LightPetRarityColorConvert(wingMaxRoll.ToString(), wingRoll, wingMaxRoll))

                        .Replace("<currentExp>", PetTextsColors.LightPetRarityColorConvert(Math.Round(CurrentAcc * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), accRoll, accMaxRoll))
                        .Replace("<expRoll>", PetTextsColors.LightPetRarityColorConvert(accRoll.ToString(), accRoll, accMaxRoll))
                        .Replace("<expMaxRoll>", PetTextsColors.LightPetRarityColorConvert(accMaxRoll.ToString(), accRoll, accMaxRoll))

                        ));
            if (wingRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetTextsColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
