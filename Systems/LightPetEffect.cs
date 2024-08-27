using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public abstract class LightPetEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
    }
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
        public string StatSummaryLine()
        {
            return PetColors.LightPetRarityColorConvert(isInt ? (Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentStatInt.ToString()) : (Math.Round(CurrentStatFloat * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%")), CurrentRoll, MaxRoll) + " " +
                Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.Quality") + " " + PetColors.LightPetRarityColorConvert(CurrentRoll.ToString(), CurrentRoll, MaxRoll) + " " + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.OutOf") + " " + PetColors.LightPetRarityColorConvert(MaxRoll.ToString(), CurrentRoll, MaxRoll);
        }
        public string BaseAndPerQuality()
        {
            return (BaseStat == 0 ? "" : (Math.Round(BaseStat * 100, 2).ToString() + " " + Language.GetTextValue("Mods.PetsOverhaul.+"))) + " " + Math.Round(StatPerRoll * 100, 2).ToString()
                + (isInt ? "" : Language.GetTextValue("Mods.PetsOverhaul.%"));
        }
    }
}
