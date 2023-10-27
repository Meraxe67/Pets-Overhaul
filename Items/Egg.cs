using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class Egg : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.RottenEgg);
            Item.maxStack = 9999;
            Item.height = 26;
            Item.width = 20;
        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ItemID.FriedEgg);
            recipe.AddIngredient(ModContent.ItemType<Egg>(), 3);
            recipe.Register();
        }
    }
}
