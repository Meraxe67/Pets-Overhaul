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
        public int bossCoin = 7;
        public int bossItem = 5;
        public int bagCoin = 5;
        public int bagItem = 4;
        private int chanceToRollItem = 0;
        public override bool OnPickup(Item item)
        {
            if (item.TryGetGlobalItem(out ItemPet itemChck) && Pet.PickupChecks(item, ItemID.OrnateShadowKey, itemChck))
            {
                chanceToRollItem = 0;
                if (itemChck.itemFromNpc == true)
                {
                    if (item.IsACoin)
                    {
                        chanceToRollItem += npcCoin * item.stack;
                    }
                    else
                    {
                        chanceToRollItem += npcItem * item.stack;
                    }
                }
                if (itemChck.itemFromBoss == true && ItemID.Sets.BossBag[item.type] == false)
                {
                    if (item.IsACoin)
                    {
                        chanceToRollItem += bossCoin * item.stack;
                    }
                    else
                    {
                        chanceToRollItem += bossItem * item.stack;
                    }
                }
                if (itemChck.itemFromBag == true)
                {
                    if (item.IsACoin)
                    {
                        chanceToRollItem += bagCoin * item.stack;
                    }
                    else
                    {
                        chanceToRollItem += bagItem * item.stack;
                    }
                }
                for (int i = 0; i < ItemPet.Randomizer(chanceToRollItem); i++)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), item, 1);
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
