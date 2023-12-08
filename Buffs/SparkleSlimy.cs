using PetsOverhaul.PetEffects.Vanilla;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs
{
    public class SparkleSlimy : GlobalBuff
    {
        public override void Update(int type, NPC npc, ref int buffIndex)
        {
            Main.buffNoTimeDisplay[BuffID.GelBalloonBuff] = false;
            Main.buffNoTimeDisplay[BuffID.Wet] = false;
            Main.buffNoTimeDisplay[BuffID.Slimed] = false;
            if (type == BuffID.GelBalloonBuff && GlobalPet.QueenSlimePetActive(out Player queenSlime))
            {
                npc.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.QueenSlime, queenSlime.GetModPlayer<SlimePrincess>().slow, 1, npc);
            }
            else if (type == BuffID.GelBalloonBuff && GlobalPet.DualSlimePetActive(out Player dualSlime))
            {
                npc.GetGlobalNPC<NpcPet>().AddSlow(NpcPet.SlowId.QueenSlime, dualSlime.GetModPlayer<DualSlime>().slow, 1, npc);
            }
        }
    }
}
