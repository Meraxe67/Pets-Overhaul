using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetCool : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            CoolSlime coolSlime = Main.LocalPlayer.GetModPlayer<CoolSlime>();
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCID.TownSlimeGreen && Main.LocalPlayer.Distance(npc.Center) < coolSlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetCool>()).Replace("<Name>", npc.FullName);
                    break;
                }
                else
                {
                    buffName = "Cool Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetCool>())
                .Replace("<CoolCrit>", coolSlime.critHitsAreCool.ToString())
                .Replace("<CoolMining>", coolSlime.DefaultMiningFort.ToString());
            rare = 0;
        }
    }
}
