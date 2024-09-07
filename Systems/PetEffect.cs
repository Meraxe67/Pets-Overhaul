using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    /// <summary>
    /// Abstract class that contains what every Primary Pet Effect contains.
    /// </summary>
    public abstract class PetEffect : ModPlayer
    {
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
}
