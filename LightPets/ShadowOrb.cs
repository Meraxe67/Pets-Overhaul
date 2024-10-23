using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class ShadowOrbEffect : LightPetEffect
    {
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ShadowOrb shadowOrb))
            {
                Player.statManaMax2 += shadowOrb.Mana.CurrentStatInt;
                Player.moveSpeed += shadowOrb.MovementSpeed.CurrentStatFloat;
                Pet.harvestingFortune += shadowOrb.HarvestingFortune.CurrentStatInt;
            }
        }
    }
    public sealed class ShadowOrb : GlobalItem
    {
        public LightPetStat Mana = new(10, 2, 20);
        public LightPetStat MovementSpeed = new(15, 0.005f, 0.025f);
        public LightPetStat HarvestingFortune = new(15, 1, 5);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.ShadowOrb;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Mana.SetRoll();
            MovementSpeed.SetRoll();
            HarvestingFortune.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Mana.CurrentRoll);
            writer.Write((byte)MovementSpeed.CurrentRoll);
            writer.Write((byte)HarvestingFortune.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Mana.CurrentRoll = reader.ReadByte();
            MovementSpeed.CurrentRoll = reader.ReadByte();
            HarvestingFortune.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("ShadowMana", Mana.CurrentRoll);
            tag.Add("ShadowExp", MovementSpeed.CurrentRoll); //exp stats are obsolete
            tag.Add("ShadowFort", HarvestingFortune.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("ShadowMana", out int mana))
            {
                Mana.CurrentRoll = mana;
            }

            if (tag.TryGet("ShadowExp", out int exp))
            {
                MovementSpeed.CurrentRoll = exp;
            }

            if (tag.TryGet("ShadowFort", out int fort))
            {
                HarvestingFortune.CurrentRoll = fort;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.LightPetTooltips.ShadowOrb")

                        .Replace("<mana>", Mana.BaseAndPerQuality())
                        .Replace("<ms>", MovementSpeed.BaseAndPerQuality())
                        .Replace("<fortune>", HarvestingFortune.BaseAndPerQuality())

                        .Replace("<manaLine>", Mana.StatSummaryLine())
                        .Replace("<msLine>", MovementSpeed.StatSummaryLine())
                        .Replace("<fortuneLine>", HarvestingFortune.StatSummaryLine())
                        ));
            if (Mana.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}
