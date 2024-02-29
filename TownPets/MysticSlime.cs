using PetsOverhaul.Buffs.TownPetBuffs;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.TownPets
{
    public class MysticSlime : TownPet
    {
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimeYellow && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetMystic>(), 2);
                    break;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetMystic>()))
            {
                Player.GetModPlayer<GlobalPet>().abilityHaste += mysticHaste;
                Pet.globalFortune += GlobalFort;
            }
        }
    }
}