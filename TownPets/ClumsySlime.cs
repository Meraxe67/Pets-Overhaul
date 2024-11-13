using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class ClumsySlime : TownPet
    {
        public float clumsyLuck = 0.01f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                clumsyLuck = 0.03f;
            }
            else if (Main.hardMode)
            {
                clumsyLuck = 0.02f;
            }
            else
            {
                clumsyLuck = 0.01f;
            }
        }
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimePurple && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetClumsy>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetClumsy>()))
            {
                Pet.globalFortune += DefaultGlobalFort;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetClumsy>()))
            {
                luck += clumsyLuck;
            }
        }
    }
}