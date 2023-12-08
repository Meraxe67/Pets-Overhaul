using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects.Vanilla
{
    public sealed class Destroyer : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int ironskinBonusDef = 8;
        public float flatDefMult = 1.22f;
        public float defItemMult = 0.5f;
        public int flatAmount = 10;
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.DestroyerPetItem))
            {
                if (Player.HasBuff(BuffID.Ironskin))
                {
                    Player.statDefense += ironskinBonusDef;
                }
                Player.statDefense.FinalMultiplier *= flatDefMult;
            }
        }
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Destroyer dest = player.GetModPlayer<Destroyer>();
            if (PickerPet.PickupChecks(item, ItemID.DestroyerPetItem, out ItemPet itemChck) && itemChck.oreBoost)
            {
                for (int i = 0; i < ItemPet.Randomizer((player.statDefense * dest.defItemMult + dest.flatAmount) * item.stack); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.miningItem), item, 1);
                }
            }
        }
    }
    public sealed class DestroyerPetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DestroyerPetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            Destroyer destroyer = Main.LocalPlayer.GetModPlayer<Destroyer>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DestroyerPetItem")
                        .Replace("<defMultChance>", Math.Round(destroyer.defItemMult * 100, 2).ToString())
                        .Replace("<flatAmount>", destroyer.flatAmount.ToString())
                        .Replace("<defMultIncrease>", destroyer.flatDefMult.ToString())
                        .Replace("<ironskinDef>", destroyer.ironskinBonusDef.ToString())
                        ));
        }
    }
}
