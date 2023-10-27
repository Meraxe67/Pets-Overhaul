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
    public sealed class ShadowMimic : ModPlayer
    {
        private GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public int npcCoin = 15;
        public int npcItem = 6;
        public int bossCoin = 5;
        public int bossItem = 10;
        public int bagCoin = 3;
        public int bagItem = 5;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.OrnateShadowKey, itemChck))
            {
                if (itemChck.itemFromNpc == true)
                {
                    if (item.IsACoin)
                    {
                        item.stack += ItemPet.Randomizer(npcCoin * item.stack);
                    }
                    else
                    {
                        item.stack += ItemPet.Randomizer(npcItem * item.stack);
                    }
                }
                if (itemChck.itemFromBoss == true && ItemID.Sets.BossBag[item.type] == false)
                {
                    if (item.IsACoin)
                    {
                        item.stack += ItemPet.Randomizer(bossCoin * item.stack);
                    }
                    else
                    {
                        item.stack += ItemPet.Randomizer(bossItem * item.stack);
                    }
                }
                if (itemChck.itemFromBag == true)
                {
                    if (item.IsACoin)
                    {
                        item.stack += ItemPet.Randomizer(bagCoin * item.stack);
                    }
                    else
                    {
                        item.stack += ItemPet.Randomizer(bagItem * item.stack);
                    }
                }
            }
            return true;
        }
    }
    public sealed class OrnateShadowKey : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.OrnateShadowKey;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down])
            {
                return;
            }

            ShadowMimic shadowMimic = Main.LocalPlayer.GetModPlayer<ShadowMimic>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.OrnateShadowKey")
                        .Replace("<npcCoin>", shadowMimic.npcCoin.ToString())
                        .Replace("<npcItem>", shadowMimic.npcItem.ToString())
                        .Replace("<bossCoin>", shadowMimic.bossCoin.ToString())
                        .Replace("<bossItem>", shadowMimic.bossItem.ToString())
                        .Replace("<bagCoin>", shadowMimic.bagCoin.ToString())
                        .Replace("<bagItem>", shadowMimic.bagItem.ToString())
                        ));
        }
    }
}
