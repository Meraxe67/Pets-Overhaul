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
    public sealed class ToyGolemEffect : LightPetEffect
    {
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ToyGolem toyGolem))
            {
                Player.lifeRegen += toyGolem.HealthRegen.CurrentStatInt;
                Player.manaRegenBonus += toyGolem.ManaRegen.CurrentStatInt;
                Player.statLifeMax2 += (int)(Player.statLifeMax2 * toyGolem.PercentHealth.CurrentStatFloat);
            }
        }
    }
    public sealed class ToyGolem : GlobalItem
    {
        public LightPetStat HealthRegen = new(4,1,-1);
        public LightPetStat PercentHealth = new(35, 0.0025f, 0.025f);
        public LightPetStat ManaRegen = new(20,5,30);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.GolemPetItem;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            HealthRegen.SetRoll();
            PercentHealth.SetRoll();
            ManaRegen.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)HealthRegen.CurrentRoll);
            writer.Write((byte)ManaRegen.CurrentRoll);
            writer.Write((byte)PercentHealth.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            HealthRegen.CurrentRoll = reader.ReadByte();
            ManaRegen.CurrentRoll = reader.ReadByte();
            PercentHealth.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("GolemRegen", HealthRegen.CurrentRoll);
            tag.Add("GolemHealth", PercentHealth.CurrentRoll);
            tag.Add("GolemExp", ManaRegen.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("GolemRegen", out int reg))
            {
                HealthRegen.CurrentRoll = reg;
            }

            if (tag.TryGet("GolemHealth", out int hp))
            {
                PercentHealth.CurrentRoll = hp;
            }

            if (tag.TryGet("GolemExp", out int exp))
            {
                ManaRegen.CurrentRoll = exp;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.ToyGolem")
    
                        .Replace("<lifeRegen>", HealthRegen.BaseAndPerQuality())
                        .Replace("<healthPercent>", PercentHealth.BaseAndPerQuality())
                        .Replace("<manaRegen>", ManaRegen.BaseAndPerQuality())

                        .Replace("<lifeRegenLine>", HealthRegen.StatSummaryLine())
                        .Replace("<healthPercentLine>", PercentHealth.StatSummaryLine())
                        .Replace("<manaRegenLine>", ManaRegen.StatSummaryLine())
                        ));
            if (PercentHealth.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
