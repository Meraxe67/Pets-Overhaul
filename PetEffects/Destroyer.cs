using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class Destroyer : PetEffect
    {
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public int ironskinBonusDef = 8;
        public float flatDefMult = 0.22f;
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
                Player.statDefense *= 1f + flatDefMult;
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
                for (int i = 0; i < GlobalPet.Randomizer((player.statDefense * dest.defItemMult + dest.flatAmount) * item.stack); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
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
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            Destroyer destroyer = Main.LocalPlayer.GetModPlayer<Destroyer>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DestroyerPetItem")
                .Replace("<class>", PetTextsColors.ClassText(destroyer.PetClassPrimary, destroyer.PetClassSecondary))
                        .Replace("<defMultChance>", Math.Round(destroyer.defItemMult * 100, 2).ToString())
                        .Replace("<flatAmount>", destroyer.flatAmount.ToString())
                        .Replace("<defMultIncrease>", Math.Round(destroyer.flatDefMult * 100, 2).ToString())
                        .Replace("<ironskinDef>", destroyer.ironskinBonusDef.ToString())
                        ));
        }
    }
}
