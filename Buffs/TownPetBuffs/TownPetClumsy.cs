using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetClumsy : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            ClumsySlime clumsySlime = Main.LocalPlayer.GetModPlayer<ClumsySlime>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == NPCID.TownSlimePurple && Main.LocalPlayer.Distance(Main.npc[i].Center) < clumsySlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetClumsy>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Unlucky Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetClumsy>())
                .Replace("<ClumsyLuck>", clumsySlime.clumsyLuck.ToString())
                .Replace("<ClumsyGlobal>", clumsySlime.DefaultGlobalFort.ToString());
            rare = 0;
        }
    }
}
