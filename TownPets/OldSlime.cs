using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class OldSlime : TownPet
    {
        public int oldDef = 1;
        public float oldKbResist = 0.05f;
        public override void PreUpdate()
        {
            if (NPC.downedMoonlord)
            {
                oldKbResist = 0.15f;
                oldDef = 3;
            }
            else if (Main.hardMode)
            {
                oldKbResist = 0.1f;
                oldDef = 2;
            }
            else
            {
                oldKbResist = 0.05f;
                oldDef = 1;
            }
        }
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimeOld && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetOld>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetOld>()))
            {
                Player.statDefense += oldDef;
                Pet.harvestingFortune += DefaultHarvFort;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            modifiers.Knockback *= 1f - oldKbResist;
        }
    }
}