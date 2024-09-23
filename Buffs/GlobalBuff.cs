using Microsoft.Xna.Framework.Graphics;
using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.DataStructures;
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
        public override bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
        {
            if (type == BuffID.LunaticCultistPet)
            {
                PhantasmalDragon dragon = Main.LocalPlayer.GetModPlayer<PhantasmalDragon>();
                switch (dragon.currentAbility)
                {
                    case 0:
                        drawParams.DrawColor = Microsoft.Xna.Framework.Color.DeepSkyBlue;
                        break;
                        case 1:
                        drawParams.DrawColor = Microsoft.Xna.Framework.Color.PaleTurquoise;
                        break;
                        case 2:
                        drawParams.DrawColor = Microsoft.Xna.Framework.Color.Coral;
                        break;
                        default:
                        break;
                }
                
            }
            return true;
        }
    }
}
