using PetsOverhaul.TownPets;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetDog : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownDog)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetDog>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Dog Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetDog>())
                .Replace("<DogFish>", Main.LocalPlayer.GetModPlayer<TownPet>().dogFish.ToString())
                .Replace("<DogFishExp>", Main.LocalPlayer.GetModPlayer<TownPet>().FishFort.ToString())
                ;
            rare = 0;
        }
    }
}
