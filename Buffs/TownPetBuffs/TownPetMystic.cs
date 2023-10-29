using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetMystic : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeYellow)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetBunny>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Mystic Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetMystic>())
                .Replace("<MysticHaste>", Math.Round(Main.LocalPlayer.GetModPlayer<TownPet>().mysticHaste * 100, 2).ToString())
                .Replace("<MysticAllExp>", Math.Round(Main.LocalPlayer.GetModPlayer<TownPet>().mysticAllExp * 100, 2).ToString())
                ;
            rare = 0;
        }
    }
}
