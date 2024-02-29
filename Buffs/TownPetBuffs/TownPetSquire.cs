using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetSquire : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeCopper)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetSquire>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Courageous Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetSquire>())
                .Replace("<SquireDmg>", Math.Round(Main.LocalPlayer.GetModPlayer<TownPet>().squireDamage * 100, 2).ToString())
                .Replace("<SquireMining>", Main.LocalPlayer.GetModPlayer<TownPet>().MiningFort.ToString())
                ;
            rare = 0;
        }
    }
}
