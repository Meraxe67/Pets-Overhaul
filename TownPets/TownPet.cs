using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class TownPet : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int auraRange = 2000;
        public float bunnyJump = 0.08f;
        public float catSpeed = 1.025f;
        public int dogFish = 1;
        public float clumsyLuck = 0.01f;
        public float squireDamage = 0.008f;
        public float critHitsAreCool = 1f;
        public int oldDef = 1;
        public float divaDisc = 0.005f;
        public float mysticHaste = 0.02f;
        public float nerdBuildSpeed = 1.05f;
        public float oldKbResist = 0.95f;
        public float nerdLightScale = 0.2f;

        public float mysticAllExp = 0.01f;
        public float bunnyHarvExp = 0.02f;
        public float dogFishExp = 0.02f;
        public float squireMiningExp = 0.02f;
        public int clumsyGlobalFort = 1;
        public int catFishFort = 2;
        public int oldHarvFort = 2;
        public int coolMiningFort = 2;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                auraRange = 2400;
                bunnyJump = 0.15f;
                catSpeed = 1.075f;
                dogFish = 3;
                squireDamage = 0.03f;
                critHitsAreCool = 3f;
                oldDef = 3;
                clumsyLuck = 0.03f;
                divaDisc = 0.015f;
                mysticHaste = 0.06f;
                nerdBuildSpeed = 1.3f;
                oldKbResist = 0.85f;
                nerdLightScale = 1.1f;

                mysticAllExp = 0.03f;
                bunnyHarvExp = 0.06f;
                dogFishExp = 0.06f;
                squireMiningExp = 0.06f;
                catFishFort = 6;
                oldHarvFort = 6;
                coolMiningFort = 6;
                clumsyGlobalFort = 3;
            }
            else if (Main.hardMode)
            {
                auraRange = 2200;
                bunnyJump = 0.11f;
                catSpeed = 1.05f;
                dogFish = 2;
                squireDamage = 0.02f;
                critHitsAreCool = 2f;
                oldDef = 2;
                clumsyLuck = 0.02f;
                divaDisc = 0.01f;
                mysticHaste = 0.04f;
                nerdBuildSpeed = 1.2f;
                oldKbResist = 0.90f;
                nerdLightScale = 0.5f;

                mysticAllExp = 0.02f;
                bunnyHarvExp = 0.04f;
                dogFishExp = 0.04f;
                squireMiningExp = 0.04f;
                catFishFort = 4;
                oldHarvFort = 4;
                coolMiningFort = 4;
                clumsyGlobalFort = 2;
            }
            else
            {
                auraRange = 2000;
                bunnyJump = 0.08f;
                catSpeed = 1.025f;
                dogFish = 1;
                squireDamage = 0.008f;
                critHitsAreCool = 1f;
                oldDef = 1;
                clumsyLuck = 0.01f;
                divaDisc = 0.005f;
                mysticHaste = 0.02f;
                nerdBuildSpeed = 1.1f;
                oldKbResist = 0.95f;
                nerdLightScale = 0.2f;

                mysticAllExp = 0.01f;
                bunnyHarvExp = 0.02f;
                dogFishExp = 0.02f;
                squireMiningExp = 0.02f;
                clumsyGlobalFort = 1;
                catFishFort = 2;
                oldHarvFort = 2;
                coolMiningFort = 2;
            }
        }
    }
}
