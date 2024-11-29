using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public abstract class LightPetEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
    }
    public abstract class LightPetItem : GlobalItem
    {
        public abstract int LightPetItemID { get; }
        public sealed override bool InstancePerEntity => true;
        public sealed override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ExtraAppliesToEntity(entity, lateInstantiation) && entity.type == LightPetItemID;
        }
        /// <summary>
        /// Defaults to true. Override this to change result of AppliesToEntity. AppliesToEntity will return true for the entities who has the type of given Pet Effect's PetItemID.
        /// </summary>
        public virtual bool ExtraAppliesToEntity(Item entity, bool lateInstantation)
        { return true; }
        /// <summary>
        /// Consumes item1 and item2, in result; creates a new Light Pet that inherits highest rolls of both Light Pets. If it fails, returns null and doesn't consume anything.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static Item CombineLightPets(Item item1, Item item2)
        {
            if (item1.type == item2.type)
            {
                Item newPet = item1.Clone();
                foreach (var globalOfNewPet in newPet.Globals)
                {
                    if (globalOfNewPet.GetType().IsSubclassOf(typeof(LightPetItem)))
                    {
                        foreach (var light in item2.Globals)
                        {
                            if (light.GetType().IsSubclassOf(typeof(LightPetItem)))
                            {
                                FieldInfo[] lightPetRolls = light.GetType().GetFields();
                                for (int i = 0; i < lightPetRolls.Length; i++)
                                {
                                    LightPetStat newPetRoll = (LightPetStat)lightPetRolls[i].GetValue(globalOfNewPet);
                                    LightPetStat secondPetRoll = (LightPetStat)lightPetRolls[i].GetValue(light);
                                    if (newPetRoll.CurrentRoll < secondPetRoll.CurrentRoll)
                                    lightPetRolls[i].SetValue(globalOfNewPet, secondPetRoll);
                                }
                                item1.TurnToAir();
                                item2.TurnToAir();
                                return newPet;
                            }
                        }
                    }
                }
            }
                return null;
        }
    }
    /// <summary>
    /// Struct that contains all a singular Light Pet Stat has & methods for easy tooltip.
    /// </summary>
    public struct LightPetStat
    {
        public int CurrentRoll = 0;
        public int MaxRoll = 1;
        public float StatPerRoll = 0;
        public float BaseStat = 0;
        private readonly bool isInt = false;
        public LightPetStat(int maxRoll, int statPerRoll, int baseStat = 0)
        {
            MaxRoll = maxRoll;
            StatPerRoll = statPerRoll;
            BaseStat = baseStat;
            isInt = true;
        }

        public LightPetStat(int maxRoll, float statPerRoll, float baseStat = 0)
        {
            MaxRoll = maxRoll;
            StatPerRoll = statPerRoll;
            BaseStat = baseStat;
            isInt = false;
        }
        public readonly float CurrentStatFloat => BaseStat + StatPerRoll * CurrentRoll;
        public readonly int CurrentStatInt => (int)Math.Ceiling(CurrentStatFloat);
        /// <summary>
        /// Sets the roll of this stat. Commonly used in UpdateInventory()
        /// </summary>
        public void SetRoll()
        {
            if (CurrentRoll <= 0)
            {
                CurrentRoll = Main.rand.Next(MaxRoll) + 1;
            }
        }
        /// <summary>
        /// Returns the stat's current value with its + or %, localized 'Quality' text, and its current quality, localized 'out of' text next to it and the Max roll this stat can achieve. And correctly colors them.
        /// </summary>
        public readonly string StatSummaryLine()
        {
            return PetTextsColors.LightPetRarityColorConvert(isInt ? (Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentStatInt.ToString()) : (Math.Round(CurrentStatFloat * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%")), CurrentRoll, MaxRoll) + " " +
                Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.Quality") + " " + PetTextsColors.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.OutOf") + " " + PetTextsColors.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        /// <summary>
        /// Use this overload if Summary line is intended to show the current stat differently than what StatSummaryLine() does.
        /// </summary>
        public readonly string StatSummaryLine(string currentStat)
        {
            return PetTextsColors.LightPetRarityColorConvert(currentStat, CurrentRoll, MaxRoll) + " " + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.Quality") + " " +
                PetTextsColors.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.OutOf") + " " + PetTextsColors.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        /// <summary>
        /// Returns the stat's Base and Per Roll stats, alongside required spacings, multiplication and operators.
        /// </summary>
        public readonly string BaseAndPerQuality()
        {
            int mult = isInt ? 1 : 100;
            return (BaseStat == 0 ? "" : (Math.Round(BaseStat * mult, 2).ToString() + " " + Language.GetTextValue("Mods.PetsOverhaul.+") + " ")) + Math.Round(StatPerRoll * mult, 2).ToString()
                + (isInt ? "" : Language.GetTextValue("Mods.PetsOverhaul.%")) + " " + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.Per");
        }
        /// <summary>
        /// Use this overload if displayed values are intended to be displayed in a different way than BaseAndPerQuality().
        /// </summary>
        public readonly string BaseAndPerQuality(string perRoll, string baseRoll = "")
        {
            return (BaseStat == 0 ? "" : (baseRoll + " " + Language.GetTextValue("Mods.PetsOverhaul.+") + " ")) + perRoll + " " + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.Per");
        }
    }
}
