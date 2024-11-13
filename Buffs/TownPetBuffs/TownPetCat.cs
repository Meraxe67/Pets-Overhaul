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
            foreach (var npc in Main.ActiveNPCs)
            {
                if (npc.type == NPCID.TownCat && Main.LocalPlayer.Distance(npc.Center) < cat.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetCat>()).Replace("<Name>", npc.FullName);
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
