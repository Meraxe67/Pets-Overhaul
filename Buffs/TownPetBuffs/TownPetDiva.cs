using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetDiva : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeRainbow)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetDiva>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Shining Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetDiva>()).Replace("<DivaDisc>", Main.LocalPlayer.GetModPlayer<TownPet>().divaDisc.ToString());
            rare = 0;
        }
    }
}
