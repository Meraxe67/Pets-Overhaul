using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class Bunny : TownPet
    {
        public float bunnyJump = 0.08f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                bunnyJump = 0.15f;
            }
            else if (Main.hardMode)
            {
                bunnyJump = 0.11f;
            }
            else
            {
                bunnyJump = 0.08f;
            }
        }
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownBunny && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetBunny>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {

            if (Player.HasBuff(ModContent.BuffType<TownPetBunny>()))
            {
                Player.jumpSpeedBoost += Player.jumpSpeed * bunnyJump;
                Pet.harvestingFortune += DefaultHarvFort;
            }
        }
    }
}