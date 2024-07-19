namespace PetsOverhaul.ModSupport
{
    public static class ModManager
    {
        public static CalamitySupport CalamityMod;
        public static ThoriumSupport ThoriumMod;
        public static void LoadMods()
        {
            CalamityMod = new CalamitySupport();
            CalamityMod.InitializeMod();
            ThoriumMod = new ThoriumSupport();
            ThoriumMod.InitializeMod();
        }
    }
}