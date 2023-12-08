using Microsoft.Xna.Framework;
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
    public sealed class DirtiestBlock : ModPlayer
    {
        public GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int dirtCoin = 300;
        public int soilCoin = 200;
        public int everythingCoin = 100;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            Junimo Juni = player.GetModPlayer<Junimo>();
            DirtiestBlock dirt = player.GetModPlayer<DirtiestBlock>();
            if (PickerPet.PickupChecks(item, ItemID.DirtiestBlock, out ItemPet itemChck) && itemChck.blockNotByPlayer == true)
            {
                if (item.type == ItemID.DirtBlock)
                {
                    PickerPet.GiveCoins(ItemPet.Randomizer(item.stack * dirt.dirtCoin));
                    if (Juni.JunimoExpCheck())
                    {
                        int value = ItemPet.Randomizer((int)(dirt.dirtCoin * Juni.junimoInUseMultiplier * item.stack * PickerPet.miningExpBoost), 10000);
                        Juni.junimoMiningExp += value;
                        Juni.popupExpMining += value;
                        if (value > 0)
                        {
                            Juni.popupIndexMining = Juni.PopupExp(Juni.popupIndexMining, Juni.popupExpMining, new Color(150, 168, 176));
                        }
                    }
                }
                else if (itemChck.commonBlock == true)
                {
                    PickerPet.GiveCoins(ItemPet.Randomizer(item.stack * dirt.soilCoin));
                    if (Juni.JunimoExpCheck())
                    {
                        int value = ItemPet.Randomizer((int)(dirt.dirtCoin * Juni.junimoInUseMultiplier * item.stack * PickerPet.miningExpBoost), 10000);
                        Juni.junimoMiningExp += value;
                        Juni.popupExpMining += value;
                        if (value > 0)
                        {
                            Juni.popupIndexMining = Juni.PopupExp(Juni.popupIndexMining, Juni.popupExpMining, new Color(150, 168, 176));
                        }
                    }
                }
                else
                {
                    PickerPet.GiveCoins(ItemPet.Randomizer(item.stack * dirt.everythingCoin));
                }
            }

        }
    }
    public sealed class DirtiestBlockItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.DirtiestBlock;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            DirtiestBlock dirtiestBlock = Main.LocalPlayer.GetModPlayer<DirtiestBlock>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DirtiestBlock")
                        .Replace("<any>", (dirtiestBlock.everythingCoin / 100).ToString())
                        .Replace("<soil>", (dirtiestBlock.soilCoin / 100).ToString())
                        .Replace("<dirt>", (dirtiestBlock.dirtCoin / 100).ToString())
                        ));
        }
    }
}
