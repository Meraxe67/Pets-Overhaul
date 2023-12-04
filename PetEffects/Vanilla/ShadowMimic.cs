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
        public int npcCoin = 15;
        public int npcItem = 6;
        public int bossCoin = 7;
        public int bossItem = 5;
        public int bagCoin = 5;
        public int bagItem = 4;
        private int chanceToRollItem = 0;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public void PreOnPickup(Item item, Player player)
        {
            GlobalPet Pet = player.GetModPlayer<GlobalPet>();
            if (Pet.PickupChecks(item, ItemID.OrnateShadowKey, out ItemPet itemChck))
            {
                chanceToRollItem = 0;
                if (itemChck.itemFromNpc == true)
                {
                    chanceToRollItem += (item.IsACoin ? npcCoin : npcItem) * item.stack;
                }
                if (itemChck.itemFromBoss == true && ItemID.Sets.BossBag[item.type] == false)
                {
                    chanceToRollItem += (item.IsACoin ? bossCoin : bossItem) * item.stack;
                }
                if (itemChck.itemFromBag == true)
                {
                    chanceToRollItem += (item.IsACoin ? bagCoin : bagItem) * item.stack;
                }
                for (int i = 0; i < ItemPet.Randomizer(chanceToRollItem); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), item, 1);
                }
            }
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
