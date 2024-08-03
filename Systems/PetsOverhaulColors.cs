using Microsoft.Xna.Framework;
using Terraria;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Class that contains colors used withnin tooltips etc. in Pets Overhaul.
    /// </summary>
    public class PetColors
    {
        /// <summary>
        /// Converts given text to be corresponding color of Light Pet quality values
        /// </summary>
        /// <param name="text">Text to be converted</param>
        /// <param name="currentRoll">Current roll of the stat</param>
        /// <param name="maxRoll">Maximum roll of the stat</param>
        /// <returns>Text with its color changed depending on quality amount</returns>
        public static string LightPetRarityColorConvert(string text, int currentRoll, int maxRoll)
        {
            if (currentRoll == maxRoll)
            {
                return $"[c/{MaxQuality.Hex3()}:{text}]";
            }
            else if (currentRoll > maxRoll * 0.66f)
            {
                return $"[c/{HighQuality.Hex3()}:{text}]";
            }
            else if (currentRoll > maxRoll * 0.33f)
            {
                return $"[c/{MidQuality.Hex3()}:{text}]";
            }
            else
            {
                return $"[c/{LowQuality.Hex3()}:{text}]";
            }
        }
        public static Color LowQuality => new(130, 130, 130);
        public static Color MidQuality => new(77, 117, 154);
        public static Color HighQuality => new(252, 194, 0);
        /// <summary>
        /// Alternates between (165, 249, 255) and (255, 207, 249) every frame.
        /// </summary>
        public static Color MaxQuality => Color.Lerp(new Color(165, 249, 255), new Color(255, 207, 249), GlobalPet.ColorVal);
        public static Color MeleeClass => new(230, 145, 56);
        public static Color RangedClass => new(255, 179, 186);
        public static Color MagicClass => new(51, 153, 255);
        public static Color SummonerClass => new(138, 43, 226);
        public static Color UtilityClass => new(27, 222, 255);
        public static Color MobilityClass => new(204, 245, 245);
        public static Color HarvestingClass => new(205, 225, 0);
        public static Color MiningClass => new(150, 168, 176);
        public static Color FishingClass => new(3, 130, 233);
        public static Color OffensiveClass => new(246, 84, 106);
        public static Color DefensiveClass => new(14, 168, 14);
        public static Color SupportiveClass => new(255, 20, 147);
        public static Color ClassEnumToColor(PetClasses Class)
        {
            return Class switch
            {
                PetClasses.None => new(0, 0, 0),
                PetClasses.Melee => MeleeClass,
                PetClasses.Ranged => RangedClass,
                PetClasses.Magic => MagicClass,
                PetClasses.Summoner => SummonerClass,
                PetClasses.Utility => UtilityClass,
                PetClasses.Mobility => MobilityClass,
                PetClasses.Harvesting => HarvestingClass,
                PetClasses.Mining => MiningClass,
                PetClasses.Fishing => FishingClass,
                PetClasses.Offensive => OffensiveClass,
                PetClasses.Defensive => DefensiveClass,
                PetClasses.Supportive => SupportiveClass,
                _ => new(0, 0, 0),
            };
        }
        /// <summary>
        /// Writes out Pet's Classes and their color mix. Works fine if only one class is given.
        /// </summary>
        public static string ClassText(PetClasses Class1, PetClasses Class2 = PetClasses.None)
        {
            if (Class1 == Class2)
            {
                Class2 = PetClasses.None;
            }

            if (Class1 == PetClasses.None && Class2 == PetClasses.None)
            {
                return "No class given.";
            }
            else if (Class1 != PetClasses.None && Class2 != PetClasses.None)
            { 
                {
                    Color color = Color.Lerp(ClassEnumToColor(Class1), ClassEnumToColor(Class2),0.5f);
                    return $"[c/{color.Hex3()}:{Class1} and {Class2} Pet]";
                }
            }
            else if (Class2 == PetClasses.None)
            {
                return $"[c/{ClassEnumToColor(Class1).Hex3()}:{Class1} Pet]";
            }
            else
            {
                return $"[c/{ClassEnumToColor(Class2).Hex3()}:{Class2} Pet]";
            }
        }
    }
}
