using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class Dog : TownPet
    {
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownDog && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetDog>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetDog>()))
            {
                Player.fishingSkill += dogFish;
                Pet.fishingExpBoost += dogFishExp;
            }
        }
    }
}