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
    public sealed class CreeperEggEffect : ModPlayer
    {
        GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.DD2PetGhost && Player.miscEquips[1].TryGetGlobalItem(out CreeperEgg creeperEgg))
            {
                Player.GetDamage<SummonDamageClass>() += creeperEgg.CurrentSummonerDmg;
                Player.GetDamage<MeleeDamageClass>() += creeperEgg.CurrentMeleeDmg;
                Player.GetAttackSpeed<MeleeDamageClass>() += creeperEgg.CurrentAtkSpd;
            }
        }
    }
    public sealed class CreeperEgg : GlobalItem
    {
        public float baseSum = 0.04f;
        public float sumPerRoll = 0.005f;
        public int sumMaxRoll = 16;
        public int sumRoll = 0;
        public float CurrentSummonerDmg => baseSum + sumPerRoll * sumRoll;

        public float baseMelee = 0.04f;
        public float meleePerRoll = 0.005f;
        public int meleeMaxRoll = 16;
        public int meleeRoll = 0;
        public float CurrentMeleeDmg => baseMelee + meleePerRoll * meleeRoll;

        public float baseAtkSpd = 0.025f;
        public float atkSpdPerRoll = 0.004f;
        public int atkSpdMaxRoll = 20;
        public int atkSpdRoll = 0;
        public float CurrentAtkSpd => baseAtkSpd + atkSpdPerRoll * atkSpdRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DD2PetGhost;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (sumRoll <= 0)
                sumRoll = Main.rand.Next(sumMaxRoll) + 1;
            if (meleeRoll <= 0)
                meleeRoll = Main.rand.Next(meleeMaxRoll) + 1;
            if (atkSpdRoll <= 0)
                atkSpdRoll = Main.rand.Next(atkSpdMaxRoll) + 1;
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("FlickerwickSum", sumRoll);
            tag.Add("FlickerwickMelee", meleeRoll);
            tag.Add("FlickerwickAtkSpd", atkSpdRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            sumRoll = tag.GetInt("FlickerwickSum");
            meleeRoll = tag.GetInt("FlickerwickMelee");
            atkSpdRoll = tag.GetInt("FlickerwickAtkSpd");
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.CreeperEgg")

                        .Replace("<sumBase>", Math.Round(baseSum * 100, 2).ToString())
                        .Replace("<sumPer>", Math.Round(sumPerRoll * 100, 2).ToString())

                        .Replace("<meleeBase>", Math.Round(baseMelee * 100, 2).ToString())
                        .Replace("<meleePer>", Math.Round(meleePerRoll * 100, 2).ToString())

                        .Replace("<atkSpdBase>", Math.Round(baseAtkSpd * 100, 2).ToString())
                        .Replace("<atkSpdPer>", Math.Round(atkSpdPerRoll * 100, 2).ToString())

                        .Replace("<currentSum>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentSummonerDmg * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), sumRoll, sumMaxRoll))
                        .Replace("<sumRoll>", GlobalPet.LightPetRarityColorConvert(sumRoll.ToString(), sumRoll, sumMaxRoll))
                        .Replace("<sumMaxRoll>", GlobalPet.LightPetRarityColorConvert(sumMaxRoll.ToString(), sumRoll, sumMaxRoll))

                        .Replace("<currentMelee>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentMeleeDmg * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), meleeRoll, meleeMaxRoll))
                        .Replace("<meleeRoll>", GlobalPet.LightPetRarityColorConvert(meleeRoll.ToString(), meleeRoll, meleeMaxRoll))
                        .Replace("<meleeMaxRoll>", GlobalPet.LightPetRarityColorConvert(meleeMaxRoll.ToString(), meleeRoll, meleeMaxRoll))

                        .Replace("<currentAtkSpd>", GlobalPet.LightPetRarityColorConvert(Math.Round(CurrentAtkSpd * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), atkSpdRoll, atkSpdMaxRoll))
                        .Replace("<atkSpdRoll>", GlobalPet.LightPetRarityColorConvert(atkSpdRoll.ToString(), atkSpdRoll, atkSpdMaxRoll))
                        .Replace("<atkSpdMaxRoll>", GlobalPet.LightPetRarityColorConvert(atkSpdMaxRoll.ToString(), atkSpdRoll, atkSpdMaxRoll))
                        ));
        }
    }
}
