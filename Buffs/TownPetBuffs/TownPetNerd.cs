using PetsOverhaul.TownPets;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.Buffs.TownPetBuffs
{
    public class TownPetNerd : ModBuff
    {
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            NerdySlime nerdySlime = Main.LocalPlayer.GetModPlayer<NerdySlime>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == NPCID.TownSlimeBlue && Main.LocalPlayer.Distance(Main.npc[i].Center) < nerdySlime.auraRange)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetNerd>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Nerdy Aura";
                }
            }
            if (NPC.downedMoonlord)
            {
                tip = Language.GetTextValue("Mods.PetsOverhaul.Buffs.TownPetNerd.PostMoonlordDescription")
                    .Replace("<NerdPlacement>", nerdySlime.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", nerdySlime.nerdLightScale.ToString());
            }
            else if (Main.hardMode)
            {
                tip = Language.GetTextValue("Mods.PetsOverhaul.Buffs.TownPetNerd.PostHardmodeDescription")
                    .Replace("<NerdPlacement>", nerdySlime.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", nerdySlime.nerdLightScale.ToString());
            }
            else
            {
                tip = Language.GetTextValue("Mods.PetsOverhaul.Buffs.TownPetNerd.Description")
                    .Replace("<NerdPlacement>", nerdySlime.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", nerdySlime.nerdLightScale.ToString());
            }
            rare = 0;
        }
    }
}
