using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PetsOverhaul.Systems;

namespace PetsOverhaul.TownPets
{
    public class DivaSlime : TownPet
    {
        public override void PostUpdateBuffs()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Player.Distance(Main.npc[i].Center) < auraRange && Main.npc[i].type == NPCID.TownSlimeRainbow && Main.npc[i].active == true)
                {
                    Player.AddBuff(ModContent.BuffType<TownPetDiva>(), 2);
                    break;
                }
            }
        }
        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (Player.HasBuff(ModContent.BuffType<TownPetDiva>()))
            {
                int refundAmount = ItemPet.Randomizer((int)(item.GetStoreValue() * divaDisc*1000),1000);
                if (refundAmount > 1000000)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.PlatinumCoin, refundAmount / 1000000);
                    refundAmount %= 1000000;
                }
                if (refundAmount > 10000)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.GoldCoin, refundAmount / 10000);
                    refundAmount %= 10000;
                }
                if (refundAmount > 100)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.SilverCoin, refundAmount / 100);
                    refundAmount %= 100;
                }
                if (refundAmount >= 1)
                {
                    Player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySource_Pet.TypeId.globalItem), ItemID.CopperCoin, refundAmount / 1);
                }
            }
        }
    }
}