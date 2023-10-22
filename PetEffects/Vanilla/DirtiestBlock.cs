using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameInput;
using PetsOverhaul.Config;

using PetsOverhaul.Config;
using Terraria.GameInput;

namespace PetsOverhaul.PetEffects.Vanilla
{
    sealed public class DirtiestBlock : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public int dirtCoin = 300;
        public int soilCoin = 200;
        public int everythingCoin = 100;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.DirtiestBlock, itemChck))
            {
                if (itemChck.dirt == true)
                {
                    Player.QuickSpawnItem(Player.GetSource_Misc("MiningItem"), ItemID.CopperCoin, ItemPet.Randomizer(item.stack * dirtCoin));
                }
                else if (itemChck.commonBlock == true)
                {
                    Player.QuickSpawnItem(Player.GetSource_Misc("MiningItem"), ItemID.CopperCoin, ItemPet.Randomizer(item.stack * soilCoin));
                }
                else if (itemChck.blockNotByPlayer == true)
                {
                    Player.QuickSpawnItem(Player.GetSource_Misc("MiningItem"), ItemID.CopperCoin, ItemPet.Randomizer(item.stack * everythingCoin));
                }
            }
            return true;

        }
    }
    sealed public class DirtiestBlockItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.DirtiestBlock;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            DirtiestBlock dirtiestBlock = Main.LocalPlayer.GetModPlayer<DirtiestBlock>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.DirtiestBlock")
                        .Replace("<any>", (dirtiestBlock.everythingCoin / 100).ToString())
                        .Replace("<soil>", (dirtiestBlock.soilCoin / 100).ToString())
                        .Replace("<dirt>", (dirtiestBlock.dirtCoin / 100).ToString())
                        ));
        }
    }
}
