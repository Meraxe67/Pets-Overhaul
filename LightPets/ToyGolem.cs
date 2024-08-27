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
    public sealed class ToyGolemEffect : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].type == ItemID.GolemPetItem && Player.miscEquips[1].TryGetGlobalItem(out ToyGolem toyGolem))
            {
                Player.lifeRegen += toyGolem.CurrentRegen;
                Player.manaRegenBonus += toyGolem.CurrentMana;
                Player.statLifeMax2 += (int)(Player.statLifeMax2 * toyGolem.CurrentHealth);
            }
        }
    }
    public sealed class ToyGolem : GlobalItem
    {
        public int baseRegen = -1;
        public int regenPerRoll = 1;
        public int regenMaxRoll = 4;
        public int regenRoll = 0;
        public int CurrentRegen => baseRegen + regenPerRoll * regenRoll;

        public float baseHealth = 0.025f;
        public float healthPerRoll = 0.0025f;
        public int healthMaxRoll = 35;
        public int healthRoll = 0;
        public float CurrentHealth => baseHealth + healthPerRoll * healthRoll;

        public int baseMana = 30;
        public int manaPerRoll = 5;
        public int manaMaxRoll = 20;
        public int manaRoll = 0;
        public int CurrentMana => baseMana + manaPerRoll * manaRoll;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.GolemPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (regenRoll <= 0)
            {
                regenRoll = Main.rand.Next(regenMaxRoll) + 1;
            }

            if (manaRoll <= 0)
            {
                manaRoll = Main.rand.Next(manaMaxRoll) + 1;
            }

            if (healthRoll <= 0)
            {
                healthRoll = Main.rand.Next(healthMaxRoll) + 1;
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)regenRoll);
            writer.Write((byte)manaRoll);
            writer.Write((byte)healthRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            regenRoll = reader.ReadByte();
            manaRoll = reader.ReadByte();
            healthRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("GolemRegen", regenRoll);
            tag.Add("GolemHealth", healthRoll);
            tag.Add("GolemExp", manaRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("GolemRegen", out int reg))
            {
                regenRoll = reg;
            }

            if (tag.TryGet("GolemHealth", out int hp))
            {
                healthRoll = hp;
            }

            if (tag.TryGet("GolemExp", out int exp))
            {
                manaRoll = exp;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.ToyGolem")
    
                        .Replace("<regenBase>", baseRegen.ToString())
                        .Replace("<regenPer>", regenPerRoll.ToString())

                        .Replace("<healthBase>", Math.Round(baseHealth * 100, 2).ToString())
                        .Replace("<healthPer>", Math.Round(healthPerRoll * 100, 2).ToString())

                        .Replace("<expBase>", baseMana.ToString())
                        .Replace("<expPer>", manaPerRoll.ToString())

                        .Replace("<currentRegen>", PetTextsColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentRegen.ToString(), regenRoll, regenMaxRoll))
                        .Replace("<regenRoll>", PetTextsColors.LightPetRarityColorConvert(regenRoll.ToString(), regenRoll, regenMaxRoll))
                        .Replace("<regenMaxRoll>", PetTextsColors.LightPetRarityColorConvert(regenMaxRoll.ToString(), regenRoll, regenMaxRoll))

                        .Replace("<currentExp>", PetTextsColors.LightPetRarityColorConvert(Language.GetTextValue("Mods.PetsOverhaul.+") + CurrentMana.ToString(), manaRoll, manaMaxRoll))
                        .Replace("<expRoll>", PetTextsColors.LightPetRarityColorConvert(manaRoll.ToString(), manaRoll, manaMaxRoll))
                        .Replace("<expMaxRoll>", PetTextsColors.LightPetRarityColorConvert(manaMaxRoll.ToString(), manaRoll, manaMaxRoll))

                        .Replace("<currentHealth>", PetTextsColors.LightPetRarityColorConvert(Math.Round(CurrentHealth * 100, 2).ToString() + Language.GetTextValue("Mods.PetsOverhaul.%"), healthRoll, healthMaxRoll))
                        .Replace("<healthRoll>", PetTextsColors.LightPetRarityColorConvert(healthRoll.ToString(), healthRoll, healthMaxRoll))
                        .Replace("<healthMaxRoll>", PetTextsColors.LightPetRarityColorConvert(healthMaxRoll.ToString(), healthRoll, healthMaxRoll))
                        ));
            if (healthRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/" + PetTextsColors.LowQuality.Hex3() + ":" + Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.NotRolled") + "]"));
            }
        }
    }
}
