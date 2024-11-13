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
            SquireSlime squireSlime = Main.LocalPlayer.GetModPlayer<SquireSlime>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == NPCID.TownSlimeCopper && Main.LocalPlayer.Distance(Main.npc[i].Center) < squireSlime.auraRange)
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
                .Replace("<SquireDmg>", Math.Round(squireSlime.squireDamage * 100, 2).ToString())
                .Replace("<SquireMining>", squireSlime.DefaultMiningFort.ToString());
            rare = 0;
        }
    }
}
