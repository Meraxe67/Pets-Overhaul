using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetMystic : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            MysticSlime mysticSlime = Main.LocalPlayer.GetModPlayer<MysticSlime>();
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCID.TownSlimeYellow && Main.LocalPlayer.Distance(npc.Center) < mysticSlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetBunny>()).Replace("<Name>", npc.FullName);
                    break;
                }
                else
                {
                    buffName = "Mystic Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetMystic>())
                .Replace("<MysticHaste>", Math.Round(mysticSlime.mysticHaste * 100, 2).ToString())
                .Replace("<MysticAllExp>", mysticSlime.DefaultGlobalFort.ToString());
            rare = 0;
        }
    }
}
