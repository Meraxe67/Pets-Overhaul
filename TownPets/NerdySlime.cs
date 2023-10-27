using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class NerdySlime : TownPet
    {
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
        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetNerd>()))
            {
                if (NPC.downedMoonlord)
                {
                    Lighting.AddLight(Player.Center, TorchID.UltraBright);
                    Player.nightVision = true;
                }
                if (Main.hardMode)
                {
                    Player.InfoAccMechShowWires = true;
                }
                Player.dangerSense = true;
                Player.tileSpeed *= nerdBuildSpeed;
            }
        }
    }
}