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
    public sealed class JackOLanternEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.PumpkingPetItem && Player.miscEquips[1].TryGetGlobalItem(out JackOLantern jackOLantern))
            {
                Player.GetAttackSpeed<GenericDamageClass>() += jackOLantern.CurrentAtkSpd;
                Pet.harvestingFortune += jackOLantern.CurrentHarvFort;
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

        public int baseHarvFort = 10;
        public int harvFortPerRoll = 1;
        public int harvFortMaxRoll = 20;
        public int harvFortRoll = 0;
        public int CurrentHarvFort => baseHarvFort + harvFortPerRoll * harvFortRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.PumpkingPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (atkSpdRoll <= 0)
            {
                atkSpdRoll = Main.rand.Next(atkSpdMaxRoll) + 1;
            }

            if (harvFortRoll <= 0)
            {
                harvFortRoll = Main.rand.Next(harvFortMaxRoll) + 1;
            }

            if (luckRoll <= 0)
            {
                luckRoll = Main.rand.Next(luckMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)atkSpdRoll);
            writer.Write((byte)harvFortRoll);
            writer.Write((byte)luckRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            atkSpdRoll = reader.ReadByte();
            harvFortRoll = reader.ReadByte();
            luckRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("PumpkinAtkSpd", atkSpdRoll);
            tag.Add("PumpkinLuck", luckRoll);
            tag.Add("PumpkinExp", harvFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("PumpkinAtkSpd", out int aSpd))
            {
                atkSpdRoll = aSpd;
            }

            if (tag.TryGet("PumpkinLuck", out int luck))
            {
                luckRoll = luck;
            }

            if (tag.TryGet("PumpkinExp", out int exp))
            {
                harvFortRoll = exp;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.JackOLantern")

                        .Replace("<atkSpdBase>", Math.Round(baseAtkSpd * 100, 2).ToString())
                        .Replace("<atkSpdPer>", Math.Round(atkSpdPerRoll * 100, 2).ToString())

                        .Replace("<expBase>", baseHarvFort.ToString())
                        .Replace("<expPer>", harvFortPerRoll.ToString())

                        .Replace("<luckBase>", baseLuck.ToString())
                        .Replace("<luckPer>", luckPerRoll.ToString())

                        .Replace("<currentAtkSpd>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentAtkSpd * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), atkSpdRoll, atkSpdMaxRoll))
                        .Replace("<atkSpdRoll>", PetColors.LightPetRarityColorConvert(atkSpdRoll.ToString(), atkSpdRoll, atkSpdMaxRoll))
                        .Replace("<atkSpdMaxRoll>", PetColors.LightPetRarityColorConvert(atkSpdMaxRoll.ToString(), atkSpdRoll, atkSpdMaxRoll))

                        .Replace("<currentExp>", PetColors.LightPetRarityColorConvert(CurrentHarvFort.ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), harvFortRoll, harvFortMaxRoll))
                        .Replace("<expRoll>", PetColors.LightPetRarityColorConvert(harvFortRoll.ToString(), harvFortRoll, harvFortMaxRoll))
                        .Replace("<expMaxRoll>", PetColors.LightPetRarityColorConvert(harvFortMaxRoll.ToString(), harvFortRoll, harvFortMaxRoll))

                        .Replace("<currentLuck>", PetColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + Math.Round(CurrentLuck, 2).ToString(), luckRoll, luckMaxRoll))
                        .Replace("<luckRoll>", PetColors.LightPetRarityColorConvert(luckRoll.ToString(), luckRoll, luckMaxRoll))
                        .Replace("<luckMaxRoll>", PetColors.LightPetRarityColorConvert(luckMaxRoll.ToString(), luckRoll, luckMaxRoll))
                        ));
            if (luckRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
