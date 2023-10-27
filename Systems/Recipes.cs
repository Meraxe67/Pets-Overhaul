using PetsOverhaul.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Systems
{
    public class Recipes : ModSystem
    {
        public static void MasterPetCraft(int result, int itemToPairWithMasteryShard)
        {
            Recipe recipe = Recipe.Create(result);
            recipe.AddIngredient(ModContent.ItemType<MasteryShard>());
            recipe.AddIngredient(itemToPairWithMasteryShard);
            recipe.Register();
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
        }
    }
}
