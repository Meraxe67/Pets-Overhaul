using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class CavelingGardener : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int cavelingRegularPlantChance = 30;
        public int cavelingGemTreeChance = 100;
        public int cavelingRarePlantChance = 15;
        public float shineMult = 0.5f;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            CavelingGardener caveling = player.GetModPlayer<CavelingGardener>();
            if (PickerPet.PickupChecks(item, ItemID.GlowTulip, out ItemPet itemChck))
            {
                if (itemChck.herbBoost && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight))
                {
                    for (int i = 0; i < ItemPet.Randomizer(((Junimo.HarvestingXpPerGathered.Find(x => x.plantList.Contains(item.type)).expAmount >= ItemPet.MinimumExpForRarePlant) ? caveling.cavelingRarePlantChance : caveling.cavelingRegularPlantChance + (ItemPet.gemstoneTreeItem[item.type] ? caveling.cavelingGemTreeChance : 0)) * item.stack); i++)
                    {
                        player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.harvestingItem), item, 1);
                    }
                }
            }
        }
        public override void UpdateEquips()
        {
            if (Pet.PetInUse(ItemID.GlowTulip))
            {
                Lighting.AddLight(Player.Center, new Vector3(0.0013f * Main.mouseTextColor, 0.0064f * Main.mouseTextColor, 0.0115f * Main.mouseTextColor) * shineMult);
            }
        }
    }
    public sealed class GlowTulip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.GlowTulip;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            CavelingGardener cavelingGardener = Main.LocalPlayer.GetModPlayer<CavelingGardener>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.GlowTulip")
                .Replace("<harvestChance>", cavelingGardener.cavelingRegularPlantChance.ToString())
                .Replace("<rarePlantChance>", cavelingGardener.cavelingRarePlantChance.ToString())
                .Replace("<gemstoneTreeChance>", cavelingGardener.cavelingGemTreeChance.ToString())
                .Replace("<shineMult>", cavelingGardener.shineMult.ToString())
            ));
        }
    }
}

