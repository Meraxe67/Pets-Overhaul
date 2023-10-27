using PetsOverhaul.Buffs.TownPetBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
                int refundAmount = (int)(item.GetStoreValue() / 100 * divaDisc);
                if (refundAmount > 1000000)
                {
                    Player.QuickSpawnItem(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<TownPetDiva>())), ItemID.PlatinumCoin, refundAmount / 1000000);
                    refundAmount %= 1000000;
                }
                if (refundAmount > 10000)
                {
                    Player.QuickSpawnItem(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<TownPetDiva>())), ItemID.GoldCoin, refundAmount / 10000);
                    refundAmount %= 10000;
                }
                if (refundAmount > 100)
                {
                    Player.QuickSpawnItem(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<TownPetDiva>())), ItemID.SilverCoin, refundAmount / 100);
                    refundAmount %= 100;
                }
                if (refundAmount >= 1)
                {
                    Player.QuickSpawnItem(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<TownPetDiva>())), ItemID.CopperCoin, refundAmount / 1);
                }
            }
        }
    }
}