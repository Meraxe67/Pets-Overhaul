using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class PetGlobalBuff : GlobalBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[BuffID.GelBalloonBuff] = false;
            Main.buffNoTimeDisplay[BuffID.Wet] = false;
            Main.buffNoTimeDisplay[BuffID.Slimed] = false;
        }
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            if (type == BuffID.GelBalloonBuff && GlobalPet.QueenSlimePetActive(out Player queenSlime))
            {
                NpcPet.AddSlow(new NpcPet.PetSlow(queenSlime.GetModPlayer<SlimePrincess>().slow, 1, PetSlowIDs.QueenSlime), npc);
            }
            else if (type == BuffID.GelBalloonBuff && GlobalPet.DualSlimePetActive(out Player dualSlime))
            {
                NpcPet.AddSlow(new NpcPet.PetSlow(dualSlime.GetModPlayer<DualSlime>().slow, 1, PetSlowIDs.QueenSlime), npc);
            }
        }
    }
}
