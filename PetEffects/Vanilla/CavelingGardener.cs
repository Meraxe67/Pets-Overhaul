using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class CavelingGardener : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int cavelingRegularPlantChance = 30;
        public int cavelingGemTreeChance = 100;
        public int cavelingRarePlantChance = 15;
        public float shineMult = 0.5f;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.GlowTulip, itemChck))
            {
                if ((itemChck.gemTree || itemChck.herbBoost) && (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight || Player.ZoneUnderworldHeight))
                {
                    for (int i = 0; i < ItemPet.Randomizer((cavelingRegularPlantChance + (itemChck.gemTree ? cavelingGemTreeChance : 0)) * item.stack); i++)
                    {
                        Player.QuickSpawnItem(Player.GetSource_Misc("HarvestingItem"), item, 1);
                    }
                }

                if (itemChck.rareHerbBoost && (Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight || Player.ZoneUnderworldHeight))
                {
                    for (int i = 0; i < ItemPet.Randomizer(cavelingRarePlantChance * item.stack); i++)
                    {
                        Player.QuickSpawnItem(Player.GetSource_Misc("HarvestingItem"), item, 1);
                    }
                }
            }

            return true;
        }
        public override void UpdateEquips()
        {
            if (Pet.PetInUse(ItemID.GlowTulip))
            {
                Lighting.AddLight(Player.Center, new Microsoft.Xna.Framework.Vector3(0.0013f * Main.mouseTextColor, 0.0064f * Main.mouseTextColor, 0.0115f * Main.mouseTextColor) * shineMult);
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

