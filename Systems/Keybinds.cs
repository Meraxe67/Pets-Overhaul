using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Loader;

namespace PetsOverhaul.Systems
{
    public class Keybinds : ModSystem
    {
        public static ModKeybind UsePetAbility { get; private set; }
        public static ModKeybind PetTooltipSwap { get; private set; }
        public static ModKeybind PetTooltipHide { get; private set; }
        public override void Load()
        {
            UsePetAbility = KeybindLoader.RegisterKeybind(Mod, "UsePetAbility", Keys.Z);
            PetTooltipSwap = KeybindLoader.RegisterKeybind(Mod, "PetTooltipSwap", Keys.P);
            PetTooltipHide = KeybindLoader.RegisterKeybind(Mod, "PetTooltipHide", Keys.LeftShift);
        }
    }
}
