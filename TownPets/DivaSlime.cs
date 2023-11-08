using PetsOverhaul.Buffs.TownPetBuffs;
using PetsOverhaul.Systems;
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
                Player.GetModPlayer<GlobalPet>().GiveCoins(ItemPet.Randomizer((int)(item.GetStoreValue() * divaDisc * 1000), 1000));
            }
        }
    }
}