using PetsOverhaul.Buffs.TownPetBuffs;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class MysticSlime : TownPet
    {
        public float mysticHaste = 0.02f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                mysticHaste = 0.06f;
            }
            else if (Main.hardMode)
            {
                mysticHaste = 0.04f;
            }
            else
            {
                mysticHaste = 0.02f;
            }
        }
        public override void PostUpdateBuffs()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownSlimeYellow)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetMystic>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetMystic>()))
            {
                Player.GetModPlayer<GlobalPet>().abilityHaste += mysticHaste;
                Pet.globalFortune += DefaultGlobalFort;
            }
        }
    }
}