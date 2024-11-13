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
            Cat cat = Main.LocalPlayer.GetModPlayer<Cat>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == NPCID.TownCat && Main.LocalPlayer.Distance(Main.npc[i].Center) < cat.auraRange)
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
                .Replace("<CatMovespd>", cat.catSpeed.ToString())
                .Replace("<CatFish>", cat.DefaultFishFort.ToString());
            rare = 0;
        }
    }
}
