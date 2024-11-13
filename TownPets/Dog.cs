using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class Dog : TownPet
    {
        public int dogFish = 1;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                dogFish = 3;
            }
            else if (Main.hardMode)
            {
                dogFish = 2;
            }
            else
            {
                dogFish = 1;
            }
        }
        public override void PostUpdateBuffs()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownDog)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetDog>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetDog>()))
            {
                Player.fishingSkill += dogFish;
                Pet.fishingFortune += DefaultFishFort;
            }
        }
    }
}