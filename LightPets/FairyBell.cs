using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class FairyBellEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.FairyBell && Player.miscEquips[1].TryGetGlobalItem(out FairyBell fairyBell))
            {
                Pet.abilityHaste += fairyBell.CurrentHaste;
                Pet.globalFortune += fairyBell.CurrentGlobalFort;
            }
        }
    }
    public sealed class FairyBell : GlobalItem
    {
        public float baseHaste = 0.1f;
        public float hastePerRoll = 0.01f;
        public int hasteMaxRoll = 15;
        public int hasteRoll = 0;
        public float CurrentHaste => baseHaste + hastePerRoll * hasteRoll;

        public int baseGlobalFort = 5;
        public int globalFortPerRoll = 1;
        public int globalFortMaxRoll = 20;
        public int globalFortRoll = 0;
        public int CurrentGlobalFort => baseGlobalFort + globalFortPerRoll * globalFortRoll;

        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.FairyBell;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (hasteRoll <= 0)
            {
                hasteRoll = Main.rand.Next(hasteMaxRoll) + 1;
            }

            if (globalFortRoll <= 0)
            {
                globalFortRoll = Main.rand.Next(globalFortMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)hasteRoll);
            writer.Write((byte)globalFortRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            hasteRoll = reader.ReadByte();
            globalFortRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("FairyHaste", hasteRoll);
            tag.Add("FairyFort", globalFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("FairyHaste", out int haste))
            {
                hasteRoll = haste;
            }

            if (tag.TryGet("FairyFort", out int fort))
            {
                globalFortRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.FairyBell")

                        .Replace("<hasteBase>", Math.Round(baseHaste * 100, 2).ToString())
                        .Replace("<hastePer>", Math.Round(hastePerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseGlobalFort.ToString())
                        .Replace("<fortPer>", globalFortPerRoll.ToString())

                        .Replace("<currentHaste>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentHaste * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), hasteRoll, hasteMaxRoll))
                        .Replace("<hasteRoll>", PetColors.LightPetRarityColorConvert(hasteRoll.ToString(), hasteRoll, hasteMaxRoll))
                        .Replace("<hasteMaxRoll>", PetColors.LightPetRarityColorConvert(hasteMaxRoll.ToString(), hasteRoll, hasteMaxRoll))

                        .Replace("<currentFort>", PetColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentGlobalFort.ToString(), globalFortRoll, globalFortMaxRoll))
                        .Replace("<fortRoll>", PetColors.LightPetRarityColorConvert(globalFortRoll.ToString(), globalFortRoll, globalFortMaxRoll))
                        .Replace("<fortMaxRoll>", PetColors.LightPetRarityColorConvert(globalFortMaxRoll.ToString(), globalFortRoll, globalFortMaxRoll))

                        ));
            if (globalFortRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
