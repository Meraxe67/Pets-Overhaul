using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetBunny : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Bunny bunny = Main.LocalPlayer.GetModPlayer<Bunny>();
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCID.TownBunny && Main.LocalPlayer.Distance(npc.Center) < bunny.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetBunny>()).Replace("<Name>", npc.FullName);
                    break;
                }
                else
                {
                    buffName = "Bunny Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetBunny>())
                .Replace("<BunnyJump>", Math.Round(bunny.bunnyJump * 100, 2).ToString())
                .Replace("<BunnyHarvesting>", bunny.DefaultHarvFort.ToString());
            rare = 0;
        }
    }
}
