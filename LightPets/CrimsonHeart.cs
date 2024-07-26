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
    public sealed class CrimsonHeartEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.CrimsonHeart && Player.miscEquips[1].TryGetGlobalItem(out CrimsonHeart crimsonHeart))
            {
                Player.statLifeMax2 += crimsonHeart.CurrentHealth;
                Player.moveSpeed += crimsonHeart.CurrentMs;
                Pet.fishingFortune += crimsonHeart.CurrentFishFort;
            }
        }
    }
    public sealed class CrimsonHeart : GlobalItem
    {
        public int baseHealth = 10;
        public int healthPerRoll = 1;
        public int healthMaxRoll = 10;
        public int healthRoll = 0;
        public int CurrentHealth => baseHealth + healthPerRoll * healthRoll;

        public float baseMs = 0.025f;
        public float msPerRoll = 0.005f;
        public int msMaxRoll = 15;
        public int msRoll = 0;
        public float CurrentMs => baseMs + msPerRoll * msRoll;

        public int baseFishFort = 5;
        public int fishFortPerRoll = 1;
        public int fishFortMaxRoll = 15;
        public int fishFortRoll = 0;
        public int CurrentFishFort => baseFishFort + fishFortPerRoll * fishFortRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.CrimsonHeart;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (healthRoll <= 0)
            {
                healthRoll = Main.rand.Next(healthMaxRoll) + 1;
            }

            if (msRoll <= 0)
            {
                msRoll = Main.rand.Next(msMaxRoll) + 1;
            }

            if (fishFortRoll <= 0)
            {
                fishFortRoll = Main.rand.Next(fishFortMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)healthRoll);
            writer.Write((byte)msRoll);
            writer.Write((byte)fishFortRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            healthRoll = reader.ReadByte();
            msRoll = reader.ReadByte();
            fishFortRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("CrimsonHealth", healthRoll);
            tag.Add("CrimsonExp", msRoll); //Exp stats are obsolete
            tag.Add("CrimsonFort", fishFortRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("CrimsonHealth", out int hp))
            {
                healthRoll = hp;
            }

            if (tag.TryGet("CrimsonExp", out int exp))
            {
                msRoll = exp;
            }

            if (tag.TryGet("CrimsonFort", out int fort))
            {
                fishFortRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.CrimsonHeart")

                        .Replace("<hpBase>", baseHealth.ToString())
                        .Replace("<hpPer>", healthPerRoll.ToString())

                        .Replace("<expBase>", Math.Round(baseMs * 100, 2).ToString())
                        .Replace("<expPer>", Math.Round(msPerRoll * 100, 2).ToString())

                        .Replace("<fortBase>", baseFishFort.ToString())
                        .Replace("<fortPer>", fishFortPerRoll.ToString())

                        .Replace("<currentHp>", PetColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentHealth.ToString(), healthRoll, healthMaxRoll))
                        .Replace("<hpRoll>", PetColors.LightPetRarityColorConvert(healthRoll.ToString(), healthRoll, healthMaxRoll))
                        .Replace("<hpMaxRoll>", PetColors.LightPetRarityColorConvert(healthMaxRoll.ToString(), healthRoll, healthMaxRoll))

                        .Replace("<currentExp>", PetColors.LightPetRarityColorConvert(Math.Round(CurrentMs * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), msRoll, msMaxRoll))
                        .Replace("<expRoll>", PetColors.LightPetRarityColorConvert(msRoll.ToString(), msRoll, msMaxRoll))
                        .Replace("<expMaxRoll>", PetColors.LightPetRarityColorConvert(msMaxRoll.ToString(), msRoll, msMaxRoll))

                        .Replace("<currentFort>", PetColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentFishFort.ToString(), fishFortRoll, fishFortMaxRoll))
                        .Replace("<fortRoll>", PetColors.LightPetRarityColorConvert(fishFortRoll.ToString(), fishFortRoll, fishFortMaxRoll))
                        .Replace("<fortMaxRoll>", PetColors.LightPetRarityColorConvert(fishFortMaxRoll.ToString(), fishFortRoll, fishFortMaxRoll))
                        ));
            if (msRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
