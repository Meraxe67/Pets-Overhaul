using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetOld : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeOld)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetOld>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Antique Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetOld>()).Replace("<OldDef>", Main.LocalPlayer.GetModPlayer<TownPet>().oldDef.ToString())
                .Replace("<OldKb>", Main.LocalPlayer.GetModPlayer<TownPet>().oldKbResist.ToString());

            rare = 0;
        }
    }
}
