using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetCat : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownCat)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetCat>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Cat Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetCat>())
                .Replace("<CatMovespd>", Main.LocalPlayer.GetModPlayer<TownPet>().catSpeed.ToString())
                .Replace("<CatFish>", Main.LocalPlayer.GetModPlayer<TownPet>().catFishFort.ToString())
                ;
            rare = 0;
        }
    }
}
