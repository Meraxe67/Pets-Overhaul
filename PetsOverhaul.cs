using PetsOverhaul.ModSupport;
using Terraria.ModLoader;

namespace PetsOverhaul
{
    public class PetsOverhaul : Mod
    {
        public override void PostSetupContent()
        {
            ModManager.LoadMods();
        }
    }
}