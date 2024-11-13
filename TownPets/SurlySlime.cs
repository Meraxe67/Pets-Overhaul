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
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownSlimeRed)
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