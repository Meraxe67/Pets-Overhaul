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
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.LocalPlayer.Distance(Main.npc[i].Center) < Main.LocalPlayer.GetModPlayer<TownPet>().auraRange && Main.npc[i].type == NPCID.TownSlimeBlue)
                {
                    buffName = Lang.GetBuffName(ModContent.BuffType<TownPetNerd>()).Replace("<Name>", Main.npc[i].FullName);
                    break;
                }
                else
                {
                    buffName = "Nerdy Aura";
                }
            }
            TownPet townPet = Main.LocalPlayer.GetModPlayer<TownPet>();
            if (NPC.downedMoonlord)
            {
                tip = Language.GetTextValue("Mods.PetsOverhaul.Buffs.TownPetNerd.PostMoonlordDescription")
                    .Replace("<NerdPlacement>", townPet.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", townPet.nerdLightScale.ToString());
            }
            else if (Main.hardMode)
            {
                tip = Language.GetTextValue("Mods.PetsOverhaul.Buffs.TownPetNerd.PostHardmodeDescription")
                    .Replace("<NerdPlacement>", townPet.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", townPet.nerdLightScale.ToString());
            }
            else
            {
                tip = Language.GetTextValue("Mods.PetsOverhaul.Buffs.TownPetNerd.Description")
                    .Replace("<NerdPlacement>", townPet.nerdBuildSpeed.ToString())
                    .Replace("<NerdShineScale>", townPet.nerdLightScale.ToString());
            }
            rare = 0;
        }
    }
}
