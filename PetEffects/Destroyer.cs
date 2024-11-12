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
        public float flatDefMult = 0.15f;
        public float defItemMult = 0.5f;
        public int flatAmount = 10;
        public int miningFort = 10;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.DestroyerPetItem))
            {
                if (Player.HasBuff(BuffID.Ironskin))
                {
                    Player.statDefense += ironskinBonusDef;
                    Pet.miningFortune += miningFort;
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
    public sealed class DestroyerPetItem : PetTooltip
    {
        public override PetEffect PetsEffect => destroyer;
        public static Destroyer destroyer
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out Destroyer pet))
                    return pet;
                else
                    return ModContent.GetInstance<Destroyer>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DestroyerPetItem")
                        .Replace("<defMultChance>", Math.Round(destroyer.defItemMult * 100, 2).ToString())
                        .Replace("<flatAmount>", destroyer.flatAmount.ToString())
                        .Replace("<defMultIncrease>", Math.Round(destroyer.flatDefMult * 100, 2).ToString())
                        .Replace("<ironskinDef>", destroyer.ironskinBonusDef.ToString())
                        .Replace("<miningFortune>", destroyer.miningFort.ToString());
    }
}
