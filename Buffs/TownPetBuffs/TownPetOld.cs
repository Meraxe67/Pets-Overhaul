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
            OldSlime oldSlime = Main.LocalPlayer.GetModPlayer<OldSlime>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == NPCID.TownSlimeOld && Main.LocalPlayer.Distance(Main.npc[i].Center) < oldSlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetOld>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Antique Aura";
                }
            }
            tip = Lang.GetBuffDescription(ModContent.BuffType<TownPetOld>())
                .Replace("<OldDef>", oldSlime.oldDef.ToString())
                .Replace("<OldKb>", oldSlime.oldKbResist.ToString())
                .Replace("<OldHarvesting>",oldSlime.DefaultHarvFort.ToString());
            rare = 0;
        }
    }
}
