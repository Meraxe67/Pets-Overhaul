using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetDiva : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            DivaSlime divaSlime = Main.LocalPlayer.GetModPlayer<DivaSlime>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == NPCID.TownSlimeRainbow && Main.LocalPlayer.Distance(Main.npc[i].Center) < divaSlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetDiva>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Shining Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetDiva>())
                .Replace("<DivaDisc>", Math.Round(divaSlime.divaDisc * 100, 2).ToString());
            rare = 0;
        }
    }
}
