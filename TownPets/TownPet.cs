using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class TownPet : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();

        public int auraRange = 2000;
        public int DefaultGlobalFort = 2;
        public int DefaultFishFort = 4;
        public int DefaultHarvFort = 4;
        public int DefaultMiningFort = 4;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                auraRange = 4800;
                DefaultFishFort = 12;
                DefaultHarvFort = 12;
                DefaultMiningFort = 12;
                DefaultGlobalFort = 6;
            }
            else if (Main.hardMode)
            {
                auraRange = 4400;
                DefaultFishFort = 8;
                DefaultHarvFort = 8;
                DefaultMiningFort = 8;
                DefaultGlobalFort = 4;
            }
            else
            {
                auraRange = 4000;
                DefaultGlobalFort = 2;
                DefaultFishFort = 4;
                DefaultHarvFort = 4;
                DefaultMiningFort = 4;
            }
        }
    }
}
