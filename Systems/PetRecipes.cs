using PetsOverhaul.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class PetRecipes : ModSystem
    {
        public static void MasterPetCraft(int result, int itemToPairWithMasteryShard)
        {
            Recipe.Create(result)
            .AddIngredient(ModContent.ItemType<MasteryShard>())
            .AddIngredient(itemToPairWithMasteryShard)
            .Register();
        }
        public override void AddRecipes()
        {
            MasterPetCraft(ItemID.KingSlimePetItem, ItemID.KingSlimeTrophy);
            MasterPetCraft(ItemID.EyeOfCthulhuPetItem, ItemID.EyeofCthulhuTrophy);
            MasterPetCraft(ItemID.EaterOfWorldsPetItem, ItemID.EaterofWorldsTrophy);
            MasterPetCraft(ItemID.BrainOfCthulhuPetItem, ItemID.BrainofCthulhuTrophy);
            MasterPetCraft(ItemID.QueenBeePetItem, ItemID.QueenBeeTrophy);
            MasterPetCraft(ItemID.SkeletronPetItem, ItemID.SkeletronTrophy);
            MasterPetCraft(ItemID.DeerclopsPetItem, ItemID.DeerclopsTrophy);
            MasterPetCraft(ItemID.QueenSlimePetItem, ItemID.QueenSlimeTrophy);
            MasterPetCraft(ItemID.DestroyerPetItem, ItemID.DestroyerTrophy);
            MasterPetCraft(ItemID.TwinsPetItem, ItemID.RetinazerTrophy);
            MasterPetCraft(ItemID.TwinsPetItem, ItemID.SpazmatismTrophy);
            MasterPetCraft(ItemID.SkeletronPrimePetItem, ItemID.SkeletronPrimeTrophy);
            MasterPetCraft(ItemID.PlanteraPetItem, ItemID.PlanteraTrophy);
            MasterPetCraft(ItemID.DukeFishronPetItem, ItemID.DukeFishronTrophy);
            MasterPetCraft(ItemID.LunaticCultistPetItem, ItemID.AncientCultistTrophy);
            MasterPetCraft(ItemID.MoonLordPetItem, ItemID.MoonLordTrophy);
            MasterPetCraft(ItemID.DD2OgrePetItem, ItemID.BossTrophyOgre);
            MasterPetCraft(ItemID.DD2BetsyPetItem, ItemID.BossTrophyBetsy);
            MasterPetCraft(ItemID.MartianPetItem, ItemID.MartianSaucerTrophy);
            MasterPetCraft(ItemID.EverscreamPetItem, ItemID.EverscreamTrophy);
            MasterPetCraft(ItemID.IceQueenPetItem, ItemID.IceQueenTrophy);

            MasterPetCraft(ModContent.ItemType<LihzahrdWrench>(), ItemID.GolemTrophy);
            MasterPetCraft(ModContent.ItemType<PrismaticOptic>(), ItemID.FairyQueenTrophy);
            MasterPetCraft(ModContent.ItemType<PumpkingsHead>(), ItemID.PumpkingTrophy);

            Recipe.Create(ItemID.ShadowOrb)
                .AddIngredient(ItemID.ShadowScale, 12)
                .AddTile(TileID.Anvils)
                .Register();
            Recipe.Create(ItemID.CrimsonHeart)
                .AddIngredient(ItemID.TissueSample, 12)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ItemID.GolemPetItem)
                .AddIngredient(ItemID.LunarTabletFragment, 10)
                .AddCondition(Condition.PlayerCarriesItem(ModContent.ItemType<LihzahrdWrench>()))
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe.Create(ItemID.FairyQueenPetItem)
                .AddIngredient(ItemID.SoulofFlight, 15)
                .AddCondition(Condition.PlayerCarriesItem(ModContent.ItemType<PrismaticOptic>()))
                .AddTile(TileID.MythrilAnvil)
                .Register();
            Recipe.Create(ItemID.PumpkingPetItem)
                .AddIngredient(ItemID.Pumpkin, 75)
                .AddIngredient(ItemID.SpookyWood, 25)
                .AddCondition(Condition.PlayerCarriesItem(ModContent.ItemType<PumpkingsHead>()))
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
