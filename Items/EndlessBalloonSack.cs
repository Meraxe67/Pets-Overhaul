using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaul.Items
{
    public class EndlessBalloonSack : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GelBalloon);
            Item.maxStack = 1;
            Item.consumable = false;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 0;
        }
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<EndlessBalloonSack>())
                .AddIngredient(ItemID.GelBalloon, 10)
                .Register();
        }
    }
}
