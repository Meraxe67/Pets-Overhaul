namespace PetsOverhaul.ModSupport
{
    public static class ModManager
    {
        //public static CalamitySupport Calamity;
        public static ThoriumSupport ThoriumMod;
        public static void LoadMods()
        {
            //Calamity = new CalamitySupport();
            //Calamity.InitializeMod();
            ThoriumMod = new ThoriumSupport();
            ThoriumMod.InitializeMod();
        }
    }
}