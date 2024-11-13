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
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Player.Distance(npc.Center) < auraRange && npc.type == NPCID.TownSlimeOld)
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