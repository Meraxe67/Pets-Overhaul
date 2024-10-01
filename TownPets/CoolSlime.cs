using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class CoolSlime : TownPet
    {
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimeGreen && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetCool>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetCool>()))
            {
                Player.GetCritChance<GenericDamageClass>() += critHitsAreCool;
                Pet.miningFortune += MiningFort;
            }
        }
    }
}