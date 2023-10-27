using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class SurlySlime : TownPet
    {
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimeRed && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetSurly>(), 2);
                    Player.ZoneShadowCandle = true;
                    break;
                }
            }
        }
    }
    public class SurlySpawnrate : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {

            if (player.HasBuff(ModContent.BuffType<TownPetSurly>()))
            {
                if (NPC.downedMoonlord)
                {
                    spawnRate -= spawnRate / 10;
                }
                else if (Main.hardMode)
                {
                    spawnRate -= spawnRate / 15;
                }
                else
                {
                    spawnRate -= spawnRate / 20;
                }
            }
        }
    }
}