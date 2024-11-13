using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class NerdySlime : TownPet
    {
        public float nerdBuildSpeed = 1.05f;
        public float nerdLightScale = 0.2f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                nerdBuildSpeed = 1.3f;
                nerdLightScale = 1.1f;
            }
            else if (Main.hardMode)
            {
                nerdBuildSpeed = 1.2f;
                nerdLightScale = 0.5f;
            }
            else
            {
                nerdBuildSpeed = 1.1f;
                nerdLightScale = 0.2f;
            }
        }
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < 2000 && Main.npc[i].type == NPCID.TownSlimeBlue && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetNerd>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetNerd>()))
            {
                if (NPC.downedMoonlord)
                {
                    Player.InfoAccMechShowWires = true;
                }
                if (Main.hardMode)
                {
                    Player.nightVision = true;
                    Player.dangerSense = true;
                }
                Lighting.AddLight(Player.Center, new Microsoft.Xna.Framework.Vector3(2.55f, 2.55f, 2.55f) * nerdLightScale);
                Player.tileSpeed *= nerdBuildSpeed;
            }
        }
    }
}