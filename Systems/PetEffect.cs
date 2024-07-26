using Microsoft.CodeAnalysis.Classification;
using PetsOverhaul.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Abstract class that contains what every Primary Pet Effect contains.
    /// </summary>
    public abstract class PetEffect : ModPlayer
    {
        public enum PetClasses
        {
            None = 0, 
            Melee = 1, 
            Ranged = 2, 
            Magic = 3, 
            Summoner = 4, 
            Utility = 5, 
            Mobility = 6, 
            Harvesting = 7, 
            Mining = 8, 
            Fishing = 9, 
            Offensive = 10, 
            Defensive = 11, 
            Supportive = 12
        }
        /// <summary>
        /// Accesses the GlobalPet class, which has useful methods and fields for Pet implementation.
        /// </summary>
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        /// <summary>
        /// Primary Class of Pet that will appear on its tooltip with its color.
        /// </summary>
        public abstract PetClasses PetClassPrimary { get; }
        /// <summary>
        /// Secondary Class of Pet that will appear on its tooltip, which will mix its color with the Primary Classes color. Defaults to None.
        /// </summary>
        public virtual PetClasses PetClassSecondary => PetClasses.None;
    }
    //public class LightPetRoll
    //{
    //    //Will be looked at later
    //}
}
