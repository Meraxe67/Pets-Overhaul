using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class CoolSlime : TownPet
    {
        public float critHitsAreCool = 1f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                critHitsAreCool = 3f;
            }
            else if (Main.hardMode)
            {
                critHitsAreCool = 2f;
            }
            else
            {
                critHitsAreCool = 1f;
            }
        }
        public override void PostUpdateBuffs()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownSlimeGreen)
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
                Pet.miningFortune += DefaultMiningFort;
            }
        }
    }
}