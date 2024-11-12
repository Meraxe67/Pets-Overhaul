using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace PetsOverhaul.PetEffects
{
    public sealed class DirtiestBlock : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Mining;
        public int dirtCoin = 850;
        public int soilCoin = 600;
        public int everythingCoin = 300;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            DirtiestBlock dirt = player.GetModPlayer<DirtiestBlock>();
            if (PickerPet.PickupChecks(item, ItemID.DirtiestBlock, out ItemPet itemChck) && itemChck.blockNotByPlayer == true)
            {
                if (item.type == ItemID.DirtBlock)
                {
                    PickerPet.GiveCoins(GlobalPet.Randomizer(item.stack * dirt.dirtCoin));
                }
                else if (itemChck.commonBlock == true)
                {
                    PickerPet.GiveCoins(GlobalPet.Randomizer(item.stack * dirt.soilCoin));
                }
                else
                {
                    PickerPet.GiveCoins(GlobalPet.Randomizer(item.stack * dirt.everythingCoin));
                }
            }

        }
    }
    public sealed class DirtiestBlockItem : PetTooltip
    {
        public override PetEffect PetsEffect => dirtiestBlock;
        public static DirtiestBlock dirtiestBlock
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out DirtiestBlock pet))
                    return pet;
                else
                    return ModContent.GetInstance<DirtiestBlock>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DirtiestBlock")
                        .Replace("<any>", (dirtiestBlock.everythingCoin / 100).ToString())
                        .Replace("<soil>", (dirtiestBlock.soilCoin / 100).ToString())
                        .Replace("<dirt>", (dirtiestBlock.dirtCoin / 100).ToString());
    }
}
