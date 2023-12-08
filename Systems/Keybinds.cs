using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Loader;

namespace PetsOverhaul.Systems
{
    public class Keybinds : ModSystem
    {
        public static ModKeybind DualSlimeTooltipSwap { get; private set; }
        public static ModKeybind PetTooltipHide { get; private set; }
        public override void Load()
        {
            DualSlimeTooltipSwap = KeybindLoader.RegisterKeybind(Mod, "DualSlimeTooltipSwap", Keys.P);
            PetTooltipHide = KeybindLoader.RegisterKeybind(Mod, "PetTooltipHide", Keys.LeftShift);
        }
    }
}
