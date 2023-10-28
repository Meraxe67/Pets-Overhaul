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
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        private Junimo Juni => Player.GetModPlayer<Junimo>();
        public int dirtCoin = 300;
        public int soilCoin = 200;
        public int everythingCoin = 100;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.DirtiestBlock, itemChck))
            {
                if (itemChck.dirt == true)
                {
                    Main.NewText("dirt");
                    Player.QuickSpawnItem(Player.GetSource_Misc("MiningItem"), ItemID.CopperCoin, ItemPet.Randomizer(item.stack * dirtCoin));
                    if (Juni.junimoExpCheck())
                    {
                       
                        int value = ItemPet.Randomizer((int)(dirtCoin * Juni.junimoInUseMultiplier * item.stack * Pet.miningExpBoost), 10000);
                        Juni.junimoMiningExp += value;
                        Juni.popupExpMining += value;
                        if (value > 0)
                        {
                            Juni.popupIndexMining = Juni.PopupExp(Juni.popupIndexMining, Juni.popupExpMining, Color.LightGray);
                        }
                    }
                }
                else if (itemChck.commonBlock == true)
                {
                    Player.QuickSpawnItem(Player.GetSource_Misc("MiningItem"), ItemID.CopperCoin, ItemPet.Randomizer(item.stack * soilCoin));
                    if (Juni.junimoExpCheck())
                    {
                        Main.NewText("stone");
                        int value = ItemPet.Randomizer((int)(dirtCoin * Juni.junimoInUseMultiplier * item.stack * Pet.miningExpBoost), 10000);
                        Juni.junimoMiningExp += value;
                        Juni.popupExpMining += value;
                        if (value > 0)
                        {
                            Juni.popupIndexMining = Juni.PopupExp(Juni.popupIndexMining, Juni.popupExpMining, Color.LightGray);
                        }
                    }
                }
                else if (itemChck.blockNotByPlayer == true)
                {
                    Main.NewText("any");
                    Player.QuickSpawnItem(Player.GetSource_Misc("MiningItem"), ItemID.CopperCoin, ItemPet.Randomizer(item.stack * everythingCoin));
                }
            }
            return true;

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
