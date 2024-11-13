using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class Cat : TownPet
    {

        public float catSpeed = 1.025f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                catSpeed = 1.075f;
            }
            else if (Main.hardMode)
            {
                catSpeed = 1.05f;
            }
            else
            {
                catSpeed = 1.025f;
            }
        }
        public override void PostUpdateBuffs()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownCat)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetCat>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetCat>()))
            {
                Player.moveSpeed *= catSpeed;
                Pet.fishingFortune += DefaultFishFort;
            }
        }
    }
}